# *** Settings ***
# Library         AltTesterLibrary
# Library         BuiltIn
# Library         Collections
# Suite Setup     Initialize AltDriver With Custom Host And Port
# Test Setup      SetUp Tests
# Suite Teardown    Stop Altdriver
# Resource        utils_keywords.robot

# *** Test Cases ***
# Test Scroll NIS
#                 ${player}=    Find Object    NAME    Player
#                 ${condition}=    Get Component Property    ${player}    AltNIPDebugScript    wasScrolled    Assembly-CSharp
#                 Should Not Be True    ${condition}
#                 Scroll    30    duration=1    wait=${True}
#                 ${condition}=    Get Component Property    ${player}    AltNIPDebugScript    wasScrolled    Assembly-CSharp
#                 Should Be True    ${condition}

# Test Key Down And Key Up NIS
#                 ${player}=    Find Object    NAME    Player
#                 ${initial_position}=    Get Component Property    ${player}    UnityEngine.Transform    position    UnityEngine.CoreModule
#                 Key Down    A
#                 Key Up    A
#                 ${lef_position}=    Get Component Property    ${player}    UnityEngine.Transform    position    UnityEngine.CoreModule
#                 Should Not Be Equal    ${lef_position}    ${initial_position}
#                 Key Down    D
#                 Key Up    D
#                 ${right_position}=    Get Component Property    ${player}    UnityEngine.Transform    position    UnityEngine.CoreModule
#                 Should Not Be Equal    ${right_position}    ${lef_position}
#                 Key Down    W
#                 Key Up    W
#                 ${up_position}=    Get Component Property    ${player}    UnityEngine.Transform    position    UnityEngine.CoreModule
#                 Should Not Be Equal    ${up_position}    ${right_position}
#                 Key Down    S
#                 Key Up    S
#                 ${down_position}=    Get Component Property    ${player}    UnityEngine.Transform    position    UnityEngine.CoreModule
#                 Should Not Be Equal    ${down_position}    ${up_position}

# Test Press Key NIS
#                 ${player}=    Find Object    NAME    Player
#                 ${initial_position}=    Get Component Property    ${player}    UnityEngine.Transform    position    UnityEngine.CoreModule
#                 Press Key    A
#                 ${lef_position}=    Get Component Property    ${player}    UnityEngine.Transform    position    UnityEngine.CoreModule
#                 Should Not Be Equal    ${lef_position}    ${initial_position}
#                 Press Key    D
#                 ${right_position}=    Get Component Property    ${player}    UnityEngine.Transform    position    UnityEngine.CoreModule
#                 Should Not Be Equal    ${right_position}    ${lef_position}
#                 Press Key    W
#                 ${up_position}=    Get Component Property    ${player}    UnityEngine.Transform    position    UnityEngine.CoreModule
#                 Should Not Be Equal    ${up_position}    ${right_position}
#                 Press Key    S
#                 ${down_position}=    Get Component Property    ${player}    UnityEngine.Transform    position    UnityEngine.CoreModule
#                 Should Not Be Equal    ${down_position}    ${up_position}

# Test Press Keys NIS
#                 ${player}=    Find Object    NAME    Player
#                 ${initial_position}=    Get Component Property    ${player}    UnityEngine.Transform    position    UnityEngine.CoreModule
#                 ${keys}=    Create List    W    Mouse0
#                 Press Keys    ${keys}
#                 ${final_position}=    Get Component Property    ${player}    UnityEngine.Transform    position    UnityEngine.CoreModule
#                 Should Not Be Equal    ${initial_position}    ${final_position}
#                 Wait For Object    NAME    SimpleProjectile(Clone)

# *** Keywords ***
# SetUp Tests
#                 Reset Input
#                 Load Scene    ${scene10}
