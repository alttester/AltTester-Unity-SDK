# Errors

Every failure AltDriver reports is an exception derived from `AltException`. Catching that base
class catches everything AltTester® raises; catching a subclass lets you react to one kind of
failure without swallowing the rest.

```eval_rst
.. note::
    The names below are the Python names. The C#, Java and Robot Framework bindings raise the
    equivalent type with the naming convention of that language — ``ObjectNotFoundException`` in
    C# and Java, for example.
```

## When a test cannot find something

All of these derive from `NotFoundException`, so catching that one covers the whole group. This is
the most common family in day-to-day use.

```eval_rst
.. list-table::
   :widths: 38 62
   :header-rows: 1

   * - Exception
     - Raised when
   * - ``ObjectNotFoundException``
     - No object matched the selector. The usual causes are a wrong selector, an object that is
       inactive while ``enabled`` is ``True``, or the object living in a scene that is not loaded.
   * - ``SceneNotFoundException``
     - The named scene does not exist in the build. Scenes must be included in the build even when
       they are loaded from an AssetBundle.
   * - ``CameraNotFoundException``
     - No camera matched ``cameraBy``/``cameraValue``. Screen coordinates are calculated through a
       camera, so a wrong camera selector fails the whole find.
   * - ``ComponentNotFoundException``
     - The object exists but carries no component of that name. Check the assembly as well as the
       component name — the Inspector shows both for a selected object.
   * - ``PropertyNotFoundException``
     - The component exists but has no such property.
   * - ``MethodNotFoundException``
     - The component exists but has no such method.
   * - ``MethodWithGivenParametersNotFoundException``
     - A method of that name exists, but not with the parameter list you supplied. Overloads are
       matched on their parameters.
   * - ``AssemblyNotFoundException``
     - The named assembly is not present in the build.
```

## When a wait runs out

```eval_rst
.. list-table::
   :widths: 38 62
   :header-rows: 1

   * - Exception
     - Raised when
   * - ``WaitTimeOutException``
     - A ``WaitFor…`` command reached its ``timeout`` without the condition becoming true. The app
       is alive and answering; the thing being waited for did not happen.
   * - ``CommandResponseTimeoutException``
     - The app did not answer a single command within the command response timeout. This is a
       transport-level failure, not a condition that failed — the app is busy, frozen, or gone.
```

Telling these two apart is the fastest diagnostic you have. `WaitTimeOutException` points at your
test or the game state; `CommandResponseTimeoutException` points at the app or the connection. See
`Writing reliable tests <../guides/writing-reliable-tests.html>`_ for how the two timeouts relate.

## When the connection is the problem

All of these derive from `ConnectionError`.

```eval_rst
.. list-table::
   :widths: 44 56
   :header-rows: 1

   * - Exception
     - Raised when
   * - ``ConnectionTimeoutError``
     - The driver could not reach the AltTester® Server within its connection ``timeout``.
   * - ``NoAppConnected``
     - The server is reachable but no instrumented app is connected to it.
   * - ``AppDisconnectedError``
     - The app closed the connection or disconnected unexpectedly mid-run.
   * - ``MultipleDriverError``
     - Another driver is already connected. Lite accounts allow one driver at a time.
   * - ``MultipleDriversTryingToConnectException``
     - Two drivers attempted to connect simultaneously.
   * - ``MaxNoOfConnectionsDriversExceededException``
     - The number of drivers allowed by your licence is already connected.
```

```eval_rst
.. note::
    ``NoAppConnected`` and ``MultipleDriverError`` are licence- and setup-shaped, not test-shaped.
    If they appear mid-suite rather than at startup, check whether an earlier test left a driver
    open — call ``Stop()`` in your tear-down.
```

## When a command is rejected

```eval_rst
.. list-table::
   :widths: 44 56
   :header-rows: 1

   * - Exception
     - Raised when
   * - ``PropertyCannotBeSetException``
     - The property was found but its value could not be updated.
   * - ``InvalidPathException``
     - A ``By.PATH`` selector could not be parsed. See the path syntax in
       `BY-Selector <commands.html#by-selector>`_.
   * - ``InvalidCommandException``
     - The command itself was not valid for this app.
   * - ``FailedToParseArgumentsException``
     - AltTester® could not parse the arguments given to a method.
   * - ``CouldNotParseJsonStringException``
     - AltTester® could not parse a JSON command.
   * - ``CouldNotPerformOperationException``
     - The operation was understood but could not be carried out.
   * - ``NullReferenceException``
     - A null object reference was dereferenced inside the app.
   * - ``WrongAltObjectTypeException``
     - The object is not of the type the command expects — for example addressing a Unity
       UI Toolkit element with a command meant for a GameObject.
   * - ``AltTesterInputModuleException``
     - The input module failed to perform the requested input.
```

## When the driver rejects your arguments

These are raised by the bindings before anything reaches the app, and they also derive from
Python's `TypeError` and `ValueError` respectively, so existing handlers still catch them.

```eval_rst
.. list-table::
   :widths: 44 56
   :header-rows: 1

   * - Exception
     - Raised when
   * - ``InvalidParameterTypeException``
     - A parameter has the wrong type. The message names the parameter, the expected types and
       what was received.
   * - ``InvalidParameterValueException``
     - A parameter has the right type but an unusable value.
   * - ``AltTesterInvalidServerResponse``
     - The server replied with something other than the expected response. The message shows both.
   * - ``FormatException``
     - A value could not be formatted as required.
   * - ``UnknownErrorException``
     - An unexpected error occurred. If you can reproduce this one, please report it — see
       `Contributing <contributing.html>`_.
```

## Catching them

Catch the narrowest class that expresses what you mean. Catching `AltException` in a test body
will also swallow connection failures and argument mistakes, which you almost never want.

```eval_rst
.. tabs::

    .. group-tab:: C#

        .. code-block:: c#

            // Only the object is in question - let connection failures fail the test.
            try
            {
                altDriver.WaitForObject(By.NAME, "StartButton", timeout: 10);
            }
            catch (WaitTimeOutException)
            {
                Assert.Fail("Main menu did not finish loading within 10s.");
            }

    .. group-tab:: Python

        .. code-block:: python

            from alttester.exceptions import WaitTimeOutException

            try:
                alt_driver.wait_for_object(By.NAME, "StartButton", timeout=10)
            except WaitTimeOutException:
                self.fail("Main menu did not finish loading within 10s.")
```
