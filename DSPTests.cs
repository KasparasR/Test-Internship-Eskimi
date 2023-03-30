using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace Eskimi_InternshipTask
{
    public class DSPTests
    {
        public IWebDriver Login()
        {
            var driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://dsp.eskimi.com");
            driver.FindElement(By.Id("username")).SendKeys("kasparas.ruseckas@gmail.com");
            driver.FindElement(By.Id("password")).SendKeys("123456789101112");
            driver.FindElement(By.ClassName("login")).Click();
            return driver;
        }

        // The method below can be used in case an engineer wants to log in with his own credentials.

        //public IWebDriver Login(string userEmail, string userPassword) 
        //{
        //    var driver = new ChromeDriver();
        //    driver.Navigate().GoToUrl("https://dsp.eskimi.com");
        //    driver.FindElement(By.Id("username")).SendKeys(userEmail);
        //    driver.FindElement(By.Id("password")).SendKeys(userPassword);
        //    driver.FindElement(By.ClassName("login")).Click();
        //    return driver;
        //}

        [Fact]
        public void UpdatePersonalInfoScenario1Part1()
        {
            var driver = Login();
            driver.FindElement(By.Id("navSettingsMenu")).Click();
            driver.FindElement(By.XPath("//a[contains(text(), 'Personal info')]")).Click();
            driver.FindElement(By.XPath("//input[@name = 'contact_person_full_name']")).Clear();
            driver.FindElement(By.XPath("//input[@name = 'contact_person_full_name']")).SendKeys("Vardenis Pavardenis");
            driver.FindElement(By.XPath("//input[@name = 'contact_person_email']")).Clear();
            driver.FindElement(By.XPath("//input[@name = 'contact_person_email']")).SendKeys("vardenis1@randomemail.com");
            driver.FindElement(By.XPath("//input[@name = 'contact_person_phone']")).Clear();
            driver.FindElement(By.XPath("//input[@name = 'contact_person_phone']")).SendKeys("+37012345678");
            driver.FindElement(By.XPath("//button[@type = 'submit']")).Click();
            string typedName = driver.FindElement(By.XPath("//input[@name = 'contact_person_full_name']")).GetAttribute("value");
            string typedEmail = driver.FindElement(By.XPath("//input[@name = 'contact_person_email']")).GetAttribute("value");
            string typedPhone = driver.FindElement(By.XPath("//input[@name = 'contact_person_phone']")).GetAttribute("value");
            Assert.Equal("Vardenis Pavardenis", typedName);
            Assert.Equal("vardenis1@randomemail.com", typedEmail);
            Assert.Equal("+37012345678", typedPhone);
            driver.Quit();
        }

        [Fact]
        public void UpdateBillingInfoScenario1Part2()
        {
            var driver = Login();
            driver.FindElement(By.Id("navSettingsMenu")).Click();
            driver.FindElement(By.XPath("//a[contains(text(), 'Personal info')]")).Click();
            driver.FindElement(By.ClassName("company[title]")).Clear();
            driver.FindElement(By.ClassName("company[title]")).SendKeys("UAB Bandymas");
            driver.FindElement(By.ClassName("company[address]")).Clear();
            driver.FindElement(By.ClassName("company[address]")).SendKeys("Testo g.11");
            string companyName = driver.FindElement(By.XPath("//input[@name = 'company[title]']")).GetAttribute("value");
            Assert.Equal("UAB Bandymas", companyName);
            driver.Quit();
        }

        [Fact]
        public void UpdatePasswordToBeWeakThenToStrongScenario2()
        {
            var driver = Login();
            driver.FindElement(By.Id("navSettingsMenu")).Click();
            driver.FindElement(By.XPath("//a[contains(text(), 'Personal info')]")).Click();
            driver.FindElement(By.Id("yourPassword")).SendKeys("123456789101112");
            driver.FindElement(By.Id("password")).SendKeys("123456");
            driver.FindElement(By.XPath("(//INPUT[@type='password'])[3]")).SendKeys("123456");
            driver.FindElement(By.XPath("//button[@type = 'submit']")).Click();
            bool savedSuccessfully = driver.PageSource.Contains("User saved successfully");
            Assert.True(savedSuccessfully);
            driver.FindElement(By.Id("yourPassword")).SendKeys("123456");
            driver.FindElement(By.Id("password")).SendKeys("123456789101112");
            driver.FindElement(By.XPath("(//INPUT[@type='password'])[3]")).SendKeys("123456789101112");
            driver.FindElement(By.XPath("//button[@type = 'submit']")).Click();
            Assert.True(savedSuccessfully);
            driver.Quit();
        }

        [Fact]
        public void NewCampaingGroupAndCheckIfItWasCreatedinDraftStateScenario3()
        {
            var driver = Login();
            driver.Navigate().GoToUrl("https://dsp.eskimi.com/admin?function=dashboard");
            driver.FindElement(By.XPath("//i[@class = 'fas fa-bullhorn']")).Click();
            //UI Needs to completely load before clicking a on "campaign" sign.
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("(//I[@class='fas fa-bullhorn'])[2]")).Click();
            driver.FindElement(By.ClassName("fa-plus-circle")).Click();
            SelectElement dropDown = new SelectElement(driver.FindElement(By.Id("js-campaign-type")));
            dropDown.SelectByValue("1");
            driver.FindElement(By.Id("name")).SendKeys("TestCampaing1");
            driver.FindElement(By.XPath("//button[@class = 'btn btn-info btn-block']")).Click();
            bool correctName = driver.PageSource.Contains("TestCampaing1");
            Assert.True(correctName);
            bool savedState = driver.PageSource.Contains("Dra");
            Assert.True(savedState);
            driver.Quit();
        }
    }
}