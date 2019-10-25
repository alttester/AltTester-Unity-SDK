# Command: SetKeyPlayerPref

## Description:

Set a key-value pair in the game player pref.

## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| keyName      |     string    |   false   | Key that will be stored in player pref|
| intValue      |     int    |   true   | Value stored together with the key|
| floatValue      |     float    |   true   | Value stored together with the key|
| strinValue      |     string    |   true   | Value stored together with the key|

######## Only one of the three type of value is needed


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
        [Test]
        public void TestSetKeyFloat()
        {
            altUnityDriver.DeletePlayerPref();
            altUnityDriver.SetKeyPlayerPref("test", 1f);
            var val = altUnityDriver.GetFloatKeyPlayerPref("test");
            Assert.AreEqual(1f, val);
        }

        [Test]
        public void TestSetKeyString()
        {
            altUnityDriver.DeletePlayerPref();
            altUnityDriver.SetKeyPlayerPref("test", "test");
            var val = altUnityDriver.GetStringKeyPlayerPref("test");
            Assert.AreEqual("test", val);
        }
    .. code-tab:: java

        @Test
        public void testSetKeyInt() throws Exception {
            altUnityDriver.deletePlayerPref();
            altUnityDriver.setKeyPlayerPref("test", 1);
            int val = altUnityDriver.getIntKeyPlayerPref("test");
            assertEquals(1, val);
        }

        @Test
        public void testSetKeyFloat() throws Exception {
            altUnityDriver.deletePlayerPref();
            altUnityDriver.setKeyPlayerPref("test", 1f);
            float val = altUnityDriver.getFloatKeyPlayerPref("test");
            assertEquals(1f, val, 0.01);
        }

        @Test
        public void testSetKeyString() throws Exception {
            altUnityDriver.deletePlayerPref();
            altUnityDriver.setKeyPlayerPref("test", "test");
            String val = altUnityDriver.getStringKeyPlayerPref("test");
            assertEquals("test", val);
        }


    .. code-tab:: py

        def test_set_player_pref_keys_int(self):
            self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
            self.altdriver.delete_player_prefs()
            self.altdriver.set_player_pref_key('test', 1, PlayerPrefKeyType.Int)
            value = self.altdriver.get_player_pref_key('test', PlayerPrefKeyType.Int)
            self.assertEqual(int(value), 1)

        def test_set_player_pref_keys_float(self):
            self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
            self.altdriver.delete_player_prefs()
            self.altdriver.set_player_pref_key('test', 1.3, PlayerPrefKeyType.Float)
            value = self.altdriver.get_player_pref_key('test', PlayerPrefKeyType.Float)
            self.assertEqual(float(value), 1.3)

        def test_set_player_pref_keys_string(self):
            self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
            self.altdriver.delete_player_prefs()
            self.altdriver.set_player_pref_key('test', 'string value', PlayerPrefKeyType.String)
            value = self.altdriver.get_player_pref_key('test', PlayerPrefKeyType.String)
            self.assertEqual(value, 'string value')
```

