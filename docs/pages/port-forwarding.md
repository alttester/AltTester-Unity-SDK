# Port Forwarding

For port fowarding we are using the following commands:  
Android: `adb [-s UDID] forward tcp:local_port tcp:device_port`  
iOS: `iproxy LOCAL_TCP_PORT DEVICE_TCP_PORT [UDID]`

## Instalation

### ADB

To install ADB you can get from Android SDK. For more information check this [article](https://www.xda-developers.com/install-adb-windows-macos-linux/)

### IProxy

To install IProxy use the following command:  
`brew install libimobiledevice`

## Set port forwarding in test

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