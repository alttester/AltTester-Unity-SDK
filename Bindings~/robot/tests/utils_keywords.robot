*** Settings ***
Library           AltTesterLibrary
Library           BuiltIn
Library           Collections
Library           OperatingSystem

*** Variables ***
${host}           127.0.0.1
${port}           13000
${scene1}         Scene 1 AltDriverTestScene
${scene2}         Scene 2 Draggable Panel
${scene3}         Scene 3 Drag And Drop
${scene5}         Scene 5 Keyboard Input
${scene7A}        Scene 7 New Input System Actions
${scene7B}        Scene 7 Drag And Drop NIS
${scene9}         scene 9 NIS
${scene10}        Scene 10 Sample NIS

*** Keywords ***
Get Sprite Name
    [Arguments]    ${source_image_name}    ${image_source_drop_zone_name}
    ${object}=    Find Object    NAME    ${source_image_name}
    ${image_source}=    Get Component Property    ${object}    UnityEngine.UI.Image    sprite.name    UnityEngine.UI
    ${object}=    Find Object    NAME    ${image_source_drop_zone_name}
    ${image_source_drop_zone}=    Get Component Property    ${object}    UnityEngine.UI.Image    sprite.name    UnityEngine.UI
    Return From Keyword    ${image_source}    ${image_source_drop_zone}

Drop Image With Multipoint Swipe
    [Arguments]    ${object_names}    ${duration}    ${wait}
    ${positions}=    Create List
    FOR    ${name}    IN    @{object_names}
        ${alt_object}=    Find Object    NAME    ${name}
        ${alt_object_positions}=    Get Screen Position    ${alt_object}
        Append To List    ${positions}    ${alt_object_positions}
    END
    Multipoint Swipe    ${positions}    duration=${duration}    wait=${wait}

Initialize AltDriver With Custom Host And Port
    [Arguments]
    ${host} =  Get Environment Variable  ALTSERVER_HOST
    ${port} =  Get Environment Variable  ALTSERVER_PORT
    Initialize AltDriver    ${host}    ${port}