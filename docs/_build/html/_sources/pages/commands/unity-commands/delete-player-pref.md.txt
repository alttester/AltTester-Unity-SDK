# Command: DeletePlayerPref

## Description:

Delete entire player pref of the game

## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
|None|
## Return:
- Nothing

## Examples
```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
            public void TestSetKeyInt()
            {
                altUnityDriver.DeletePlayerPref();
                altUnityDriver.SetKeyPlayerPref("test", 1);
                var val = altUnityDriver.GetIntKeyPlayerPref("test");
                Assert.AreEqual(1, val);
            }
    .. code-tab:: java

         @Test
            public void testSetKeyFloat() throws Exception {
                altUnityDriver.deletePlayerPref();
                altUnityDriver.setKeyPlayerPref("test", 1f);
                float val = altUnityDriver.getFloatKeyPlayerPref("test");
                assertEquals(1f, val, 0.01);
            }


    .. code-tab:: py

       def test_set_player_pref_keys_int(self):
              self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
              self.altdriver.delete_player_prefs()
              self.altdriver.set_player_pref_key('test', 1, PlayerPrefKeyType.Int)
              value = self.altdriver.get_player_pref_key('test', PlayerPrefKeyType.Int)
              self.assertEqual(int(value), 1)
```

