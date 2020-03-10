# How to connect to the server from the tests

Let's say that you have a game working with AltUnity Tester on an Android phone and want to run the test from a computer. On the Android device the server is running on port 13000(default port but can be changed). To connect to the server you have two options:

## Connect through port forwarding

 Connect the device with a USB cable to the computer. Then [forward the port](port-forwarding.html#port-forwarding) that the game is running on, in this case, 13000, to a port that is free on your computer. Then you can instantiate the driver on Localhost and the free port on your computer that you set.

```eval_rst
.. tabs::
    .. code-tab:: c#

            altUnityDriver = new AltUnityDriver("127.0.0.1", 13000);
    .. code-tab:: java

            altUnityDriver = new AltUnityDriver("127.0.0.1", 13000, ";", "&", true);
    .. code-tab:: py

            cls.altdriver = AltrunUnityDriver(None, 'android','127.0.0.1',13000)
```
## Connect directly through IP

  Instantiate the driver giving the port that the game is running on and the device IP. In this case, the device and the computer must be in the same network or have public IPs.
  
```eval_rst
.. tabs::
    .. code-tab:: c#

            altUnityDriver = new AltUnityDriver("deviceIp", 13000);

    .. code-tab:: java

            altUnityDriver = new AltUnityDriver("deviceIp", 13000, ";", "&", true;

    .. code-tab:: py

            cls.altdriver = AltrunUnityDriver(None, 'android','deviceIp',13000)
```

After you have done the port forwarding, you can use the driver in your tests to send commands to the server and receive information about the game.

The same steps are valid if you want to test on an iOS device. If you want to test a build on the same computer, then you just have to instantiate the driver to connect to Localhost and the port on which the game is running.

## Connect multiple mobile devices

For two devices you have to do the same steps either [Connect directly through IP](port-forwarding.html#connect-directly-through-ip) or [Connect through port forwarding](port-forwarding.html#connect-through-port-forwarding) twice. So, in the end, you will have:

- 2 devices, each with one AltUnity server
- 1 computer with two drivers

Then, in your tests, you will send commands from each of the drivers.

The same happens with n devices, repeat the steps n times.

*If you want to run two builds on the same device you will need to change the port. For example, you will build a game that runs on 13000 and another one that runs on 13001.*


## Port Forwarding

For port fowarding we are using the following commands:  
Android: `adb [-s UDID] forward tcp:local_port tcp:device_port`  
iOS: `iproxy LOCAL_TCP_PORT DEVICE_TCP_PORT [UDID]`

### Instalation

#### ADB

To install ADB you can get from Android SDK. For more information check this [article](https://www.xda-developers.com/install-adb-windows-macos-linux/)

#### IProxy

To install IProxy use the following command:  
`brew install libimobiledevice`

### Set port forwarding in test

```eval_rst
.. tabs::

    .. tab:: C#

        In AltUnityPortHandler class you can find the following static methods:
        
        - ForwardAndroid(string deviceId="",int localPort=13000,int remotePort=13000)
        - RemoveForwardAndroid(int localPort=-1,string deviceId="")
        - ForwardIos(string id="",int localPort=13000,int remotePort=13000)
        - KillIProxy(int id)

        With this methods you can handle port forwarding logic from tests.

    .. tab:: java

        In AltUnityDriver class you can find the following static methods:

        - setupPortForwarding(String platform,String deviceId, int local_tcp_port, int remote_tcp_port)
        - removePortForwarding()

        With this methods you can handle port forwarding logic from tests.

    .. tab:: python

        In runner file you can find the following classes:
        
        - AltUnityAndroidPortForwarding with the following methods:
            - forward_port_device(self, local_port=13000, device_port=13000, device_id="")
            - remove_forward_port_device(self, port=13000, device_id="")
            - remove_all_forwards(self):
        
        - AltUnityiOSPortForwarding with the following methods:
            - forward_port_device(local_port=13000, device_port=13000, device_id="")
            - kill_iproxy_process(pid)
                - pid is returned by forward_port_device
            - kill_all_iproxy_process()
    
```