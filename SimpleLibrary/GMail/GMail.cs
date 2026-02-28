using Autofac;
using SimpleLibrary.Logger;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace SimpleLibrary.GMail
{
    public class GMail : PrintLogger
    {
        /// <summary>
        /// 📧 發信人的 Email 信箱地址
        /// </summary>
        private readonly string _EmailAddress = "";

        /// <summary>
        /// 🔑 發信人的 Email 密碼或是應用程式專用密碼
        /// </summary>
        private readonly string _EmailPassword = "";

        public GMail(string emailAddress, string emailPassword, ContainerBuilder builder = null)
        {
            _EmailAddress  = emailAddress;
            _EmailPassword = emailPassword;

            InitLogger(builder);
        }

        public void SendMessage(string displayName, string subject, string body, List<string> ToAdd)
        {
            MailMessage mailMessage_ = new MailMessage
            {
                // 🧑‍💻 前面為發信人的 Email 網址，後面則為顯示名稱
                From = new MailAddress(_EmailAddress, displayName)
            };

            // 📬 加入所有收信者的 Email
            for (int i = 0; i < ToAdd.Count; ++i)
            {
                mailMessage_.To.Add(ToAdd[i]);
            }

            if (mailMessage_.To.Count > 0)
            {

                // ⭐️ 設定信件的優先權
                mailMessage_.Priority = MailPriority.Normal;

                // 🏷️ 信件標題
                mailMessage_.Subject = subject;

                // 📝 信件內容
                mailMessage_.Body = body;

                // 🌐 設定內容是否支援使用 Html 標籤
                mailMessage_.IsBodyHtml = true;

                // 🚀 設定 Gmail 的 SMTP 伺服器 (這是 Google 提供的端點)
                SmtpClient smtpClient_ = new SmtpClient("smtp.gmail.com", 587)
                {
                    // 🔑 輸入您在 Gmail 的帳號與授權密碼
                    Credentials = new NetworkCredential(_EmailAddress, _EmailPassword),

                    // 🔒 開啟 SSL 加密連線
                    EnableSsl = true
                };

                // 📤 發送郵件
                smtpClient_.Send(mailMessage_);

                // 🧹 釋放掉宣告出來的 SmtpClient 資源
                smtpClient_ = null;
            }

            // 🧹 釋放掉宣告出來的 MailMessage 資源
            mailMessage_.Dispose();
        }
    }
}
