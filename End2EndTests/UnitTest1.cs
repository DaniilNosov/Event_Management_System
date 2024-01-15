using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;


namespace End2EndTests;

[TestClass]
public class UnitTest1
{
    private IWebDriver _driver;
    private string _websiteUrl = "https://localhost:44382";

    [TestInitialize]
    public void SetUp()
    {
        _driver = new EdgeDriver();
    }

    [TestMethod]
    public void TestRegistration()
    {
        _driver.Navigate().GoToUrl($"{_websiteUrl}/Identity/Account/Register");

        _driver.FindElement(By.Id("Input_FirstName")).SendKeys($"{Guid.NewGuid()}");
        _driver.FindElement(By.Id("Input_LastName")).SendKeys($"{Guid.NewGuid()}");
        _driver.FindElement(By.Id("Input_Email")).SendKeys($"{Guid.NewGuid()}@gmail.com");
        _driver.FindElement(By.Id("Input_PhoneNumber")).SendKeys("53245234");
        _driver.FindElement(By.Id("Input_Password")).SendKeys("Zxcvb1!");
        _driver.FindElement(By.Id("Input_ConfirmPassword")).SendKeys("Zxcvb1!");

        _driver.FindElement(By.Id("registerSubmit")).Click();

        var successMessageElement = _driver.FindElement(By.Id("EventTable"));
        Assert.IsNotNull(successMessageElement);
    }
    [TestMethod]
    public void TestLogin()
    {
        _driver.Navigate().GoToUrl($"{_websiteUrl}/Identity/Account/Login");

        _driver.FindElement(By.Id("Input_Email")).SendKeys($"John@gmail.com");
        _driver.FindElement(By.Id("Input_Password")).SendKeys("Zxcvb1!");

        _driver.FindElement(By.Id("login-submit")).Click();

        var successMessageElement = _driver.FindElement(By.Id("EventTable"));
        Assert.IsNotNull(successMessageElement);
    }
    [TestMethod]
    public void TestEventCreation()
    {
        _driver.Navigate().GoToUrl($"{_websiteUrl}/Identity/Account/Login");

        _driver.FindElement(By.Id("Input_Email")).SendKeys("John@gmail.com");
        _driver.FindElement(By.Id("Input_Password")).SendKeys("Zxcvb1!");
        _driver.FindElement(By.Id("login-submit")).Click();

        _driver.Navigate().GoToUrl($"{_websiteUrl}/Event/Create");

        _driver.FindElement(By.Id("Title")).SendKeys($"{Guid.NewGuid()}");
        _driver.FindElement(By.Id("Description")).SendKeys("This is a test event description.");
        _driver.FindElement(By.Id("Location")).SendKeys("Test Location");

        var dateInput = _driver.FindElement(By.Id("Date"));
        var script = $"arguments[0].value = '{DateTime.Now.AddDays(7).ToString("yyyy-MM-dd")}';";
        ((IJavaScriptExecutor)_driver).ExecuteScript(script, dateInput);

        _driver.FindElement(By.Id("Time")).SendKeys("14:00");

        var categoryDropdown = new SelectElement(_driver.FindElement(By.Id("CategoryId")));
        categoryDropdown.SelectByText("Good");

        _driver.FindElement(By.Id("CreateButton")).Click();

        var successMessageElement = _driver.FindElement(By.Id("EventTable"));
        Assert.IsNotNull(successMessageElement);
    }


    [TestCleanup]
    public void TearDown()
    {
        _driver.Quit();
    }
}