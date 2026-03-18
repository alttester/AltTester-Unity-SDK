*** Settings ***
Library    AltTesterLibrary
Library    SeleniumLibrary
Suite Setup    Suite Setup Tests
Suite Teardown    Stop Altdriver 

*** Keywords ***
Suite Setup Tests
    Open Browser    http://localhost:8000/index.html    chrome
    Set Connection Data    127.0.0.1    13005    my_app    60
    Initialize Altdriver    host="127.0.0.1"  port=13005    app_name="my_app"

Set Connection Data
    [Arguments]    ${host}=None    ${port}=None    ${app_name}=None    ${dont_show_this_again}=False    ${implicit_wait_timeout}=60
    
    # Set implicit wait
    Set Selenium Implicit Wait    ${implicit_wait_timeout}s
    
    # Update host if provided
    Run Keyword If    '${host}' != 'None'
    ...    Input Text    id:AltTesterHostInputField    ${host}
    
    # Update port if provided
    Run Keyword If    '${port}' != 'None'
    ...    Input Text    id:AltTesterPortInputField    ${port}
    
    # Update app_name if provided
    Run Keyword If    '${app_name}' != 'None'
    ...    Input Text    id:AltTesterAppNameInputField    ${app_name}
    
    # Set "Don't show this again" if specified
    ${checkbox_selected}=    Run Keyword If    '${dont_show_this_again}' == 'True'    Get Element Attribute    id:AltTesterDontShowAgainCheckbox    checked
    Run Keyword If    '${dont_show_this_again}' == 'True' and '${checkbox_selected}' != 'true'    Click Element    id:AltTesterDontShowAgainCheckbox

    # Press OK button
    Click Button    id:AltTesterOkButton