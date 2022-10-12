[Test]
public void TestDeleteKey()
{
    altDriver.DeletePlayerPref();
    altDriver.SetKeyPlayerPref("test", 1);
    var val = altDriver.GetIntKeyPlayerPref("test");
    Assert.AreEqual(1, val);
    altDriver.DeleteKeyPlayerPref("test");
    try
    {
        altDriver.GetIntKeyPlayerPref("test");
        Assert.Fail();
    }
    catch (NotFoundException exception)
    {
        Assert.AreEqual("notFound", exception.Message);
    }

}
