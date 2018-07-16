using System;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System.Threading;
using System.Xml;

namespace AutoCheckIn
{
    class Program
    {
        static string configFileNamePath = "./config.xml";
        static Random rnd = new Random();
        static int DelaySecond = rnd.Next(1, 900); //1sec to 900 sec(15 min)

        public static void Main(string[] args)
        {
            try
            {
                //Read config
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(configFileNamePath);
                string Name = xmldoc.SelectSingleNode("/root/name").InnerText.Trim();
                string URL = xmldoc.SelectSingleNode("/root/url").InnerText.Trim();

                //create Driver
                IWebDriver driver = new PhantomJSDriver();
                checkstatus status = (checkstatus)Enum.Parse(typeof(checkstatus), args[0]);
               CheckInOut chk = new CheckInOut(driver, URL, Name, status);

                //Check before chcek In/Out
                bool before_result = chk.IsChecked(Name, status);
                if(before_result == true)//Already Check
                {
                    chk.TakingScreenShoot("BeforeCheckInOut");
                    Console.WriteLine($"Already checked, Name={Name}, Status={args[0]}, Time={DateTime.Now.ToString("yyyyMMddHHmmss")}");
                    chk?.close();
                    return;
                }

                //delay
                Thread.Sleep(DelaySecond * 1000);

                //start check
                chk.startcheckInOut();

                bool after_result = chk.IsChecked(Name, status);
                chk.TakingScreenShoot("AfterCheckInOut");
                if (after_result == false)
                    Console.WriteLine($"Check Failed, Name={Name}, Status={args[1]}, Time={DateTime.Now.ToString("yyyyMMddHHmmss")}");
                else
                    Console.WriteLine($"Check OK, Name={Name}, Status={args[1]}, Time={DateTime.Now.ToString("yyyyMMddHHmmss")}");

                chk.close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
