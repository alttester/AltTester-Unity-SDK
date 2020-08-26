@Test
public void testDeleteKey() throws Exception
{
    altUnityDriver.deletePlayerPref();
    altUnityDriver.setKeyPlayerPref("test", 1.0f);
    float val = altUnityDriver.getFloatKeyPlayerPref("test");
    assertEquals(1.0f, val);
    altUnityDriver.deleteKeyPlayerPref("test");
    try
    {
        altUnityDriver.getFloatKeyPlayerPref("test");
        fail();
    } 
    catch(NotFoundException e)
    {
        assertEquals(e.getMessage(), "error:notFound");
    } 
}