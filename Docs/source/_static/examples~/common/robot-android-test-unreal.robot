*** Settings***
Library    AltTesterLibrary
Suite Setup    SetUp Tests   
Suite Teardown    Teardown Tests    

*** Test Cases ***
Test Open Close Panel
    Load Scene    MainMenu
    ${close_button}=    Find Object    NAME    Close Button
    Click Object    ${close_button}
    ${button}=    Find Object    NAME    Button
    Click Object    ${button}
    ${panel_element}=         Wait For Object    NAME         Panel
    Should Be True    ${panel_element.enabled}
    
*** Keywords ***
SetUp Tests
    Reverse Port Forwarding Android
    Initialize Altdriver

Teardown Tests
    Stop Altdriver
    Remove Reverse Port Forwarding Android