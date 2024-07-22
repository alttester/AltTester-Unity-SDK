*** Settings ***
Library           AltTesterLibrary
Library           OperatingSystem
Resource          utils_keywords.robot

*** Test Cases ***
Test Init Driver With Correct Host And Port
    ${host} =  Get Environment Variable  ALTSERVER_HOST
    ${port} =  Get Environment Variable  ALTSERVER_PORT
    
    Initialize AltDriver  ${host}  ${port}
    [Teardown]    Stop AltDriver

Test Init Driver With Correct Host, Port, App Name And Platform
    ${host} =  Get Environment Variable  ALTSERVER_HOST
    ${port} =  Get Environment Variable  ALTSERVER_PORT
    
    Initialize AltDriver  ${host}  ${port}   app_name=__default__   platform=OSXPlayer
    [Teardown]    Stop AltDriver

Test Init Driver With Incorrect Host
    ${host} =  Get Environment Variable  ALTSERVER_HOST
    ${port} =  Get Environment Variable  ALTSERVER_PORT
    Run Keyword And Expect Error    ConnectionError: Connection closed by AltTester(R) Server with reason: None.    Initialize AltDriver    host=somehost    timeout=1
    [Teardown]    Run Keyword And Ignore Error    Stop AltDriver

Test Init Driver With Incorrect Port
    ${host} =  Get Environment Variable  ALTSERVER_HOST
    ${port} =  Get Environment Variable  ALTSERVER_PORT
    Run Keyword And Expect Error    ConnectionError: Connection closed by AltTester(R) Server with reason: None.    Initialize AltDriver    port=12345    timeout=1
    [Teardown]    Run Keyword And Ignore Error    Stop AltDriver