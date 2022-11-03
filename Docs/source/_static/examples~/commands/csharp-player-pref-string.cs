[Test]
public void TestDeleteKey()
{
    altDriver.DeletePlayerPref();
    altDriver.SetKeyPlayerPref("test", "1");
    var val = altDriver.GetStringKeyPlayerPref("test");
    Assert.AreEqual("1", val);
    altDriver.DeleteKeyPlayerPref("test");
    try
    {
        altDriver.GetStringKeyPlayerPref("test");
        Assert.Fail();
    }
    catch (NotFoundException exception)
    {
        Assert.AreEqual("notFound", exception.Message);
    }

}
