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
import requests
import time
import datetime
from alttester import AltDriver
from appium.options.android import UiAutomator2Options
from browserstack.local import Local
from appium import webdriver

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

def get_ui_automator_capabilities(platform_name, platform_version, device_name, url, build_name):
    return {
            "platformName": platform_name,
            "platformVersion": platform_version,
            "deviceName": device_name,
            "app": url,

            # Set other BrowserStack capabilities
            'bstack:options': {
                "projectName": "AltTester",
                "buildName": build_name,
                "sessionName": 'tests-{date:%Y-%m-%d_%H:%M:%S}'
                .format(date=datetime.datetime.now()),
                "local": "true",
                "wsLocalSupport": "true",
                "deviceOrientation": "landscape",
                "networkLogs": "true",
                "userName": get_browserstack_username(),
                "accessKey": get_browserstack_key()
            }
        }


@pytest.fixture(scope="session")
def altdriver(appium_driver):
    altdriver = AltDriver(
        host=get_host(),
        port=get_port(),
        app_name=get_app_name(),
        enable_logging=True,
        timeout=60
    )
    print("altdriver started")

    yield altdriver

    altdriver.stop()


@pytest.fixture(scope="session")
def appium_driver(request):
    appium_driver = None

    if os.environ.get("RUN_IN_BROWSERSTACK", "") == "true":
        if os.environ.get("RUN_ANDROID_IN_BROWSERSTACK", "") == "true":
            files = {
            'file': ('sampleGame.apk', open('sampleGame.apk', 'rb')),
            }
            response = requests.post(
            'https://api-cloud.browserstack.com/app-automate/upload',
            files=files,
            auth=(get_browserstack_username(), get_browserstack_key()))
            try:
                app_url = response.json()['app_url']
            except Exception():
                pytest.fail("Error uploading app to BrowserStack, response: "
                            + str(response.text))
            options = UiAutomator2Options().load_capabilities(get_ui_automator_capabilities("android", "12.0", "Google Pixel 6", app_url, "alttester-pipeline-python-android"))
        if os.environ.get("RUN_IOS_IN_BROWSERSTACK", "") == "true":
            files = {
            'file': ('sampleGame.ipa', open('sampleGame.ipa', 'rb')),
            }
            response = requests.post(
            'https://api-cloud.browserstack.com/app-automate/upload',
            files=files,
            auth=(get_browserstack_username(), get_browserstack_key()))
            try:
                app_url = response.json()['app_url']
            except Exception():
                pytest.fail("Error uploading app to BrowserStack, response: "
                            + str(response.text))
            options = UiAutomator2Options().load_capabilities(get_ui_automator_capabilities("ios", "16", "iPhone 14", app_url, "alttester-pipeline-python-ios"))
        bs_local = Local()
        bs_local_args = {"key": get_browserstack_key(),
                         "forcelocal": "true",
                         "force": "true"}
        bs_local.start(**bs_local_args)
        appium_driver = webdriver.Remote("http://hub.browserstack.com/wd/hub",
                                         options=options)
        time.sleep(10)
    yield appium_driver

    if os.environ.get("RUN_IN_BROWSERSTACK", "") == "true":
        appium_driver.quit()
        bs_local.stop()


@pytest.fixture(autouse=True)
def do_something_with_appium(appium_driver):
    if os.environ.get("RUN_IN_BROWSERSTACK", "") != "true":
        return
    # browserstack has an idle timeout of max 300 seconds
    # so we need to do something with the appium driver
    # to keep it alive
    appium_driver.get_window_size()
