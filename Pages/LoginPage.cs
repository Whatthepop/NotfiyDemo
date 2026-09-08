using OpenQA.Selenium;
using Notify_Demo.Helpers;

namespace Notify_Demo.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly string _url = "https://www.saucedemo.com/";

        private readonly By _username = By.Id("user-name");
        private readonly By _password = By.Id("password");
        private readonly By _loginBtn = By.Id("login-button");

        public LoginPage(IWebDriver driver) => _driver = driver;

        public void Navigate() => _driver.Navigate().GoToUrl(_url);

        public void EnterUsername(string u) => WaitHelpers.WaitUntilVisible(_driver, _username).SendKeys(u);
        public void EnterPassword(string p) => WaitHelpers.WaitUntilVisible(_driver, _password).SendKeys(p);
        public void ClickLogin() => WaitHelpers.WaitUntilClickable(_driver, _loginBtn).Click();

        public void Login(string u, string p)
        {
            EnterUsername(u);
            EnterPassword(p);
            ClickLogin();
        }
    }
}