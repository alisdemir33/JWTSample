using System;
using System.Net;
using System.Net.Mail;

namespace VakifIlan
{
    public class Mail
    {        
        private bool disposed = false;

        public Mail()
        {

        }

        public string SendMail1(string strFrom, string strTo, string strSubject, string strMessage,
                                string sHost, string sUserName, string sPassword, int iPort)
        {
            string strSonuc = "OK";
            OpenSmtp.Mail.MailMessage clsMail = new OpenSmtp.Mail.MailMessage();
            OpenSmtp.Mail.Smtp clsSmtp = new OpenSmtp.Mail.Smtp();

            clsSmtp.Host = sHost;
            clsSmtp.Username = sUserName;
            clsSmtp.Password = sPassword;
            clsSmtp.SendTimeout = 10000;
            clsSmtp.Port = iPort;

            OpenSmtp.Mail.MailMessage clsMailMessage = new OpenSmtp.Mail.MailMessage();
            clsMailMessage.HtmlBody = strMessage;
            clsMailMessage.Subject = strSubject;
            clsMailMessage.Charset = "windows-1254";
            clsMailMessage.Priority = MailPriority.High.ToString();

            try
            {
                OpenSmtp.Mail.EmailAddress Gonderen = new OpenSmtp.Mail.EmailAddress(strFrom);
                clsMailMessage.From = Gonderen;

                OpenSmtp.Mail.EmailAddress Too = new OpenSmtp.Mail.EmailAddress(strTo);
                clsMailMessage.To.Add(Too);

                clsSmtp.SendMail(clsMailMessage);
            }
            catch (OpenSmtp.Mail.SmtpException ex)
            {
                if (ex != null)
                {
                    if (ex.Message != null)
                    {
                        strSonuc = "E-Posta gönderilemedi. Sebebi:" + ex.ToString();

                        //if (ex.Message.Length > 75)
                        //{
                        //    strSonuc = "E-Posta gönderilemedi. Sebebi:" + ex.Message.ToString().Substring(0, 75);
                        //}
                        //else
                        //{
                        //    strSonuc = "E-Posta gönderilemedi. Sebebi:" + ex.Message;
                        //}
                    }
                }
                else
                {
                    strSonuc = "E-Posta gönderilemedi. Bilinmeyen Hata";
                }
            }

            return strSonuc;
        }


        //public string SendMail2(string strFrom, string strTo, string strSubject, string strMessage,
        //                        string sHost, string sUserName, string sPassword, int iPort)
        //{
        //    string sResult = "OK";
        //    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(strFrom, strTo, strSubject, strMessage);
        //    mail.IsBodyHtml = true;
        //    mail.Priority = MailPriority.Normal;
        //    System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(sHost, iPort);           
        //    client.Credentials = new System.Net.NetworkCredential(sUserName, sPassword);
        //    //client.EnableSsl = true;

        //    try
        //    {
        //        client.Send(mail);
        //    }
        //    catch (Exception ex)
        //    {
        //        sResult = "E-Posta gönderilemedi. Sebebi:" + ex.Message;
        //    }

        //    return sResult;
        //}

        public string SendMail(string strFrom, string strTo, string strSubject, string strMessage,
                                string sHost, string sUserName, string sPassword, int iPort)
        {
            string sResult = "OK";
            MailMessage myMessage = new MailMessage(new MailAddress(strFrom), new MailAddress(strTo));
            myMessage.IsBodyHtml = true;
            myMessage.Priority = MailPriority.High;           
            myMessage.Body = strMessage;
            myMessage.Subject = strSubject;

            SmtpClient client = new SmtpClient(sHost, iPort);
            NetworkCredential cred = new NetworkCredential(sUserName, sPassword);
            client.UseDefaultCredentials = true;
            client.Credentials = cred;


            try
            {
                client.Send(myMessage);
            }
            catch (Exception Ex)
            {
                sResult = Ex.ToString();
            }

            return sResult;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {

                }
            }
            disposed = true;
        }

        ~Mail()
        {
            Dispose(false);
        }
    }
}