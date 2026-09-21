# Screenshots

## Screenshot

### GetPNGScreenshot

Creates a screenshot of the current screen in png format.

**_Parameters_**

```eval_rst
.. list-table:: GetPNGScreenshot Parameters
   :widths: 15 15 10 60
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``path``
     - string
     - Yes
     - Location where the image is created.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetScreenshot()
        {
            var path="testC.png";
            altDriver.GetPNGScreenshot(path);
            FileAssert.Exists(path);
        }

    .. code-tab:: java

        @Test
        public void testScreenshot()
        {
            String path="testJava2.png";
            altDriver.getPNGScreenshot(path);
            assertTrue(new File(path).isFile());
        }

    .. code-tab:: py

        def test_screenshot(self):
            png_path = "testPython.png"
            self.alt_driver.get_png_screenshot(png_path)
            assert path.exists(png_path)

    .. code-tab:: robot

        Test Screenshot
            ${png_path}=    Set Variable    testPython.png
            Get Png Screenshot    ${png_path}
            File Should Exist    ${png_path}

```
