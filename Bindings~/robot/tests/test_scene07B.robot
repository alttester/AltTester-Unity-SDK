*** Settings ***
Library         AltTesterLibrary
Library         BuiltIn
Library         Collections
Suite Setup     Initialize AltDriver With Custom Host And Port
Test Setup      SetUp Tests
Suite Teardown    Stop Altdriver
Resource        utils_keywords.robot

*** Test Cases ***
Test Multipoint Swipe NIS
                ${list1}=    Create List    Drag Image1    Drop Box1
                ${list2}=    Create List    Drag Image2    Drop Box1    Drop Box2
                Drop Image With Multipoint Swipe    ${list1}    1    ${False}
                Drop Image With Multipoint Swipe    ${list2}    1    ${False}
                ${image_source}    ${image_source_drop_zone}=    Get Sprite Name    Drag Image1    Drop Image
                Should Be Equal    ${image_source}    ${image_source_drop_zone}
                ${image_source}    ${image_source_drop_zone}=    Get Sprite Name    Drag Image2    Drop
                Should Be Equal    ${image_source}    ${image_source_drop_zone}

*** Keywords ***
SetUp Tests
                Reset Input
                Load Scene    ${scene7B}
