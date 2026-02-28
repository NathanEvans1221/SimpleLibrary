using Autofac;
using Newtonsoft.Json;
using SimpleLibrary.Logger;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace SimpleLibrary.Line
{
    public class Line : PrintLogger
    {
        private string _Url     = "";
        private string _UserId  = "";        
        private string _ApiKey  = "";

        public Line(string url, string userId, string apiKey, ContainerBuilder builder = null)
        {
            _Url    = url;
            _UserId = userId;            
            _ApiKey = apiKey;

            InitLogger(builder);
        }

        /// <summary>
        /// 💬 傳送 Line Notify 的通知訊息
        /// </summary>
        /// <param name="message">📝 欲傳送的訊息內容字串</param>
        public void Notify(string message)
        {
            string word_ = message;

            //JSON
            var msg_ = new
            {
                to = _UserId,
                messages = new[] {
                    new {
                        type = "text",
                        text = word_
                    }
                }
            };

            //POST
            string msgStr_ = JsonConvert.SerializeObject(msg_);
            Uri myUri_ = new Uri(_Url);
            var data_ = Encoding.UTF8.GetBytes(msgStr_);
            SendRequest(myUri_, data_, "application/json", "POST", _ApiKey);
        }

        /// <summary>
        /// 🌐 透過 HTTP 請求的方式將資訊傳送給伺服器
        /// </summary>
        /// <param name="uri">🔗 目標 API 的 Uri 網址</param>
        /// <param name="jsonDataBytes">📦 要傳送的 JSON 轉換成的位元組陣列</param>
        /// <param name="contentType">🏷️ 傳送內容的格式類型 (例如 application/json)</param>
        /// <param name="method">📤 請求的方法 (例如 POST)</param>
        /// <param name="authorization">🔑 API 操作需要的授權碼 (Bearer Token)</param>
        private static void SendRequest(Uri uri, byte[] jsonDataBytes, string contentType, string method, string authorization)
        {
            WebRequest req_ = WebRequest.Create(uri);
            {
                req_.ContentType = contentType;
                req_.Method = method;
                req_.ContentLength = jsonDataBytes.Length;
                req_.Headers.Add("Authorization", $"Bearer {authorization}");

                var stream = req_.GetRequestStream();
                stream.Write(jsonDataBytes, 0, jsonDataBytes.Length);
                stream.Close();

                WebResponse response = req_.GetResponse();
                {
                    stream = response.GetResponseStream();
                    var reader = new StreamReader(stream);
                    reader.ReadToEnd();
                }
            }
        }

    }
}
