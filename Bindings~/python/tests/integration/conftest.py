"""Holds test fixtures that need to be shared among all tests."""

import os

import pytest

from alttester import AltDriver


def get_port():
    return int(os.environ.get("ALTSERVER_PORT", 13000))


def get_host():
    return os.environ.get("ALTSERVER_HOST", "127.0.0.1")


def get_app_name():
    return os.environ.get("ALTSERVER_APP_NAME", "__default__")


@pytest.fixture(scope="session")
def altdriver():
    altdriver = AltDriver(
        host=get_host(),
        port=get_port(),
        app_name=get_app_name(),
        enable_logging=True,
        timeout=60 * 5
    )

    yield altdriver

    altdriver.stop()
