using IdleMonitor.Controllers;
using IdleMonitor.Infrastructure;
using IdleMonitor.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;

namespace IdleMonitor
{
    public class Startup
    {
        

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
           
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers(options => options.ModelBinderProviders.Insert(0, new ModelBinderProvider()));
            //services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
           app.UseCors(builder => builder
               .AllowAnyHeader()
               .AllowAnyMethod()
               .SetIsOriginAllowed((host) => true)
               .AllowCredentials()
             );

            app.Use(next => async context =>
            {
                var stream = context.Request.Body;

                if (stream == Stream.Null || stream.CanSeek)
                {
                    await next(context);

                    return;
                }

                try
                {
                    using (var buffer = new MemoryStream())
                    {
                        await stream.CopyToAsync(buffer);
                        buffer.Position = 0L;
                        context.Request.Body = buffer;
                        await next(context);
                    }
                }

                finally
                {
                    context.Request.Body = stream;
                }
            });


            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(env.ContentRootPath, "ui")),
                RequestPath = "/ui"
            });

            app.UseDefaultFiles();

           // app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                
            });

            //System.Data.Common.DbProviderFactories.RegisterFactory("Npgsql", Npgsql.NpgsqlFactory.Instance);
           // System.Data.Common.DbProviderFactories.RegisterFactory("SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
        }
    }
}
