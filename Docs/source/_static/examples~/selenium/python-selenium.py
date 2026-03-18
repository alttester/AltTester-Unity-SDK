import unittest

from alttester import AltDriver
from selenium import webdriver
from selenium.webdriver.common.by import By

class TestBase(unittest.TestCase):

    @classmethod
    def setUpClass(cls):
        cls.driver = webdriver.Chrome()
        cls.driver.get("http://localhost:8000")

        # Set connection data in the app
        app_name = "__default__"
        altserver_host = "127.0.0.1"
        altserver_port = "13000"
        cls.set_connection_data(host=altserver_host, port=altserver_port, app_name=app_name)

        # Initialize AltDriver
        cls.altdriver = AltDriver(host=altserver_host, port=int(altserver_port), app_name=app_name)
    
    @classmethod
    def set_connection_data(cls, host=None, port=None, app_name=None, dont_show_this_again=False, implicit_wait_timeout=60):
        if cls.driver is None:
            raise ValueError("Selenium driver cannot be None")

        # Set a longer implicit wait to ensure elements are found during connection setup
        cls.driver.implicitly_wait(implicit_wait_timeout)
            
        try:
            # Update host if provided
            if host is not None:
                host_field = cls.driver.find_element(by=By.ID, value="AltTesterHostInputField")
                host_field.clear()
                host_field.send_keys(host)

            # Update port if provided
            if port is not None:
                port_field = cls.driver.find_element(by=By.ID, value="AltTesterPortInputField")
                port_field.clear()
                port_field.send_keys(port)

            # Update app_name if provided
            if app_name is not None:
                app_name_field = cls.driver.find_element(by=By.ID, value="AltTesterAppNameInputField")
                app_name_field.clear()
                app_name_field.send_keys(app_name)

            # Set "Don't show this again" if specified
            if dont_show_this_again:
                dont_show_again_checkbox = cls.driver.find_element(by=By.ID, value="AltTesterDontShowAgainCheckbox")
                if not dont_show_again_checkbox.is_selected():
                    dont_show_again_checkbox.click()

            # Press OK button
            ok_button = cls.driver.find_element(by=By.ID, value="AltTesterOkButton")
            ok_button.click()

        except Exception as ex:
            raise Exception(f"Error while setting connection data: {str(ex)}") from ex

    @classmethod
    def tearDownClass(cls):
        if cls.altdriver:
            cls.altdriver.stop()
        if cls.driver:
            cls.driver.quit()