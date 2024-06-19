def test_delete_key_player_pref(self):
    self.alt_driver.load_scene("Scene 1 AltDriverTestScene")
    self.alt_driver.delete_player_prefs()
    player_pref_key_type = PlayerPrefKeyType.String
    self.alt_driver.set_player_pref_key("test", "1", player_pref_key_type)
    val = self.alt_driver.get_player_pref_key("test", player_pref_key_type)
    self.assertEqual("1", str(val))
    self.alt_driver.delete_player_pref_key("test")
    try:
        self.alt_driver.get_player_pref_key("test", player_pref_key_type)
        self.fail()
    except NotFoundException as exception:
        self.assertEqual("notFound", str(exception))
