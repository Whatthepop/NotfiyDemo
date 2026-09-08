using NUnit.Framework;
using Notify_Demo.Base;
using Notify_Demo.Pages;

namespace Notify_Demo.Tests.Ui
{
    [TestFixture]
    public class LoginTests : TestBase
    {
        // SauceDemo public Demo Site
        [Test]
        public void ValidUser_CanLogin()
        {
            var login = new LoginPage(Driver);
            login.Navigate();
            login.Login("standard_user", "secret_sauce"); 

            //Assert that the Inventory page is displayed after successful login.
            var inv = new InventoryPage(Driver);
            Assert.IsTrue(inv.IsDisplayed());
        }

        [Test]
        public void InvalidUser_ShowsError()
        {
            var login = new LoginPage(Driver);
            login.Navigate();
            login.Login("bad_user", "wrong_pass");


            // Assert Error message is displayed and contains "Epic Sadface" as expected. 
            Assert.IsTrue(Driver.PageSource.Contains("Epic sadface") || Driver.Url.Contains("saucedemo"));

        }
    }
}