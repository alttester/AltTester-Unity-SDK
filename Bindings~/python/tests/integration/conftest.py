"""
    Copyright(C) 2026 Altom Consulting

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

import allure
import datetime
import os
import sys
import pytest
import time
from alttester import AltDriver, exceptions
from appium import webdriver
from appium.webdriver.common.mobileby import MobileBy
from appium.options.android import UiAutomator2Options
from appium.options.ios import XCUITestOptions

sys.stdout = sys.stderr

devices = [
    {"name": "Samsung Galaxy S23", "os": "android", "os_version": "13.0"},
    {"name": "Google Pixel 6 Pro", "os": "android", "os_version": "12.0"},
    {"name": "iPhone 16", "os": "ios", "os_version": "18"},
    {"name": "iPhone 15 Pro Max", "os": "ios", "os_version": "17"},
    {"name": "iPhone 14 Pro Max", "os": "ios", "os_version": "16"},
    {"name": "OnePlus 9", "os": "android", "os_version": "11.0"},
    {"name": "iPhone 13 Pro Max", "os": "ios", "os_version": "15"},
    {"name": "Google Pixel 6", "os": "android", "os_version": "12.0"},
    # Add more devices as needed
]

android_devices = [
    {"name": "Samsung Galaxy S23", "os": "android", "os_version": "13.0"},
    {"name": "OnePlus 9", "os": "android", "os_version": "11.0"},
    {"name": "Google Pixel 6", "os": "android", "os_version": "12.0"},
]

ios_devices = [
    {"name": "iPhone 16", "os": "ios", "os_version": "18"},
    {"name": "iPhone 15 Pro Max", "os": "ios", "os_version": "17"},
    {"name": "iPhone 14 Pro Max", "os": "ios", "os_version": "16"},
    {"name": "iPhone 13 Pro Max", "os": "ios", "os_version": "15"},
    {"name": "iPhone 12 Pro", "os": "ios", "os_version": "14"},
]

local_run_device = [

    {"name": "__default__", "os": "unknown", "os_version": "unknown"},
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
def alt_driver(request, appium_driver, worker_id, current_device):
    platform = current_device["os"]
    if current_device["os"] == "ios":
        platform = "iphone"
    alt_driver = AltDriver(
        host=get_host(),
        port=get_port(),
        app_name=get_app_name(),
        platform=platform,
        platform_version=current_device["os_version"].split(".")[0],
        timeout=180
    )
    if alt_driver is None:
        raise RuntimeError(
            "Failed to initialize AltDriver instance. Please verify configuration and environment settings.")
    request.cls.alt_driver = alt_driver
    print("Started alt_driver (worker {})".format(worker_id) +
          " with device: {}".format(current_device))
    yield alt_driver

    alt_driver.stop()


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


@allure.step("Log: {message}")
def log_to_report(message):
    print(message)


@pytest.fixture(autouse=True, scope="class")
def current_device(request, worker_id):
    global devices
    global local_run_device
    global android_devices
    global ios_devices
    current_device = None
    selected_devices = []
    if os.environ.get("RUN_IN_BROWSERSTACK", "") != "true":
        current_device = local_run_device[0]
    else:
        if worker_id == "master":
            current_device = devices[0]
        else:
            index = int(worker_id.split("gw")[1])
            if os.environ.get("RUN_IN_BROWSERSTACK_ANDROID_ONLY", "") == "true":
                selected_devices = android_devices
            elif os.environ.get("RUN_IN_BROWSERSTACK_IOS_ONLY", "") == "true":
                selected_devices = ios_devices
            else:
                selected_devices = devices
            current_device = selected_devices[index]
    log_to_report("Using device: {}".format(current_device))
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
        print("Starting appium driver (worker id: {})".format(worker_id) +
              " for device: {}".format(current_device["name"]))
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
                        "No Allow button found: {}".format(type(e).__name__)
                    )
                    ok_button = appium_driver.find_element(MobileBy.ID, "OK")
                    ok_button.click()
                except Exception as e:
                    print("No OK button found: {}".format(type(e).__name__))
                    pass
    request.cls.appium_driver = appium_driver
    request.cls.current_device = current_device

    yield appium_driver

    if os.environ.get("RUN_IN_BROWSERSTACK", "") == "true":
        request.cls.appium_driver.quit()


def restart_app(appium_driver, current_device):
    """Terminate and relaunch the app via Appium."""
    try:
        capabilities = getattr(appium_driver, "capabilities", {}) or {}
        if current_device["os"] == "android":
            package = (
                capabilities.get("appPackage")
                or capabilities.get("appium:appPackage")
                or appium_driver.current_package
            )
            if not package:
                print("Could not determine package name to restart app")
                return False
            appium_driver.terminate_app(package)
            time.sleep(2)
            appium_driver.activate_app(package)
        else:
            bundle_id = (
                capabilities.get("bundleId")
                or capabilities.get("appium:bundleId")
            )
            if not bundle_id:
                app_info = appium_driver.execute_script("mobile: activeAppInfo")
                bundle_id = app_info.get("bundleId") if app_info else None
            if bundle_id:
                appium_driver.terminate_app(bundle_id)
                time.sleep(2)
                appium_driver.activate_app(bundle_id)
            else:
                print("Could not determine bundle ID to restart app")
                return False
        time.sleep(5)
        print("App restarted successfully")
        return True
    except Exception as e:
        print("Failed to restart app: {}".format(type(e).__name__))
        return False


@pytest.fixture(autouse=True)
def do_something_with_appium(request, current_device, worker_id):
    if os.environ.get("RUN_IN_BROWSERSTACK", "") != "true":
        return
    # browserstack has an idle timeout of max 300 seconds
    # so we need to do something with the appium driver
    # to keep it alive
    try:
        request.cls.appium_driver.get_window_size()
    except Exception as e:
        print("Could not get window size: {}".format(type(e).__name__))
    if not hasattr(request.cls, 'alt_driver'):
        return
    try:
        request.cls.alt_driver.get_current_scene()
    except exceptions.CommandResponseTimeoutException as e:
        print("get_current_scene timed out ({}), skipping reconnect check".format(type(e).__name__))
        return
    except (exceptions.ConnectionError, exceptions.NoAppConnected, exceptions.AppDisconnectedError) as e:
        print("Reconnecting alt_driver because connection was lost: {}".format(type(e).__name__))
        try:
            request.cls.alt_driver.stop()
        except Exception:
            pass
        platform = current_device["os"]
        if current_device["os"] == "ios":
            platform = "iphone"
        try:
            request.cls.alt_driver = AltDriver(
                host=get_host(),
                port=get_port(),
                app_name=get_app_name(),
                platform=platform,
                platform_version=current_device["os_version"].split(".")[0],
                timeout=180
            )
            print("Reconnected alt_driver (worker {})".format(worker_id) +
                  " with device: {}".format(current_device))
        except Exception:
            print("App appears to have crashed — attempting restart")
            if restart_app(request.cls.appium_driver, current_device):
                try:
                    request.cls.alt_driver = AltDriver(
                        host=get_host(),
                        port=get_port(),
                        app_name=get_app_name(),
                        platform=platform,
                        platform_version=current_device["os_version"].split(".")[0],
                        timeout=180
                    )
                    print("Reconnected alt_driver after restart (worker {})".format(worker_id) +
                          " with device: {}".format(current_device))
                except Exception as reconnect_error:
                    print("Failed to reconnect alt_driver after restart: {}".format(
                        type(reconnect_error).__name__))
