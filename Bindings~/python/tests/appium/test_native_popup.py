"""
    Copyright(C) 2026 Altom Consulting

    Tests that verify the AltTester native connection settings popup appears
    and can be interacted with via Appium on iOS and Android.
"""

import pytest
from appium.webdriver.common.appiumby import AppiumBy


POPUP_ELEMENTS = [
    "AltTesterHostInputField",
    "AltTesterPortInputField",
    "AltTesterAppNameInputField",
    "AltTesterDontShowAgainCheckbox",
    "AltTesterOkButton",
]

HOST_PLACEHOLDER = "Host"
PORT_PLACEHOLDER = "Port"
APP_NAME_PLACEHOLDER = "App Name"
IMPLICIT_WAIT_TIMEOUT = 60


class TestNativePopup:

    @pytest.fixture(autouse=True)
    def setup(self, appium_driver, current_device):
        self.driver = appium_driver
        self.device = current_device
        self.driver.implicitly_wait(IMPLICIT_WAIT_TIMEOUT)

    def _find(self, accessibility_id):
        return self.driver.find_element(
            by=AppiumBy.ACCESSIBILITY_ID, value=accessibility_id
        )

    def test_popup_appears_on_launch(self):
        """Verify the connection settings popup is displayed when the app launches."""
        for element_id in POPUP_ELEMENTS:
            element = self._find(element_id)
            assert element.is_displayed(), \
                "Expected '{}' to be visible but it was not".format(element_id)

    def test_popup_host_field_starts_empty(self):
        """Verify the host input field starts empty, showing its placeholder text."""
        host_field = self._find("AltTesterHostInputField")
        assert host_field.text == HOST_PLACEHOLDER, \
            "Expected placeholder '{}', got '{}'".format(HOST_PLACEHOLDER, host_field.text)

    def test_popup_port_field_starts_empty(self):
        """Verify the port input field starts empty, showing its placeholder text."""
        port_field = self._find("AltTesterPortInputField")
        assert port_field.text == PORT_PLACEHOLDER, \
            "Expected placeholder '{}', got '{}'".format(PORT_PLACEHOLDER, port_field.text)

    def test_popup_app_name_field_starts_empty(self):
        """Verify the app name input field starts empty, showing its placeholder text."""
        app_name_field = self._find("AltTesterAppNameInputField")
        assert app_name_field.text == APP_NAME_PLACEHOLDER, \
            "Expected placeholder '{}', got '{}'".format(APP_NAME_PLACEHOLDER, app_name_field.text)

    def test_popup_fields_are_editable(self):
        """Verify host, port, and app name fields accept user input."""
        host_field = self._find("AltTesterHostInputField")
        host_field.clear()
        host_field.send_keys("192.168.1.1")
        assert host_field.text == "192.168.1.1"

        port_field = self._find("AltTesterPortInputField")
        port_field.clear()
        port_field.send_keys("13001")
        assert port_field.text == "13001"

        app_name_field = self._find("AltTesterAppNameInputField")
        app_name_field.clear()
        app_name_field.send_keys("MyApp")
        assert app_name_field.text == "MyApp"

    def _is_checked(self):
        # Android exposes "selected" and "checked" as distinct accessibility
        # properties; a CheckBox only ever updates "checked" on toggle, so
        # is_selected() (which reads "selected") never reflects its state.
        return self._find("AltTesterDontShowAgainCheckbox").get_attribute("checked") == "true"

    def test_dont_show_again_checkbox_is_toggleable(self):
        """Verify the 'Don't show this again' checkbox can be toggled."""
        initial_state = self._is_checked()
        self._find("AltTesterDontShowAgainCheckbox").click()
        assert self._is_checked() != initial_state, \
            "Checkbox state did not change after click"
        # restore
        self._find("AltTesterDontShowAgainCheckbox").click()
        assert self._is_checked() == initial_state

    def test_popup_dismisses_on_ok(self):
        """Verify the popup disappears after tapping OK."""
        ok_button = self._find("AltTesterOkButton")
        ok_button.click()

        self.driver.implicitly_wait(5)
        elements = self.driver.find_elements(
            by=AppiumBy.ACCESSIBILITY_ID, value="AltTesterOkButton"
        )
        assert len(elements) == 0, "Popup was still visible after tapping OK"
