@Test
public void testDeleteKey() throws Exception
{
    altUnityDriver.deletePlayerPref();
    altUnityDriver.setKeyPlayerPref("test", 1);
    int val = altUnityDriver.getIntKeyPlayerPref("test");
    assertEquals(1, val);
    altUnityDriver.deleteKeyPlayerPref("test");
    try
    {
        altUnityDriver.getIntKeyPlayerPref("test");
        fail();
    }
    catch(NotFoundException e)
    {
        assertEquals(e.getMessage(), "error:notFound");
    }
}   