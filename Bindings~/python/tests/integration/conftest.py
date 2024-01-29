"""
    Copyright(C) 2023 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
"""

import os
import pytest
import time
from alttester import AltDriver
from appium.options.android import UiAutomator2Options
from appium.options.ios import XCUITestOptions
from browserstack.local import Local
from appium import webdriver
from appium.webdriver.common.mobileby import MobileBy

"""Holds test fixtures that need to be shared among all tests."""


def get_port():
    return int(os.environ.get("ALTSERVER_PORT", 13000))


def get_host():
    return os.environ.get("ALTSERVER_HOST", "127.0.0.1")


def get_app_name():
    return os.environ.get("ALTSERVER_APP_NAME", "__default__")


def get_browserstack_username():
    return os.environ.get("BROWSERSTACK_USERNAME", "")


def get_browserstack_key():
    return os.environ.get("BROWSERSTACK_KEY", "")


@pytest.fixture(scope="session")
def altdriver(appium_driver):
    altdriver = AltDriver(
        host=get_host(),
        port=get_port(),
        app_name=get_app_name(),
        timeout=60
    )
    print("altdriver started")

    yield altdriver

    altdriver.stop()


@pytest.fixture(scope="session")
def appium_driver(request):
    appium_driver = None
    if os.environ.get("RUN_IN_BROWSERSTACK", "") == "true":
        appium_driver = webdriver.Remote("http://hub.browserstack.com/wd/hub",
                                         request.getfixturevalue("session_capabilities"))
        time.sleep(10)
    yield appium_driver

    if os.environ.get("RUN_IN_BROWSERSTACK", "") == "true":
        appium_driver.quit()


@pytest.fixture(autouse=True)
def do_something_with_appium(appium_driver):
    if os.environ.get("RUN_IN_BROWSERSTACK", "") != "true":
        return
    # browserstack has an idle timeout of max 300 seconds
    # so we need to do something with the appium driver
    # to keep it alive
    appium_driver.get_window_size()
