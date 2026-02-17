*** Settings ***
Library    SeleniumLibrary
Library    AltTesterLibrary

*** Test Cases ***
Test Connection Settings Popup
    Open Browser    http://localhost:8080/index.html    chrome
    
    # Set connection data in the app
    ${app_name}=    Set Variable    my_app
    ${altserver_host}=    Set Variable    127.0.0.1
    ${altserver_port}=    Set Variable    13005
    
    Set Connection Data    ${altserver_host}    ${altserver_port}    ${app_name}
    
    # Initialize AltDriver
    Initialize Altdriver    host=${altserver_host}    port=${altserver_port}    app_name=${app_name}
    
    Close AltDriver
    Close Browser

*** Keywords ***
Set Connection Data
    [Arguments]    ${host}=None    ${port}=None    ${app_name}=None    ${implicit_wait_timeout}=60
    
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
    
    # Press OK button
    Click Button    id:AltTesterOkButton