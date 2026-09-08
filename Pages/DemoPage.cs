using OpenQA.Selenium;

namespace Notify_Demo.Pages
{
    public class DemoPage
    {
        private readonly IWebDriver _driver;
        private readonly string _url = "https://seleniumbase.io/demo_page";

        public DemoPage(IWebDriver driver) => _driver = driver;

        public void Navigate() => _driver.Navigate().GoToUrl(_url);

        private IWebElement NameField => _driver.FindElement(By.Id("myTextInput"));
        private IWebElement SubmitButton => _driver.FindElement(By.Id("myButton"));
        private IWebElement Output => _driver.FindElement(By.Id("myTextarea"));

        public void EnterName(string name)
        {
            NameField.Clear();
            NameField.SendKeys(name);
        }

        public void ClickSubmit() => SubmitButton.Click();

        public bool IsNameAsExpected(string expectedName)
        {
            try
            {
                var input = _driver.FindElement(By.XPath(" //tbody[contains(@id,'tbodyId')]/tr[2]/td[2]/input[contains(@id,'myTextInput')]"));
                var value = input.GetAttribute("value") ?? string.Empty;
                return value.Contains(expectedName);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
