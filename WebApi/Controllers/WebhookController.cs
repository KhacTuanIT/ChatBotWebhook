using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using WebApi.Models;
using ZaloDotNetSDK;


namespace WebApi.Controllers
{
    public class WebhookController : ApiController
    {
        static string exeFile = (new System.Uri(Assembly.GetEntryAssembly().CodeBase)).AbsolutePath;
        static string exeDir = Path.GetDirectoryName(exeFile);

        public string relPath = "..\\WebApi\\App_Data\\";
        public string access_token = "jW4l5ouYJdom22eNBojAVBODAn0eCGmmrKyDEKmeS5QWO7T3MWH4TvXlH7jpE410iMHbIsKbKroC1bLISqPfH90HJ7X8KZ56XGetJ4T192E_4X43Ut4k6BmyCW5BVmTjbLa0RLqAEbcBIJf24saYUDOT1beqGIuww3L6DdLWOLxjEcjh0KbzRC0pMGCZKaOluJT793aGVYNVU6y_Cbj91VSJAZmyHn49_cSLA3OS2Xt9Q0uiAoyrFD9NBIKuA0eIncfZ8oKLKJVETc0-K4wk4b1OAprDUW";
        //public string converPath = "E:/Project4_2019-2020/Server/Webhook/Pass/WebApi/App_Data/conversation.json";
        //public string replyPath = "E:/Project4_2019-2020/Server/Webhook/Pass/WebApi/App_Data/reply.json";

        public string converPath = Path.Combine(exeDir, "..\\..\\App_Data\\conversation.json");
        public string replyPath = Path.Combine(exeDir, "..\\..\\App_Data\\reply.json");

        [HttpPost]
        public IHttpActionResult SendMessage(Message message)
        {
            JObject result = null;
            string userId = null;
            string messageSend = "";
            if (message.Sender != null)
            {
                if (message.event_name.Equals("user_send_text"))
                {
                    userId = message.Sender.Id;
                    string key = "";
                    string messageText = "";
                    if (userId != null)
                    {
                        messageText = message.message.Text;
                        key = GetJsonFile(converPath, messageText);
                        if (key == "" || key == null)
                        {
                            key = "error";
                        }
                        messageSend = GetJsonFile(replyPath, key);
                    }
                    if (!"".Equals(messageSend))
                    {
                        result = Send(userId, messageSend);
                        if ("error".Equals(key))
                        {
                            Dictionary<string, string> dict = ReadTextJson(converPath);
                            List<string> keys = GetAllKeyDictionary(dict);
                            string sameKey = GetSameKey(keys, messageText);
                            string value = GetValueInDictionary(dict, sameKey);
                            AddNewKeyValue(dict, messageText, value);
                            SaveNewKeyValue(converPath, dict);
                        }
                    }
                }
            }
            
            return Ok(result);
        }

        public Dictionary<string, string> ReadTextJson(string path)
        {
            string json = File.ReadAllText(@path);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return dict;
        }

        public string GetValueInDictionary(Dictionary<string, string> dict, string key)
        {
            return dict[key];
        }

        public List<string> GetAllKeyDictionary(Dictionary<string, string> dict)
        {
            return new List<string>(dict.Keys);
        }

        public string GetSameKey(List<string> keys, string key)
        {
            int error = (int) Math.Round(key.Length * 0.3);
            string sameKeyReturn = "";
            foreach (string k in keys)
            {
                if (CompareTwoString(key, k, error))
                {
                    sameKeyReturn = k;
                    break;
                }
            }
            return sameKeyReturn;
        }

        public bool CompareTwoString(string key, string sameKey, int error)
        {
            int lenKey = key.Length;
            int lenSameKey = sameKey.Length;
            key = key.ToLower();
            sameKey = sameKey.ToLower();

            if (lenSameKey < (lenKey - error) || lenSameKey > (lenKey + error))
            {
                return false;
            }
            int i = 0, j = 0, k;
            int errorLan = 0;
            while (i < lenKey && j < lenSameKey)
            {
                if (key[i] != sameKey[j])
                {
                    errorLan++;
                    for (k = 1; k <= errorLan; k++)
                    {
                        if ((i + k < lenKey) && key[i+k] == sameKey[j])
                        {
                            i += k;
                            break;
                        }
                        else if ((j + k < lenSameKey) && key[i] == sameKey[j + k])
                        {
                            j += k;
                            break;
                        }
                    }
                }
                i++;
                j++;
            }
            errorLan += lenKey - i + lenSameKey - j;
            if (errorLan <= error) return true;
            return false;
        }

        public void AddNewKeyValue(Dictionary<string, string> dict, string key, string value)
        {
            dict.Add(key, value);
        }

        public void SaveNewKeyValue(string path, Dictionary<string, string> dict)
        {
            string json = JsonConvert.SerializeObject(dict);
            File.WriteAllText(@path, json);
        }

        public string GetJsonFile(string path, string key)
        {
            JObject obj = new JObject();
            using (StreamReader file = File.OpenText(@path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                obj = (JObject)JToken.ReadFrom(reader);
            }
            return (string) obj[key];
        }

        public JObject Send(string userId, string messageSend)
        {
            ZaloClient client = new ZaloClient(access_token);
            JObject result = client.sendTextMessageToUserId(userId, messageSend);
            return result;
        }
    }
}
