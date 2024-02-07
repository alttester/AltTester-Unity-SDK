*** Settings ***
Library           AltTesterLibrary
Suite Setup       SetUp Tests
Suite Teardown    TearDown Tests
Resource          utils_keywords.robot

*** Test Cases ***
Test Open Close Panel
    Load Scene    ${scene2}
    ${close_button}=    Find Object    NAME    Close Button
    Tap Object    ${close_button}
    ${button}=    Find Object    NAME    Button
    Tap Object    ${button}
    ${panel_element}=    Wait For Object    NAME    Panel
    ${is_enabled}=    Get Object Enabled    ${panel_element}
    Should Be True    ${is_enabled}

*** Keywords ***
SetUp Tests
    Reverse Port Forwarding Android
    Initialize Altdriver

TearDown Tests
    Stop Altdriver
    Remove Reverse Port Forwarding Android