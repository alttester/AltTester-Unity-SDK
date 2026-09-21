# AltTester® input

AltTester® Unity SDK has an `Input` class which overrides the Input class implemented by Unity. This way AltTester® intercepts the input actions to be performed in the instrumented app and simulates them through this class.
In case you are using assembly definitions inside your project, you will have to reference the `AltTesterUnitySDK.asmdef` in all your .asmdef files which use input actions.

## AltTester® input vs. regular input

AltTester®'s custom input is active, by default, in any instrumented build. This means that certain input related actions (the ones that are part of Unity's `Input` class) will be inactive for regular input (the device's input). Because of this, pressing a key from the keyboard for example will not have any effect on the app. However, the simulated input from the tests, like the `PressKey` command, will be able to manipulate the object within the scene. While the AltTester® input is active, the icon from the right bottom corner is green. You can change this behaviour by clicking on the AltTester®'s icon and unchecking the box with the `AltTester® Input` message. Now the icon will turn darker, signaling that the regular input is active. In this state, you can interfere with the object from the app using the keyboard or other input. Keep in mind that, input actions from the AltTester® Desktop won't have any effect while regular input is active. At the same time, if you want to run some automated tests, the AltTester® input will be activated automatically for you.
