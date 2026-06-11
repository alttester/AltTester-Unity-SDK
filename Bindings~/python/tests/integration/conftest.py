"""
    Copyright(C) 2026 Altom Consulting
"""

import allure
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

saucelabs_devices = [
    {"name": "Samsung Galaxy S23.*", "os": "android", "os_version": "13"},
    {"name": "Samsung Galaxy S22.*", "os": "android", "os_version": "12"},
    {"name": "iPhone 16.*", "os": "ios", "os_version": "18"},
    {"name": "iPhone 15.*", "os": "ios", "os_version": "17"},
    {"name": "iPhone 14.*", "os": "ios", "os_version": "16"},
    {"name": "OnePlus 9.*", "os": "android", "os_version": "11"},
]

saucelabs_android_devices = [
    d for d in saucelabs_devices if d["os"] == "android"]
saucelabs_ios_devices = [d for d in saucelabs_devices if d["os"] == "ios"]


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


def get_saucelabs_username():
    return os.environ.get("SAUCE_USERNAME", "")


def get_saucelabs_access_key():
    return os.environ.get("SAUCE_ACCESS_KEY", "")


def get_saucelabs_region():
    return os.environ.get("SAUCE_REGION", "us-west-1")


def get_saucelabs_tunnel_name():
    return os.environ.get("SAUCE_TUNNEL_NAME", "alttester-tunnel")


def get_saucelabs_app_url(device):
    if device["os"] == "android":
        return os.environ.get("SAUCE_APP_URL_ANDROID", "")
    return os.environ.get("SAUCE_APP_URL_IOS", "")


def get_saucelabs_capabilities(device):
    return {
        "platformName": "Android" if device["os"] == "android" else "iOS",
        "appium:deviceName": device["name"],
        "appium:platformVersion": device["os_version"],
        "appium:app": get_saucelabs_app_url(device),
        "appium:automationName": "UiAutomator2" if device["os"] == "android" else "XCUITest",
        "appium:newCommandTimeout": 2000,
        "sauce:options": {
            "username": get_saucelabs_username(),
            "accessKey": get_saucelabs_access_key(),
            "build": "AltTester SDK",
            "name": "tests-{date:%Y-%m-%d_%H:%M:%S}".format(date=datetime.datetime.now()),
            "tunnelName": get_saucelabs_tunnel_name(),
            "tunnelOwner": get_saucelabs_username(),
            "appiumVersion": "latest",
            "idleTimeout": 300,
        }
    }


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


@pytest.fixture(autouse=True, scope="session")
def current_device(request, worker_id):
    global devices
    global local_run_device
    global android_devices
    global ios_devices
    global saucelabs_devices
    global saucelabs_android_devices
    global saucelabs_ios_devices
    current_device = None
    selected_devices = []
    if os.environ.get("RUN_IN_SAUCELABS", "") == "true":
        if worker_id == "master":
            current_device = saucelabs_devices[0]
        else:
            index = int(worker_id.split("gw")[1])
            if os.environ.get("RUN_IN_SAUCELABS_ANDROID_ONLY", "") == "true":
                selected_devices = saucelabs_android_devices
            elif os.environ.get("RUN_IN_SAUCELABS_IOS_ONLY", "") == "true":
                selected_devices = saucelabs_ios_devices
            else:
                selected_devices = saucelabs_devices
            current_device = selected_devices[index % len(selected_devices)]
    elif os.environ.get("RUN_IN_BROWSERSTACK", "") == "true":
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
    else:
        current_device = local_run_device[0]
    log_to_report("Using device: {}".format(current_device))
    yield current_device


def create_appium_session(current_device, worker_id):
    driver = None
    if os.environ.get("RUN_IN_SAUCELABS", "") == "true":
        caps = get_saucelabs_capabilities(current_device)
        region = get_saucelabs_region()
        hub_url = "https://ondemand.{}.saucelabs.com:443/wd/hub".format(region)
        if current_device["os"] == "android":
            options = UiAutomator2Options().load_capabilities(caps)
        else:
            options = XCUITestOptions().load_capabilities(caps)
        print("Starting appium driver (worker id: {})".format(worker_id) +
              " for device: {}".format(current_device["name"]))
        driver = webdriver.Remote(hub_url, options=options)
        time.sleep(10)
        if current_device["os"] == "ios":
            try:
                allow_button = driver.find_element(MobileBy.ID, "Allow")
                allow_button.click()
            except Exception as e:
                try:
                    print("No Allow button found: {}".format(type(e).__name__))
                    ok_button = driver.find_element(MobileBy.ID, "OK")
                    ok_button.click()
                except Exception as e:
                    print("No OK button found: {}".format(type(e).__name__))
                    pass
    elif os.environ.get("RUN_IN_BROWSERSTACK", "") == "true":
        if current_device["os"] == "android":
            options = UiAutomator2Options().load_capabilities(
                get_ui_automator_capabilities(current_device)
            )
        else:
            options = XCUITestOptions().load_capabilities(
                get_ui_automator_capabilities(current_device)
            )
        print("Starting appium driver (worker id: {})".format(worker_id) +
              " for device: {}".format(current_device["name"]))
        driver = webdriver.Remote(
            "http://hub.browserstack.com/wd/hub", options=options
        )
        time.sleep(10)
        if current_device["os"] == "ios":
            try:
                allow_button = driver.find_element(MobileBy.ID, "Allow")
                allow_button.click()
            except Exception as e:
                try:
                    print("No Allow button found: {}".format(type(e).__name__))
                    ok_button = driver.find_element(MobileBy.ID, "OK")
                    ok_button.click()
                except Exception as e:
                    print("No OK button found: {}".format(type(e).__name__))
                    pass
    return driver


@pytest.fixture(scope="session", autouse=True)
def appium_session(current_device, worker_id):
    session = {"driver": create_appium_session(current_device, worker_id)}
    yield session
    if session["driver"] is not None:
        try:
            session["driver"].quit()
        except Exception:
            pass


@pytest.fixture(scope="class", autouse=True)
def appium_driver(request, appium_session, current_device, worker_id):
    is_cloud = os.environ.get("RUN_IN_SAUCELABS", "") == "true" or \
        os.environ.get("RUN_IN_BROWSERSTACK", "") == "true"
    if is_cloud and appium_session["driver"] is not None:
        try:
            appium_session["driver"].get_window_size()
        except Exception:
            print("Appium session lost, recreating (worker id: {})".format(worker_id))
            try:
                appium_session["driver"].quit()
            except Exception:
                pass
            appium_session["driver"] = create_appium_session(
                current_device, worker_id)

    request.cls.appium_driver = appium_session["driver"]
    request.cls.current_device = current_device
    yield appium_session["driver"]


@pytest.fixture(autouse=True)
def do_something_with_appium(request):
    if os.environ.get("RUN_IN_BROWSERSTACK", "") != "true" and \
            os.environ.get("RUN_IN_SAUCELABS", "") != "true":
        return
    # cloud providers have an idle timeout of max 300 seconds
    # so we need to do something with the appium driver
    # to keep it alive
    try:
        request.cls.appium_driver.get_window_size()
    except Exception as e:
        print("Could not get window size: {}".format(type(e).__name__))
        pass
