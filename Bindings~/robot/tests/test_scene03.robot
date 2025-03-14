*** Settings ***
Library         AltTesterLibrary
Library         BuiltIn
Library         Collections
Suite Setup     Initialize AltDriver With Custom Host And Port
Test Setup      SetUp Tests
Suite Teardown    Stop Altdriver
Resource        utils_keywords.robot

*** Test Cases ***
Test Pointer Enter And Exit
                ${alt_object}=    Find Object    NAME    Drop Image
                ${color1}=    Get Component Property    ${alt_object}    AltExampleScriptDropMe    highlightColor    Assembly-CSharp
                Pointer Enter    ${alt_object}
                ${color2}=    Get Component Property    ${alt_object}    AltExampleScriptDropMe    highlightColor    Assembly-CSharp
                ${color1_r}=    Get Percent From Specific Color    ${color1}    r
                ${color1_g}=    Get Percent From Specific Color    ${color1}    g
                ${color1_b}=    Get Percent From Specific Color    ${color1}    b
                ${color1_a}=    Get Percent From Specific Color    ${color1}    a
                ${color2_r}=    Get Percent From Specific Color    ${color2}    r
                ${color2_g}=    Get Percent From Specific Color    ${color2}    g
                ${color2_b}=    Get Percent From Specific Color    ${color2}    b
                ${color2_a}=    Get Percent From Specific Color    ${color2}    a
                Evaluate    ${color1_r}!=${color2_r} or ${color1_g}!=${color2_g} or ${color1_b}!=${color2_b} or ${color1_a}!=${color2_a}
                Pointer Exit    ${alt_object}
                ${color3}=    Get Component Property    ${alt_object}    AltExampleScriptDropMe    highlightColor    Assembly-CSharp
                ${color3_r}=    Get Percent From Specific Color    ${color3}    r
                ${color3_g}=    Get Percent From Specific Color    ${color3}    g
                ${color3_b}=    Get Percent From Specific Color    ${color3}    b
                ${color3_a}=    Get Percent From Specific Color    ${color3}    a
                Evaluate    ${color3_r}!=${color2_r} or ${color3_g}!=${color2_g} or ${color3_b}!=${color2_b} or ${color3_a}!=${color2_a}
                Evaluate    ${color1_r}!=${color3_r} or ${color1_g}!=${color3_g} or ${color1_b}!=${color3_b} or ${color1_a}!=${color3_a}

Test Multiple Swipes
                Drop Image    Drag Image2    Drop Box2    1    ${False}
                Drop Image    Drag Image2    Drop Box1    1    ${False}
                Drop Image    Drag Image1    Drop Box1    2    ${False}
                Wait For Swipe To Finish
                ${image_source}    ${image_source_drop_zone}=    Get Sprite Name    Drag Image1    Drop Image
                Should Be Equal    ${image_source}    ${image_source_drop_zone}
                ${image_source}    ${image_source_drop_zone}=    Get Sprite Name    Drag Image2    Drop
                Should Be Equal    ${image_source}    ${image_source_drop_zone}

Test Multiple Swipe And Waits
                Drop Image    Drag Image2    Drop Box2    1    ${True}
                Drop Image    Drag Image2    Drop Box1    1    ${True}
                Drop Image    Drag Image1    Drop Box1    1    ${True}
                ${image_source}    ${image_source_drop_zone}=    Get Sprite Name    Drag Image1    Drop Image
                Should Be Equal    ${image_source}    ${image_source_drop_zone}
                ${image_source}    ${image_source_drop_zone}=    Get Sprite Name    Drag Image2    Drop
                Should Be Equal    ${image_source}    ${image_source_drop_zone}

Test Multiple Swipe With Multipoint Swipe
                ${list1}=    Create List    Drag Image1    Drop Box1
                ${list2}=    Create List    Drag Image2    Drop Box1    Drop Box2
                Drop Image With Multipoint Swipe    ${list1}    1    ${False}
                Drop Image With Multipoint Swipe    ${list2}    1    ${False}
                ${image_source}    ${image_source_drop_zone}=    Get Sprite Name    Drag Image1    Drop Image
                Should Be Equal    ${image_source}    ${image_source_drop_zone}
                ${image_source}    ${image_source_drop_zone}=    Get Sprite Name    Drag Image2    Drop
                Should Be Equal    ${image_source}    ${image_source_drop_zone}

Test Multiple Swipe And Waits With Multipoint Swipe
                ${list1}=    Create List    Drag Image1    Drop Box1
                ${list2}=    Create List    Drag Image2    Drop Box1    Drop Box2
                Drop Image With Multipoint Swipe    ${list1}    1    ${True}
                Drop Image With Multipoint Swipe    ${list2}    1    ${True}
                ${image_source}    ${image_source_drop_zone}=    Get Sprite Name    Drag Image1    Drop Image
                Should Be Equal    ${image_source}    ${image_source_drop_zone}
                ${image_source}    ${image_source_drop_zone}=    Get Sprite Name    Drag Image2    Drop
                Should Be Equal    ${image_source}    ${image_source_drop_zone}

Test Begin Move End Touch
                ${alt_object1}=    Find Object    NAME    Drag Image1
                ${alt_object2}=    Find Object    NAME    Drop Box1
                ${coordinates_begin}=    Get Screen Position    ${alt_object1}
                ${coordinates_finish}=    Get Screen Position    ${alt_object2}
                ${id}=    Begin Touch    ${coordinates_begin}
                Move Touch    ${id}    ${coordinates_finish}
                End Touch    ${id}
                ${image_source}=    Get Component Property    ${alt_object1}    UnityEngine.UI.Image    sprite.name    UnityEngine.UI
                ${alt_object}=    Find Object    NAME    Drop Image
                ${image_source_drop_zone}=    Get Component Property    ${alt_object}    UnityEngine.UI.Image    sprite.name    UnityEngine.UI
                Should Be Equal    ${image_source}    ${image_source_drop_zone}

*** Keywords ***
SetUp Tests
                Reset Input
                Load Scene    ${scene3}

Wait For Swipe To Finish
                Wait For Object To Not Be Present    NAME    icon

Drop Image
                [Arguments]    ${drag_location_name}    ${drop_location_name}    ${duration}    ${wait}
                ${drag_location}=    Find Object    NAME    ${drag_location_name}
                ${drop_location}=    Find Object    NAME    ${drop_location_name}
                ${drag_location_position}=    Get Screen Position    ${drag_location}
                ${drop_location_position}=    Get Screen Position    ${drop_location}
                Swipe    ${drag_location_position}    ${drop_location_position}    duration=${duration}    wait=${wait}

Get Percent From Specific Color
                [Arguments]    ${alt_object}    ${color}
                ${percent}=    Collections.Get From Dictionary    ${alt_object}    ${color}
                Return From Keyword    ${percent}
