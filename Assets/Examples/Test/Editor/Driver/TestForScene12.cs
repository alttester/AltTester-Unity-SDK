using AltTester.AltTesterUnitySDK.Driver;
using NUnit.Framework;

public class TestForScene12 : TestBase
{   //Important! If your test file is inside a folder that contains an .asmdef file, please make sure that the assembly definition references NUnit.

    public TestForScene12()
    {
        sceneName = "Sceme 12 2D Objects";
    }

    [TestCase("Square")]
    [TestCase("Circle")]
    [TestCase("Triangle")]
    public void TestClickOnObjets(string name)
    {
        var altObject = altDriver.FindObject(By.NAME, name);
        altObject.Click();
        Assert.That($"Clicked on {name}".Equals(altDriver.FindObject(By.NAME, "Text").GetText()));
    }
    [TestCase("Square")]
    [TestCase("Circle")]
    [TestCase("Triangle")]
    public void TestTapOnObjets(string name)
    {
        var altObject = altDriver.FindObject(By.NAME, name);
        altObject.Tap();
        Assert.That($"Clicked on {name}".Equals(altDriver.FindObject(By.NAME, "Text").GetText()));
    }
    [TestCase("Square")]
    [TestCase("Circle")]
    [TestCase("Triangle")]
    public void TestTapOnCoordinates(string name)
    {
        var altObject = altDriver.FindObject(By.NAME, name);
        altDriver.Tap(altObject.GetScreenPosition());
        Assert.That($"Clicked on {name}".Equals(altDriver.FindObject(By.NAME, "Text").GetText()));
    }

    [TestCase("Square")]
    [TestCase("Circle")]
    [TestCase("Triangle")]
    public void TestClickOnCoordinates(string name)
    {
        var altObject = altDriver.FindObject(By.NAME, name);
        altDriver.Click(altObject.GetScreenPosition());
        Assert.That($"Clicked on {name}".Equals(altDriver.FindObject(By.NAME, "Text").GetText()));
    }

}