*** Settings***
Library    AltTesterLibrary
Suite Setup    Initialize Altdriver
Suite Teardown    Stop Altdriver 

*** Test Cases ***
Test Open Close Panel
    Load Scene    MainMenu
    ${close_button}=    Find Object    NAME    Close Button
    Click Object    ${close_button}
    ${button}=    Find Object    NAME    Button
    Click Object    ${button}
    ${panel_element}=         Wait For Object    NAME         Panel
    Should Be True    ${panel_element.enabled}
