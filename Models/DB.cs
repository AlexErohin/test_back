using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NReco.Data;
using System.Collections.Generic;
using IdleMonitor.Infrastructure;
using System.Data;
using Newtonsoft.Json.Linq;

using System.IO;

namespace IdleMonitor.Models
{
	public class DB
	{
	

		public DB(IConfiguration configuration, int user_id)
		{
			}


		public string GetData(string table)
        {
			if (!File.Exists("UserData/" + table + ".json"))
				return "";

			string ret=File.ReadAllText("UserData/" + table + ".json");
			return ret;
		}

		public string GetDataByCode(string table, string code)
		{
			string str = GetData(table);

			List<JObject> json = JSONSerializer.Deserialize<List<JObject>>(str);

			List<JObject> arr = json.FindAll(delegate (JObject b){return ((JValue)b["code"]).Value+"" == code;});

			if (arr.Count == 0)
				return "[]";

			return "["+arr[0].ToString()+"]";
		}

		public string GetDataByFilter(string table, string filter)
		{
			string str = GetData(table);
			string filterLower = filter.ToLower();

			List<JObject> json = JSONSerializer.Deserialize<List<JObject>>(str);

			List<JObject> arr = json.FindAll(delegate (JObject b) {

				foreach (var pair in b)
				{
					if ((pair.Value + "").ToLower().IndexOf(filterLower) > -1)
						return true;				}

				return false;
			});

			if (arr.Count == 0)
				return "[]";

			string result = JArray.FromObject(arr).ToString();

			return result;
		}


		public string SetData(string table, Dictionary<string, object> data)
		{


			if (!data.ContainsKey("code") ||  data["code"] +""== "")
			{
				return AddData(table, data);
			}

			return EditData(table, data);

		}

		string EditData(string table, Dictionary<string, object> data)
		{

			string code = data["code"]+"";

			string str = GetData(table);

			List<JObject> json = JSONSerializer.Deserialize<List<JObject>>(str);

			 int index = json.FindIndex(delegate (JObject b) { return ((JValue)b["code"]).Value + "" == code; });

			if (index==-1)
				return AddData(table, data);

			json[index] = JObject.FromObject(data);

			string result = JArray.FromObject(json).ToString();

			File.WriteAllText("UserData/" + table + ".json", result);

			return code;
		
		}

		 string AddData(string table, Dictionary<string, object> data)
		{

			string code = Guid.NewGuid().ToString();

			data["code"] = code;

			string str = GetData(table);

			if (str=="")
					str = "[]";

			List<JObject> json = JSONSerializer.Deserialize<List<JObject>>(str);

			JObject obj=JObject.FromObject(data);			

			json.Add(obj);

			string result = JArray.FromObject(json).ToString();

			File.WriteAllText("UserData/" + table + ".json", result);

			return code;

		}
	}
}
