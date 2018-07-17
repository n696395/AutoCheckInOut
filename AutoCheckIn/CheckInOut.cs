using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AutoCheckIn
{
    public class CheckInOut
    {
        private IWebDriver _Driver;
        private string _URL = "";
        private string _Name = "";
        private checkstatus _Status;
        private TimeSpan _timeout = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Constructor
        /// </summary>
        public CheckInOut()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="WebDriver">Web Driver</param>
        /// <param name="URL">Web address</param>
        /// <param name="Name">your name</param>
        /// <param name="status">In or out</param>
        public CheckInOut(IWebDriver WebDriver, string URL,string Name, checkstatus status)
        {
            _Driver = WebDriver;
            _URL = URL;
            _Name = Name;
            _Status = status;
        }

        /// <summary>
        /// Start Check In/Out
        /// </summary>
        public void startcheckInOut()
        {
            try
            {
                _Driver.Navigate().GoToUrl(_URL);
                WebDriverWait wait = new WebDriverWait(_Driver, _timeout); //Wait page load complete

                IWebElement left_displayname = _Driver.FindElement(By.Name("left_displayname"));
                left_displayname.SendKeys(_Name);

                IWebElement left_inout = _Driver.FindElement(By.Name("left_inout"));
                if(_Status == checkstatus.IN)
                    left_inout.SendKeys("上班");
                else //checkstatus.Out
                    left_inout.SendKeys("下班");

                IWebElement submit_button = _Driver.FindElement(By.Name("submit_button"));
                //submit_button.Click();
            }
            catch(WebDriverTimeoutException toex)
            {
                Console.WriteLine("[TimeOut]"+toex.ToString());
                throw toex;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// Capture current web view
        /// </summary>
        public void TakingScreenShoot(string timing)
        {
            if(_Driver == null) return;
            Screenshot ss = ((ITakesScreenshot)_Driver)?.GetScreenshot();
            ss.SaveAsFile($"{Directory.GetCurrentDirectory()}\\ScreenShoot\\{DateTime.Now.ToString("yyyyMMddHHmmss")+ timing}.png", ScreenshotImageFormat.Png);
        }

        /// <summary>
        /// Start Check In/Out
        /// </summary>
        /// <param name="status"> Check Status</param>
        public void startcheck(checkstatus status)
        {
            _Status = status;
            startcheckInOut();
        }

        /// <summary>
        /// Check if already check In/Out
        /// </summary>
        /// <param name="status">In/Out</param>
        /// <returns>true mean already check in/out, false mean not.</returns>
        public bool IsChecked(string Name, checkstatus status)
        {
            try
            {
                _Driver.Navigate().GoToUrl(_URL);
                WebDriverWait wait = new WebDriverWait(_Driver, _timeout); //Wait page load complete

                IWebElement table = _Driver.FindElement(By.ClassName("right_main")).FindElement((By.ClassName("right_main_text")));
                IReadOnlyList<IWebElement> tableRow = table.FindElements(By.TagName("tr"));

                string ChtStatus = status == checkstatus.IN ? "上班" : "下班";

                var ss = from row in tableRow where row.Text.Contains(Name) && row.Text.Contains(ChtStatus) select row;
                if (ss?.Count() > 0)
                {
                    foreach (var row in tableRow)
                    {
                        if (row.Text.Contains(Name) && row.Text.Contains(ChtStatus))
                        {
                            Console.WriteLine($"Check In/Out OK, find=>[{row.Text}]");
                            return true;
                        }
                    }
                }
                Console.WriteLine($"Can not find Name={Name}, CheckIn/Out={status}");
                return false;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Close the Web Driver
        /// </summary>
        public void close()
        {
            _Driver?.Close();
            _Driver?.Dispose();
        }

        /// <summary>
        /// URL string
        /// </summary>
        public string URL
        {
            get
            {
                return _URL;
            }
            set
            {
                _URL = value;
            }
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        /// <summary>
        /// Check In or Out
        /// </summary>
        public checkstatus CheckStatus
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
            }
        }

        /// <summary>
        /// Page load time out, default is 10 sec.
        /// </summary>
        public TimeSpan Timeout
        {
            get
            {
                return _timeout;
            }
            set
            {
                _timeout = value;
            }
        }
    }

    /// <summary>
    /// Check Status
    /// </summary>
    public enum checkstatus:byte
    {
        IN=1,
        OUT=2
    }
}
