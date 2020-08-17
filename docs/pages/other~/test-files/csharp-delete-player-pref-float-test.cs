[Test]
public void TestDeleteKey()
{
    altUnityDriver.DeletePlayerPref();
    altUnityDriver.SetKeyPlayerPref("test", 1.0f);
    var val = altUnityDriver.GetFloatKeyPlayerPref("test");
    Assert.AreEqual(1.0f, val);
    altUnityDriver.DeleteKeyPlayerPref("test");
    try
    {
        altUnityDriver.GetFloatKeyPlayerPref("test");
        Assert.Fail();
    }
    catch (NotFoundException exception)
    {
        Assert.AreEqual("error:notFound", exception.Message);
    }

}