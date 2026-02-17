# Running tests together with Appium / Selenium

AltTester® Unity SDK can be used together with different automation frameworks depending on your target platform:

- **[Appium](http://appium.io)** for mobile automation testing on **Android** and **iOS** devices
- **[Selenium](https://www.selenium.dev/)** for **WebGL-based** builds running in web browsers

Both are open source projects that enable automated testing and can be used alongside AltTester® Unity SDK to handle platform-specific interactions and operations that AltTester® Unity SDK cannot perform directly.

## Appium - Mobile Testing (Android & iOS)

When it comes to mobile automation testing, [Appium](http://appium.io) is a popular choice for automating tests on both **Android** and **iOS** devices.
No registration is needed and you can either download the latest version of the standalone app [here](https://github.com/appium/appium-desktop/releases/), or you can install the cli version by running:

```
> brew install node      # get node.js
> npm install -g appium  # get appium
> npm install wd         # get appium client
> appium &               # start appium
```

We've created an example Python project which can be found [here](https://github.com/alttester-test-examples/Python-Android-with-Appium-AltTrashCat-) which hopefully can get you started on your own projects. Using it will also automatically install the requirements needed for running the tests. More details about it [below](#alttester-unity-sdk-with-appium-example).

### Why use Appium together with AltTester® Unity SDK

There's a couple of reasons/scenarios for which you would want to use both of these frameworks:

-   By itself, AltTester® Unity SDK cannot launch an app on a device. If you want to run tests in a pipeline, or by using [cloud services](./alttester-with-cloud), you can either create a script which will start your app, or you can use Appium before the tests execution;
-   AltTester® Unity SDK cannot perform some types of actions, such as interacting with any native popups your app might have, or putting the app in the background and resuming it. In any of these cases you can use Appium to do the things that AltTester® Unity SDK can't.

### AltTester® Unity SDK with Appium example

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

## Selenium - WebGL Testing

[Selenium](https://www.selenium.dev/) is a powerful automation framework for web browsers. When running AltTester® Unity SDK on WebGL builds, Selenium allows you to automate browser interactions and handle web-specific operations that complement AltTester® Unity SDK's game automation capabilities.

### Why use Selenium together with AltTester® Unity SDK

There are several scenarios where you would want to use Selenium with AltTester® Unity SDK for WebGL builds:

- By itself, AltTester® Unity SDK cannot handle browser-specific operations such as opening new tabs, managing cookies, or interacting with browser dialogs;
- Selenium provides robust browser management and can handle native browser popups and dialogs that AltTester® Unity SDK cannot interact with;
- You can use Selenium to perform cross-browser testing of your WebGL builds on different browsers (Chrome, Firefox, Safari, Edge, etc.);
- Selenium allows you to automate tasks before launching your WebGL application, such as logging in on a web page or navigating to a specific URL.

## Connection settings popup for Appium / Selenium flows

When running AltTester® Unity SDK builds in the cloud and driving them with Appium or Selenium, you might not know in advance which app name to use from your test scripts to connect to the correct instrumented instance. To address this, AltTester® Unity SDK provides an optional native connection settings popup that can be shown in the app.

The popup allows you to:

- Enter the AltTester® Server host;
- Enter the AltTester® Server port;
- Enter the app name that will be used by the tests when connecting.

Because the popup is built using native UI elements, you can fully interact with it from Appium or Selenium: locate the fields and buttons using your preferred locator strategy (for example accessibility id, xpath, or text), type the desired host, port and app name values, and confirm the dialog before starting your AltTester® tests.

Whether this popup is shown or not is controlled by a dedicated setting in AltTester® Unity SDK. Enable this setting when you want the popup to appear (for example in cloud/Appium/Selenium runs where connection details are provided dynamically), and disable it when you prefer to configure connection parameters directly in your game or test code without any additional UI.

### Examples: identifying and interacting with the popup in Appium / Selenium

```eval_rst

.. tabs::

    .. tab:: Appium
        Below is a simple example that shows how you can identify and fill in the connection settings popup with Appium before starting your AltTester® tests.

        .. tabs::
            .. tab:: C#

                .. literalinclude:: ../_static/examples~/appium/csharp-appium.cs
                    :language: c#

            .. tab:: Java

                .. literalinclude:: ../_static/examples~/appium/java-appium.java
                    :language: java

            .. tab:: Python

                .. literalinclude:: ../_static/examples~/appium/python-appium.py
                    :language: py

            .. tab:: Robot

                .. literalinclude:: ../_static/examples~/appium/robot-appium.robot
                    :language: robot

    .. tab:: Selenium
        Below is a simple example that shows how you can identify and fill in the connection settings popup with Selenium before starting your AltTester® tests.

        .. tabs::
            .. tab:: C#

                .. literalinclude:: ../_static/examples~/selenium/csharp-selenium.cs
                    :language: c#

            .. tab:: Java

                .. literalinclude:: ../_static/examples~/selenium/java-selenium.java
                    :language: java

            .. tab:: Python

                .. literalinclude:: ../_static/examples~/selenium/python-selenium.py
                    :language: py

            .. tab:: Robot

                .. literalinclude:: ../_static/examples~/selenium/robot-selenium.robot
                    :language: robot

```
