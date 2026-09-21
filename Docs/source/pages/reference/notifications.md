# Notifications

Notifications allow you to listen for specific events in the Unity application under test. To activate a notification, use the `AddNotificationListener` command and specify the appropriate `NotificationType`.

## Scene Loaded (NotificationType.LOADSCENE)
Triggered when a scene is loaded in the Unity app.

**How to activate:**
`AddNotificationListener<AltLoadSceneNotificationResultParams>(NotificationType.LOADSCENE, callback, true)`

**Returns:**
- `sceneName`: Name of the loaded scene
- `loadSceneMode`: How the scene was loaded (Additive or Single)

**Example:**
```eval_rst
.. tabs::
  .. code-tab:: c#
  
    altDriver.AddNotificationListener<AltLoadSceneNotificationResultParams>(NotificationType.LOADSCENE, (result) => {
      Debug.Log($"Scene loaded: {result.sceneName}, Mode: {result.loadSceneMode}");
    }, true);

  .. code-tab:: java

    altDriver.addNotificationListener(NotificationType.LOADSCENE, (AltLoadSceneNotificationResultParams result) -> {
      System.out.println("Scene loaded: " + result.sceneName + ", Mode: " + result.loadSceneMode);
    }, true);

  .. code-tab:: py

    def on_scene_loaded(result):
      print(f"Scene loaded: {result.scene_name}, Mode: {result.load_scene_mode}")
    alt_driver.add_notification_listener(NotificationType.LOADSCENE, on_scene_loaded, True)

  .. code-tab:: robot

        *** Settings ***
        Library    AltTesterLibrary

        *** Test Cases ***
        Listen For Scene Loaded
            Add Notification Listener    LOADSCENE    Log Scene Loaded    overwrite=${True}
            # ... trigger scene load ...
            Remove Notification Listener    LOADSCENE

        Log Scene Loaded
            [Arguments]    ${result}
            Log    Scene loaded: ${result.sceneName}, Mode: ${result.loadSceneMode}

```

## Scene Unloaded (NotificationType.UNLOADSCENE)
Triggered when a scene is unloaded in the Unity app.

**How to activate:**
`AddNotificationListener<string>(NotificationType.UNLOADSCENE, callback, true)`

**Returns:**
- `sceneName`: Name of the unloaded scene

**Example:**
```eval_rst
.. tabs::
  .. code-tab:: c#
  
    altDriver.AddNotificationListener<string>(NotificationType.UNLOADSCENE, (sceneName) => {
      Debug.Log($"Scene unloaded: {sceneName}");
    }, true);

  .. code-tab:: java

    altDriver.addNotificationListener(NotificationType.UNLOADSCENE, (String sceneName) -> {
      System.out.println("Scene unloaded: " + sceneName);
    }, true);

  .. code-tab:: py

    def on_scene_unloaded(scene_name):
      print(f"Scene unloaded: {scene_name}")
    alt_driver.add_notification_listener(NotificationType.UNLOADSCENE, on_scene_unloaded, True)

  .. code-tab:: robot

        *** Settings ***
        Library    AltTesterLibrary

        *** Test Cases ***
        Listen For Scene Unloaded
            Add Notification Listener    NotificationType.UNLOADSCENE    Log Scene Unloaded    overwrite=${True}
            # ... trigger scene unload ...
            Remove Notification Listener    NotificationType.UNLOADSCENE

        Log Scene Unloaded
            [Arguments]    ${sceneName}
            Log    Scene unloaded: ${sceneName}
   
```

## Log Notification (NotificationType.LOG)
Triggered when a log is generated in the Unity app.

**How to activate:**
`AddNotificationListener<AltLogNotificationResultParams>(NotificationType.LOG, callback, true)`

**Returns:**
- `message`: The log message
- `stackTrace`: The stack trace of the log
- `level`: The log level (e.g., Error, Warning, etc.)

**Example:**
```eval_rst
.. tabs::
  .. code-tab:: c#

    altDriver.AddNotificationListener<AltLogNotificationResultParams>(NotificationType.LOG, (log) => {
      Debug.Log($"Log: {log.message}\nLevel: {log.level}\nStackTrace: {log.stackTrace}");
    }, true);

  .. code-tab:: java

    altDriver.addNotificationListener(NotificationType.LOG, (AltLogNotificationResultParams log) -> {
      System.out.println("Log: " + log.message + "\nLevel: " + log.level + "\nStackTrace: " + log.stackTrace);
    }, true);

  .. code-tab:: py

    def on_log(log):
      print(f"Log: {log.message}\nLevel: {log.level}\nStackTrace: {log.stack_trace}")
    alt_driver.add_notification_listener(NotificationType.LOG, on_log, True)

  .. code-tab:: robot

        *** Settings ***
        Library    AltTesterLibrary

        *** Test Cases ***
        Listen For Log Notification
            Add Notification Listener    LOG    Log Notification Callback    overwrite=${True}
            # ... trigger log event ...
            Remove Notification Listener    LOG

        Log Notification Callback
            [Arguments]    ${log}
            Log    Log: ${log.message}\nLevel: ${log.level}\nStackTrace: ${log.stackTrace}
   
```

## Application Paused (NotificationType.APPLICATION_PAUSED)
Triggered when the application is paused or resumed.

**How to activate:**
`AddNotificationListener<bool>(NotificationType.APPLICATION_PAUSED, callback, true)`

**Returns:**
- `applicationPaused`: Boolean indicating if the application is paused

**Example:**
```eval_rst
.. tabs::
  .. code-tab:: c#

    altDriver.AddNotificationListener<bool>(NotificationType.APPLICATION_PAUSED, (paused) => {
      Debug.Log($"Application paused: {paused}");
    }, true);

  .. code-tab:: java

    altDriver.addNotificationListener(NotificationType.APPLICATION_PAUSED, (Boolean paused) -> {
      System.out.println("Application paused: " + paused);
    }, true);

  .. code-tab:: py

    def on_paused(paused):
      print(f"Application paused: {paused}")
    alt_driver.add_notification_listener(NotificationType.APPLICATION_PAUSED, on_paused, True)

  .. code-tab:: robot
  
        *** Settings ***
        Library    AltTesterLibrary

        *** Test Cases ***
        Listen For Application Paused
            Add Notification Listener    APPLICATION_PAUSED    Log Application Paused    overwrite=${True}
            # ... trigger pause event ...
            Remove Notification Listener    APPLICATION_PAUSED

        Log Application Paused
            [Arguments]    ${paused}
            Log    Application paused: ${paused}
    
```
