*** Settings ***
Library    AppiumLibrary


*** Keywords ***
Set Connection Data
	[Arguments]    ${host}=    ${port}=    ${app_name}=    ${implicit_wait_timeout}=60

	# Wait to ensure elements are found during connection setup
    Wait Until Page Contains Element  accessibility_id=AltTesterHostInputField  timeout=${implicit_wait_timeout}

	# Update host if provided
	Run Keyword If    '${host}' != ''    Clear Text    accessibility_id=AltTesterHostInputField
	Run Keyword If    '${host}' != ''    Input Text    accessibility_id=AltTesterHostInputField    ${host}

	# Update port if provided
	Run Keyword If    '${port}' != ''    Clear Text    accessibility_id=AltTesterPortInputField
	Run Keyword If    '${port}' != ''    Input Text    accessibility_id=AltTesterPortInputField    ${port}

	# Update app name if provided
	Run Keyword If    '${app_name}' != ''    Clear Text    accessibility_id=AltTesterAppNameInputField
	Run Keyword If    '${app_name}' != ''    Input Text    accessibility_id=AltTesterAppNameInputField    ${app_name}

	# Press OK button
	Click Element    accessibility_id=AltTesterOkButton

