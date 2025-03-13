# *** Settings ***
# Library         AltTesterLibrary
# Library         BuiltIn
# Library         Collections
# Suite Setup     Initialize AltDriver With Custom Host And Port
# Test Setup      SetUp Tests
# Suite Teardown    Stop Altdriver
# Resource        utils_keywords.robot

# *** Test Cases ***
# Test Get All Elements
#                 ${elements}=    Get All Elements    enabled=${False}
#                 Should Not Be Empty    ${elements}
#                 ${expected_names}=    Create List    EventSystem    Canvas    Panel Drag Area    Panel    Header    Text    Drag Zone    Resize Zone    Close Button    Debugging    SF Scene Elements    Main Camera    Background    Particle System
#                 ${input_marks}=    Create List
#                 ${names}=    Create List
#                 FOR    ${element}    IN    @{elements}
#                 ${name}=    Get Object Name    ${element}
#                 Run Keyword If    '${name}'== 'InputMark(Clone)'    Append TransformId To List    ${element}    ${input_marks}
#                 ${element_name}=    Get Object Name    ${element}
#                 Append To List    ${names}    ${element_name}
#                 END
#                 FOR    ${name}    IN    @{expected_names}
#                 Should Contain    ${names}    ${name}
#                 END

# Test Get All Enabled Elements
#                 ${elements}=    Get All Elements    enabled=${False}
#                 ${names}=    Create List
#                 Should Not Be Empty    ${elements}
#                 ${expected_names}=    Create List    EventSystem    Canvas    Panel Drag Area    Panel    Header    Text    Drag Zone    Resize Zone    Close Button    Debugging    SF Scene Elements    Main Camera    Background    Particle System
#                 FOR    ${element}    IN    @{elements}
#                 ${name}=    Get Object Name    ${element}
#                 Append To List    ${names}    ${name}
#                 END
#                 ${list_length}=    Get length    ${names}
#                 Evaluate    ${list_length}>=22
#                 FOR    ${name}    IN    @{expected_names}
#                 Should Contain    ${names}    ${name}
#                 END

# Test Resize Panel
#                 ${alt_object}=    Find Object    NAME    Resize Zone
#                 ${alt_object_x}=    Get Object X    ${alt_object}
#                 ${alt_object_y}=    Get Object Y    ${alt_object}
#                 ${position_init}=    Create List    ${alt_object_x}    ${alt_object_y}
#                 ${screen_position}=    Get Screen Position    ${alt_object}
#                 ${new_x}=    Evaluate    ${alt_object_x}-200
#                 ${new_y}=    Evaluate    ${alt_object_y}-200
#                 ${new_screen_position}=    Create List    ${new_x}    ${new_y}
#                 Swipe    ${screen_position}    ${new_screen_position}    duration=2
#                 ${alt_object}=    Find Object    NAME    Resize Zone
#                 ${alt_object_x}=    Get Object X    ${alt_object}
#                 ${alt_object_y}=    Get Object Y    ${alt_object}
#                 ${position_final}=    Create List    ${alt_object_x}    ${alt_object_y}
#                 Should Not Be Equal    ${position_init}    ${position_final}

# Test Resize Panel With Multipoint Swipe
#                 ${alt_object}=    Find Object    NAME    Resize Zone
#                 ${alt_object_x}=    Get Object X    ${alt_object}
#                 ${alt_object_y}=    Get Object Y    ${alt_object}
#                 ${position_init}=    Create List    ${alt_object_x}    ${alt_object_y}
#                 ${screen_position}=    Get Screen Position    ${alt_object}
#                 ${positions}=    Create List    ${screen_position}
#                 ${new_x}=    Evaluate    ${alt_object_x}-200
#                 ${new_y}=    Evaluate    ${alt_object_y}-200
#                 ${new_screen_position}=    Create List    ${new_x}    ${new_y}
#                 Append To List    ${positions}    ${new_screen_position}
#                 ${new_x}=    Evaluate    ${alt_object_x}-300
#                 ${new_y}=    Evaluate    ${alt_object_y}-100
#                 ${new_screen_position}=    Create List    ${new_x}    ${new_y}
#                 Append To List    ${positions}    ${new_screen_position}
#                 ${new_x}=    Evaluate    ${alt_object_x}-50
#                 ${new_y}=    Evaluate    ${alt_object_y}-100
#                 ${new_screen_position}=    Create List    ${new_x}    ${new_y}
#                 Append To List    ${positions}    ${new_screen_position}
#                 ${new_x}=    Evaluate    ${alt_object_x}-100
#                 ${new_y}=    Evaluate    ${alt_object_y}-100
#                 ${new_screen_position}=    Create List    ${new_x}    ${new_y}
#                 Append To List    ${positions}    ${new_screen_position}
#                 Multipoint Swipe    ${positions}    duration=4
#                 ${alt_object}=    Find Object    NAME    Resize Zone
#                 ${alt_object_x}=    Get Object X    ${alt_object}
#                 ${alt_object_y}=    Get Object Y    ${alt_object}
#                 ${position_final}=    Create List    ${alt_object_x}    ${alt_object_y}
#                 Should Not Be Equal    ${position_init}    ${position_final}

# Test Pointer Down From Object
#                 ${panel}=    Find Object    NAME    Panel
#                 ${color1}=    Get Component Property    ${panel}    AltExampleScriptPanel    normalColor    Assembly-CSharp
#                 Pointer Down    ${panel}
#                 ${color2}=    Get Component Property    ${panel}    AltExampleScriptPanel    highlightColor    Assembly-CSharp
#                 Should Not Be Equal    ${color1}    ${color2}

# Test Pointer Up From Object
#                 ${panel}=    Find Object    NAME    Panel
#                 ${color1}=    Get Component Property    ${panel}    AltExampleScriptPanel    normalColor    Assembly-CSharp
#                 Pointer Down    ${panel}
#                 Pointer Up    ${panel}
#                 ${color2}=    Get Component Property    ${panel}    AltExampleScriptPanel    highlightColor    Assembly-CSharp
#                 Should Be Equal    ${color1}    ${color2}

# Test New Touch Commands
#                 ${draggable_area}=    Find Object    NAME    Drag Zone
#                 ${initial_position}=    Get Screen Position    ${draggable_area}
#                 ${finger_id}=    Begin Touch    ${initial_position}
#                 ${draggable_area_x}=    Get Object X    ${draggable_area}
#                 ${draggable_area_y}=    Get Object Y    ${draggable_area}
#                 ${new_x}=    Evaluate    ${draggable_area_x}+10
#                 ${new_y}=    Evaluate    ${draggable_area_y}+10
#                 ${new_screen_position}=    Create List    ${new_x}    ${new_y}
#                 Move Touch    ${finger_id}    ${new_screen_position}
#                 End Touch    ${finger_id}
#                 ${draggable_area}=    Find Object    NAME    Drag Zone
#                 ${final_position}=    Get Screen Position    ${draggable_area}
#                 Should Not Be Equal    ${initial_position}    ${final_position}

# *** Keywords ***
# SetUp Tests
#                 Reset Input
#                 Load Scene    ${scene2}

# Append TransformId To List
#                 [Arguments]    ${element}    ${list_to_append}
#                 ${element_transformId}=    Get Object TransformId    ${element}
#                 Append To List    ${list_to_append}    ${element_transformId}
