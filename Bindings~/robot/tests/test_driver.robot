# *** Settings ***
# Library         AltTesterLibrary
# Library         OperatingSystem
# Library         Collections
# Suite Setup     Initialize AltDriver With Custom Host And Port
# Suite Teardown    Stop Altdriver
# Resource        utils_keywords.robot

# *** Test Cases ***
# Test Load And Wait For Scene
#                 Load Scene    ${scene1}
#                 Wait For Current Scene To Be    ${scene1}    timeout=1
#                 Load Scene    ${scene2}
#                 Wait For Current Scene To Be    ${scene2}    timeout=1
#                 ${current_scene}=    Get Current Scene
#                 Should Be Equal    ${current_scene}    ${scene2}

# Test Wait For Current Scene To Be With A Non Existing Scene
#                 ${scene_name}=    Set Variable    Scene 0
#                 ${expected_error}=    Set Variable    WaitTimeOutException: Scene ${scene_name} not loaded after 1 seconds
#                 ${error}=    Run Keyword And Ignore Error
#                 ...    Wait For Current Scene To Be    ${scene_name}    timeout=1    interval=0.5
#                 Should Be Equal As Strings    ${error[1]}    ${expected_error}

# Test Set And Get Time Scale
#                 Set Time Scale    0.1
#                 ${time_scale}=    Get Time Scale
#                 Should Be Equal As Numbers    ${time_scale}    0.1
#                 Set Time Scale    1

# Test Screenshot
#                 ${png_path}=    Set Variable    testPython.png
#                 Get Png Screenshot    ${png_path}
#                 File Should Exist    ${png_path}

# Test Wait For Object Which Contains With Tag
#                 ${alt_object}=    Wait For Object Which Contains    NAME    Canva    camera_by=TAG    camera_value=MainCamera
#                 ${name}=    Get Object Name    ${alt_object}
#                 Should Be Equal As Strings    ${name}    Canvas

# Test Load Additive Scenes
#                 Load Scene    ${scene1}    load_single=${True}
#                 ${initial_number_of_elements}=    Get All Elements
#                 Load Scene    ${scene2}    load_single=${False}
#                 ${final_number_of_elements}=    Get All Elements
#                 ${initial_number_of_elements_length}=    Get Length    ${initial_number_of_elements}
#                 ${final_number_of_elements_length}=    Get Length    ${final_number_of_elements}
#                 Should Be True    ${final_number_of_elements_length}>${initial_number_of_elements_length}
#                 ${all_loaded_scenes}=    Get All Loaded Scenes
#                 ${number_of_scenes}=    Get Length    ${all_loaded_scenes}
#                 Should Be Equal As Integers    ${number_of_scenes}    2

# Test Load Scene With Invalid Scene Name
#                 ${scene_name}=    Set Variable    Scene 0
#                 ${expected_error}=    Set Variable    SceneNotFoundException: Could not found a scene with the name: ${scene_name}.
#                 ${error}=    Run Keyword And Ignore Error    Load Scene    ${scene_name}
#                 Should Be Equal As Strings    ${error[1]}    ${expected_error}

# Test Unload Scene
#                 Load Scene    ${scene1}    load_single=${True}
#                 Load Scene    ${scene2}    load_single=${False}
#                 ${scenes}=    Get All Loaded Scenes
#                 ${scenes_number}=    Get Length    ${scenes}
#                 Should Be Equal As Integers    ${scenes_number}    2
#                 Unload Scene    ${scene2}
#                 ${scenes}=    Get All Loaded Scenes
#                 ${scenes_number}=    Get Length    ${scenes}
#                 Should Be Equal As Integers    ${scenes_number}    1
#                 ${scenes}=    Get All Loaded Scenes
#                 ${scene}=    Get From List    ${scenes}    0
#                 Should Be Equal As Strings    ${scene}    ${scene1}

# Test Unload Only Scene
#                 Load Scene    ${scene1}    load_single=${True}
#                 ${expected_error}=    Set Variable    CouldNotPerformOperationException: Cannot unload scene: ${scene1}
#                 ${error}=    Run Keyword And Ignore Error    Unload Scene    ${scene1}
#                 Should Be Equal As Strings    ${error[1]}    ${expected_error}

# Test Set Server Logging
#                 ${param}=    Create List    AltServerFileRule
#                 ${rule}=    Call Static Method    AltTester.AltTesterUnitySDK.Logging.ServerLogManager    Instance.Configuration.FindRuleByName    Assembly-CSharp    parameters=${param}
#                 ${levels}=    Get From Dictionary    ${rule}    Levels
#                 ${levels_number}=    Get Length    ${levels}
#                 Should Be Equal As Integers    ${levels_number}    5
#                 Set Server Logging    File    Off
#                 ${rule}=    Call Static Method    AltTester.AltTesterUnitySDK.Logging.ServerLogManager    Instance.Configuration.FindRuleByName    Assembly-CSharp    parameters=${param}
#                 ${levels}=    Get From Dictionary    ${rule}    Levels
#                 ${levels_number}=    Get Length    ${levels}
#                 Should Be Equal As Integers    ${levels_number}    0
#                 Set Server Logging    File    Debug

# Test Invalid Paths
#                 Invalid Path Raise Exception    path
#                 Invalid Path Raise Exception    ["//[1]"
#                 Invalid Path Raise Exception    CapsuleInfo[@tag=UI]
#                 Invalid Path Raise Exception    //CapsuleInfo[@tag=UI/Text
#                 Invalid Path Raise Exception    //CapsuleInfo[0/Text

# *** Keywords ***
# Invalid Path Raise Exception
#                 [Arguments]    ${path}
#                 ${error}=    Run Keyword And Ignore Error    Find Object    PATH    ${path}
#                 ${error_message}=    Get From List    ${error}    1
#                 Should Contain    ${error_message}    InvalidPathException
