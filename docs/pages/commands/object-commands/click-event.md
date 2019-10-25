# Command: ClickEvent

## Description:

Simulate a click on the object. It will click the object even if the object is not visible something that you could not do on a real device.


## Examples

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestDifferentCamera()
        {
            var altButton = altUnityDriver.FindObject(By.NAME,"Button", "Main Camera");
            altButton.ClickEvent();
            altButton.ClickEvent();
            var altElement = altUnityDriver.FindObject(By.NAME,"Capsule", "Main Camera");
            var altElement2 = altUnityDriver.FindObject(By.NAME,"Capsule", "Camera");
            Vector2 pozOnScreenFromMainCamera = new Vector2(altElement.x, altElement.y);
            Vector2 pozOnScreenFromSecondaryCamera = new Vector2(altElement2.x, altElement2.y);

            Assert.AreNotEqual(pozOnScreenFromSecondaryCamera, pozOnScreenFromMainCamera);

        }

    .. code-tab:: java

        @Test
        public void testDifferentCamera() throws Exception {
            AltUnityObject altButton = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Button","Main Camera");
            altButton.clickEvent();
            altButton.clickEvent();
            AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Capsule", "Main Camera");
            AltUnityObject altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Capsule", "Camera");
            assertNotSame(altElement.x, altElement2.x);
            assertNotSame(altElement.y, altElement2.y);
        }



    .. code-tab:: py
     //TODO

```