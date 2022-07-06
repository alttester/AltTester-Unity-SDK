"""Holds test fixtures that need to be shared among all tests."""

import os

import pytest

from altunityrunner import AltUnityDriver


def get_altunitytester_port():
    port = os.environ.get("ALTUNITYDRIVER_PORT", 13000)
    return int(port)


def get_altunitytester_host():
    return os.environ.get("ALTUNITYDRIVER_HOST", "127.0.0.1")


@pytest.fixture(scope="session")
def altdriver():
    altdriver = AltUnityDriver(
        host=get_altunitytester_host(),
        port=get_altunitytester_port(),
        enable_logging=True,
        timeout=None
    )

    yield altdriver

    altdriver.stop()
