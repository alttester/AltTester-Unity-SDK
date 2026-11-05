"""
    Copyright(C) 2026 Altom Consulting
"""

import datetime
import os
import time
import pytest
from appium import webdriver
from appium.options.android import UiAutomator2Options
from appium.options.ios import XCUITestOptions


saucelabs_devices = [
    {"name": "Samsung Galaxy S23.*", "os": "android", "os_version": "13"},
    {"name": "Samsung Galaxy S22.*", "os": "android", "os_version": "12"},
    {"name": "iPhone 16.*", "os": "ios", "os_version": "18"},
    {"name": "iPhone 15.*", "os": "ios", "os_version": "17"},
    {"name": "iPhone 14.*", "os": "ios", "os_version": "16"},
    {"name": "OnePlus 9.*", "os": "android", "os_version": "11"},
]

saucelabs_android_devices = [d for d in saucelabs_devices if d["os"] == "android"]
saucelabs_ios_devices = [d for d in saucelabs_devices if d["os"] == "ios"]


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
            "build": "AltTester SDK - Native Popup",
            "name": "native-popup-{date:%Y-%m-%d_%H:%M:%S}".format(date=datetime.datetime.now()),
            "tunnelName": get_saucelabs_tunnel_name(),
            "tunnelOwner": get_saucelabs_username(),
            "appiumVersion": "latest",
            "idleTimeout": 300,
        }
    }


def create_appium_driver(device):
    caps = get_saucelabs_capabilities(device)
    region = get_saucelabs_region()
    hub_url = "https://ondemand.{}.saucelabs.com:443/wd/hub".format(region)
    if device["os"] == "android":
        options = UiAutomator2Options().load_capabilities(caps)
    else:
        options = XCUITestOptions().load_capabilities(caps)
    driver = webdriver.Remote(hub_url, options=options)
    time.sleep(10)
    return driver


@pytest.fixture(autouse=True, scope="session")
def worker_id(request):
    request.config.getoption("--dist", default="no", skip=True)
    if hasattr(request.config, "workerinput"):
        return request.config.workerinput["workerid"]
    return "master"


@pytest.fixture(autouse=True, scope="session")
def current_device(worker_id):
    if os.environ.get("RUN_IN_SAUCELABS_ANDROID_ONLY", "") == "true":
        selected_devices = saucelabs_android_devices
    elif os.environ.get("RUN_IN_SAUCELABS_IOS_ONLY", "") == "true":
        selected_devices = saucelabs_ios_devices
    else:
        selected_devices = saucelabs_devices

    if worker_id == "master":
        return selected_devices[0]
    index = int(worker_id.split("gw")[1])
    return selected_devices[index % len(selected_devices)]


@pytest.fixture(scope="class")
def appium_driver(current_device):
    driver = create_appium_driver(current_device)
    yield driver
    try:
        driver.quit()
    except Exception:
        pass
