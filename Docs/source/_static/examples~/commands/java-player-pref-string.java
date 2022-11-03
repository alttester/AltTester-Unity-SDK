@Test
public void testDeleteKey() throws Exception
{
    altDriver.deletePlayerPref();
    altDriver.setKeyPlayerPref("test", "1");
    String val = altDriver.getStringKeyPlayerPref("test");
    assertEquals("1", val);
    altDriver.deleteKeyPlayerPref("test");
    try
    {
        altDriver.getStringKeyPlayerPref("test");
        fail();
    }
    catch(NotFoundException e)
    {
        assertEquals(e.getMessage(), "error:notFound");
    }
}