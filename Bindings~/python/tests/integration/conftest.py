"""Holds test fixtures that need to be shared among all tests."""

import os

import pytest

from alttester import AltDriver


def get_alttester_port():
    port = os.environ.get("ALTTESTER_DRIVER_PORT", 13000)
    return int(port)


def get_alttester_host():
    return os.environ.get("ALTTESTER_DRIVER_HOST", "127.0.0.1")


@pytest.fixture(scope="session")
def altdriver():
    altdriver = AltDriver(
        host=get_alttester_host(),
        port=get_alttester_port(),
        enable_logging=True,
        timeout=None
    )

    yield altdriver

    altdriver.stop()
