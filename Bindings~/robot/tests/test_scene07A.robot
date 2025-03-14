*** Settings ***
Library         AltTesterLibrary
Library         BuiltIn
Library         Collections
Suite Setup     Initialize AltDriver With Custom Host And Port
Test Setup      SetUp Tests
Suite Teardown    Stop Altdriver
Resource        utils_keywords.robot

*** Test Cases ***
Test Tap Element NIS
                ${capsule}=    Find Object    NAME    Capsule
                Tap Object    ${capsule}
                ${property_value}=    Get Component Property    ${capsule}    AltExampleNewInputSystem    jumpCounter    Assembly-CSharp    max_depth=1
                Should Be Equal As Integers    ${property_value}    1

Test Tap Coordinates NIS
                ${capsule}=    Find Object    NAME    Capsule
                ${capsule_coordinate}=    Get Screen Position    ${capsule}
                Tap    ${capsule_coordinate}
                ${action_info}=    Wait For Object    PATH    //ActionText[@text=Capsule was tapped!]    timeout=1
                ${action_info_text}=    Get Text    ${action_info}
                Should Be Equal As Strings    ${action_info_text}    Capsule was tapped!

Test Click Element NIS
                ${capsule}=    Find Object    NAME    Capsule
                Click Object    ${capsule}
                ${property_value}=    Get Component Property    ${capsule}    AltExampleNewInputSystem    jumpCounter    Assembly-CSharp    max_depth=1
                Should Be Equal As Integers    ${property_value}    1

Test Click Coordinates NIS
                ${capsule}=    Find Object    NAME    Capsule
                ${capsule_coordinate}=    Get Screen Position    ${capsule}
                Click    ${capsule_coordinate}
                ${action_info}=    Wait For Object    PATH    //ActionText[@text=Capsule was clicked!]    timeout=1
                ${action_info_text}=    Get Text    ${action_info}
                Should Be Equal As Strings    ${action_info_text}    Capsule was clicked!

Test Tilt
                ${cube}=    Find Object    NAME    Cube (1)
                ${initial_position}=    Get World Position    ${cube}
                ${acceleration}=    Create List    1000    10    10
                Tilt    ${acceleration}    duration=1
                ${final_position}=    Get World Position    ${cube}
                ${is_moved}=    Get Component Property    ${cube}    AltCubeNIS    isMoved    Assembly-CSharp
                Should Be True    ${is_moved}

*** Keywords ***
SetUp Tests
                Reset Input
                Load Scene    ${scene7A}
