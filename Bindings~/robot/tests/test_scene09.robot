# *** Settings ***
# Library         AltTesterLibrary
# Library         BuiltIn
# Library         Collections
# Suite Setup     Initialize AltDriver With Custom Host And Port
# Test Setup      SetUp Tests
# Suite Teardown    Stop Altdriver
# Resource        utils_keywords.robot

# *** Test Cases ***
# Test Scroll Element NIS
#                 ${scrollbar_initial}=    Find Object    NAME    Scrollbar Vertical
#                 ${scrollbar_initial_value}=    Get Component Property    ${scrollbar_initial}    UnityEngine.UI.Scrollbar    value    UnityEngine.UI
#                 ${scroll_view}=    Find Object    NAME    Scroll View
#                 ${scroll_view_coordinate}=    Get Screen Position    ${scroll_view}
#                 Move Mouse    ${scroll_view_coordinate}    duration=0.3    wait=${True}
#                 Scroll    speed_vertical=-3000    duration=0.5    wait=${True}
#                 ${scrollbar_final}=    Find Object    NAME    Scrollbar Vertical
#                 ${scrollbar_final_value}=    Get Component Property    ${scrollbar_final}    UnityEngine.UI.Scrollbar    value    UnityEngine.UI
#                 Should Not Be Equal    ${scrollbar_initial_value}    ${scrollbar_final_value}

# Test Swipe NIS
#                 ${scrollbar}=    Find Object    NAME    Handle
#                 ${scrollbar_position}=    Get Screen Position    ${scrollbar}
#                 ${button}=    Find Object    PATH    //Scroll View/Viewport/Content/Button (4)
#                 ${button_coordinate}=    Get Screen Position    ${button}
#                 ${new_y}=    Evaluate    ${button_coordinate[1]}+20
#                 ${new_coordinate}=    Create List    ${button_coordinate}[0]    ${new_y}
#                 Swipe    ${button_coordinate}    ${new_coordinate}    duration=0.5
#                 ${scrollbar_final}=    Find Object    NAME    Handle
#                 ${scrollbar_final_position}=    Get Screen Position    ${scrollbar_final}
#                 Should Not Be Equal    ${scrollbar_position}    ${scrollbar_final_position}

# *** Keywords ***
# SetUp Tests
#                 Reset Input
#                 Load Scene    ${scene9}
