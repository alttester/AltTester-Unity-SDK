# Other commands

## Other

### SetServerLogging

Sets the level of logging on AltTester® Unity SDK.

**_Parameters_**

```eval_rst
.. list-table:: SetServerLogging Parameters
   :widths: 20 20 40
   :header-rows: 1

   * - Name
     - Type
     - Description
   * - ``logger``
     - AltLogger
     - The type of logger.
   * - ``logLevel``
     - AltLogLevel
     - The logging level.
```

**_Returns_**

-   Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        altDriver.SetServerLogging(AltLogger.File, AltLogLevel.Off);
        altDriver.SetServerLogging(AltLogger.Unity, AltLogLevel.Info);

    .. code-tab:: java

        altDriver.setServerLogging(AltLogger.File, AltLogLevel.Off);
        altDriver.setServerLogging(AltLogger.Unity, AltLogLevel.Info);

    .. code-tab:: py

        alt_driver.set_server_logging(AltLogger.File, AltLogLevel.Off);
        alt_driver.set_server_logging(AltLogger.Unity, AltLogLevel.Info);

    .. code-tab:: robot

        Test Set Server Logging
            ${param}=    Create List    AltServerFileRule
            ${rule}=    Call Static Method    AltTester.AltTesterUnitySDK.Logging.AltTesterLogManager    Instance.Configuration.FindRuleByName    Assembly-CSharp    parameters=${param}
            ${levels}=    Get From Dictionary    ${rule}    Levels
            ${levels_number}=    Get Length    ${levels}
            Should Be Equal As Integers    ${levels_number}    5
            Set Server Logging    File    Off
            ${rule}=    Call Static Method    AltTester.AltTesterUnitySDK.Logging.AltTesterLogManager    Instance.Configuration.FindRuleByName    Assembly-CSharp    parameters=${param}
            ${levels}=    Get From Dictionary    ${rule}    Levels
            ${levels_number}=    Get Length    ${levels}
            Should Be Equal As Integers    ${levels_number}    0
            Set Server Logging    File    Debug

```
