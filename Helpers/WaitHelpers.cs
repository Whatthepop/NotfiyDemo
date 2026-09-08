using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Notify_Demo.Helpers
{
    public static class WaitHelpers
    {
        public static IWebElement WaitUntilVisible(IWebDriver driver, By by, int seconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            return wait.Until(d =>
            {
                try
                {
                    var el = d.FindElement(by);
                    return el.Displayed ? el : null;
                }
                catch
                {
                    return null;
                }
            })!;
        }

        public static IWebElement WaitUntilClickable(IWebDriver driver, By by, int seconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            return wait.Until(d =>
            {
                try
                {
                    var el = d.FindElement(by);
                    return el.Displayed && el.Enabled ? el : null;
                }
                catch
                {
                    return null;
                }
            })!;
        }

        public static void WaitForPageToLoad(IWebDriver driver, int seconds = 10)
        {
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(seconds);
        }
    }
}