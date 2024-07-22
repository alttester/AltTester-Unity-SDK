*** Settings ***
Library           String
Library           AltTesterLibrary
Library           BuiltIn
Library           Collections
Suite Setup       Initialize AltDriver With Custom Host And Port
Test Setup        SetUp Tests
Suite Teardown    Stop Altdriver
Resource          utils_keywords.robot

*** Test Cases ***
Test Tap UI Object
    ${object}=    Find Object    NAME    UIButton
    Tap Object    ${object}
    ${capsule_info}=    Wait For Object    PATH    //CapsuleInfo[@text=UIButton clicked to jump capsule!]    timeout=1
    ${text}=    Get Text    ${capsule_info}
    Should Be Equal    ${text}    UIButton clicked to jump capsule!

Test Tap Object
    ${object}=    Find Object    NAME    Capsule
    Tap Object    ${object}
    ${capsule_info}=    Wait For Object    PATH    //CapsuleInfo[@text=Capsule was clicked to jump!]    timeout=1
    ${text}=    Get Text    ${capsule_info}
    Should Be Equal    ${text}    Capsule was clicked to jump!

Test Find Object By Name
    ${plane}=    Find Object    NAME    Plane
    ${capsule}=    Find Object    NAME    Capsule
    ${plane_name}=    Get Object Name    ${plane}
    ${capsule_name}=    Get Object Name    ${capsule}
    Should Be Equal    ${plane_name}    Plane
    Should Be Equal    ${capsule_name}    Capsule

Test Find Object By Name And Parent
    ${alt_object}=    Find Object    NAME    Canvas/CapsuleInfo
    ${alt_object_name}=    Get Object Name    ${alt_object}
    Should Be Equal    ${alt_object_name}    CapsuleInfo

Test Find Object Child
    ${alt_object}=    Find Object    PATH    //UIButton/*
    ${alt_object_name}=    Get Object Name    ${alt_object}
    Should Be Equal    ${alt_object_name}    Text

Test Find Object By Tag
    ${alt_object}=    Find Object    TAG    plane
    ${alt_object_name}=    Get Object Name    ${alt_object}
    Should Be Equal    ${alt_object_name}    Plane

Test Find Object By Layer
    ${alt_object}=    Find Object    LAYER    Water
    ${alt_object_name}=    Get Object Name    ${alt_object}
    Should Be Equal    ${alt_object_name}    Capsule

Test Find Object By Text
    ${alt_object}=    Find Object    NAME    CapsuleInfo
    ${text}=    Get Text    ${alt_object}
    ${element}=    Find Object    TEXT    ${text}
    ${element_text}=    Get Text    ${element}
    Should Be Equal    ${element_text}    ${text}

Test Find Object By Component 1
    ${alt_object}=    Find Object    COMPONENT    CapsuleCollider
    ${alt_object_name}=    Get Object Name    ${alt_object}
    Should Be Equal    ${alt_object_name}    Capsule

Test Find Object By Component 2
    ${alt_object}=    Find Object    COMPONENT    AltTesterExamples.Scripts.AltExampleScriptCapsule
    ${alt_object_name}=    Get Object Name    ${alt_object}
    Should Be Equal    ${alt_object_name}    Capsule

Test Find Object Which Contains
    ${alt_object}=    Find Object Which Contains    NAME    Pla
    ${alt_object_name}=    Get Object Name    ${alt_object}
    Should Contain    ${alt_object_name}    Pla

Test Find Objects By Name
    ${alt_objects}=    Find Objects    NAME    Plane
    ${appears}=    Get Length    ${alt_objects}
    Should Be Equal As Integers    2    ${appears}

Test Find Objects By Tag
    ${alt_objects}=    Find Objects    TAG    plane
    ${appears}=    Get Length    ${alt_objects}
    Should Be Equal As Integers    2    ${appears}
    FOR    ${obj}    IN    @{alt_objects}
        ${name}=    Get Object Name    ${obj}
        Should Be Equal As Strings    ${name}    Plane
    END

Test Find Objects By Layer
    ${alt_objects}=    Find Objects    LAYER    Default
    ${appears}=    Get Length    ${alt_objects}
    Should Be True    10<=${appears}

Test Find Objects By Component
    ${alt_objects}=    Find Objects    COMPONENT    UnityEngine.MeshFilter
    ${appears}=    Get Length    ${alt_objects}
    Should Be Equal As Integers    5    ${appears}

Test Find Parent Using Path
    ${parent}=    Find Object    PATH    //CapsuleInfo/..
    ${parent_name}=    Get Object Name    ${parent}
    Should Be Equal    ${parent_name}    Canvas

Test Find Objects Which Contain By Name
    ${alt_objects}=    Find Objects Which Contain    NAME    Capsule
    ${appears}=    Get Length    ${alt_objects}
    Should Be Equal As Integers    2    ${appears}
    FOR    ${obj}    IN    @{alt_objects}
        ${name}=    Get Object Name    ${obj}
        Should Contain    ${name}    Capsule
    END

Test Find Object Which Contains With Not Existing Object
    ${element_name}=    Set Variable    EventNonExisting
    ${error_message}=    Set Variable    NotFoundException: Object //*[contains(@name,${element_name})] not found
    ${error}=    Run Keyword And Ignore Error    Find Object Which Contains    NAME    ${element_name}
    Should Be Equal As Strings    ${error[1]}    ${error_message}

Test Get All Components
    ${components}=    Find Object    NAME    Canvas
    ${all_components}=    Get All Components    ${components}
    ${appears}=    Get Length    ${all_components}
    Should Be Equal As Integers    5    ${appears}

Test Wait For Object
    ${alt_object}=    Wait For Object    NAME    Capsule
    ${alt_object_name}=    Get Object Name    ${alt_object}
    Should Be Equal    ${alt_object_name}    Capsule

Test Wait For Object With Non Existing Object
    ${element_name}=    Set Variable    EventNonExisting
    ${expected_error}=    Set Variable    WaitTimeOutException: Element ${element_name} not found after 1 seconds
    ${error}=    Run Keyword And Ignore Error    Wait For Object    NAME    ${element_name}    timeout=1
    Should Be Equal As Strings    ${error[1]}    ${expected_error}

Test Wait For Object By Name
    ${plane}=    Wait For Object    NAME    Plane
    ${capsule}=    Wait For Object    NAME    Capsule
    ${plane_name}=    Get Object Name    ${plane}
    ${capsule_name}=    Get Object Name    ${capsule}
    Should Be Equal    ${plane_name}    Plane
    Should Be Equal    ${capsule_name}    Capsule

Test Get Application Screen Size
    ${screen_size}=    Get Application Screensize
    Should Not Be Equal As Numbers    ${screen_size[0]}    0
    Should Not Be Equal As Numbers    ${screen_size[1]}    0

Test Wait For Object With Text
    ${capsule_info}=    Find Object    NAME    CapsuleInfo
    ${capsule_info_text}=    Get Text    ${capsule_info}
    ${alt_object}=    Wait For Object    PATH    //CapsuleInfo[@text=${capsule_info_text}]    timeout=1
    ${alt_object_name}=    Get Object Name    ${alt_object}
    ${alt_object_text}=    Get Text    ${alt_object}
    Should Be Equal As Strings    ${alt_object_name}    CapsuleInfo
    Should Be Equal As Strings    ${alt_object_text}    ${capsule_info_text}

Test Wait For Object With Wrong Text
    ${path}=    Set Variable    //CapsuleInfo[@text=aaaaa]
    ${error}=    Run Keyword And Ignore Error    Wait For Object    PATH    ${path}    timeout=1
    ${error_message}=    Set Variable    WaitTimeOutException: Element ${path} not found after 1 seconds
    Should Be Equal As Strings    ${error[1]}    ${error_message}

Test Wait For Object Which Contains
    ${alt_object}=    Wait For Object Which Contains    NAME    Main
    ${alt_object_name}=    Get Object Name    ${alt_object}
    Should Be Equal As Strings    ${alt_object_name}    Main Camera

Test Wait For Object To Not Be Present
    Wait For Object To Not Be Present    NAME    Capsuule

Test Wait For Object To Not Be Present Fail
    ${error_message}=    Set Variable    WaitTimeOutException: Element Capsule still found after 1 seconds
    ${error}=    Run Keyword And Ignore Error    Wait For Object To Not Be Present    NAME    Capsule    timeout=1
    Should Be Equal As Strings    ${error[1]}    ${error_message}

Test Get Text With Non English Text
    ${alt_object}=    Find Object    NAME    NonEnglishText
    ${alt_object_text}=    Get Text    ${alt_object}
    Should Be Equal As Strings    ${alt_object_text}    BJÖRN'S PASS

Test Get Text With Chinese Letters
    ${alt_object}=    Find Object    NAME    ChineseLetters
    ${alt_object_text}=    Get Text    ${alt_object}
    Should Be Equal    ${alt_object_text}    哦伊娜哦

Test Set Text
    ${text_object}=    Find Object    NAME    NonEnglishText
    ${original_text}=    Get Text    ${text_object}
    Set Text    ${text_object}    ModifiedText
    ${after_text}=    Get Text    ${text_object}
    Should Not Be Equal As Strings    ${original_text}    ${after_text}
    Should Be Equal As Strings    ${after_text}    ModifiedText

Test Double Tap
    ${counter_button}=    Find Object    NAME    ButtonCounter
    ${counter_button_text}=    Find Object    NAME    ButtonCounter/Text
    ${counter_button_before_text}=    Get Text    ${counter_button_text}
    Tap Object    ${counter_button}    count=2
    ${counter_button_after_text}=    Get Text    ${counter_button_text}
    ${number_of_counts}=    Evaluate    ${counter_button_before_text}+2
    Should Be Equal As Integers    ${number_of_counts}    ${counter_button_after_text}

Test Tap On Screen Where There Are No Objects
    ${counter_button}=    Find Object    NAME    ButtonCounter
    ${counter_button_y}=    Get Object Y    ${counter_button}
    ${value_y}=    Evaluate    ${counter_button_y}+100
    ${coordinates}=    Create Dictionary    x=1    y=${value_y}
    Tap    ${coordinates}

Test Hold Button
    ${button}=    Find Object    NAME    UIButton
    ${button_position}=    Get Screen Position    ${button}
    Hold Button    ${button_position}    duration=1
    ${capsule_info}=    Find Object    NAME    CapsuleInfo
    ${text}=    Get Text    ${capsule_info}
    Should Be Equal As Strings    ${text}    UIButton clicked to jump capsule!

Test Hold Button Without Wait
    ${button}=    Find Object    NAME    UIButton
    ${button_position}=    Get Screen Position    ${button}
    Hold Button    ${button_position}    duration=1    wait=False
    Sleep    2
    ${capsule_info}=    Find Object    NAME    CapsuleInfo
    ${text}=    Get Text    ${capsule_info}
    Should Be Equal As Strings    ${text}    UIButton clicked to jump capsule!

Test Wait For Component Property
    ${alt_object}=    Find Object    NAME    Capsule
    ${result}=    Wait For Component Property    ${alt_object}    AltExampleScriptCapsule    TestBool    ${True}    Assembly-CSharp
    Should Be Equal    ${result}    ${True}

Test Wait For Component Property Get Property As String
    ${Canvas} =    Wait For Object    PATH    /Canvas
    Wait For Component Property    ${Canvas}    UnityEngine.RectTransform    name    Canvas    UnityEngine.CoreModule    1    get_property_as_string=${True}    max_depth=1

Test Get Component Property
    ${alt_object}=    Find Object    NAME    Capsule
    ${result}=    Get Component Property    ${alt_object}    AltExampleScriptCapsule    arrayOfInts    Assembly-CSharp
    ${list}=    Create List    ${1}    ${2}    ${3}
    Should Be Equal    ${result}    ${list}

Test Get Component Property With Bool
    ${alt_object}=    Find Object    NAME    Capsule
    ${result}=    Get Component Property    ${alt_object}    AltExampleScriptCapsule    TestBool    Assembly-CSharp
    Should Be Equal    ${result}    ${True}

Test Set Component Property
    ${alt_object}=    Find Object    NAME    Capsule
    ${list}=    Create List    ${2}    ${3}    ${4}
    Set Component Property    ${alt_object}    AltExampleScriptCapsule    arrayOfInts    Assembly-CSharp    ${list}
    ${alt_object}=    Find Object    NAME    Capsule
    ${result}=    Get Component Property    ${alt_object}    AltExampleScriptCapsule    arrayOfInts    Assembly-CSharp
    Should Be Equal    ${result}    ${list}

Test Call Component Method
    ${alt_object}=    Find Object    NAME    Capsule
    ${parameters}=    Create List    setFromMethod
    ${result}=    Call Component Method    ${alt_object}    AltExampleScriptCapsule    Jump    Assembly-CSharp    parameters=${parameters}
    Should Be Equal    ${result}    ${None}
    Wait For Object    PATH    //CapsuleInfo[@text=setFromMethod]    timeout=1
    ${capsule_info}=    Find Object    NAME    CapsuleInfo
    ${capsule_info_text}=    Get Text    ${capsule_info}
    Should Be Equal    ${capsule_info_text}    setFromMethod

Test Call Component Method With No Parameters
    ${result}=    Find Object    PATH    /Canvas/Button/Text
    ${text}=    Call Component Method    ${result}    UnityEngine.UI.Text    get_text    UnityEngine.UI
    Should Be Equal    ${text}    Change Camera Mode

Test Call Component Method With Parameters
    ${alt_object}=    Find Object    PATH    /Canvas/UnityUIInputField/Text
    ${params}=    Create List    ${16}
    Call Component Method    ${alt_object}    UnityEngine.UI.Text    set_fontSize    UnityEngine.UI    ${params}
    ${empty_list}=    Create List
    ${font_size}=    Call Component Method    ${alt_object}    UnityEngine.UI.Text    get_fontSize    UnityEngine.UI    ${empty_list}
    Should Be Equal As Integers    ${font_size}    16

Test Call Component Method With Assembly
    ${capsule}=    Find Object    NAME    Capsule
    ${initial_rotation}=    Get Component Property    ${capsule}    UnityEngine.Transform    rotation    UnityEngine.CoreModule
    ${parameters}=    Create List    ${10}    ${10}    ${10}
    ${type_of_parameters}=    Create List    System.Single    System.Single    System.Single
    Call Component Method    ${capsule}    UnityEngine.Transform    Rotate    UnityEngine.CoreModule    parameters=${parameters}    type_of_parameters=${type_of_parameters}
    ${capsule_after_rotation}=    Find Object    NAME    Capsule
    ${final_rotation}=    Get Component Property    ${capsule_after_rotation}    UnityEngine.Transform    rotation    UnityEngine.CoreModule
    ${initial_rotation_x}=    Get From Dictionary    ${initial_rotation}    x
    ${initial_rotation_y}=    Get From Dictionary    ${initial_rotation}    y
    ${initial_rotation_z}=    Get From Dictionary    ${initial_rotation}    z
    ${initial_rotation_w}=    Get From Dictionary    ${initial_rotation}    w
    ${final_rotation_x}=    Get From Dictionary    ${final_rotation}    x
    ${final_rotation_y}=    Get From Dictionary    ${final_rotation}    y
    ${final_rotation_z}=    Get From Dictionary    ${final_rotation}    z
    ${final_rotation_w}=    Get From Dictionary    ${final_rotation}    w
    Should Be True    ${initial_rotation_x}!=${final_rotation_x} or ${initial_rotation_y}!=${final_rotation_y} or ${initial_rotation_z}!=${final_rotation_z} or ${initial_rotation_w}!=${final_rotation_w}

Test Call Component Method With Multiple Parameters
    ${alt_object}=    Find Object    NAME    Capsule
    ${list}=    Create List    ${1}    ${2}    ${3}
    ${parameters}=    Create List    ${1}    stringparam    ${0.5}    ${list}
    ${result}=    Call Component Method    ${alt_object}    AltExampleScriptCapsule    TestCallComponentMethod    Assembly-CSharp    parameters=${parameters}
    Should Be Equal As Strings    ${result}    1,stringparam,0.5,[1,2,3]

Test Call Component Method With Multiple Definitions
    ${capsule}=    Find Object    NAME    Capsule
    ${parameters}=    Create List    ${2}
    ${type_of_parameters}=    Create List    System.Int32
    Call Component Method    ${capsule}    AltExampleScriptCapsule    Test    Assembly-CSharp    parameters=${parameters}    type_of_parameters=${type_of_parameters}
    ${capsule_info}=    Find Object    NAME    CapsuleInfo
    ${capsule_info_text}=    Get Text    ${capsule_info}
    Should Be Equal    ${capsule_info_text}    6

Test Call Component Method With Invalid Assembly
    ${alt_object}=    Find Object    NAME    Capsule
    ${component}=    Set Variable    RandomComponent
    ${method}=    Set Variable    TestMethodWithManyParameters
    ${assembly}=    Set Variable    RandomAssembly
    ${list}=    Create List    ${1}    ${2}    ${3}
    ${parameters}=    Create List    ${1}    stringparam    ${0.5}    ${list}
    ${type_of_parameters}=    Create List
    ${expected_error}=    Set Variable    AssemblyNotFoundException: Assembly not found
    ${error}=    Run Keyword And Ignore Error
    ...    Call Component Method    ${alt_object}    ${component}    ${method}    ${assembly}    parameters=${parameters}    type_of_parameters=${type_of_parameters}
    Should Be Equal As Strings    ${error[1]}    ${expected_error}

Test Call Component Method With Incorrect Number Of Parameters
    ${alt_object}=    Find Object    NAME    Capsule
    ${component}=    Set Variable    AltExampleScriptCapsule
    ${method}=    Set Variable    TestMethodWithManyParameters
    ${assembly}=    Set Variable    Assembly-CSharp
    ${list}=    Create List    ${1}    ${2}    ${3}
    ${parameters}=    Create List    stringparam    ${0.5}    ${list}
    ${type_of_parameters}=    Create List
    ${expected_error}=    Set Variable    MethodWithGivenParametersNotFoundException: No method found with 3 parameters matching signature: ${method}(System.String[])
    ${error}=    Run Keyword And Ignore Error
    ...    Call Component Method    ${alt_object}    ${component}    ${method}    ${assembly}    parameters=${parameters}    type_of_parameters=${type_of_parameters}
    Should Be Equal As Strings    ${error[1]}    ${expected_error}

Test Call Component Method With Invalid Method Argument Types
    ${alt_object}=    Find Object    NAME    Capsule
    ${component}=    Set Variable    AltExampleScriptCapsule
    ${method}=    Set Variable    TestMethodWithManyParameters
    ${assembly}=    Set Variable    Assembly-CSharp
    ${list}=    Create List    ${1}    ${2}    ${3}
    ${parameters}=    Create List    stringnoint    stringparam    ${0.5}    ${list}
    ${type_of_parameters}=    Create List
    ${expected_error}=    Set Variable    FailedToParseArgumentsException: Could not parse parameter '\"${parameters[0]}\"' to type System.Int32
    ${error}=    Run Keyword And Ignore Error
    ...    Call Component Method    ${alt_object}    ${component}    ${method}    ${assembly}    parameters=${parameters}
    Should Be Equal As Strings    ${error[1]}    ${expected_error}

Test Call Static Method
    ${list_to_set}=    Create List    Test    ${1}
    ${list_to_get}=    Create List    Test    ${2}
    Call Static Method    UnityEngine.PlayerPrefs    SetInt    UnityEngine.CoreModule    parameters=${list_to_set}
    ${value}=    Call Static Method    UnityEngine.PlayerPrefs    GetInt    UnityEngine.CoreModule    parameters=${list_to_get}
    Should Be Equal As Integers    ${value}    ${1}

Test Set Player Pref Keys Int
    Delete Player Pref
    Set Player Pref Key    test    ${1}    Int
    ${actual_value}=    Get Player Pref Key    test    Int
    Should Be Equal As Integers    ${actual_value}    ${1}

Test Set Player Pref Keys Float
    Delete Player Pref
    Set Player Pref Key    test    ${1.3}    Float
    ${actual_value}=    Get Player Pref Key    test    Float
    Should Be Equal As Numbers    ${actual_value}    ${1.3}

Test Set Player Pref Keys String
    Delete Player Pref
    Set Player Pref Key    test    string value    String
    ${actual_value}=    Get Player Pref Key    test    String
    Should Be Equal As Strings    ${actual_value}    string value

Test Delete Player Pref Key
    Delete Player Pref
    Set Player Pref Key    test    1    String
    ${actual_value}=    Get Player Pref Key    test    String
    Should Be Equal As Strings    ${actual_value}    1
    Delete Player Pref Key    test
    Run Keyword And Expect Error    NotFoundException: PlayerPrefs key test not found
    ...    Get Player Pref Key    test    String

Test Press Next Scene
    ${initial_scene}=    Get Current Scene
    ${next_scene}=    Find Object    NAME    NextScene
    Tap Object    ${next_scene}
    ${current_scene}=    Get Current Scene
    Should Not Be Equal    ${initial_scene}    ${current_scene}

Test Acceleration
    Load Scene    Scene 1 AltDriverTestScene
    ${capsule}=    Find Object    NAME    Capsule
    ${capsule_WorldX}=    Get Object WorldX    ${capsule}
    ${capsule_WorldY}=    Get Object WorldY    ${capsule}
    ${capsule_WorldZ}=    Get Object WorldZ    ${capsule}
    ${initial_position}=    Create List    ${capsule_WorldX}    ${capsule_WorldY}    ${capsule_WorldZ}
    ${list}=    Create List    ${1}    ${1}    ${1}
    Tilt    ${list}    duration=0.1    wait=${False}
    Sleep    ${0.1}
    ${capsule}=    Find Object    NAME    Capsule
    ${capsule_WorldX}=    Get Object WorldX    ${capsule}
    ${capsule_WorldY}=    Get Object WorldY    ${capsule}
    ${capsule_WorldZ}=    Get Object WorldZ    ${capsule}
    ${final_position}=    Create List    ${capsule_WorldX}    ${capsule_WorldY}    ${capsule_WorldZ}
    Should Not Be Equal    ${initial_position}    ${final_position}

Test Acceleration And Wait
    Load Scene    Scene 1 AltDriverTestScene
    ${capsule}=    Find Object    NAME    Capsule
    ${capsule_WorldX}=    Get Object WorldX    ${capsule}
    ${capsule_WorldY}=    Get Object WorldY    ${capsule}
    ${capsule_WorldZ}=    Get Object WorldZ    ${capsule}
    ${initial_position}=    Create List    ${capsule_WorldX}    ${capsule_WorldY}    ${capsule_WorldZ}
    ${list}=    Create List    ${1}    ${1}    ${1}
    Tilt    ${list}    duration=0.1
    ${capsule}=    Find Object    NAME    Capsule
    ${capsule_WorldX}=    Get Object WorldX    ${capsule}
    ${capsule_WorldY}=    Get Object WorldY    ${capsule}
    ${capsule_WorldZ}=    Get Object WorldZ    ${capsule}
    ${final_position}=    Create List    ${capsule_WorldX}    ${capsule_WorldY}    ${capsule_WorldZ}
    Should Not Be Equal    ${initial_position}    ${final_position}

Test Find Object With Camera Id
    ${alt_button}=    Find Object    PATH    //Button
    Tap Object    ${alt_button}
    Tap Object    ${alt_button}
    ${camera}=    Find Object    PATH    //Camera
    ${camera_id}=    Get Object Id    ${camera}
    ${alt_object}=    Find Object    COMPONENT    CapsuleCollider    camera_by=ID    camera_value=${camera_id}
    ${alt_object_name}=    Get Object Name    ${alt_object}
    Should Be Equal As Strings    ${alt_object_name}    Capsule
    ${camera2}=    Find Object    PATH    //Main Camera
    ${camera_id2}=    Get Object Id    ${camera2}
    ${alt_object2}=    Find Object    COMPONENT    CapsuleCollider    camera_by=ID    camera_value=${camera_id2}
    ${alt_object_name2}=    Get Object Name    ${alt_object2}
    ${alt_object_x}=    Get Object X    ${alt_object}
    ${alt_object2_x}=    Get Object X    ${alt_object2}
    ${alt_object_y}=    Get Object Y    ${alt_object}
    ${alt_object2_y}=    Get Object Y    ${alt_object2}
    Should Be True    ${alt_object_x}!=${alt_object2_x}
    Should Be True    ${alt_object_y}!=${alt_object2_y}

Test Wait For Object With Camera Id
    ${alt_button}=    Find Object    PATH    //Button
    Tap Object    ${alt_button}
    Tap Object    ${alt_button}
    ${camera}=    Find Object    PATH    //Camera
    ${camera_id}=    Get Object Id    ${camera}
    ${alt_object}=    Wait For Object    COMPONENT    CapsuleCollider    camera_by=ID    camera_value=${camera_id}
    ${alt_object_name}=    Get Object Name    ${alt_object}
    Should Be Equal As Strings    ${alt_object_name}    Capsule
    ${camera2}=    Find Object    PATH    //Main Camera
    ${camera_id2}=    Get Object Id    ${camera2}
    ${alt_object2}=    Wait For Object    COMPONENT    CapsuleCollider    camera_by=ID    camera_value=${camera_id2}
    ${alt_object_name2}=    Get Object Name    ${alt_object2}
    ${alt_object_x}=    Get Object X    ${alt_object}
    ${alt_object2_x}=    Get Object X    ${alt_object2}
    ${alt_object_y}=    Get Object Y    ${alt_object}
    ${alt_object2_y}=    Get Object Y    ${alt_object2}
    Should Be True    ${alt_object_x}!=${alt_object2_x}
    Should Be True    ${alt_object_y}!=${alt_object2_y}

Test Find Objects With Camera Id
    ${alt_button}=    Find Object    PATH    //Button
    Tap Object    ${alt_button}
    Tap Object    ${alt_button}
    ${camera}=    Find Object    PATH    //Camera
    ${camera_id}=    Get Object Id    ${camera}
    ${alt_objects1}=    Find Objects    NAME    Plane    camera_by=ID    camera_value=${camera_id}
    ${first_element1}=    Set Variable    ${alt_objects1[0]}
    ${first_element1_name}=    Get Object Name    ${first_element1}
    Should Be Equal As Strings    ${first_element1_name}    Plane
    ${camera2}=    Find Object    PATH    //Main Camera
    ${camera_id2}=    Get Object Id    ${camera2}
    ${alt_objects2}=    Find Objects    NAME    Plane    camera_by=ID    camera_value=${camera_id2}
    ${first_element2}=    Set Variable    ${alt_objects2[0]}
    ${first_element1_x}=    Get Object X    ${first_element1}
    ${first_element2_x}=    Get Object X    ${first_element2}
    ${first_element1_y}=    Get Object Y    ${first_element1}
    ${first_element2_y}=    Get Object Y    ${first_element2}
    Should Be True    ${first_element1_x}!=${first_element2_x}
    Should Be True    ${first_element1_y}!=${first_element2_y}

Test Wait For Object Not Be Present With Camera Id
    ${camera}=    Find Object    PATH    //Main Camera
    ${camera_id}=    Get Object Id    ${camera}
    Wait For Object To Not Be Present    NAME    ObjectDestroyedIn5Secs    camera_by=ID    camera_value=${camera_id}
    ${elements}=    Get All Elements
    Should Not Contain    ${elements}    ObjectDestroyedIn5Secs

Test Wait For Object With Text With Camera Id
    ${capsule_info}=    Find Object    NAME    CapsuleInfo
    ${capsule_info_text}=    Get Text    ${capsule_info}
    ${camera}=    Find Object    PATH    //Main Camera
    ${camera_id}=    Get Object Id    ${camera}
    ${alt_object}=    Wait For Object    PATH    //CapsuleInfo[@text=${capsule_info_text}]    camera_by=ID    camera_value=${camera_id}    timeout=1
    ${alt_object_text}=    Get Text    ${alt_object}
    Should Not Be Equal    ${alt_object}    ${None}
    Should Be Equal    ${alt_object_text}    ${capsule_info_text}

Test Wait For Object Which Contains With Camera Id
    ${camera}=    Find Object    PATH    //Main Camera
    ${camera_id}=    Get Object Id    ${camera}
    ${alt_object}=    Wait For Object Which Contains    NAME    Canva    camera_by=ID    camera_value=${camera_id}
    ${alt_object_name}=    Get Object Name    ${alt_object}
    Should Be Equal As Strings    ${alt_object_name}    Canvas

Test Find Object With Tag
    ${alt_button}=    Find Object    PATH    //Button
    Tap Object    ${alt_button}
    Tap Object    ${alt_button}
    ${alt_object1}=    Find Object    COMPONENT    CapsuleCollider    camera_by=TAG    camera_value=MainCamera
    ${alt_object1_name}=    Get Object Name    ${alt_object1}
    Should Be Equal As Strings    ${alt_object1_name}    Capsule
    ${alt_object2}=    Find Object    COMPONENT    CapsuleCollider    camera_by=TAG    camera_value=Untagged
    ${alt_object1_x}=    Get Object X    ${alt_object1}
    ${alt_object2_x}=    Get Object X    ${alt_object2}
    ${alt_object1_y}=    Get Object Y    ${alt_object1}
    ${alt_object2_y}=    Get Object Y    ${alt_object2}
    Should Not Be Equal As Numbers    ${alt_object1_x}    ${alt_object2_x}
    Should Not Be Equal As Numbers    ${alt_object1_y}    ${alt_object2_y}

Test Wait For Object With Tag
    ${alt_button}=    Find Object    PATH    //Button
    Tap Object    ${alt_button}
    Tap Object    ${alt_button}
    ${alt_object1}=    Wait For Object    COMPONENT    CapsuleCollider    camera_by=TAG    camera_value=MainCamera
    ${alt_object1_name}=    Get Object Name    ${alt_object1}
    Should Be Equal As Strings    ${alt_object1_name}    Capsule
    ${alt_object2}=    Wait For Object    COMPONENT    CapsuleCollider    camera_by=TAG    camera_value=Untagged
    ${alt_object1_x}=    Get Object X    ${alt_object1}
    ${alt_object2_x}=    Get Object X    ${alt_object2}
    ${alt_object1_y}=    Get Object Y    ${alt_object1}
    ${alt_object2_y}=    Get Object Y    ${alt_object2}
    Should Not Be Equal As Numbers    ${alt_object1_x}    ${alt_object2_x}
    Should Not Be Equal As Numbers    ${alt_object1_y}    ${alt_object2_y}

Test Find Objects With Tag
    ${alt_button}=    Find Object    PATH    //Button
    Tap Object    ${alt_button}
    Tap Object    ${alt_button}
    ${alt_objects1}=    Find Objects    NAME    Plane    camera_by=TAG    camera_value=MainCamera
    ${first_element1}=    Set Variable    ${alt_objects1[0]}
    ${first_element1_name}=    Get Object Name    ${first_element1}
    Should Be Equal As Strings    ${first_element1_name}    Plane
    ${alt_objects2}=    Find Objects    NAME    Plane    camera_by=TAG    camera_value=Untagged
    ${first_element2}=    Set Variable    ${alt_objects2[0]}
    ${first_element1_x}=    Get Object X    ${first_element1}
    ${first_element2_x}=    Get Object X    ${first_element2}
    ${first_element1_y}=    Get Object Y    ${first_element1}
    ${first_element2_y}=    Get Object Y    ${first_element2}
    Should Be True    ${first_element1_x}!=${first_element2_x}
    Should Be True    ${first_element1_y}!=${first_element2_y}

Test Wait For Object Not Be Present With Tag
    ${camera}=    Find Object    PATH    //Main Camera
    Wait For Object To Not Be Present    NAME    ObjectDestroyedIn5Secs    camera_by=TAG    camera_value=MainCamera
    ${elements}=    Get All Elements
    Should Not Contain    ${elements}    ObjectDestroyedIn5Secs

Test Wait For Object With Text With Tag
    ${capsule_info}=    Find Object    NAME    CapsuleInfo
    ${capsule_info_text}=    Get Text    ${capsule_info}
    ${alt_object}=    Wait For Object    PATH    //CapsuleInfo[@text=${capsule_info_text}]    camera_by=TAG    camera_value=MainCamera    timeout=1
    ${alt_object_text}=    Get Text    ${alt_object}
    Should Not Be Equal    ${alt_object}    ${None}
    Should Be Equal    ${alt_object_text}    ${capsule_info_text}

Test Find Object By Camera
    ${button}=    Find Object    PATH    //Button
    Tap Object    ${button}
    Tap Object    ${button}
    ${alt_object}=    Find Object    COMPONENT    CapsuleCollider    camera_value=Camera
    ${alt_object_name}=    Get Object Name    ${alt_object}
    Should Be Equal As Strings    ${alt_object_name}    Capsule
    ${alt_object2}=    Find Object    COMPONENT    CapsuleCollider    camera_by=NAME    camera_value=Main Camera
    ${alt_object_x}=    Get Object X    ${alt_object}
    ${alt_object2_x}=    Get Object X    ${alt_object2}
    ${alt_object_y}=    Get Object Y    ${alt_object}
    ${alt_object2_y}=    Get Object Y    ${alt_object2}
    Should Be True    ${alt_object_x}!=${alt_object2_x}
    Should Be True    ${alt_object_y}!=${alt_object2_y}

Test Wait For Object By Camera
    ${button}=    Find Object    PATH    //Button
    Tap Object    ${button}
    Tap Object    ${button}
    ${alt_object}=    Wait For Object    COMPONENT    CapsuleCollider    camera_value=Camera
    ${alt_object_name}=    Get Object Name    ${alt_object}
    Should Be Equal As Strings    ${alt_object_name}    Capsule
    ${alt_object2}=    Wait For Object    COMPONENT    CapsuleCollider    camera_by=NAME    camera_value=Main Camera
    ${alt_object_x}=    Get Object X    ${alt_object}
    ${alt_object2_x}=    Get Object X    ${alt_object2}
    ${alt_object_y}=    Get Object Y    ${alt_object}
    ${alt_object2_y}=    Get Object Y    ${alt_object2}
    Should Be True    ${alt_object_x}!=${alt_object2_x}
    Should Be True    ${alt_object_y}!=${alt_object2_y}

Test Find Objects By Camera
    ${button}=    Find Object    PATH    //Button
    Tap Object    ${button}
    Tap Object    ${button}
    ${alt_object}=    Find Objects    NAME    Plane    camera_by=NAME    camera_value=Camera
    ${alt_object_name}=    Get Object Name    ${alt_object[0]}
    Should Be Equal As Strings    ${alt_object_name}    Plane
    ${alt_object2}=    Find Objects    NAME    Plane    camera_by=NAME    camera_value=Main Camera
    ${alt_object_x}=    Get Object X    ${alt_object[0]}
    ${alt_object2_x}=    Get Object X    ${alt_object2[0]}
    ${alt_object_y}=    Get Object Y    ${alt_object[0]}
    ${alt_object2_y}=    Get Object Y    ${alt_object2[0]}
    Should Be True    ${alt_object_x}!=${alt_object2_x}
    Should Be True    ${alt_object_y}!=${alt_object2_y}

Test Wait For Object Not Be Present By Camera
    Wait For Object To Not Be Present    NAME    ObjectDestroyedIn5Secs    camera_by=NAME    camera_value=Main Camera
    ${elements}=    Get All Elements
    ${list}=    Convert To String    ${elements}
    Should Not Contain    ${list}    'name': 'ObjectDestroyedIn5Secs'

Test Wait For Object By Camera2
    ${object}=    Find Object    NAME    CapsuleInfo
    ${text}=    Get Text    ${object}
    ${alt_object}=    Wait For Object    PATH    //CapsuleInfo[@text=${text}]    camera_by=NAME    camera_value=Main Camera    timeout=1
    ${alt_object_text}=    Get Text    ${alt_object}
    Should Not Be Equal    ${alt_object}    ${None}
    Should Be Equal    ${alt_object_text}    ${text}

Test Wait For Object Which Contains By Camera
    ${alt_object}=    Wait For Object Which Contains    NAME    Canva    camera_by=NAME    camera_value=Main Camera
    ${alt_object_name}=    Get Object Name    ${alt_object}
    Should Be Equal    ${alt_object_name}    Canvas

Test Get Component Property Complex Class
    ${alt_object}=    Find Object    NAME    Capsule
    Should Not Be Equal    ${alt_object}    ${None}
    ${property_value}=    Get Component Property    ${alt_object}    AltExampleScriptCapsule    AltSampleClass.testInt    assembly=Assembly-CSharp    max_depth=1
    Should Be Equal As Integers    ${property_value}    1

Test Get Component Property Complex Class2
    ${alt_object}=    Find Object    NAME    Capsule
    Should Not Be Equal    ${alt_object}    ${None}
    ${property_value}=    Get Component Property    ${alt_object}    AltExampleScriptCapsule    listOfSampleClass[1].testString    assembly=Assembly-CSharp    max_depth=1
    Should Be Equal As Strings    ${property_value}    test2

Test Set Component Property Complex Class
    ${alt_object}=    Find Object    NAME    Capsule
    Should Not Be Equal    ${alt_object}    ${None}
    Set Component Property    ${alt_object}    AltExampleScriptCapsule    AltSampleClass.testInt    Assembly-CSharp    2
    ${property_value}=    Get Component Property    ${alt_object}    AltExampleScriptCapsule    AltSampleClass.testInt    assembly=Assembly-CSharp    max_depth=1
    Should Be Equal As Integers    ${property_value}    2

Test Set Component Property Complex Class2
    ${alt_object}=    Find Object    NAME    Capsule
    Should Not Be Equal    ${alt_object}    ${None}
    Set Component Property    ${alt_object}    AltExampleScriptCapsule    listOfSampleClass[1].testString    Assembly-CSharp    test3
    ${property_value}=    Get Component Property    ${alt_object}    AltExampleScriptCapsule    listOfSampleClass[1].testString    assembly=Assembly-CSharp    max_depth=1
    Should Be Equal As Strings    ${property_value}    test3

Test Get Parent
    ${element}=    Find Object    NAME    Canvas/CapsuleInfo
    ${element_parent}=    Get Parent    ${element}
    ${element_parent_name}=    Get Object Name    ${element_parent}
    Should Be Equal As Strings    ${element_parent_name}    Canvas

Test Tap Coordinates
    ${capsule_element}=    Find Object    NAME    Capsule
    ${capsule_element_positions}=    Get Screen Position    ${capsule_element}
    Tap    ${capsule_element_positions}
    Wait For Object    PATH    //CapsuleInfo[@text=Capsule was clicked to jump!]    timeout=1

Test Click Coordinates
    ${capsule_element}=    Find Object    NAME    Capsule
    ${capsule_element_positions}=    Get Screen Position    ${capsule_element}
    Click    ${capsule_element_positions}
    Wait For Object    PATH    //CapsuleInfo[@text=Capsule was clicked to jump!]    timeout=1

Test Tap Element
    ${capsule_element}=    Find Object    NAME    Capsule
    Tap Object    ${capsule_element}    1
    Wait For Object    PATH    //CapsuleInfo[@text=Capsule was clicked to jump!]    timeout=1

Test Click Element
    ${capsule_element}=    Find Object    NAME    Capsule
    Click Object    ${capsule_element}
    Wait For Object    PATH    //CapsuleInfo[@text=Capsule was clicked to jump!]    timeout=1

Test Key Down And Key Up Mouse0
    ${button}=    Find Object    NAME    UIButton
    ${button_coordinates}=    Get Screen Position    ${button}
    Move Mouse    ${button_coordinates}    duration=0.1    wait=${True}
    Key Down    Mouse0
    ${object}=    Find Object    NAME    ChineseLetters
    ${text}=    Get Text    ${object}
    Should Be Equal As Strings    ${text}    哦伊娜哦

Test Camera Not Found Exception
    ${expected_error}=    Set Variable    CameraNotFoundException: Exception of type 'AltTester.AltTesterUnitySDK.Driver.CameraNotFoundException' was thrown.
    ${error}=    Run Keyword And Ignore Error
    ...    Find Object    NAME    Capsule    camera_by=NAME    camera_value=Camera
    Should Be Equal As Strings    ${error[1]}    ${expected_error}

Test Input Field Events
    ${input_field}=    Find Object    NAME    UnityUIInputField
    Set Text    ${input_field}    example    submit=${True}
    ${text}=    Get Text    ${input_field}
    Should Be Equal As Strings    ${text}    example
    ${property}=    Get Component Property    ${input_field}    AltInputFieldRaisedEvents    onValueChangedInvoked    Assembly-CSharp
    Should Be True    ${property}    ${None}
    ${property}=    Get Component Property    ${input_field}    AltInputFieldRaisedEvents    onSubmitInvoked    Assembly-CSharp
    Should Be True    ${property}    ${None}

Test Get Static Property
    ${parameters}=    Create List    1920    1080    True
    ${type_of_parameters}=    Create List    System.Int32    System.Int32    System.Boolean
    Call Static Method    UnityEngine.Screen    SetResolution    UnityEngine.CoreModule    parameters=${parameters}    type_of_parameters=${type_of_parameters}
    ${width}=    Get Static Property    UnityEngine.Screen    currentResolution.width    UnityEngine.CoreModule
    Should Be Equal As Integers    ${width}    1920

Test Set Static Property
    Set Static Property    AltExampleScriptCapsule    privateStaticVariable    Assembly-CSharp    5
    ${value}=    Get Static Property    AltExampleScriptCapsule    privateStaticVariable    Assembly-CSharp
    Should Be Equal As Integers    5    ${value}

Test Set Static Property2
    ${list}=    Create List    ${1}    ${5}    ${3}
    Set Static Property    AltExampleScriptCapsule    staticArrayOfInts[1]    Assembly-CSharp    5
    ${value}=    Get Static Property    AltExampleScriptCapsule    staticArrayOfInts    Assembly-CSharp
    Should Be Equal    ${list}    ${value}

Test Get Static Property Instance Null
    ${screen_width}=    Call Static Method    UnityEngine.Screen    get_width    UnityEngine.CoreModule
    ${width}=    Get Static Property    UnityEngine.Screen    width    UnityEngine.CoreModule
    Should Be Equal As Integers    ${width}    ${screen_width}

Test Float World Coordinates
    ${plane}=    Find Object    NAME    Plane
    ${worldX}=    Get Object WorldX    ${plane}
    ${worldY}=    Get Object WorldY    ${plane}
    ${worldZ}=    Get Object WorldZ    ${plane}
    ${is_float}=    Evaluate    isinstance($worldX, float)
    Should Be True    ${is_float}
    ${is_float}=    Evaluate    isinstance($worldY, float)
    Should Be True    ${is_float}
    ${is_float}=    Evaluate    isinstance($worldZ, float)
    Should Be True    ${is_float}

Test Keys Down
    ${keys}=    Create List    K    L
    Keys Down    ${keys}
    Keys Up    ${keys}
    ${alt_object}=    Find Object    NAME    Capsule
    ${property_value}=    Get Component Property    ${alt_object}    AltExampleScriptCapsule    stringToSetFromTests    Assembly-CSharp
    Should Be Equal As Strings    ${property_value}    multiple keys pressed

Test Press Keys
    ${keys}=    Create List    K    L
    Press Keys    ${keys}
    ${alt_object}=    Find Object    NAME    Capsule
    ${property_value}=    Get Component Property    ${alt_object}    AltExampleScriptCapsule    stringToSetFromTests    Assembly-CSharp
    Should Be Equal As Strings    ${property_value}    multiple keys pressed

Test Find Object By Coordinates
    ${counter_button}=    Find Object    NAME    ButtonCounter
    ${counter_button_x}=    Get Object X    ${counter_button}
    ${counter_button_y}=    Get Object Y    ${counter_button}
    ${coordinate_x}=    Evaluate    1+${counter_button_x}
    ${coordinate_y}=    Evaluate    1+${counter_button_y}
    ${coordinates}=    Create List    ${coordinate_x}    ${coordinate_y}
    ${element}=    Find Object At Coordinates    ${coordinates}
    ${element_name}=    Get Object Name    ${element}
    Should Be Equal As Strings    ${element_name}    ButtonCounter

Test Find Object By Coordinates No Element
    ${coordinates}=    Create List    -1    -1
    ${element}=    Find Object At Coordinates    ${coordinates}
    Should Be Equal    ${element}    ${None}

Test Call Private Method
    ${capsule_element}=    Find Object    NAME    Capsule
    Call Component Method    ${capsule_element}    AltExampleScriptCapsule    callJump    Assembly-CSharp
    ${capsule_info}=    Find Object    NAME    CapsuleInfo
    ${capsule_info_text}=    Get Text    ${capsule_info}
    Should Be Equal As Strings    ${capsule_info_text}    Capsule jumps!

Test Reset Input
    Key Down    P    power=1
    ${object}=    Find Object    NAME    AltTesterPrefab
    ${nis}=    Get Component Property    ${object}    AltTester.AltTesterUnitySDK.InputModule.NewInputSystem    Keyboard.pKey.isPressed    AltTester.AltTesterUnitySDK.InputModule
    Should Be True    ${nis}
    Reset Input
    ${nis}=    Get Component Property    ${object}    AltTester.AltTesterUnitySDK.InputModule.NewInputSystem    Keyboard.pKey.isPressed    AltTester.AltTesterUnitySDK.InputModule
    Should Not Be True    ${nis}
    ${countKeyDown}=    Find Object    NAME    AltTesterPrefab
    ${count}=    Get Component Property    ${countKeyDown}    Input    _keyCodesPressed.Count    AltTester.AltTesterUnitySDK.InputModule
    Should Be Equal As Integers    0    ${count}

*** Keywords ***
SetUp Tests
    Reset Input
    Load Scene    ${scene1}
