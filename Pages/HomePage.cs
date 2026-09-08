using OpenQA.Selenium;
using Notify_Demo.Helpers;

namespace Notify_Demo.Pages
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        private readonly By _welcomeBanner = By.Id("welcome"); // adjust to your app

        public HomePage(IWebDriver driver)
        {
            _driver = driver;
        }

        public bool IsLoggedIn()
        {
            try
            {
                var el = WaitHelpers.WaitUntilVisible(_driver, _welcomeBanner, 5);
                return el != null && el.Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}