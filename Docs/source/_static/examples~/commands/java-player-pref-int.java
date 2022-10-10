@Test
public void testDeleteKey() throws Exception
{
    altDriver.deletePlayerPref();
    altDriver.setKeyPlayerPref("test", 1);
    int val = altDriver.getIntKeyPlayerPref("test");
    assertEquals(1, val);
    altDriver.deleteKeyPlayerPref("test");
    try
    {
        altDriver.getIntKeyPlayerPref("test");
        fail();
    }
    catch(NotFoundException e)
    {
        assertEquals(e.getMessage(), "error:notFound");
    }
}   