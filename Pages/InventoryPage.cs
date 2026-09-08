using OpenQA.Selenium;
using Notify_Demo.Helpers;

namespace Notify_Demo.Pages
{
    public class InventoryPage
    {
        private readonly IWebDriver _driver;
        private readonly By _inventoryContainer = By.Id("inventory_container");

        public InventoryPage(IWebDriver driver) => _driver = driver;

        public bool IsDisplayed() => WaitHelpers.WaitUntilVisible(_driver, _inventoryContainer, 5) != null;
    }
}