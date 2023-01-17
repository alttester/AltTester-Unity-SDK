# Changes in version 1.8.1

## New

- "New command: Reset Input" !997
- "Add SetStaticProperty Command" !926

## Bug fixes and improvements

- "Change websocket.dll to avoid conflict with other projects" !998
- "InputSystem: Incorrect execution of coroutines for pressKey and other commands" !1008
- "[New Input System] Double execution for EndTouch, BeginTouch, KeyUp and KeyDown commands" !985
- "Change AltUnityCommands and AltUnityNotification names" !990
- "GetScreenshot has duplicated code" !992


## Contributors

-   Gombos Kriszta @GombosKriszta
-   Robert Dezmerean @robert-dezmerean
-   Iuliana Todoran @iuliana.todoran
-   Robert Poienar @robert_poienar
-   Edvin Reich @ReichEdvin
-   Ru Cindrea @ru.cindrea
-   Sandrina-Emanuela Sere @sandrina.sere


# Changes in version 1.8.0

## New

- "Create xml report when running tests from Editor/Command line" !756
- "Create a method to enable/disable AltUnityDriver logging" !742

## Bug fixes and improvements

- "Tests not shown in Tests List if created inside AltUnityTester folder" !755
- "Hovering mouse over button does not show tooltip" !748
- "Set_text() doesn't work for Unity Input Field" !759
- "[Python] move_touch send wrong parameters" !765
- "Change method that uses reflection to have assembly as required parameters" !925
- "[Advanced Usage] [Upgrade Guides] Invalid syntax for some C# examples" !763
- "[AltUnity Tester Editor] Add Section for "Test run Settings" and replace screenshots" !761
- "Some tests fail if Input System v >= 1.4.0" !766
- "[Docs] Upgrade guide has C# examples in Python section" !758

## Contributors

-   Gombos Kriszta @GombosKriszta
-   Robert Dezmerean @robert-dezmerean
-   Iuliana Todoran @iuliana.todoran
-   Robert Poienar @robert_poienar
-   Edvin Reich @ReichEdvin
-   Ru Cindrea @ru.cindrea
-   Sandrina-Emanuela Sere @sandrina.sere
-   Diana Serb @diana.serb
-   Andrei-Mihai Onica @andrei.onica


# Changes in version 1.7.2

## New

- "Multiple keypress with NIS" !721
- "Add input visualizer for NIS" !716
- "Moveable AltUnity logo in Inspector" !730

## Bug fixes and improvements

- "CallComponentMethod does not call private methods" !715
- "[AltUnity Tester Editor > Build Settings] Delete note" !731
- "[Get Started - Run your game in Unity...] Adapt admonition for Unity 2021" !735
- "[API > AltUnityDriver > Input Actions] Add section for PressKeys() command" !737
- "Fix documentation inconsistency" !728
- "[AltUnity Tester Editor > Tests List] Document the use of Refresh button feature" !726
- "Refactor the GetServerVersion command" !720
- "[Inspector - Samplescenes] Click/Press action not working for creating stars" !718

## Contributors

-   Gombos Kriszta @GombosKriszta
-   Andra Cardas @andra.cardas
-   Robert Dezmerean @robert-dezmerean
-   Iuliana Todoran @iuliana.todoran
-   Robert Poienar @robert_poienar
-   Edvin Reich @ReichEdvin
-   Ru Cindrea @ru.cindrea


# Changes in version 1.7.1

## New

- "New commands FindObjectAtCoordinates" !650
- "Documentation on how to add references to newinputsystem and newtonsoft json" !662
- "Add option for multiple key pressing simultaneously" !646
- "Logic for both input system to work together" #768
- "Click with new input system" !639
- "Scroll with new input system" !691
- "MoveMouse with new input system" !688
- "Key up and down with new input system" !685
- "Press key with new input system" !765
- "Add way to set a delay after each altUnity command" !684
- "Add logic for BeginTouch, MoveTouch and EndTouch to support NIS" !704
- "Tap with new input system" !656
- "Swipe with new input system" !707
- "Change server port from the game" !689
- "Tilt with new input system" !712
- "Updates for New Input System" !703

## Bug fixes and improvements

- "Not connecting to Android with PortForwarding on a different port" !652
- "Multiple precompiled assemblies with the same name Newtonsoft.json" !654
- "Fix issue in the python driver where WebsocketConnection class does not check the messageId" !653
- "[API - AltUnityObject] List all commands under main section and remove Object Commands subsection" !670
- "[Examples] Only one list expanded item in table of contents" !672
- "Instrumented build does not connect to AltUnity Inspector/tests" #818
- "[Get Started] "AltUnity Tester Window" or "AltUnity Tester" instead of "AltUnity Tester Editor"" !674
- "SetText with submit true does not trigger the validation event that is triggered by enter" !673
- "[License] Commands typos" !680
- "[SampleScenes-Apk] UI elements not showing" !658
- "Add examples to AltUnityPortForwarding section" !677
- "Add clarifications to the license file" !681
- "[Mac] iOS Player Settings button not functional" #780
- "Logger should be used with full NLog.Logger name" !694
- "AUT Editor Windows slow" !690
- "Compressing the screenshot triggers app block after connecting with Inspector" !695
- "Swipe should not tap elements that are dragged" !705
- "Tests for API commands sometimes fail when Active Input Handling is set to "Input System Package (New)"" !710
- "Instrumented game with AltUnity Tester 1.7.0 hangs in Bluestacks" !702
- "[AltUnityRunnerPrefab] Text elements inconsistencies for Port change feature" !711
- "[Get/SetDelayAfterCommand] Typo in description" !708
- "Connecting Issue with il2cpp builds" !701
- "Fix error thrown when only old input system is used" !706

## Contributors

-   Dorin Oltean @dorinaltom
-   Gombos Kriszta @GombosKriszta
-   Andra Cardas @andra.cardas
-   Robert Dezmerean @robert-dezmerean
-   Iuliana Todoran @iuliana.todoran
-   Robert Poienar @robert_poienar
-   Edvin Reich @ReichEdvin


# Changes in version 1.7.0

## New

-   "Set a global command timeout for commands"	!567
-   "Communication protocol" !419
-   "Communication protocol - Python" !440
-   "Java Communication Protocol"	!512

## Bug fixes and improvements

-   "CallComponentMehtod and CallStaticMethod definitions"	!492
-   "[Get started] Add a note in "Instrument your game with AltUnity Server" section" !494
-   "Invoke input Events such as OnValueChange or OnSubmit on PressKey and SetText"	!495
-   "Communication protocol - Improve error handling" !498
-   "Find a more suggestive name for "API Documentation" !500
-   "From the unity editor, users should have all the info for getting started" !501
-   "Improve the parameter validation in python" !507
-   "rewrite CreateAltunityPrefab" !511
-   "ALTUNITYTESTER scripting symbol not visible in instrumented game - Unity 2020"	!513
-   "Update websocket-sharp" !514
-   "[API] Insert missing commands in documentation" !521
-   "Add option in AUT editor window to keep ALTUNITYTESTER define when apps are not in playmode" !522
-   "[Tests list] Tests checkboxes alignment inconsistency" !523
-   "[API methods] [Docs - API] Change param names of "cameraName" and "cameraPath" in "cameraValue"" !524
-   "Fix AltUnitySetTextCommand.cs error" !525
-   "[Command] Add a new command to retrieve static properties and fields" !527
-   "Add event when scene is loaded" !528
-   "Double clicking a test should open the test file at the line where the test is" !531
-   "[Run tests] Incorrect failed tests number" !532
-   "Handle WaitTimeOutException during Connect" !534
-   "Fix connection timeout error for Python" !535
-   "Logging documentation update" !536
-   "Standardize AltUnityDriver constructor" !538
-   "remove reconnect button from instrumented game" !540
-   "Add documentation for SetTimeScale" !543
-   "Input visualizer not assigned when option is checked in Build Settings" !547
-   "Disconnecting before handshake throws error in Java" !548
-   "[AltUnity Runner prefab popup] Rearange texts to be centered" !550
-   "GetComponentProperty fails when value is null." !552
-   "LoadScene throws null reference exception if name of the scene is not found" !553
-   "Add info about setting Run in Background" !558
-   "The table with Parameters for GetComponentProperty is not entirely visible in docs" !559
-   "Test are not opened and error is thrown in console when class name is different with file name" !561
-   "Remove Duplicate AltUnityId components in Sample Scenes" !562
-   "Popup reappears after every 5 seconds" !564
-   "Set a global command timeout for commands" !567
-   "Error handling - log stacktrace from instrumented game" !568
-   "Namespaces for AUT Instrumentation side" !572
-   "[AUT logs] Replace "Server" with "Tester""	!574
-   "Rename AltElement to AltUnityObject in python for consistency"	!575
-   "[Python] Modify AltUnityObject to have world coordinate as float instead of int" !576
-   "UI issues while "Play on Editor" when InvalidOperationException error" !577
-   "Include Samplescenes project package in Examples" !578
-   "Change the command GetPNGScreenshot to use ScreenCapture.CaptureScreenshot" !580
-   "Refactor wait commands to use callback pattern" !586
-   "CommandResponseTimeoutException is raised even if the driver is disconnected" !593
-   "Add AUT+UTF example in the docs" !595
-   "[API - BY-Selector] Update screenshot for AltId" !596
-   "Tester dev documentation" !598
-   "websocket server inside AltUnity Tester"	!600
-   "Strong type return message on AltUnityObject commands"	!603
-   "Update java dependencies" !604
-   "[Java] forwardIos not working"	!605
-   "[AltUnityRunnerPrefab] Incorrect message when another process is listening on the same port" !607
-   "Docs for GetComponentProperty / SetComponentProperty"	!609
-   "Include Samplescenes project package in Examples"	!610
-   "Add known issue about portforwarding ios in mono" !611
-   "CreateProperties and unhandled exception"	!612
-   "Update docstrings for the public methods" !613
-   "Add "Tester" in error message when Inspector cannot connect" !614
-   "[Port Forwarding] [Windows] UI issues when port forwarding starts from AUT Editor"	!615
-   "Modify GetScreenshot to send screenshot information as a png image" !617
-   "Fix connection issue for python bindings" !616
-   "[AltUnityRunnerPrefab] Wrong Tester version in popup when communication protocol is initiated"	!618
-   "[Overview - How it works] Remove the Previous and Next buttons from screenshot" !619
-   "Review java Driver library for consistency of api"	!622
-   "Add information about code stripping in documentation"	!623
-   "Update newtonsoft to v.10.0.1 netstandard1.3"	!626
-   "Fix connection issue for python bindings" !628

## Contributors

-   Dorin Oltean @dorinaltom
-   Gombos Kriszta @GombosKriszta
-   Andra Cardas @andra.cardas
-   Robert Dezmerean @robert-dezmerean
-   Iuliana Todoran @iuliana.todoran
-   Robert Poienar @robert_poienar

# Changes in version 1.6.6

## New

-   "Input Handle Mouse events every frame" !467
-   "Add CameraNotFound error handler" !477
-   "Drag/Drop events for KeyUp / KeyDown" !483

## Bug fixes and improvements

-   "[Tests list] Case when Assembly-CSharp_editor.dll displays an extra number of selected tests and passed test icon" !482
-   "[Unity - Game] Large AUT popup and icon for the Asset Store package" !481
-   "Make menu item and window name for AUT editor the same" !471
-   "Tests list missing for cloned game project from Gitlab" !479
-   "Click method is not working" !476
-   "Change the Selectors name with name used in syntax" !478
-   "Eliminate note from "Write and execute first test" section" !475
-   "Preselected build location path deleted after canceling to change it" !473
-   "Fix inconsistencies in documentation" !472
-   "SetComponentProperty doesn't set value for properties in a struct" !469
-   "Add an x button to altunity popup, so it's more intuitive that it can be closed" !470
-   "Problems with maven release job" !466
-   "Pressing x button on the test report pop-up makes the pop-up reopen" !468

## Contributors

-   Dorin Oltean @dorinaltom
-   Gombos Kriszta @GombosKriszta
-   Andra Cardas @andra.cardas
-   Robert Dezmerean @robert-dezmerean
-   Iuliana Todoran @iuliana.todoran
-   Robert Poienar @robert_poienar

# Changes in version 1.6.5

## New

-   "Add new input press key command" #454
-   "Add new input commands: begintouch, touchmove, endtouch" #564
-   "simplify click & tap commands" #549

## Bug fixes and improvements

-   "Update API Documentation's examples using changed method signature" #552
-   "Unity 2021 throws warning there are 2 eventSystems in the scene" #559
-   "PointerEventData.pointerPressRaycast.gameObject missing when doing tap from tests" #503
-   "Input action and show input are not finished when timescale is 0" #560
-   "Scroll/ScrollAndWait method not working" #555
-   "KeyDownLifeCycle does not raise events for Mouse1" #525
-   "[Unity Editor - AltUnity Tools menu] UI issues in the AltUnity Inspector window" #511
-   "Add license file to the Unity Asset Store" #568
-   "Warnings in console after \"Play in Editor\"" #570
-   "[AUT Editor] Display issues" #565
-   "Move test list in a separate container" #539
-   "[Swipe/SwipeAndWait] Object swiped only for value that exits the window border" #556
-   "Swipe/SwipeAndWait method results into swiping always the same direction" #505
-   "Make pressKey command in java and python consistent with c#" #508
-   "update parameter names for python example in portforwarding" #546
-   "AltUnityMockUpPointerInputModule monobehaviour instantiated with new" #519
-   "[TesterEditorPage] Test cases are not selected in the test list when selecting test that uses TestCase" #528
-   "[TesterEditorPage] Test name is not fully displayed when using test case" #529
-   "Update python example with the latest port forwarding method" #541
-   "Add CI job for linting python code" #506
-   "UI issues on dropdowns in Editor" #543
-   "Display Number of selected tests in AltUnityTesterEditor Test List" #540
-   "[Editor] Add menu item to add and remove AltId from current loaded scene" #517
-   "[Editor] Allow only one AltId per object" #518
-   "[AltUnity Tester Editor] Misalignment of some UI elements" #548
-   "Issue with Button.ClickEvent()" #163
-   "Screenshot aspect ratio is not respected when entering larger size than the screenshot itself" #536
-   "Check for an existing Assets/Resources directory before creating, to prevent creating an 'Assets/Resources 1' duplicate path." !424
-   "Resolve deprecation warning on UnityWebRequest.isNetworkError, which was introduced in Unity 2020.2." !423

## Contributors

-   Dorin Oltean @dorinaltom
-   Gombos Kriszta @GombosKriszta
-   Andra Cardas @andra.cardas
-   Robert Dezmerean @robert-dezmerean
-   Iuliana Todoran @iuliana.todoran
-   Robert Poienar @robert_poienar
-   Frank Hickman @fhickman

# Changes in version 1.6.4

## Bug fixes and improvements

-   "Path selector parsing and error handling" !408
-   "Fix the python bindings packaging script" !409
-   "FindObjects indexer does not take into account enabled false flag" !406
-   "move mouse does not move to the right position" !412
-   "fix for moving whole delta in last frame" !402

## Contributors

-   Dorin Oltean @dorinaltom
-   Gombos Kriszta @GombosKriszta
-   Andra Cardas @andra.cardas
-   Robert Dezmerean @robert-dezmerean
-   Iuliana Todoran @iuliana.todoran
-   Robert Poienar @robert_poienar
-   Andy @effalumper

# Changes in version 1.6.3

## New

-   "Add Find By.ID support with ID component" !359
-   "New command: unload scene" !373
-   "Add possibility to have multiple selectors for an object in the path" !382
-   "Add ability to set log level or turn logs off" !383
-   "Selector By.Path Select N-th element" !358
-   "Add AltUnityPortHandler to Nuget package" !367

## Bug fixes and improvements

-   "GetScreenshot command fails in Android Emulator" !392
-   "Change WaitForObjectWithText to use the @text selector in the path" !387
-   "document By.PATH with more examples" !376
-   "CallComponentMethod with Optional Parameters methods does not work well when passing empty string" !388
-   "AltUnityPortForwarding.ForwardIOS not working with no device id specified" !384
-   "Python altElement transformId is misspelled 'tranformId' and its value is always 0" !379
-   "Get object's coordinates without dedicated camera" !378
-   "Add @text selector to be used in finding objects by path" !369
-   "[Editor] Move/Add output field closer to build location" !375
-   "Set logs limit from AltUnity editor window." !365
-   "[Editor] Open build location once build is complete" !374
-   "[Documentation] Update LoadScene Java example to use builder" !372
-   "Remove AltPopIconDrag.cs from repository" !371
-   "AltUnity Tester bug: EndLayoutGroup: BeginLayoutGroup must be called first" !368
-   "Add namespace to AltUnity.Editor scripts" !353
-   fix log overflow !364
-   "Refactor Input LifeCycle methods" !362
-   "Append tester version to the name of the unity package" !360
-   "Add explanation in documentation about unity package download" !361
-   "Make input script valid when ALTUNITYTESTER is not defined" !357
-   "Implement GetAllActiveCameras" !354
-   "Add example project for writing tests with C# nuget package" !352

## Contributors

-   Dorin Oltean @dorinaltom
-   Gombos Kriszta @GombosKriszta
-   Andra Cardas @andra.cardas
-   Robert Dezmerean @robert-dezmerean
-   Iuliana Todoran @iuliana.todoran
-   Robert Poienar @robert_poienar
-   Ochir Darmaev @ochirdarmaev

# Changes in version 1.6.2

## New

-   "Publish altUnity on nuget" !328

## Bug fixes and improvements

-   "3D object disappears when highlighted for screenshot" !349
-   "Add possibility to call a method from an object inside an gameObject" !348
-   "parentId is wrong for canvas objects" !341
-   "Click event is not triggered when swipe is moving but remains on the same object" !347
-   "Wrong shaders are assigned to materials after object is highlighted" !346
-   "call CreateJsonFileForInputMappingOfAxis during build from commandline" !343
-   "Return value for CallStaticMethod is wrong in documentation" !342
-   "Handle case when there is no camera selecting object from screenshot" !318
-   "Keypress moves the character 2 positions instead of 1" !337
-   "Remove tests requiring moq.dll from unitypackage" !340
-   "AltElement repr does not conform to the standard in python" !339
-   "Add flag to make sure AltUnity Tester can be added only to dev builds" !335
-   "Add a note to documentation regarding that only dev builds should have AltUnity Tester" !334
-   "Spelling mistake in AltUnityTester UI and documentation." !333
-   "Add an overview to License section, so users are not confused what restrictions they have" !332
-   "Port forwarding text has different color in dark theme than other text" !329
-   "Iterate enabled cameras on HightlightObjectFromCoordinatesCommand" !330
-   "Fix inspector documentation link" !326
-   "Add documentation for selecting scenes when building the game"!325
-   "Update AltUnity Tester documentation structure" !324

## Contributors

-   Dorin Oltean @dorinaltom
-   Gombos Kriszta @GombosKriszta
-   Robert Poienar @robert_poienar

# Changes in version 1.6.1

## Bug fixes and improvements

-   Solved memory problems cause by Get/SetPixels !320
-   Removed annoying Debug.LogError() !317
-   Fixed logging exceptions !316
-   Added link to AltUnityInspector !321

## Contributors

-   Dorin Oltean @dorinaltom
-   Robert Poienar @robert_poienar

# Changes in version 1.6.0

## Bug fixes and improvements

-   "Remove deprecated methods" !301
-   "Fix AltUnitySyncCommand to receive until message id matches" !311
-   AltUnityTapCommand. Add clickTime !288
-   "GetAllLoadedScenesCommand return scene by build index and not by loaded scenes" !309
-   Fix bug in GetAllLoadedScene !291
-   "String is not considerate a primitive" !307
-   "swipe and wait does not wait the expected duration because of float conversion" !306
-   "add message id to driver server communication" !303
-   "Input actions are not finished when timeScale is 0" !302
-   "Wrong scale difference/original size of screenshot is sent" !300
-   "Change AltUnityProperty and AltUnityField to be the same and add type field" !297
-   "Add more control what fields should return get_all_fields" !267
-   "Pressing mouse button doesn't click on buttons" !296
-   "Display an error message If the port AltUnityServer listens to is used by another process" !295
-   "If no size value is given for screenshot command then it should return full size screenshot" !294
-   "Method to GetAllElements with minimal information" !293
-   "GetComponentProperty sends errorNullRefferenceMessage instead of errorComponentNotFoundMessage" !292
-   "Add try catch when transforming game object to altunityobject" !287
-   "GetMemberForObjectComponent doesn't recognize all properties or fields" !286
-   "Change command name from FindObjectWhichContains to FindObjectWhichContain" !264
-   "Update retry logging in unity in AltUnityDriver" !275
-   "Update gitignore to ignore more files that do not need to be in the repo" !284
-   Update documentation

## Contributors

-   Dorin Oltean @dorinaltom
-   Ru Cindrea @ru.cindrea
-   pusoktimea @pusoktimea
-   Alexandru Rotaru @alex.rotaru
-   Raluca Vaida @raluca.vaida
-   Gombos Kriszta @GombosKriszta
-   Andra Cardas @andra.cardas
-   Robert Poienar @robert_poienar
-   SivanYakir @SivanYakir
-   Ka3u6y6a @ka3u6y6a

# Changes in version 1.5.7

## Bug fixes and improvements

-   "Remove null checker to make compatible with .net 3.x" !268
-   "GetVersion should return server version when there is a mismatch" !266
-   "Load scene additive" !265
-   "AltUnityDriver python does not display warnings by default" !263
-   Update documentation

## Contributors

-   Dorin Oltean @dorinaltom
-   Ru Cindrea @ru.cindrea
-   pusoktimea @pusoktimea
-   Alexandru Rotaru @alex.rotaru
-   Dorel @doreln
-   Bogdan Birnicu @bogdan.birnicu
-   Raluca Vaida @raluca.vaida
-   Gombos Kriszta @GombosKriszta
-   Andra Cardas @andra.cardas
-   Robert Poienar @robert_poienar

# Changes in version 1.5.6

## Bug fixes and improvements

-   "Make "Drag" and "Drop" command deprecated" !259
-   "Make AltUnityDriver interface consistent across all language clients" !257
-   "Add option to specific camera other than name in find objects commands" !207
-   "Python - iOS - exception still thrown if we try to connect but server is not yet running" !255
-   "click_element() not working in python" !254
-   "Multiple warning messages displayed when using the editor" !252
-   "Update documentation for overview and get started" !251
-   "Test List doesn't appear when configuration page is created" !250
-   "Double clicking on a test in AltUnity Tester window doesn't open the test file in IDE" !249
-   "Make the Android / iOS options under Platform more clear" !248
-   "Tilt command not working" !247
-   Exposing the AltBaseSettings to support users to write custom commands. !253
-   "AltElement in Python doesn't have parentId property" !238
-   "Class name is checked although not all tests under it are checked (in the Tests list)" !237
-   "Scroll command not functioning" !246
-   "No relevant error message and build is not made when having empty "Output path"" !244
-   "General Method to run c# test in command line" !242
-   "Examples are missing from unity package that is in documentation" !239
-   "Continuous error triggered when making changes to the test file while the game is running" !245
-   ""NullReferenceException" displayed as info message after running tests" !243
-   "Finding parent by path gives "failedToParseMethodArguments"" !228
-   "Finding object by component should accept component name just like other methods" !241
-   "Handle null exception in case object has null components(invalid scripts)" !208
-   Added get_all_fields_of() for Python along with Typing help !232
-   Test List Improvements !240
-   Added get_all_components() for Python along with Typing help !231
-   "AltUnityTesterEditorSettings should not appear in the project files" !236
-   "Update example tests in order to avoid possible naming conflicts" !233
-   "Rename some of the server commands all to respect the same pattern" !235
-   "Move "Run tests" section below "Build" and "Run" sections" !234
-   "Add example in documentation for function that are missing" !209
-   Changed all the is to the ==, where python complained !230

## Contributors

-   Robert Poienar @robert_poienar
-   Dorin Oltean @dorinaltom
-   Ru Cindrea @ru.cindrea
-   pusoktimea @pusoktimea
-   Alexandru Rotaru @alex.rotaru
-   Dorel @doreln
-   Bogdan Birnicu @bogdan.birnicu
-   Raluca Vaida @raluca.vaida
-   Mike Talalaevskiy @Day0Dreamer
-   Miguel Ibero @miguel.ibero
-   dcole-gsn @dcole-gsn

# Changes in version 1.5.5

## Bug fixes and improvements

-   "Create in sample scene a button that load other scene and test it" !221
-   "Add a way to control the amount of output" !222
-   "Add option to set port in BuildGame Method and move to Awake runner instantiation settings" !213
-   "Compile errors after adding 'ALTUNITYTESTER' in scripting define symbols" !210
-   "Add Text and TextMeshPro Text component to SetText" !220
-   "Add favicon to altom.gitlab.io" !223
-   "If the server is not started when calling getServerVersion, it's stuck" !217
-   "Add license information in documentation" !219
-   "Add google groups links in documentation" !215
-   "Move Bindings folder from BindingAndExample Folder" !214

## Contributors

-   Robert Poienar @robert_poienar
-   Ru Cindrea @ru.cindrea
-   pusoktimea @pusoktimea
-   Alexandru Rotaru @alex.rotaru
-   Mike Talalaevskiy @Day0Dreamer

# Changes in version 1.5.4

## New

-   Double tap method for AltUnityObject !190
-   Add TapCustom command !194
-   "Add AltUnity to Maven" !204

## Bug fixes and improvements

-   "C# receive method adds additional characters in the response" !206
-   "Some elements to be highlighted are not found" !205
-   "Screenshot with object highlighted not working for camera" !196
-   "Return the object with the screenshot when given x y coordinates" !201
-   "Add Tank Sample to documentation" !191
-   "Change function name in python from click_Event to click_event" !200
-   "Error when uploading to pypi the latest version" !193
-   "Modify check version for earlier version that don't have the command to not throw an error" !199
-   "Sort better the order how method are returned from getAllMethods command" !198
-   "Make input visualizer pulsate when clicking" !189
-   "Add information about communication between server and driver" !197
-   "Add a note to Documentation about not having to rebuild the game when tests are changed" !192
-   "Are there similar methods for HoldButton , HoldButtonAndWait and Send_keys in Python binding?" !184
-   "Add javadoc to AltUnity tester" !203

## Contributors

-   Robert Poienar @robert_poienar
-   Ru Cindrea @ru.cindrea
-   Ka3u6y6a @ka3u6y6a
-   Andrei Ionut Benyi @ionut.benyi
-   pusoktimea @pusoktimea
-   Alexandru Rotaru @alex.rotaru

# Changes in version 1.5.3

## New

-   Add methods for swipe by more then two points !173
-   Server needs GetVersion() command" !172

## Bug fixes and improvements

-   Move tests that search for multiple object to another scene" !185
-   Update ReadMe from project" !186
-   Add java package in pages and bindings" !183
-   Test in Python doesn't run at all" !177
-   Error thrown for object name when Encoding/Decoding" !178
-   Implement static `removePortForwarding` methods so that they don't remove all other existing ports that are forwarded" !167
-   Add link to java examples documentation" !182
-   adb path should be configurable on windows and should be part of Android (not iOS) Settings" !179
-   Python bindings: missed install requirement - "deprecated" package" !180
-   Add documentation for port forwarding for iOS and Android" !160
-   Change class names to have prefix alt|altunity" !176
-   If extension is not define in output path it will be set automatically !156
-   AltUnityPopCanvas set to be in front and option to deactivate" !175
-   Input axis not working if it not set all four values" !174
-   Bug: null reference returned for object where parent is canvas with world space and no camera set" !169
-   Update appium tests" !170
-   Python bindings - port forwarding cannot be done if Appium is not used" !159
-   HighlightSelectedObjectCommand should return "Ok" not "null"" !168
-   Refresh port forwarding throws error" !166
-   SetText doesn’t work for TextMeshPro components" !164
-   DeviceId overlapping with Local Port Id" !165

## Contributors

-   Robert Poienar @robert_poienar
-   Ru Cindrea @ru.cindrea
-   Ka3u6y6a @ka3u6y6a
-   Andrei Ionut Benyi @ionut.benyi
-   pusoktimea @pusoktimea
-   Alexandru Rotaru @alex.rotaru

# Changes in version 1.5.2

## New

-   Add option to activate input visualizer from AltUnityTester GUI !145
-   Add options to disable pop-up from GUI !154
-   Screenshot functionality for all 3 drivers !140

## Bug fixes and improvements

-   Find Objects doesn't find all objects !144
-   ALTUNITYTESTER define symbol is not inserted at the right platform when playing in editor !155
-   Add theme to documentation !147
-   Error on python 2.7: TypeError super() takes at least 1 argument (0 given) !146
-   Made IP change to accept all incoming connections to AltUnityServer, at... !151
-   Add gitter link to documentation !152
-   Fix java Wait for object not be present !150
-   Fix click at coordinates on python !149
-   wait_for_object fails !142
-   Fix documentation title and broken links

## Contributors

-   Robert Poienar @robert_poienar
-   Ru Cindrea @ru.cindrea
-   Thejus Krishna
-   Ka3u6y6a @ka3u6y6a
-   Raluca Vaida @raluca.vaida
-   pusoktimea @pusoktimea
-   Alexandru Rotaru @alex.rotaru

# Changes in version 1.5.0

## Refactoring

-   Modify C# driver not to depend on Unity !131
-   Project refactoring: Server !108
-   Project refactoring: AltDriver(python) !113
-   Project refactoring: AltDriver(Java) !115

## New

-   Add pop-up to AltUnityPrefab !106
-   Add clicks visualization !122
-   Add set text command !125
-   Open file test when clicking on a test from window editor !118
-   Create documentation in form of a wiki !128
-   Get Server output !107

## Bug fixes and improvements:

-   Divide editor window to see better the content !119
-   The method callComponentMethod was constructing an AltUnityObjectAction with...!109
-   Server throw more generic error for getComponent than it used to !130
-   Add test for Java method to callComponentMethod with assembly name too !111
-   Correct obsolete texts in c# driver !123
-   Update readme with correct url to download latest Unity package !116
-   AltUnityTester - Create new AltUnity Test doesn't do anything !110
-   Add command for getting text from ui text and text mesh pro !120
-   Missing tests for getAllElement in python !134
-   Missing tests for FindObjectWhichContains!135
-   Add test for "inspector" commands c# !137
-   Change tests to be independent of the screen resolution !129
-   Missing command in c# driver: WaitForObjectWhichContains !124

## Contributors

-   Robert Poienar @robert_poienar
-   Ru Cindrea @ru.cindrea
-   Ka3u6y6a @ka3u6y6a
-   ricardo larrahondo @ricardorlg

# Changes in version 1.4.0

## Keyboard, joystick and mouse simulation

-   Add keyboard input simulation !98
-   Joystick controls !102

## Bug fixes and improvements:

-   Modify the project to use AssemblyQualifiedName instead of just class name" !97
-   Make "Play in Editor" to open the first scene or to put the AltUnityTester in the active scene !100
-   Fix AltUnityTestWindow when using AssemblyDefinitionFiles !99
-   Add timeout to get_current_scene call during socket setup !94
-   "setup_port_forwarding" method in runner.py file from python bindings is throwing an exception !95
-   Set adbFileName in linux !93
-   Fix ClickOnScreenAtXy !92
-   driver.findElement is unable to find some elements in ver 1.3.0 !91

## Contributors

-   Robert Poienar @robert_poienar
-   Ru Cindrea @ru.cindrea
-   Arthur @LordStuart

# Changes in version 1.3.0

## Improvements to Unity Editor GUI / AltUnityTester Unity Test Runner

-   Connected devices and port forwarding status are shown in UI, more than one device can be used at a time - !81
-   Command separator is configurable (from code and UI) - !79
-   More build options - !77
-   Improvements to test running finish dialog - !75, !82
-   Ability to run tests in Editor - !55

## Running in cloud

-   Added documentation and example for running on AWS Device Farm - !20
-   Fixed issue with port fowarding on iOS Python client that caused problems with running in Bitbar and AWS cloud - !76

## Overall Refactoring:

-   Split AltUnityTesterEditor and provide an interface for building a game with AltUnityTester on different platforms - !71
-   Java refactoring and improvements - !60, !58

## Release Artifacts:

-   Create deployment to pages - !66
-   Create AltUnityTester package for every branch via CI pipeline - !65

## Bug fixes and improvements:

-   WaitForElementToNotBePresent doesn't work correctly - !89
-   IdProject is not created when Assets is not parent for AltUnityTester folder - !87
-   Change appium methods duration variable name to be suggestive - !83
-   NullREferenceException is returned when calling tapScreen twice - !74
-   AltUnityRunnerPrefab is not removed when Run in Editor - !73
-   Tester should work without any camera in the scene - !72
-   Run failed test or selected will run all the test - !69
-   Driver froze when the information receive is a certain size - !70
-   Disable objects not found by FindObjects - !68
-   Make AltUnityTester work with Unity 2017.x - !64
-   Fix links in Java bindings readme - !62
-   Update for Python Bindings runner.py - !61

## New AltUnityRunner commands and features:

-   (C# only) GetScreenshot command - !86, !78, !56, !54
-   Add way to find elements that are not enabled - !50
-   Add GetAllObjects(), GetAllMethods(), GetAllComponents, GetAllScenes(), GetAllProperties() commands, add way to find objects by IDs - !63, !59, !53, !51
-   Add command to set Time.timeline - !85

## Contributors

-   Robert Poienar @robert_poienar
-   Ru Cindrea @ru.cindrea
-   Kamil @kaszarek
-   Nikita Ershov @ershov1
-   Napster @napstr
