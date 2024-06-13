*** Settings***
Library    AltTesterLibrary
Suite Setup    SetUp Tests   
Suite Teardown    Teardown Tests    

*** Test Cases ***
Test Resize Panel
    Load Scene    Scene 2 Draggable Panel
    ${alt_object}=    Find Object    NAME    Resize Zone
    ${alt_object_x}=    Get Object X    ${alt_object}
    ${alt_object_y}=    Get Object Y    ${alt_object}
    ${position_init}=    Create List    ${alt_object_x}    ${alt_object_y}
    ${screen_position}=    Get Screen Position    ${alt_object}
    ${new_x}=    Evaluate    ${alt_object_x}-200
    ${new_y}=    Evaluate    ${alt_object_y}-200
    ${new_screen_position}=    Create List    ${new_x}    ${new_y}
    Swipe    ${screen_position}    ${new_screen_position}    duration=2
    ${alt_object}=    Find Object    NAME    Resize Zone
    ${alt_object_x}=    Get Object X    ${alt_object}
    ${alt_object_y}=    Get Object Y    ${alt_object}
    ${position_final}=    Create List    ${alt_object_x}    ${alt_object_y}
    Should Not Be Equal    ${position_init}    ${position_final}
    
*** Keywords ***
SetUp Tests
    Reverse Port Forwarding Android
    Initialize Altdriver

Teardown Tests
    Stop Altdriver
    Remove Reverse Port Forwarding Android