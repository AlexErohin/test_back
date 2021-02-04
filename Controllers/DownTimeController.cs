using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using IdleMonitor.Infrastructure;
using System.Data;
using IdleMonitor.Models;

using System.Linq;
using NReco.Data;
using System.Net.Mime;
using System.Web;
using System.Text.RegularExpressions;

namespace IdleMonitor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DowntimeController : ControllerBase
    {
        private int user_id = 0;
        private DB db;

        public DowntimeController(IConfiguration configuration)
        {
            db = new DB(configuration, user_id);
        }



        Dictionary<string,string> ParseRequest(string request)
        {

            Dictionary<string, string> dict = new Dictionary<string, string>();

            string[] arr=request.Split(new char[] { '[', '=',']' });

            dict["table"] = arr[0];
            dict["code"] = arr.Length>1 && arr[1]=="code" ? arr[2] : "";
            dict["filter"] = arr.Length > 1 && arr[1] == "filter" ? arr[2] : "";

            return dict;


        }

        [HttpGet("{request}")]
        public IActionResult Get(string request)
        {
            try
            {

                var props = ParseRequest(request);

                var json = "";

                if (props["code"] != "")
                    json = db.GetDataByCode(props["table"], props["code"]);
                else if (props["filter"] != "")
                    json = db.GetDataByFilter(props["table"], props["filter"]);
                else
                    json = db.GetData(props["table"]);

                return new ContentResult() { ContentType = "application/json", Content = json, StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ContentResult() { ContentType = "text/json", Content = ex.Message + "   " + ex.StackTrace, StatusCode = 500 };
            }
        }

        [HttpPost("{table}")]
        public IActionResult Set(string table, [FromBody] List<Dictionary<string, object>> values)
        {

            string id = db.SetData(table,values[0]);

            return new ContentResult() { ContentType = "application/json", Content = "[{\"id\" :\"" + id + "\"}]", StatusCode = 200 };
        }

    }
}
