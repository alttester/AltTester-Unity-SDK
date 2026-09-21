# Run your app

## Start the AltTester® Server Module

The AltTester® Server Module is incorporated in AltTester® Desktop. In order to start it, all you have to do is to open AltTester® Desktop.


## Run your app in Unity or on desired platform

Before running your tests you need to start the instrumented Unity application. Upon startup, your instrumented Unity app should display a popup with the message: "Waiting for connections on port: {Port}". The popup disappears when your app has successfully connected to the tests.

```eval_rst

.. tabs::

    .. tab:: Unity Editor

        1. Open AltTester® Editor
        2. In platform section select Editor
        3. Click Play in Editor

    .. tab:: PC

        1. Open AltTester® Editor
        2. In platform section select Standalone
        3. Choose your build target
        4. Click Build & Run

        .. important::

            Make sure to set the "Api Compatibility Level" to ".NET 4.x" in Unity versions lower than 2021 when building using the Standalone option.

            This setting can be found under Edit menu -> Project Settings -> Player -> Other Settings -> Configuration.

    .. tab:: Android

        Prerequisites:

        * Use the Unity Hub to install Android Build Support and the required dependencies: Android SDK & NDK tools, and OpenJDK

        Steps:

        1. Open AltTester® Editor
        2. In platform section select Android
        3. Click Build & Run


    .. tab:: iOS

        Prerequisites:

        * Use the Unity Hub to install iOS Build Support

        Steps:

        1. Open AltTester® Editor
        2. In platform section select iOS
        3. Click Build & Run

        .. note::

            Check the following link to see how to build and run your app for iOS (.ipa file) -- :alttesteriphoneblog:`link <>`

    .. tab:: WebGL

        Prerequisites:

        * Use the Unity Hub to install WebGL Build Support

        Steps:

        1. Open AltTester® Editor
        2. In platform section select WebGL
        3. Click Build & Run

.. note::

    You can switch between the regular and the AltTester® input by toggling the box with the `AltTester® Input` label. Take into consideration that if you are using the New Input System, then after activating the AltTester® input, you will only be able to interact with the instrumented build via your automated tests or the AltTester® Desktop.

```
