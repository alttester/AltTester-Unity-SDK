"""
    Copyright(C) 2025 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
"""

import re
import ipaddress

from appium.webdriver.common.appiumby import AppiumBy
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC


class AltAppiumHelper:
    """API to interact with native popups using Appium"""

    @staticmethod
    def _is_valid_host(host):
        if not host or not isinstance(host, str):
            return False

        # Try to parse as IP address (IPv4 or IPv6)
        try:
            ipaddress.ip_address(host)
            return True
        except ValueError:
            pass

        # Validate as hostname
        # Hostname pattern: labels separated by dots, each label contains alphanumeric and hyphens
        # Label cannot start or end with hyphen, max 63 characters per label
        hostname_pattern = r'^(?!-)[a-zA-Z0-9-]{1,63}(?<!-)(\.(?!-)[a-zA-Z0-9-]{1,63}(?<!-))*\.?$'
        return re.match(hostname_pattern, host) is not None
    
    @staticmethod
    def set_connection_data(appium_driver, platform, host=None, port=None, app_name=None, timeout=60):
        """
        Sets connection data in the native popup dialog

        Args:
            appium_driver: The Appium driver instance
            platform (str): The platform ('android' or 'ios')
            host (str, optional): The host value to set. If not provided, the host field won't be updated
            port (str or int, optional): The port value to set. If not provided, the port field won't be updated
            app_name (str, optional): The app name value to set. If not provided, the app name field won't be updated
            timeout (int): Timeout in seconds for waiting for elements (default: 60)

        Raises:
            Exception: When an error occurs while interacting with the popup
        """
        if appium_driver is None:
            raise ValueError("Appium driver cannot be None")

        # Check that at least one field is provided
        if host is None and port is None and app_name is None:
            raise ValueError("At least one of 'host', 'port', or 'app_name' must be provided")

        # Validate connection data
        if host is not None and not AltAppiumHelper._is_valid_host(host):
            raise ValueError(f"Invalid host: {host}. The host should be a valid host.")

        port_str = None
        if port is not None:
            port_str = str(port) if isinstance(port, int) else port
            port_int = int(port_str)
            if port_int <= 0 or port_int > 65535:
                raise ValueError(f"Invalid port: {port_int}. The port number should be between 1 and 65535.")

        try:
            # Set XPath based on platform
            if platform.lower() == 'ios':
                host_xpath = '//XCUIElementTypeTextField[@value="Host"]'
                port_xpath = '//XCUIElementTypeTextField[@value="Port"]'
                app_name_xpath = '//XCUIElementTypeTextField[@value="App Name"]'
                ok_button_xpath = '//XCUIElementTypeButton[@name="OK"]'
            elif platform.lower() == 'android':
                host_xpath = '//android.widget.EditText[@text="Host"]'
                port_xpath = '//android.widget.EditText[@text="Port"]'
                app_name_xpath = '//android.widget.EditText[@text="App Name"]'
                ok_button_xpath = '//android.widget.Button[@resource-id="android:id/button1"]'
            else:
                raise ValueError(f"Unsupported platform: {platform}. Supported platforms are 'android' and 'ios'.")

            # Wait for the connection dialog to be present
            wait = WebDriverWait(appium_driver, timeout)
            wait.until(EC.presence_of_element_located((AppiumBy.XPATH, host_xpath)))

            # Update host if provided
            if host is not None:
                host_field = appium_driver.find_element(AppiumBy.XPATH, host_xpath)
                host_field.clear()
                host_field.send_keys(host)

            # Update port if provided
            if port is not None:
                port_field = appium_driver.find_element(AppiumBy.XPATH, port_xpath)
                port_field.clear()
                port_field.send_keys(port_str)

            # Update app_name if provided
            if app_name is not None:
                app_name_field = appium_driver.find_element(AppiumBy.XPATH, app_name_xpath)
                app_name_field.clear()
                app_name_field.send_keys(app_name)

            # Press OK button
            ok_button = appium_driver.find_element(AppiumBy.XPATH, ok_button_xpath)
            ok_button.click()

        except Exception as ex:
            raise Exception(f"Error while setting connection data on {platform} platform: {str(ex)}") from ex
