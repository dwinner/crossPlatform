using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Mac;
using OpenQA.Selenium.Appium.Windows;

namespace WidgetBoard.AutomationTests;

[SetUpFixture]
public class AppiumSetup
{
   private static AppiumDriver? _driver;

   public static AppiumDriver App => _driver
                                     ?? throw new NullReferenceException("AppiumDriver is null");

   [OneTimeSetUp]
   public void RunBeforeAnyTests()
   {
      AppiumServerHelper.StartAppiumLocalServer();
      _driver = CreateDriver();
   }

   private static AppiumDriver? CreateDriver()
   {
      var platformName = TestContext.Parameters["platformName"];

      switch (platformName)
      {
         case "Android":
            var androidOptions = new AppiumOptions
            {
               AutomationName = "UIAutomator2",
               PlatformName = "Android",
               App = TestContext.Parameters["app"]
            };
            androidOptions.AddAdditionalAppiumOption("avd","Phone Xh-DPI - API 36");

            return new AndroidDriver(androidOptions);

         case "iOS":
            var iOsOptions = new AppiumOptions
            {
               AutomationName = "XCUITest",
               PlatformName = platformName,
               PlatformVersion = TestContext.Parameters["platformVersion"] ?? "17.5",
               DeviceName = TestContext.Parameters["deviceName"] ?? "iPhone 15 Pro",
               App = TestContext.Parameters["app"]
            };

            return new IOSDriver(iOsOptions);

         case "Mac":
            var macOsOptions = new AppiumOptions
            {
               AutomationName = "mac2",
               PlatformName = platformName,
               App = TestContext.Parameters["app"]
            };

            return new MacDriver(macOsOptions);

         case "Windows":
            var windowsOptions = new AppiumOptions
            {
               AutomationName = "windows",
               PlatformName = "Windows",
               // The identifier of the deployed application to test
               App = "com.companyname.widgetboard_9zz4h110yvjzm!App"
            };

            return new WindowsDriver(windowsOptions);
      }

      return null;
   }

   [OneTimeTearDown]
   public void RunAfterAllTests()
   {
      if (_driver is not null)
      {
         _driver.Quit();
         _driver.Dispose();
      }

      // If an Appium server was started locally above, make sure we clean it up here
      AppiumServerHelper.DisposeAppiumLocalServer();
   }
}