# Reverse port forwarding

## What is reverse port forwarding and when to use it

Reverse port forwarding, is the behind-the-scenes process of intercepting
data traffic and redirecting it from a device's IP and/or port to the computer's IP and/or port.

When you run your app instrumented with AltTester® Unity SDK on a device, you need
to tell your build how to connect to the AltTester® Server.

Reverse port forwarding can be set up either through the command line or in the
test code by using the methods available in the AltTester® SDK classes.

The following are some cases when reverse port forwarded is needed:

1. [Connect to the app running on a USB connected device](#connect-to-the-app-running-on-a-usb-connected-device)
2. [Connect to multiple devices running the app](#connect-to-multiple-devices-running-the-app)

### How to setup reverse port forwarding

#### In case of Android

Reverse port forwarding can be set up in two ways:

- through the command line using ADB
- in the test code by using the methods available in the AltTester® SDK classes

All methods listed above require that you have ADB installed.

For further information including how to install ADB, check [this article](https://developer.android.com/studio/command-line/adb).

#### In case of iOS

Unfortunately, IProxy does not have a way of setting up reverse port forwarding. As a workaround, to connect the device via USB you should follow the steps below:
- set the iOS device as a Personal Hotspot 
- enable Hotspot via USB on the machine running the AltTester® Server 
  - for this to work, you need to make sure that you have the `Disable unless needed` toggle disabled in the Network settings for the USB connection
  ```eval_rst
    .. image:: ../../_static/img/advanced-usage/connect-via-hotspot-USB_iOS.png
  ```
  - the hotspot network and the first device to connect to it are most of the time on `172.20.10.2` so you could set this IP for builds for iOS
- add the IP of the machine running the AltTester® Server to the first input field in the green popup from the instrumented app/game

In the routing table, the personal hotspot network would be secondary, therefore the traffic shouldn't be redirected through the hotspot:

```eval_rst
    .. image:: ../../_static/img/advanced-usage/workaround_iOS.png
```

```eval_rst
.. tabs::

    .. tab:: Command Line

        .. tabs::

            .. tab:: Android

                - Reverse port forwarding using the following command::

                    adb [-s UDID] reverse tcp:device_port tcp:local_port.

            .. tab:: iOS

                - Not available. A workaround is described above.

    .. tab:: C#

        .. tabs::

            .. tab:: Android

                Use the following static methods from the **AltReversePortForwarding** class in your test file:

                    - **ReversePortForwardingAndroid**
                    
                        .. code-block:: c#

                            ReversePortForwardingAndroid(int remotePort = 13000, int localPort = 13000, string deviceId = "", string adbPath = "")
                     
                    - **RemoveReversePortForwardingAndroid**

                        .. code-block:: c#

                            RemoveReversePortForwardingAndroid(int remotePort = 13000, string deviceId = "", string adbPath = "")

                Example test file:

                    .. literalinclude:: ../../_static/examples~/common/csharp-android-test.cs
                        :language: c#

            .. tab:: iOS

                Not available. A workaround is described above.

    .. tab:: Java

        .. tabs::

            .. tab:: Android

                Use the following static methods from the **AltReversePortForwarding** class in your test file:

                    - **reversePortForwardingAndroid**
                                        
                        .. code-block:: java

                            reversePortForwardingAndroid(int remotePort = 13000, int localPort = 13000, string deviceId = "", string adbPath = "")
                     
                    - **removeReverseForwardingAndroid**

                        .. code-block:: java

                            removeReverseForwardingAndroid(int remotePort = 13000, string deviceId = "", string adbPath = "")

                Example test file:

                    .. literalinclude:: ../../_static/examples~/common/java-android-test.java
                        :language: java

            .. tab:: iOS

                Not available. A workaround is described above.

    .. tab:: Python

        .. tabs::

            .. tab:: Android

                Use the following static methods from the **AltReversePortForwarding** class in your test file:

                    - **reverse_port_forwarding_android**
                    
                        .. code-block:: py

                            reverse_port_forwarding_android(device_port = 13000, local_port = 13000, device_id = "")
                     
                    - **remove_reverse_port_forwarding_android**
                    
                        .. code-block:: py

                            remove_reverse_port_forwarding_android(device_port = 13000, device_id = "")

                Example test file:

                    .. literalinclude:: ../../_static/examples~/common/python-android-test.py
                        :language: py

            .. tab:: iOS

                Not available. A workaround is described above.

    .. tab:: robot

        .. tabs::

            .. tab:: Android

                Use the following static methods from the **AltReversePortForwarding** class in your test file:

                    - **Reverse Port Forwarding Android**
                        .. code-block:: robot

                            Reverse Port Forwarding Android    device_port=13000    local_port=13000    device_id=your_device_id 
                    
                    - **Remove Reverse Port Forwarding Android**
                        .. code-block:: robot

                            Remove Reverse Port Forwarding Android    device_port=13000

                Example test file:

                    .. literalinclude:: ../../_static/examples~/common/robot-android-test.robot
                        :language: robot
                        :emphasize-lines: 26, 31

            .. tab:: iOS

                Not available. A workaround is described above.

```

```eval_rst
.. note::
    The default port on which the AltTester® Unity SDK is running is 13000.
    The port can be changed from the green popup. Make sure to press `Restart` after modifying its value.
```
