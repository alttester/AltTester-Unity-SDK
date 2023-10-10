# Running tests together with Appium

When it comes to mobile automation testing, there are many different choices for a test tool. One popular option is [Appium](http://appium.io), an open source project which enables running automated tests on both **Android** and **iOS** devices.

No registration is needed and you can either download the latest version of the standalone app [here](https://github.com/appium/appium-desktop/releases/), or you can install the cli version by running:

```
> brew install node      # get node.js
> npm install -g appium  # get appium
> npm install wd         # get appium client
> appium &               # start appium
```

We've created an example python project which can be found [here](https://github.com/alttester-test-examples/Python-Android-with-Appium-AltTrashCat-) which hopefully can get you started on your own projects. Using it will also automatically install the requirements needed for running the tests. More details about it [below](#alttester-unity-sdk-with-appium-example).

## Why use Appium together with AltTester® Unity SDK

There's a couple of reasons/scenarios for which you would want to use both of these frameworks:

-   By itself, AltTester® Unity SDK cannot launch an app on a device. If you want to run tests in a pipeline, or by using [cloud services](./alttester-with-cloud), you can either create a script which will start your app, or you can use Appium before the tests execution;
-   AltTester® Unity SDK cannot perform some types of actions, such as interacting with any native popups your app might have, or putting the app in the background and resuming it. In any of these cases you can use Appium to do the things that AltTester® Unity SDK can't.

## AltTester® Unity SDK with Appium example

After you cloned our example project, there are a couple of things you need to check before running the tests:

-   For **Android** you need to have Android SDK version 16 or higher installed on your machine;
-   For **iOS** you need XCode with Command Line Tools installed (will only work on Mac OSX);
-   Your mobile device needs to have developer mode enabled and be connected via USB to the machine running the tests.

**Running the tests**

-   For **Android** you can just run the script `run-tests_android.sh`
-   For **iOS**, you first need to `export IOS_UDID=<your-device-udid>` then run the script `run-tests_ios.sh`

```eval_rst
.. note::

   To find out an **iOS** device **UDID** you can go to **Finder**, click the device in the sidebar and click the info under the device name to reveal the UDID

```

The script will install any requirements that are missing from your machine (except Android SDK and XCode CLT), then run a basic test scenario:

1. The app will be started by Appium;
2. AltTester® Unity SDK will ensure it's initially loaded;
3. Appium will put the app in the background for a couple of seconds, then resume it;
4. AltTester® Unity SDK will check if the app was resumed successfully.

```eval_rst
.. note::

   Please observe the following about the setup method in **base_test.py**:

   1. A minimum amount of capabilities have to be set in order for Appium to work. More details about capabilities can be found in the official `Appium documentation <https://appium.io/docs/en/2.0/guides/caps/>`_
   
   2. Starting with **Selenium 4** the *DesiredCapabilities* are deprecated and the Webdriver now uses *Options* to pass capabilities, so if you're using Selenium 4, in order for the example project to work, you may have to update the code with the new setup - see the `Selenium documentation <https://www.selenium.dev/documentation/webdriver/getting_started/upgrade_to_selenium_4/>`_
   
   3. The Appium driver needs to be created before the reverse port forwarding needed by AltTester® Unity SDK is done. This is because Appium clears any other reverse port forwarding when it starts.

```
