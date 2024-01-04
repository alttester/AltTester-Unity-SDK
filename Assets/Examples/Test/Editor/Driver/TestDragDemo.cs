using NUnit.Framework;
using AltTester.AltTesterUnitySDK.Driver;

public class TestDragDemo : TestBase
{
    public TestDragDemo()
    {
        sceneName = "DragDemo";
    }


    [Test]
    public void TestMoveLettersWithSwipe()
    {
        var holderObject = altDriver.FindObject(By.NAME, "Holder Object (0)");
        var dragObject = altDriver.FindObject(By.NAME, "0-Object");
        var initialPosition = dragObject.GetScreenPosition();
        altDriver.Swipe(dragObject.GetScreenPosition(), holderObject.GetScreenPosition(), 2f);
        var finalPosition = altDriver.FindObject(By.NAME, "0-Object").GetScreenPosition();
        Assert.That(!initialPosition.Equals(finalPosition));

    }
    [Test]
    public void TestMoveLettersWithKeyDownUp()
    {
        var holderObject = altDriver.FindObject(By.NAME, "Holder Object (0)");
        var dragObject = altDriver.FindObject(By.NAME, "0-Object");
        var initialPosition = dragObject.GetScreenPosition();
        altDriver.MoveMouse(initialPosition, 0.3f);
        altDriver.KeyDown(AltKeyCode.Mouse0);
        altDriver.MoveMouse(holderObject.GetScreenPosition(), 0.3f);
        altDriver.KeyUp(AltKeyCode.Mouse0);
        var finalPosition = altDriver.FindObject(By.NAME, "0-Object").GetScreenPosition();
        Assert.That(!initialPosition.Equals(finalPosition));
    }
    [Test]
    public void TestMoveLettersWithTouch()
    {
        var holderObject = altDriver.FindObject(By.NAME, "Holder Object (0)");
        var dragObject = altDriver.FindObject(By.NAME, "0-Object");
        var initialPosition = dragObject.GetScreenPosition();
        var touch = altDriver.BeginTouch(initialPosition);
        altDriver.MoveTouch(touch, holderObject.GetScreenPosition());
        altDriver.EndTouch(touch);
        var finalPosition = altDriver.FindObject(By.NAME, "0-Object").GetScreenPosition();
        Assert.That(!initialPosition.Equals(finalPosition));
    }

}