from unittest import TestCase

from altunityrunner.alt_unity_port_forwarding import AltUnityPortForwarding


class AltUnityPortForwardingTests(TestCase):

    def test_get_iproxy_path(self):
        self.assertEqual(
            "overwrite", AltUnityPortForwarding._get_iproxy_path("overwrite"))
        self.assertEqual(
            "iproxy", AltUnityPortForwarding._get_iproxy_path(""))
