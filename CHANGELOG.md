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
-  Robert Poinar @robert_poienar
-  Ru Cindrea @ru.cindrea
-  Kamil @kaszarek
-  Nikita Ershov @ershov1
-  Napster @napstr
