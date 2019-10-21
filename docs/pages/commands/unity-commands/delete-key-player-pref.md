# Command: DeleteKeyPlayerPref  

## Description:

Delete from games player pref a key


## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| keyname      |     sting    |   false   | Key to be deleted|

## Return 
- Nothing

## Examples
```eval_rst
.. tabs::

    .. code-tab:: c#

         [Test]
            public void TestDeleteKey()
            {
                altUnityDriver.DeletePlayerPref();
                altUnityDriver.SetKeyPlayerPref("test", 1);
                var val = altUnityDriver.GetIntKeyPlayerPref("test");
                Assert.AreEqual(1, val);
                altUnityDriver.DeleteKeyPlayerPref("test");
                try
                {
                    altUnityDriver.GetIntKeyPlayerPref("test");
                    Assert.Fail();
                }
                catch (NotFoundException exception)
                {
                    Assert.AreEqual("error:notFound", exception.Message);
                }
        
            }
    .. code-tab:: java

        @Test
            public void testDeleteKey() throws Exception {
                altUnityDriver.deletePlayerPref();
                altUnityDriver.setKeyPlayerPref("test", 1);
                int val = altUnityDriver.getIntKeyPlayerPref("test");
                assertEquals(1, val);
                altUnityDriver.deleteKeyPlayerPref("test");
                try {
                    altUnityDriver.getIntKeyPlayerPref("test");
                    fail();
                } catch (NotFoundException e) {
                    assertEquals(e.getMessage(), "error:notFound");
                }
            }


    .. code-tab:: py

       //TODO
```

