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

import datetime
import os
import sys
import pytest
import time
from alttester import AltDriver
from appium import webdriver
from appium.webdriver.common.mobileby import MobileBy
from appium.options.android import UiAutomator2Options
from appium.options.ios import XCUITestOptions

sys.stdout = sys.stderr

devices = [
    {"name": "iPhone 14 Pro Max", "os": "ios", "os_version": "16"},
    {"name": "Samsung Galaxy S23", "os": "android", "os_version": "13.0"},
    {"name": "Google Pixel 6 Pro", "os": "android", "os_version": "13.0"},
    {"name": "OnePlus 9", "os": "android", "os_version": "11.0"},
    {"name": "iPhone 13 Pro Max", "os": "ios", "os_version": "15"},
    {"name": "Google Pixel 6", "os": "android", "os_version": "12.0"},
    # Add more devices as needed
]


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


def get_browserstack_app_url(device):
    if device["os"] == "android":
        return os.environ.get("APP_URL_ANDROID", "")
    return os.environ.get("APP_URL_IOS", "")


@pytest.fixture(scope="class", autouse=True)
def altdriver(request, appium_driver, worker_id, current_device):
    altdriver = AltDriver(
        host=get_host(), 
        port=get_port(), 
        app_name=get_app_name(), 
        platform=current_device["os"],
        platform_version=current_device["os_version"].split(".")[0],
        timeout=180
    )
    request.cls.altdriver = altdriver
    print("Started altdriver (worker {})".format(worker_id))
    yield altdriver

    altdriver.stop()


def get_ui_automator_capabilities(device):
    return {
        "platformName": device["os"],
        "platformVersion": device["os_version"],
        "deviceName": device["name"],
        "app": get_browserstack_app_url(device),
        "newCommandTimeout": "300",
        # Set other BrowserStack capabilities
        "bstack:options": {
            "projectName": "AltTester",
            "buildName": "pipeline-build-{}".format(device["os"]),
            "sessionName": "tests-{date:%Y-%m-%d_%H:%M:%S}".format(
                date=datetime.datetime.now()
            ),
            "local": "true",
            "wsLocalSupport": "true",
            "deviceOrientation": "landscape",
            "idleTimeout": "300",
            "networkLogs": "true",
            "userName": get_browserstack_username(),
            "accessKey": get_browserstack_key(),
        },
    }


@pytest.fixture(autouse=True, scope="class")
def current_device(request, worker_id):
    global devices
    current_device = None
    if worker_id == "master":
        current_device = devices[0]
    else:
        index = int(worker_id.split("gw")[1])
        current_device = devices[index]
    yield current_device


@pytest.fixture(scope="class", autouse=True)
def appium_driver(request, current_device, worker_id):
    appium_driver = None
    options = None
    if os.environ.get("RUN_IN_BROWSERSTACK", "") == "true":
        if current_device["os"] == "android":
            options = UiAutomator2Options().load_capabilities(
                get_ui_automator_capabilities(current_device)
            )
        else:
            options = options = XCUITestOptions().load_capabilities(
                get_ui_automator_capabilities(current_device)
            )
        print("Starting appium driver for device: {}".format(current_device["name"]))
        appium_driver = webdriver.Remote(
            "http://hub.browserstack.com/wd/hub", options=options
        )
        time.sleep(10)
        if current_device["os"] == "ios":
            try:
                allow_button = appium_driver.find_element(MobileBy.ID, "Allow")
                allow_button.click()
            except Exception as e:
                try:
                    print(
                        "No Allow button found, trying to find OK button, error: {}".format(
                            e
                        )
                    )
                    ok_button = appium_driver.find_element(MobileBy.ID, "OK")
                    ok_button.click()
                except Exception as e:
                    print("No OK button found, error: {}".format(e))
                    pass
    request.cls.appium_driver = appium_driver

    yield appium_driver

    if os.environ.get("RUN_IN_BROWSERSTACK", "") == "true":
        request.cls.appium_driver.quit()


@pytest.fixture(autouse=True)
def do_something_with_appium(request):
    if os.environ.get("RUN_IN_BROWSERSTACK", "") != "true":
        return
    # browserstack has an idle timeout of max 300 seconds
    # so we need to do something with the appium driver
    # to keep it alive
    try:
        request.cls.appium_driver.get_window_size()
    except Exception as e:
        print("Could not get window size: {}".format(e))
        pass
