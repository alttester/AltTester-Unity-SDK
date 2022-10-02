from alttester.portforwarding import AltUnityPortForwarding


class TestAltUnityPortForwarding:

    def test_get_iproxy_path(self):
        assert "overwrite" == AltUnityPortForwarding._get_iproxy_path("overwrite")
        assert "iproxy" == AltUnityPortForwarding._get_iproxy_path("")
