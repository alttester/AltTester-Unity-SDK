using System.Threading;
using NUnit.Framework;
using Assets.AltUnityTester.AltUnityDriver.UnityStruct;
[Timeout(5000)]
public class TestForScene3DragAndDrop  {
    private AltUnityDriver altUnityDriver;
    [OneTimeSetUp]
    public void SetUp()
    {
        altUnityDriver = new AltUnityDriver();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        altUnityDriver.Stop();
    }
    [SetUp]
    public void LoadLevel()
    {
        altUnityDriver.LoadScene("Scene 3 Drag And Drop");
    }
    [Test]
    public void MultipleDragAndDrop()
    {
        var altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image1");
        var altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
        altUnityDriver.Swipe(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 1);

        altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image2");
        altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box2");
        altUnityDriver.Swipe(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 2);

        altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image3");
        altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
        altUnityDriver.Swipe(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 2);


        altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image1");
        altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
        altUnityDriver.Swipe(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 3);

        Thread.Sleep(4000);
        
        var imageSource = altUnityDriver.FindObject(By.NAME,"Drag Image1").GetComponentProperty("UnityEngine.UI.Image", "sprite");
        var imageSourceDropZone= altUnityDriver.FindObject(By.NAME,"Drop Image").GetComponentProperty("UnityEngine.UI.Image", "sprite");
        Assert.AreNotEqual(imageSource, imageSourceDropZone);

         imageSource = altUnityDriver.FindObject(By.NAME,"Drag Image2").GetComponentProperty("UnityEngine.UI.Image", "sprite");
         imageSourceDropZone = altUnityDriver.FindObject(By.NAME,"Drop").GetComponentProperty("UnityEngine.UI.Image", "sprite");
        Assert.AreNotEqual(imageSource, imageSourceDropZone);
       
    }
    [Test]
    public void MultipleDragAndDropWait()
    {
        var altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image1");
        var altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
        altUnityDriver.SwipeAndWait(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 1);

        altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image2");
        altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box2");
        altUnityDriver.SwipeAndWait(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 1);

        altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image3");
        altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
        altUnityDriver.SwipeAndWait(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 1);


        altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image1");
        altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
        altUnityDriver.SwipeAndWait(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 1);
        var imageSource = altUnityDriver.FindObject(By.NAME,"Drag Image1").GetComponentProperty("UnityEngine.UI.Image", "sprite");
        var imageSourceDropZone = altUnityDriver.FindObject(By.NAME,"Drop Image").GetComponentProperty("UnityEngine.UI.Image", "sprite");
        Assert.AreNotEqual(imageSource, imageSourceDropZone);

        imageSource = altUnityDriver.FindObject(By.NAME,"Drag Image2").GetComponentProperty("UnityEngine.UI.Image", "sprite");
        imageSourceDropZone = altUnityDriver.FindObject(By.NAME,"Drop").GetComponentProperty("UnityEngine.UI.Image", "sprite");
        Assert.AreNotEqual(imageSource, imageSourceDropZone);

    }

    [Test]
    public void TestPointerEnterAndExit()
    {
        var altElement = altUnityDriver.FindObject(By.NAME,"Drop Image");
        var color1 = altElement.GetComponentProperty("DropMe", "highlightColor");
        altUnityDriver.FindObject(By.NAME,"Drop Image").PointerEnterObject();
        var color2 = altElement.GetComponentProperty("DropMe", "highlightColor");
        Assert.AreNotEqual(color1,color2);
        altUnityDriver.FindObject(By.NAME,"Drop Image").PointerExitObject();
        var color3 = altElement.GetComponentProperty("DropMe", "highlightColor");
        Assert.AreNotEqual(color3, color2);
        Assert.AreEqual(color1,color3);

    }
}