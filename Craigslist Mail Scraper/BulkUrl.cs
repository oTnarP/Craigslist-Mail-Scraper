using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Windows.Forms;

namespace Craigslist_Mail_Scraper
{
    class BulkUrl
    {
        public ListView listView { get; set; }
        public ListBox listCategories { get; set; }
        public TextBox txtLog { get; set; }

        int serial = 1;

        public void Scrape()
        {
            Label.CheckForIllegalCrossThreadCalls = false;
            Button.CheckForIllegalCrossThreadCalls = false;
            TextBox.CheckForIllegalCrossThreadCalls = false;
            ListBox.CheckForIllegalCrossThreadCalls = false;
            ListView.CheckForIllegalCrossThreadCalls = false;
            DataGridView.CheckForIllegalCrossThreadCalls = false;

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            //Hide Console
            service.HideCommandPromptWindow = true;
            var options = new ChromeOptions();
            options.AddArguments("--disable-notifications");
            //options.AddArguments("headless");
            options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:48.0) Gecko/20100101 Firefox/48.0");

            var driver = new ChromeDriver(service, options);

            for (int j = 1; j <= listCategories.Items.Count; j++)
            {
                //Go To Category Url
                driver.Navigate().GoToUrl(listCategories.Text);
                txtLog.Text = "Navigating to the category...";

                //Count all the Posts
                int count = driver.FindElements(By.XPath("//a[@class='result-title hdrlnk']")).Count;
                txtLog.Text = "Total Ads " + count;

                for (int i = 1; i <= count; i++)
                {
                    try
                    {
                        txtLog.Text = "Navigating to the Post...";
                        //Go To Post Urls
                        driver.FindElement(By.XPath("(//a[@class='result-title hdrlnk'])[" + i + "]")).Click();

                        string title = driver.FindElement(By.XPath("//span[@id='titletextonly']")).Text;
                        txtLog.Text = "Post Title is: " + title;
                        //Click on Reply Button
                        driver.FindElement(By.XPath("//button[contains(text(), 'reply')]")).Click();
                        Thread.Sleep(1000);

                        //Extract the mail
                        string mail = driver.FindElement(By.XPath("//a[@class='mailapp']")).Text;

                        txtLog.Text = "User Mail: " + mail;

                        ListViewItem item = new ListViewItem(serial.ToString());
                        item.SubItems.Add(driver.Url);
                        item.SubItems.Add(title);
                        item.SubItems.Add(mail);
                        listView.Items.Add(item);
                        serial++;
                    }
                    catch
                    {
                        //
                    }
                    txtLog.Text = "Changing Url...";
                    driver.Navigate().Back();
                }

                txtLog.Text = "Changing Category..."; 

                //change url
                if (listCategories.SelectedIndex < listCategories.Items.Count - 1)
                {
                    listCategories.SelectedIndex = listCategories.SelectedIndex + 1;
                }
                
            }

            driver.Quit();
        }
    }
}
