using NUnit.Framework;
using OpenQA.Selenium;
using Notify_Demo.Drivers;
using Notify_Demo.Pages;

namespace Notify_Demo.Tests.UI
{
    [TestFixture]
    public class DemoPageUiTests
    {
        private IWebDriver _driver = null!;
        private DemoPage _demoPage = null!;

        [SetUp]
        public void SetUp()
        {
            _driver = WebDriverFactory.Create("chrome");
            _demoPage = new DemoPage(_driver);
            _demoPage.Navigate();
        }

        [TearDown]
        public void TearDown()
        {
            if (_driver != null)
            {
                try { _driver.Quit(); } catch {  }
                try { _driver.Dispose(); } catch { }
                _driver = null!;
            }
        }

        [Test]
        public void SubmitName_DisplaysNameInOutput()
        {
            const string name = "Amy";
            _demoPage.EnterName(name);
            _demoPage.ClickSubmit();
            Assert.That(_demoPage.IsNameAsExpected(name));
        }
    }
}