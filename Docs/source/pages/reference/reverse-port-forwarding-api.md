# Reverse port forwarding API

## AltReversePortForwarding

API to interact with `adb` programmatically.

### ReversePortForwardingAndroid

This method calls `adb reverse [-s {deviceId}] tcp:{remotePort} tcp:{localPort}`.

**_Parameters_**

```eval_rst
.. list-table:: ReversePortForwardingAndroid Parameters
   :widths: 20 20 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``remotePort``
     - int
     - No
     - The device port to do reverse port forwarding from.
   * - ``localPort``
     - int
     - No
     - The local port to do reverse port forwarding to.
   * - ``deviceId``
     - string
     - No
     - The id of the device.
   * - ``adbPath``
     - string
     - No
     - The adb path. If no adb path is provided, it tries to use adb from `${ANDROID_SDK_ROOT}/platform-tools/adb`. If `ANDROID_SDK_ROOT` env variable is not set, it tries to execute adb from `PATH`.
```

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [OneTimeSetUp]
        public void SetUp()
        {
            AltReversePortForwarding.ReversePortForwardingAndroid();
            altDriver = new AltDriver();
        }

    .. code-tab:: java

        @BeforeClass
        public static void setUp() throws IOException {
            AltReversePortForwarding.reversePortForwardingAndroid();
            altDriver = new AltDriver();
        }

    .. code-tab:: py

        @classmethod
        def setUpClass(cls):
            AltReversePortForwarding.reverse_port_forwarding_android()
            cls.alt_driver = AltDriver()

    .. code-tab:: robot

        SetUp Tests
            Reverse Port Forwarding Android
            Initialize Altdriver

```
**Note:** Sometimes, the execution of reverse port forwarding method is too slow so the tests fail because the port is not actually forwarded when trying to establish the connection. In order to fix this problem, a `sleep()` method should be called after calling the ReversePortForwardingAndroid() method.

### RemoveReversePortForwardingAndroid

This method calls `adb reverse --remove [-s {deviceId}] tcp:{devicePort}` or `adb reverse --remove-all` if no port is provided.

**_Parameters_**

```eval_rst
.. list-table:: RemoveReversePortForwardingAndroid Parameters
   :widths: 20 20 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``devicePort``
     - int
     - No
     - The device port to be removed.
   * - ``deviceId``
     - string
     - No
     - The id of the device to be removed.
   * - ``adbPath``
     - string
     - No
     - The adb path.
```

**_Returns_**

Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [OneTimeTearDown]
        public void TearDown()
        {
            altDriver.Stop();
            AltReversePortForwarding.RemoveReversePortForwardingAndroid();
        }

    .. code-tab:: java

        @AfterClass
        public static void tearDown() throws Exception {
            altDriver.stop();
            AltReversePortForwarding.removeReversePortForwardingAndroid();
        }

    .. code-tab:: py

        @classmethod
        def tearDownClass(cls):
            cls.alt_driver.stop()
            AltReversePortForwarding.remove_reverse_port_forwarding_android()

    .. code-tab:: robot

        TearDown Tests
            Stop Altdriver
            Remove Reverse Port Forwarding Android

```

### RemoveAllReversePortForwardingsAndroid

This method calls `adb reverse --remove-all`.

**_Parameters_**

```eval_rst
.. list-table:: RemoveAllReversePortForwardingsAndroid Parameters
   :widths: 20 20 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``adbPath``
     - string
     - No
     - The adb path.
```

**_Returns_**

Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [OneTimeTearDown]
        public void TearDown()
        {
            altDriver.Stop();
            AltReversePortForwarding.RemoveAllReversePortForwardingsAndroid();
        }

    .. code-tab:: java

        @AfterClass
        public static void tearDown() throws Exception {
            altDriver.stop();
            AltReversePortForwarding.removeAllReversePortForwardingsAndroid();
        }

    .. code-tab:: py

        @classmethod
        def tearDownClass(cls):
            cls.alt_driver.stop()
            AltReversePortForwarding.remove_all__reverse_port_forwardings_android()

    .. code-tab:: robot

        TearDown Tests
            Stop Altdriver
            Remove All Reverse Port Forwarding Android
```
