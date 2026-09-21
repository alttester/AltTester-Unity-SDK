# Release mode and code stripping

## Using AltTester® Unity SDK in Release mode

By default AltTester® Unity SDK does not run in release mode. We recommended that you do not instrument your Unity application in release mode with AltTester® Unity SDK. That being said, if you do want to instrument your application in release mode, you need to uncheck `RunOnlyInDebugMode` flag on the AltRunner script inside AltTester® Unity SDK asset folder `AltTester/Runtime/Prefab/AltTesterPrefab.prefab`


## Code Stripping

AltTester® Unity SDK is using reflection in some of the commands to get information from the instrumented application. If you application is using IL2CPP scripting backend then it might strip code that you would use in your tests. If this is the case we recommend creating an `link.xml` file. More information on how to manage code stripping and create an `link.xml` file is found in [Unity documentation](https://docs.unity3d.com/Manual/ManagedCodeStripping.html)
