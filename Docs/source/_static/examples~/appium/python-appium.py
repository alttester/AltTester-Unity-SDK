def set_connection_data(cls, host=None, port=None, app_name=None, implicit_wait_timeout=60):
    if cls.appium_driver is None:
        raise ValueError("Appium driver cannot be None")

    # Set a longer implicit wait to ensure elements are found during connection setup
    cls.appium_driver.implicitly_wait(implicit_wait_timeout)
        
    try:
        # Update host if provided
        if host is not None:
            host_field = cls.appium_driver.find_element(by=AppiumBy.ACCESSIBILITY_ID, value="AltTesterHostInputField")
            host_field.clear()
            host_field.send_keys(host)

        # Update port if provided
        if port is not None:
            port_field = cls.appium_driver.find_element(by=AppiumBy.ACCESSIBILITY_ID, value="AltTesterPortInputField")
            port_field.clear()
            port_field.send_keys(port)

        # Update app_name if provided
        if app_name is not None:
            app_name_field = cls.appium_driver.find_element(by=AppiumBy.ACCESSIBILITY_ID, value="AltTesterAppNameInputField")
            app_name_field.clear()
            app_name_field.send_keys(app_name)

        # Press OK button
        ok_button = cls.appium_driver.find_element(by=AppiumBy.ACCESSIBILITY_ID, value="AltTesterOkButton")
        ok_button.click()

    except Exception as ex:
        raise Exception(f"Error while setting connection data: {str(ex)}") from ex