@Test
public void testDeleteKey() throws Exception
{
    altUnityDriver.deletePlayerPref();
    altUnityDriver.setKeyPlayerPref("test", "1");
    String val = altUnityDriver.getStringKeyPlayerPref("test");
    assertEquals("1", val);
    altUnityDriver.deleteKeyPlayerPref("test");
    try
    {
        altUnityDriver.getStringKeyPlayerPref("test");
        fail();
    }
    catch(NotFoundException e)
    {
        assertEquals(e.getMessage(), "error:notFound");
    }
}