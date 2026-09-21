# Timeouts and delays

## SetCommandResponseTimeout

Sets the value for the command response timeout.

**_Parameters_**

```eval_rst
.. list-table:: SetCommandResponseTimeout Parameters
   :widths: 15 10 10 65
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``commandTimeout``
     - int
     - Yes
     - The duration for a command response from the driver.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        altDriver.SetCommandResponseTimeout(commandTimeout);

    .. code-tab:: java

        altDriver.setCommandResponseTimeout(commandTimeout);

    .. code-tab:: py

        alt_driver.set_command_response_timeout(command_timeout)

    .. code-tab:: robot

        Set Command Response Timeout    30

```



## GetDelayAfterCommand

Gets the current delay after a command.

**_Parameters_**

None

**_Returns_**

- The current delay after a command.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        altDriver.GetDelayAfterCommand();

    .. code-tab:: java

        altDriver.getDelayAfterCommand();

    .. code-tab:: py

        alt_driver.get_delay_after_command()

    .. code-tab:: robot

        Get Delay After Command

```


## SetDelayAfterCommand

Set the delay after a command.

**_Parameters_**

```eval_rst
.. list-table:: SetDelayAfterCommand Parameters
   :widths: 15 10 10 65
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``delay``
     - int
     - Yes
     - The new delay after a command.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        altDriver.SetDelayAfterCommand(5);

    .. code-tab:: java

        altDriver.setDelayAfterCommand(5);

    .. code-tab:: py

        alt_driver.set_delay_after_command(5)

    .. code-tab:: robot

        Set Delay After Command     5

```


## SetImplicitTimeout

Sets an implicit timeout for all commands that use the `timeout` parameter.

**_Parameters_**

```eval_rst
.. tabs::

    .. tab:: C#

        .. list-table:: SetImplicitTimeout Parameters
           :widths: 15 10 10 65
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - timeout
             - positive double
             - Yes
             - The value of the new timeout. By default it is 20 seconds. Throws `ArgumentOutOfRangeException` in case of a negative timeout.

    .. tab:: Java

        .. list-table:: setImplicitTimeout Parameters
           :widths: 15 10 10 65
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - timeout
             - positive double
             - Yes
             - The value of the new timeout. By default it is 20 seconds. Throws `IllegalArgumentException` in case of a negative timeout. 

    .. tab:: Python

        .. list-table:: set_implicit_timeout Parameters
           :widths: 15 10 10 65
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - timeout
             - positive double
             - Yes
             - The value of the new timeout. By default it is 20 seconds. Raises `ValueError` in case of a negative timeout.

    .. tab:: Robot

        .. list-table:: Set Implicit Timeout Parameters
           :widths: 15 10 10 65
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - timeout
             - positive double
             - Yes
             - The value of the new timeout. By default it is 20 seconds. Raises `ValueError` in case of a negative timeout.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        altDriver.SetImplicitTimeout(5);

    .. code-tab:: java

        altDriver.setImplicitTimeout(5);

    .. code-tab:: py

        alt_driver.set_implicit_timeout(5)

    .. code-tab:: robot

        Set Implicit Timeout    5

```


## GetImplicitTimeout

Gets the implicit timeout for all commands that use the `timeout` parameter.

**_Parameters_**

None

**_Returns_**

- The value of the implicit timeout in seconds.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        altDriver.GetImplicitTimeout();

    .. code-tab:: java

        altDriver.getImplicitTimeout();

    .. code-tab:: py

        alt_driver.get_implicit_timeout()

    .. code-tab:: robot

        Get Implicit Timeout

```
