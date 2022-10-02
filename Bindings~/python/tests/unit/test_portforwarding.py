from alttester.portforwarding import AltPortForwarding


class TestAltUnityPortForwarding:

    def test_get_iproxy_path(self):
        assert "overwrite" == AltPortForwarding._get_iproxy_path("overwrite")
        assert "iproxy" == AltPortForwarding._get_iproxy_path("")
