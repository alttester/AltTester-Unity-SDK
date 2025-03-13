# *** Settings ***
# Library         AltTesterLibrary
# Library         BuiltIn
# Library         Collections
# Suite Setup     Initialize AltDriver With Custom Host And Port
# Test Setup      SetUp Tests
# Suite Teardown    Stop Altdriver
# Resource        utils_keywords.robot

# *** Test Cases ***
# Test Movement Cube
#                 ${cube}=    Find Object    NAME    Player1
#                 ${initial_position}=    Get World Position    ${cube}
#                 Scroll    speed_vertical=30    duration=0.1    wait=${False}
#                 Press Key    K    power=1    duration=0.1    wait=${False}
#                 Press Key    O    power=1    duration=0.1
#                 ${cube}=    Find Object    NAME    Player1
#                 ${final_position}=    Get World Position    ${cube}
#                 Should Not Be Equal    ${initial_position}    ${final_position}

# Test Camera Movement
#                 ${cube}=    Find Object    NAME    Player1
#                 ${initial_position}=    Get World Position    ${cube}
#                 Press Key    W    power=1    duration=0.1    wait=${False}
#                 ${cube}=    Find Object    NAME    Player1
#                 ${final_position}=    Get World Position    ${cube}
#                 Should Not Be Equal    ${initial_position}    ${final_position}

# Test Update AltObject
#                 ${cube}=    Find Object    NAME    Player1
#                 ${initial_position_z}=    Get Object WorldZ    ${cube}
#                 Press Key    W    power=1    duration=0.1    wait=${False}
#                 Sleep    5
#                 ${cube_updated}=    Update Object    ${cube}
#                 ${final_position_z}=    Get Object WorldZ    ${cube_updated}
#                 Should Not Be Equal    ${initial_position_z}    ${final_position_z}

# Test Creating Stars
#                 ${stars}=    Find Objects Which Contain    NAME    Star    camera_by=NAME    camera_value=Player2
#                 ${appears}=    Get Length    ${stars}
#                 Should Be Equal As Integers    1    ${appears}
#                 Find Objects Which Contain    NAME    Player    camera_by=NAME    camera_value=Player2
#                 ${pressing_point_1}=    Find Object    NAME    PressingPoint1    camera_by=NAME    camera_value=Player2
#                 ${pressing_point_1_coordinates}=    Get Screen Position    ${pressing_point_1}
#                 Move Mouse    ${pressing_point_1_coordinates}    duration=0.1    wait=${False}
#                 Sleep    0.1
#                 Press Key    Mouse0    power=1    duration=0.1    wait=${False}
#                 ${pressing_point_2}=    Find Object    NAME    PressingPoint2    camera_by=NAME    camera_value=Player2
#                 ${pressing_point_2_coordinates}=    Get Screen Position    ${pressing_point_1}
#                 Move Mouse    ${pressing_point_2_coordinates}    duration=0.1    wait=${False}
#                 Press Key    Mouse0    power=1    duration=0.1    wait=${False}
#                 Sleep    0.1
#                 ${stars}=    Find Objects Which Contain    NAME    Star
#                 ${appears}=    Get Length    ${stars}
#                 Should Be Equal As Integers    3    ${appears}

# Test Power Joystick
#                 ${axis_name}=    Find Object    NAME    AxisName
#                 ${axis_value}=    Find Object    NAME    AxisValue
#                 Press Key    D    power=0.5    duration=1
#                 ${axis_name_text}=    Get Text    ${axis_name}
#                 ${axis_value_text}=    Get Text    ${axis_value}
#                 Should Be Equal As Numbers    ${axis_value_text}    0.5
#                 Should Be Equal As Strings    ${axis_name_text}    Horizontal
#                 Press Key    W    power=0.5    duration=1
#                 ${axis_name_text}=    Get Text    ${axis_name}
#                 ${axis_value_text}=    Get Text    ${axis_value}
#                 Should Be Equal As Numbers    ${axis_value_text}    0.5
#                 Should Be Equal As Strings    ${axis_name_text}    Vertical

# Test Scroll
#                 ${player2}=    Find Object    NAME    Player2
#                 ${cube_initial_position_x}=    Get Object WorldX    ${player2}
#                 ${cube_initial_position_y}=    Get Object WorldY    ${player2}
#                 ${cube_initial_position}=    Create List    ${cube_initial_position_x}    ${cube_initial_position_y}    ${cube_initial_position_y}
#                 Scroll    4    duration=1    wait=${False}
#                 Sleep    1
#                 ${player2}=    Find Object    NAME    Player2
#                 ${cube_final_position_x}=    Get Object WorldX    ${player2}
#                 ${cube_final_position_y}=    Get Object WorldY    ${player2}
#                 ${cube_final_position}=    Create List    ${cube_final_position_x}    ${cube_final_position_y}    ${cube_final_position_y}
#                 Should Not Be Equal    ${cube_initial_position}    ${cube_final_position}

# Test Scroll And Wait
#                 ${player2}=    Find Object    NAME    Player2
#                 ${cube_initial_position_x}=    Get Object WorldX    ${player2}
#                 ${cube_initial_position_y}=    Get Object WorldY    ${player2}
#                 ${cube_initial_position}=    Create List    ${cube_initial_position_x}    ${cube_initial_position_y}    ${cube_initial_position_y}
#                 Scroll    4    duration=0.3
#                 ${player2}=    Find Object    NAME    Player2
#                 ${cube_final_position_x}=    Get Object WorldX    ${player2}
#                 ${cube_final_position_y}=    Get Object WorldY    ${player2}
#                 ${cube_final_position}=    Create List    ${cube_final_position_x}    ${cube_final_position_y}    ${cube_final_position_y}
#                 Should Not Be Equal    ${cube_initial_position}    ${cube_final_position}

# Test Key Down And Key Up
#                 Key Down    A
#                 ${last_key_down}=    Find Object    NAME    LastKeyDownValue
#                 ${last_key_press}=    Find Object    NAME    LastKeyPressedValue
#                 ${last_key_down_text}=    Get Text    ${last_key_down}
#                 ${last_key_press_text}=    Get Text    ${last_key_press}
#                 Should Be Equal As Numbers    ${last_key_down_text}    97
#                 Should Be Equal As Numbers    ${last_key_press_text}    97
#                 Key Up    A
#                 ${last_key_up}=    Find Object    NAME    LastKeyUpValue
#                 ${last_key_up_text}=    Get Text    ${last_key_up}
#                 Should Be Equal As Numbers    ${last_key_up_text}    97

# *** Keywords ***
# SetUp Tests
#                 Reset Input
#                 Load Scene    ${scene5}
