# Changes in version 1.5.4
## New

- Double tap method for AltUnityObject !190 
- Add TapCustom command !194 
- "Add AltUnity to Maven" !204 

## Bug fixes and improvements

- "C# receive method adds additional characters in the response" !206 
- "Some elements to be highlighted are not found" !205 
- "Screenshot with object highlighted not working for camera" !196 
- "Return the object with the screenshot when given x y coordinates" !201 
- "Add Tank Sample to documentation" !191 
- "Change function name in python from click_Event to click_event" !200 
- "Error when uploading to pypi the latest version" !193 
- "Modify check version for earlier version that don't have the command to not throw an error" !199 
- "Sort better the order how method are returned from getAllMethods command" !198 
- "Make input visualizer pulsate when clicking" !189 
- "Add information about communication between server and driver" !197 
- "Add a note to Documentation about not having to rebuild the game when tests are changed" !192 
- "Are there similar methods for HoldButton , HoldButtonAndWait and Send_keys in Python binding?" !184
- "Add javadoc to AltUnity tester" !203 

## Contributors

- Robert Poienar @robert_poienar
- Ru Cindrea @ru.cindrea
- Ka3u6y6a @ka3u6y6a
- Andrei Ionut Benyi @ionut.benyi
- pusoktimea @pusoktimea
- Alexandru Rotaru @alex.rotaru


# Changes in version 1.5.3
## New

- Add methods for swipe by more then two points !173 
- Server needs GetVersion() command" !172 

## Bug fixes and improvements

- Move tests that search for multiple object to another scene" !185 
- Update ReadMe from project" !186 
- Add java package in pages and bindings" !183 
- Test in Python doesn't run at all" !177 
- Error thrown for object name when Encoding/Decoding" !178 
- Implement static `removePortForwarding` methods so that they don't remove all other existing ports that are forwarded" !167 
- Add link to java examples documentation" !182 
- adb path should be configurable on windows and should be part of Android (not iOS) Settings" !179 
- Python bindings: missed install requirement - "deprecated" package" !180 
- Add documentation for port forwarding for iOS and Android" !160 
- Change class names to have prefix alt|altunity" !176 
- If extension is not define in output path it will be set automatically !156 
- AltUnityPopCanvas set to be in front and option to deactivate" !175 
- Input axis not working if it not set all four values" !174 
- Bug: null reference returned for object where parent is canvas with world space and no camera set" !169 
- Update appium tests" !170 
- Python bindings - port forwarding cannot be done if Appium is not used" !159 
- HighlightSelectedObjectCommand should return "Ok" not "null"" !168 
- Refresh port forwarding throws error" !166 
- SetText doesn’t work for TextMeshPro components" !164 
- DeviceId overlapping with Local Port Id" !165 

## Contributors

- Robert Poienar @robert_poienar
- Ru Cindrea @ru.cindrea
- Ka3u6y6a @ka3u6y6a
- Andrei Ionut Benyi @ionut.benyi
- pusoktimea @pusoktimea
- Alexandru Rotaru @alex.rotaru

# Changes in version 1.5.2

## New

- Add option to activate input visualizer from AltUnityTester GUI !145
- Add options to disable pop-up from GUI !154
- Screenshot functionality for all 3 drivers !140

## Bug fixes and improvements

- Find Objects doesn't find all objects !144
- ALTUNITYTESTER define symbol is not inserted at the right platform when playing in editor !155
- Add theme to documentation !147
- Error on python 2.7: TypeError super() takes at least 1 argument (0 given) !146
- Made IP change to accept all incoming connections to AltUnityServer, at... !151
- Add gitter link to documentation !152
- Fix java Wait for object not be present !150
- Fix click at coordinates on python !149
- wait_for_object fails !142
- Fix documentation title and broken links

## Contributors

- Robert Poienar @robert_poienar
- Ru Cindrea @ru.cindrea
- Thejus Krishna
- Ka3u6y6a @ka3u6y6a
- Raluca Vaida @raluca.vaida
- pusoktimea @pusoktimea
- Alexandru Rotaru @alex.rotaru

# Changes in version 1.5.0

## Refactoring

- Modify C# driver not to depend on Unity !131 
- Project refactoring: Server !108 
- Project refactoring: AltDriver(python) !113 
- Project refactoring: AltDriver(Java) !115 

## New

- Add pop-up to AltUnityPrefab !106 
- Add clicks visualization !122 
- Add set text command !125
- Open file test when clicking on a test from window editor !118 
- Create documentation in form of a wiki !128 
- Get Server output !107 

## Bug fixes and improvements:

- Divide editor window to see better the content !119 
- The method callComponentMethod was constructing an AltUnityObjectAction with...!109 
- Server throw more generic error for getComponent than it used to !130 
- Add test for Java method to callComponentMethod with assembly name too !111 
- Correct obsolete texts in c# driver !123 
- Update readme with correct url to download latest Unity package !116 
- AltUnityTester - Create new AltUnity Test doesn't do anything !110 
- Add command for getting text from ui text and text mesh pro !120 
- Missing tests for getAllElement in python !134 
- Missing tests for FindObjectWhichContains!135 
- Add test for "inspector" commands c# !137 
- Change tests to be independent of the screen resolution !129 
- Missing command in c# driver: WaitForObjectWhichContains !124 

## Contributors

-  Robert Poienar @robert_poienar
-  Ru Cindrea @ru.cindrea
-  Ka3u6y6a @ka3u6y6a
-  ricardo larrahondo @ricardorlg
 
# Changes in version 1.4.0

## Keyboard, joystick and mouse simulation
- Add keyboard input simulation !98 
- Joystick controls !102 

## Bug fixes and improvements:
- Modify the project to use AssemblyQualifiedName instead of just class name" !97 
- Make "Play in Editor" to open the first scene or to put the AltUnityTester in the active scene !100 
- Fix AltUnityTestWindow when using AssemblyDefinitionFiles !99 
- Add timeout to get_current_scene call during socket setup !94 
- "setup_port_forwarding" method in runner.py file from python bindings is throwing an exception !95 
- Set adbFileName in linux !93 
- Fix ClickOnScreenAtXy !92 
- driver.findElement is unable to find some elements in ver 1.3.0 !91 

## Contributors
-  Robert Poienar @robert_poienar
-  Ru Cindrea @ru.cindrea
-  Arthur @LordStuart 
# Changes in version 1.3.0

## Improvements to Unity Editor GUI / AltUnityTester Unity Test Runner
-  Connected devices and port forwarding status are shown in UI, more than one device can be used at a time  - !81
-  Command separator is configurable (from code and UI)  - !79
-  More build options - !77 
-  Improvements to test running finish dialog  - !75, !82
-  Ability to run tests in Editor - !55 

## Running in cloud
-  Added documentation and example for running on AWS Device Farm  - !20       
-  Fixed issue with port fowarding on iOS Python client that caused problems with running in Bitbar and AWS cloud - !76

## Overall Refactoring:
-  Split AltUnityTesterEditor and provide an interface for building a game with AltUnityTester on different platforms  - !71
-  Java refactoring and improvements - !60, !58 

## Release Artifacts:
-  Create deployment to pages  - !66  
-  Create AltUnityTester package for every branch via CI pipeline  - !65        

## Bug fixes and improvements:
-  WaitForElementToNotBePresent doesn't work correctly  - !89 
-  IdProject is not created when Assets is not parent for AltUnityTester folder  - !87
-  Change appium methods duration variable name to be suggestive  - !83
-  NullREferenceException is returned when calling tapScreen twice  - !74
-  AltUnityRunnerPrefab is not removed when  Run in Editor   - !73      
-  Tester should work without any camera in the scene  - !72       
-  Run failed test or selected will run all the test  - !69       
-  Driver froze when the information receive is a certain size  - !70       
-  Disable objects not found by FindObjects  - !68       
-  Make AltUnityTester work with Unity 2017.x  - !64
-  Fix links in Java bindings readme - !62   
-  Update for Python Bindings runner.py - !61   


## New AltUnityRunner commands and features:
-  (C# only) GetScreenshot command - !86, !78, !56, !54
-  Add way to find elements that are not enabled - !50
-  Add GetAllObjects(), GetAllMethods(), GetAllComponents, GetAllScenes(), GetAllProperties() commands, add way to find objects by IDs - !63, !59, !53, !51
-  Add command to set Time.timeline  - !85  

## Contributors
-  Robert Poienar @robert_poienar
-  Ru Cindrea @ru.cindrea
-  Kamil @kaszarek
-  Nikita Ershov @ershov1
-  Napster @napstr
