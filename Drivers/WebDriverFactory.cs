using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace Notify_Demo.Drivers
{
    public static class WebDriverFactory
    {
        /// <summary>
        /// Create an IWebDriver instance. Default is Chrome.
        /// Set browser to "firefox" to create FirefoxDriver.
        /// Honor HEADLESS env var if set to "true".
        /// When running non-headless (visible) Chrome, use a persistent profile directory
        /// so the test runs in a local browser instance instead of creating a new temporary profile.
        /// </summary>
        public static IWebDriver Create(string browser = "chrome")
        {
            var headless = string.Equals(Environment.GetEnvironmentVariable("HEADLESS"), "true", StringComparison.OrdinalIgnoreCase);

            if (string.Equals(browser, "firefox", StringComparison.OrdinalIgnoreCase))
            {
                var options = new FirefoxOptions();
                if (headless) options.AddArgument("--headless");
                var driver = new FirefoxDriver(options);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                return driver;
            }

            var chromeOptions = new ChromeOptions();
            // Helpful defaults
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-extensions");
            chromeOptions.AddArgument("--disable-infobars");
            chromeOptions.AddArgument("--no-default-browser-check");
            chromeOptions.AddArgument("--no-first-run");

            if (headless)
            {
                chromeOptions.AddArgument("--headless=new");
                chromeOptions.AddArgument("--window-size=1920,1080");
            }
            else
            {
                // Use a persistent profile directory for local visible runs so ChromeDriver
                // launches with a stable user profile instead of creating a temporary one.
                // This results in a true local/browser instance and avoids many transient-file conflicts.
                var profileDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "NotifyDemo",
                    "selenium-profile");

                try
                {
                    Directory.CreateDirectory(profileDir);
                    chromeOptions.AddArgument($"--user-data-dir={profileDir}");
                    // Optionally choose a profile subdirectory:
                    // chromeOptions.AddArgument("--profile-directory=Default");
                }
                catch
                {
                    // If creating profile dir fails, fall back to default behavior.
                }

                chromeOptions.AddArgument("--start-maximized");
            }

            var chromeDriver = new ChromeDriver(chromeOptions);
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            return chromeDriver;
        }
    }
}