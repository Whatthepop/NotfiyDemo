using NUnit.Framework;
using OpenQA.Selenium;
using Notify_Demo.Drivers;

namespace Notify_Demo.Base
{
    public abstract class TestBase
    {
        protected IWebDriver Driver { get; private set; } = null!;

        [SetUp]
        public virtual void SetUp()
        {
            Driver = WebDriverFactory.Create();
            Driver.Manage().Window.Maximize();
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (Driver != null)
            {
                try
                {
                    Driver.Quit();
                }
                catch
                {
                    // ignore
                }

                try
                {
                    Driver.Dispose();
                }
                catch
                {
                    // ignore
                }
            }
        }
    }
}