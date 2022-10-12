def test_delete_key_player_pref(self):
    self.altDriver.load_scene("Scene 1 AltDriverTestScene")
    self.altDriver.delete_player_prefs()
    player_pref_key_type = PlayerPrefKeyType.String
    self.altDriver.set_player_pref_key("test", "1", player_pref_key_type)
    val = self.altDriver.get_player_pref_key("test", player_pref_key_type)
    self.assertEqual("1", str(val))
    self.altDriver.delete_player_pref_key("test")
    try:
        self.altDriver.get_player_pref_key("test", player_pref_key_type)
        self.fail()
    except NotFoundException as exception:
        self.assertEqual("notFound", str(exception))
