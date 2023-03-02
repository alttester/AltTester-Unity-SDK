"""Holds test fixtures that need to be shared among all tests."""

import os

import pytest

from alttester import AltDriver


def get_alttester_port():
    return int(os.environ.get("ALTSERVER_PORT", 13000))


def get_alttester_host():
    return os.environ.get("ALTSERVER_HOST", "127.0.0.1")


@pytest.fixture(scope="session")
def altdriver():
    altdriver = AltDriver(
        host=get_alttester_host(),
        port=get_alttester_port(),
        enable_logging=True,
        timeout=60 * 5
    )

    yield altdriver

    altdriver.stop()
