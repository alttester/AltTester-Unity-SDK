# Command: TapScreen(c#) / TapAtCoordinates(python/java)

## Description:

Simulate a tap action on the screen at the given coordinates.

## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| x      |     float    |   false   |  x coordinate of the screen|
| y      |     float    |   false   |  y coordinate of the screen|



## Examples

```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
            public void TestClickScreen()
            {
                const string name = "UIButton";
                var altElement2 = altUnityDriver.FindObject(By.NAME,name);
                var altElement = altUnityDriver.TapScreen(altElement2.x, altElement2.y);
                Assert.AreEqual(name, altElement.name);
                altUnityDriver.WaitForObjectWithText(By.NAME,"CapsuleInfo", "UIButton clicked to jump capsule!");
            }

    .. code-tab:: java
        @Test
            public void testTapScreen() throws Exception {
                AltUnityObject capsule = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Capsule");
                AltUnityObject capsuleInfo = altUnityDriver.findObject(AltUnityDriver.By.NAME,"CapsuleInfo");
                altUnityDriver.tapScreen(capsule.x, capsule.y);
                Thread.sleep(2);
                String text = capsuleInfo.getText();
                assertEquals(text, "Capsule was clicked to jump!");
            }

    .. code-tab:: py
        def test_tap_at_coordinates(self):
                self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
                capsule_element = self.altdriver.find_element('Capsule')
                self.altdriver.tap_at_coordinates(capsule_element.x, capsule_element.y)
                self.altdriver.wait_for_element_with_text('CapsuleInfo', 'Capsule was clicked to jump!','',1)


```
