def test_delete_key_player_pref(self):
    self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
    self.altdriver.delete_player_prefs()
    player_pref_key_type = PlayerPrefKeyType.String
    self.altdriver.set_player_pref_key("test", "1", player_pref_key_type)
    val = self.altdriver.get_player_pref_key("test", player_pref_key_type)
    self.assertEqual("1", str(val))
    self.altdriver.delete_player_pref_key("test")
    try:
        self.altdriver.get_player_pref_key("test", player_pref_key_type)
        self.fail()
    except NotFoundException as exception:
        self.assertEqual("error:notFound", str(exception))
