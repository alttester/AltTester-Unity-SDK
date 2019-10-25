# Command: GetKeyPlayer

## Description:

Get value of a key from player pref. Depending of which type does the value is stored togheter with the key you will have to use one of these commands:

- GetIntKeyPlayerPref
- GetFloatKeyPlayerPref
- GetStringKeyPlayerPref

## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| KeyName      |     string    |   false   | Name of the key for which you want the value|

## Returns

- int(GetIntKeyPlayerPref)
- float(GetFloatKeyPlayerPref)
- string(GetStringKeyPlayerPref)

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