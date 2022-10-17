@Test
public void testDeleteKey() throws Exception
{
    altDriver.deletePlayerPref();
    altDriver.setKeyPlayerPref("test", 1.0f);
    float val = altDriver.getFloatKeyPlayerPref("test");
    assertEquals(1.0f, val);
    altDriver.deleteKeyPlayerPref("test");
    try
    {
        altDriver.getFloatKeyPlayerPref("test");
        fail();
    } 
    catch(NotFoundException e)
    {
        assertEquals(e.getMessage(), "error:notFound");
    } 
}