using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json.Linq;

namespace WebApi.Controllers
{
    public class HomeController : Controller
    {
        string path = Path.GetPathRoot("WebApi/App_Data/conversation.json");
        
        public string converPath = "E:/Project4_2019-2020/Server/Webhook/Pass/WebApi/App_Data/conversation.json";
        public string replyPath = "E:/Project4_2019-2020/Server/Webhook/Pass/WebApi/App_Data/reply.json";

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            Dictionary<string, string> dictConver = ReadJsonFile(converPath);
            List<string> keys = new List<string>(dictConver.Keys);
            List<string> values = new List<string>(dictConver.Values);
            ViewBag.Keys = keys;
            ViewBag.Values = values;
            return View();
        }

        [HttpPost]
        public ActionResult Index(string key, string value)
        {
            ViewBag.Title = "Home Page";
            
            Dictionary<string, string> dataAdd = ReadJsonFile(converPath);
            dataAdd.Add(key, value);
            SaveNewKeyValue(converPath, dataAdd);
            ViewBag.Status = "Save success: {'" + key + "': '"+ value + "'}";
            List<string> keys = new List<string>(dataAdd.Keys);
            List<string> values = new List<string>(dataAdd.Values);
            ViewBag.Keys = keys;
            ViewBag.Values = values;
            return View();
        }

        public ActionResult Reply()
        {
            ViewBag.Title = "Reply Page";
            Dictionary<string, string> dictConver = ReadJsonFile(replyPath);
            List<string> keys = new List<string>(dictConver.Keys);
            List<string> values = new List<string>(dictConver.Values);
            ViewBag.Keys = keys;
            ViewBag.Values = values;
            return View();
        }

        [HttpPost]
        public ActionResult Reply(string key, string value)
        {
            ViewBag.Title = "Home Page";

            Dictionary<string, string> dataAdd = ReadJsonFile(replyPath);
            dataAdd.Add(key, value);
            SaveNewKeyValue(replyPath, dataAdd);
            ViewBag.Status = "Save success: {'" + key + "': '" + value + "'}";
            List<string> keys = new List<string>(dataAdd.Keys);
            List<string> values = new List<string>(dataAdd.Values);
            ViewBag.Keys = keys;
            ViewBag.Values = values;
            return View();
        }

        private Dictionary<string, string> ReadJsonFile(string path)
        {
            string json = System.IO.File.ReadAllText(@path);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return dict;
        }

        public void SaveNewKeyValue(string path, Dictionary<string, string> dict)
        {
            string json = JsonConvert.SerializeObject(dict);
            System.IO.File.WriteAllText(@path, json);
        }
    }
}
