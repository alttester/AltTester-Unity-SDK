## AltDriver

The `AltDriver` is how your tests drive the app. Creating one opens a connection to the
AltTester® Server, which routes it to a connected instrumented app; calling `Stop` closes it.

Create one in your set-up and stop it in your tear-down.

```eval_rst
.. tabs::

    .. group-tab:: C#

        .. code-block:: c#

            altDriver = new AltDriver();
            // ...
            altDriver.Stop();

    .. group-tab:: Python

        .. code-block:: python

            alt_driver = AltDriver()
            # ...
            alt_driver.stop()
```

### Which app does the driver connect to?

The server can have several instrumented apps connected at once. The driver does not name one
directly — it sends the criteria below when it connects, and the server routes it to an app whose
own reported details match.

That is the key thing to understand about these parameters: **they are filters describing the app
you want, not values you assign to it.** The values themselves come from elsewhere:

- The **app** reports its own `platform`, `platformVersion` and `deviceInstanceId` when it
  registers with the server. They come from the device it is running on, which is why they have
  real values you never set — for example on a cloud device farm.
- **`appId`** is assigned by the server to uniquely identify one connected app.
- **`appName`** is the one value you control from the app side: it is set in the AltTester®
  settings of the instrumented build, and it is arbitrary. Pick something meaningful and match it
  in your tests.

Any criterion left at its default of `unknown` is not used for filtering — it matches any value.
With one app connected you can leave them all alone; `AltDriver()` with no arguments is the
normal case.

You can read the values of every connected app from the Connected Apps list in AltTester® Desktop,
and copy them straight into your tests.

```eval_rst
.. note::
    To tell two runs of the *same* app apart — two devices in a matrix, or two suites in
    parallel — give each instrumented build a distinct ``appName``, or select on
    ``deviceInstanceId``. Filtering on ``appName`` alone cannot distinguish two identical builds.
```

### Parameters

```eval_rst
.. list-table::
   :widths: 22 10 12 56
   :header-rows: 1

   * - Name
     - Type
     - Default
     - Description
   * - ``host``
     - string
     - ``127.0.0.1``
     - Host the AltTester® Server is listening on. Use the IP of the machine running AltTester®
       Desktop when your app runs on a device.
   * - ``port``
     - int
     - ``13000``
     - Port the AltTester® Server is listening on. Must match the port configured in the
       instrumented build.
   * - ``appName``
     - string
     - ``__default__``
     - Name of the app to connect to, as set in the instrumented build's AltTester® settings.
       ``__default__`` is the SDK's own default, so it matches a build whose name was never
       changed.
   * - ``enableLogging``
     - boolean
     - ``false``
     - Turns on driver-side logging. See `AltDriver logging <../guides/logging.html#altdriver-logging>`_.
   * - ``timeout``
     - int, float
     - ``60``
     - Seconds to wait for the connection to be established. Set to ``None`` to wait
       indefinitely. Raises ``ConnectionTimeoutError`` if it elapses. This governs connecting
       only — not how long later commands may take.
   * - ``platform``
     - string
     - ``unknown``
     - Connect only to an app reporting this platform, for example ``Android`` or ``WindowsPlayer``.
       ``unknown`` matches any platform.
   * - ``platformVersion``
     - string
     - ``unknown``
     - Connect only to an app reporting this platform version. ``unknown`` matches any version.
   * - ``deviceInstanceId``
     - string
     - ``unknown``
     - Connect only to an app running on this device. Useful when the same build runs on several
       devices at once. ``unknown`` matches any device.
   * - ``appId``
     - string
     - ``unknown``
     - Connect only to the app with this server-assigned id. Use it to disambiguate when the other
       criteria still match more than one app. ``unknown`` lets the server resolve the app from the
       remaining criteria.
```

```eval_rst
.. note::
    Parameter names follow each language's convention — ``appName`` in C# and Java,
    ``app_name`` in Python, ``App Name`` in Robot Framework. The meanings and defaults are
    identical across all four.
```

### What it raises

```eval_rst
.. list-table::
   :widths: 40 60
   :header-rows: 1

   * - Exception
     - Raised when
   * - ``ConnectionTimeoutError``
     - The server could not be reached within ``timeout``.
   * - ``NoAppConnected``
     - The server is reachable, but no instrumented app matches the criteria given.
   * - ``MultipleDriverError``
     - A driver is already connected and your licence allows only one.
   * - ``MaxNoOfConnectionsDriversExceededException``
     - Your licence's driver limit is already in use — often an abandoned driver from an earlier
       run that was never stopped.
```

See [Errors](errors.md) for the full list.
