# Advanced Usage
## Build games from the command line

If you have a custom build, from your game, you can add the following two lines to your build method:
```
AltUnityBuilder.AddAltUnityTesterInScritpingDefineSymbolsGroup(BuildTargetGroup.Android);
AltUnityBuilder.InsertAltUnityInScene(FirstSceneOfTheGame);
```

```eval_rst
.. note::

    Change `BuildTargetGroup` above to the target group for which you are building.
```

If you want to build a game from the command line the following example script can be used.  
It sets all the project settings needed and uses the same two important lines from above.
 
```eval_rst
.. include:: other/build-from-command-line.txt
    :code: c#
    
```
 
This is our method to build for Android that we call with the following command:
```eval_rst
.. code-block:: bash

    <UnityPath>/Unity -projectPath $CI_PROJECT_DIR -executeMethod BuilderClass.BuildFromCommandLine -logFile logFile.log -quit
```
 
You can find more information about the build command and arguments [here](https://docs.unity3d.com/Manual/CommandLineArguments.html).  

## Connect to AltUnity server running inside the game (localhost, external)

### What is port forwarding and when to use it

Port forwarding is the process of intercepting data sent to a computer's IP/port combination and redirecting it to a different IP and/or port.

In the case of AltUnity Tester, the server is running on port 13000 (default port but can be changed) and port forwarding is needed if another process is already using this port.

```eval_rst
.. image:: ../_static/images/port-forwarding.png
   :scale: 100 %
   :alt: port forwarding image
   :align: center
   :target: `Connect to AltUnity server running inside the game (localhost, external)`_
```

In the example above IP 10.0.0.1 sends a request to 10.0.0.3 on port 80. This request is intercepted by 10.0.0.2 and redirected to 10.0.0.4 on port 81.
Response from 10.0.0.4:81 is sent to 10.0.0.2 which sends it to 10.0.0.1.  
10.0.0.1 sees the request and response as coming from 10.0.0.3.

### Setup Port Forwarding

See instructions below based on platform used.

```eval_rst
.. tabs::

    .. tab:: Android
        
        - Install ADB - for more information check this `article <https://www.xda-developers.com/install-adb-windows-macos-linux/>`_.
        - Forward the port using the following command: 

            ``adb [-s UDID] forward tcp:local_port tcp:device_port``

    .. tab:: iOS
        
        - Install IProxy: ``brew install libimobiledevice``
        - Forward the port using the following command: 
        
            ``iproxy LOCAL_TCP_PORT DEVICE_TCP_PORT [UDID]``
        

```

Setup port forwarding in AltUnity test:

```eval_rst
.. tabs::

    .. tab:: C#

        In AltUnityPortHandler class you can find the following static methods:
        
        - ForwardAndroid (string deviceId = "", int localPort = 13000, int remotePort = 13000)
        - RemoveForwardAndroid (int localPort = -1, string deviceId = "")
        - ForwardIos (string id = "", int localPort = 13000, int remotePort = 13000)
        - KillIProxy (int id)

    .. tab:: Java

        In AltUnityDriver class you can find the following static methods:

        - setupPortForwarding (String platform, String deviceId, int local_tcp_port, int remote_tcp_port)
        - removePortForwarding ()

    .. tab:: Python

        In runner file you can find the following classes:
        
        - AltUnityAndroidPortForwarding with the following methods:
            
            - forward_port_device (self, local_port = 13000, device_port = 13000, device_id = "")
            - remove_forward_port_device (self, port = 13000, device_id = "")
            - remove_all_forwards (self):
        
        - AltUnityiOSPortForwarding with the following methods:

            - forward_port_device (local_port = 13000, device_port = 13000, device_id = "")
            - kill_iproxy_process (pid)  // pid is returned by forward_port_device
            - kill_all_iproxy_process()

```

### Connect through port forwarding 

You can connect directly through an IP address if port 13000 is available and the IP address is reachable.

If port 13000 is not available port forwarding can be used. In this case the computer must be in the same network or have a public IP address.

Forwarding the port on which the game is running on, in this case 13000, to a port that is free on your computer can be done using the following code:

```eval_rst
.. tabs::
    .. code-tab:: c#

            altUnityDriver = new AltUnityDriver ("deviceIp", 13000); 

    .. code-tab:: java

            altUnityDriver = new AltUnityDriver ("deviceIp", 13000, ";", "&", true);  

    .. code-tab:: py

            cls.altdriver = AltrunUnityDriver (None, 'android', 'deviceIp', 13000)
```

```eval_rst
.. note::
    If running locally, deviceIp is `127.0.0.1` and connection is done automatically.
```

After you have done the port forwarding, you can use the driver in your tests to send commands to the server and receive information about the game.

The same steps are valid if you want to test on an iOS device. If you want to test a build on the same computer, then you just have to instantiate the driver to connect to Localhost and the port on which the game is running.

## Connect multiple mobile devices

For two devices you have to do the same steps above, by [connecting through port forwarding](advanced-usage.html#connect-through-port-forwarding) twice.  
So, in the end, you will have:

- 2 devices, each with one AltUnity server
- 1 computer with two drivers

Then, in your tests, you will send commands from each of the drivers.

The same happens with n devices, repeat the steps n times.

*If you want to run two builds on the same device you will need to change the port. For example, you will build a game that runs on 13000 and another one that runs on 13001.*


