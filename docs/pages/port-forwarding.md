# Port Forwarding
For port fowarding we are using the following commands:  
Android: `adb [-s UDID] forward tcp:local_port tcp:device_port`  
iOS: `iproxy LOCAL_TCP_PORT DEVICE_TCP_PORT [UDID]`

## Instalation

### ADB

To install ADB you can get from Android SDK. For more information about this check this [articles](https://www.xda-developers.com/install-adb-windows-macos-linux/)

### IProxy

To install IProxy on your make use the following command:  
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

        With this method you can handle port forwarding logic from tests.

        Example:
        ``` c#

        ```

    .. tab:: java

        In AltUnityPortHandler class you can find the following static methods:

        - ForwardAndroid(string deviceId="",int localPort=13000,int remotePort=13000)
        - RemoveForwardAndroid(int localPort=-1,string deviceId="")
        - ForwardIos(string id="",int localPort=13000,int remotePort=13000)
        - KillIProxy(int id)

        With this method you can handle port forwarding logic from tests.

        Example:
        ``` java

        ```

    .. tab:: python

        In AltUnityPortHandler class you can find the following static methods:
        - ForwardAndroid(string deviceId="",int localPort=13000,int remotePort=13000)
        - RemoveForwardAndroid(int localPort=-1,string deviceId="")
        - ForwardIos(string id="",int localPort=13000,int remotePort=13000)
        - KillIProxy(int id)

        With this method you can handle port forwarding logic from tests.

        Example:
        ``` py

        ```


    .. code-tab:: c#
    
       [Test]
           public void TestWaitForObjectToNotExistFail()
           {
               try
               {
                   altUnityDriver.WaitForObjectNotBePresent(By.NAME,"Capsule", timeout: 1, interval: 0.5f);
                   Assert.Fail();
               }
               catch (WaitTimeOutException exception)
               {
                   Assert.AreEqual("Element //Capsule still found after 1 seconds", exception.Message);
               }
           }

    .. code-tab:: java

        //TODO


    .. code-tab:: py
        def test_wait_for_object(self):
            altElement=self.altdriver.wait_for_object(By.NAME,"Capsule")
            self.assertEqual(altElement.name,"Capsule")

```