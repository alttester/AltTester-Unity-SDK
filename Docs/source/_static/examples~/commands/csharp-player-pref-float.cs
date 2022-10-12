[Test]
public void TestDeleteKey()
{
    altDriver.DeletePlayerPref();
    altDriver.SetKeyPlayerPref("test", 1.0f);
    var val = altDriver.GetFloatKeyPlayerPref("test");
    Assert.AreEqual(1.0f, val);
    altDriver.DeleteKeyPlayerPref("test");
    try
    {
        altDriver.GetFloatKeyPlayerPref("test");
        Assert.Fail();
    }
    catch (NotFoundException exception)
    {
        Assert.AreEqual("notFound", exception.Message);
    }
}
