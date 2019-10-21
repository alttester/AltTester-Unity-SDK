# Command: Swipe

## Description:

Simulate a swipe action in your game. This command does not wait for the action to finish. To also wait for the action to finish use [SwipeAndWait]({{ site.baseurl }}/pages/commands/input-actions/swipe-and-wait)

## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| start      |     Vector2(C#)    |   false   | Starting location of the swipe|
| end      |     Vector2(C#)    |   false   | Ending location of the swipe|
| xStart      |     float(python/java)    |   false   | x coordinate of the screen where the swipe begins.|
| yStart      |     float(python/java)    |   false   | y coordinate of the screen where the swipe begins|
| xEnd      |     float(python/java)    |   false   | x coordinate of the screen where the swipe ends|
| yEnd      |     float(python/java)    |   false   | x coordinate of the screen where the swipe ends|
| duration      |     float    |   false   | The time measured in seconds to move the mouse from current position to the set location.|


## Examples

```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
            public void MultipleDragAndDrop()
            {
                var altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image1");
                var altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
                altUnityDriver.Swipe(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 1);
        
                altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image2");
                altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box2");
                altUnityDriver.Swipe(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 2);
        
                altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image3");
                altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
                altUnityDriver.Swipe(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 2);
        
        
                altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image1");
                altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
                altUnityDriver.Swipe(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 3);
        
                Thread.Sleep(4000);
                
                var imageSource = altUnityDriver.FindObject(By.NAME,"Drag Image1").GetComponentProperty("UnityEngine.UI.Image", "sprite");
                var imageSourceDropZone= altUnityDriver.FindObject(By.NAME,"Drop Image").GetComponentProperty("UnityEngine.UI.Image", "sprite");
                Assert.AreNotEqual(imageSource, imageSourceDropZone);
        
                 imageSource = altUnityDriver.FindObject(By.NAME,"Drag Image2").GetComponentProperty("UnityEngine.UI.Image", "sprite");
                 imageSourceDropZone = altUnityDriver.FindObject(By.NAME,"Drop").GetComponentProperty("UnityEngine.UI.Image", "sprite");
                Assert.AreNotEqual(imageSource, imageSourceDropZone);
               
            }

    .. code-tab:: java
        @Test
            public void testMultipleDragAndDrop() throws Exception {
                AltUnityObject altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image1");
                AltUnityObject altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box1");
                altUnityDriver.swipe(altElement1.x, altElement1.y,altElement2.x, altElement2.y, 2);
        
                altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image2");
                altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box2");
                altUnityDriver.swipe(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 2);
        
                altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image3");
                altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box1");
                altUnityDriver.swipe(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 3);
        
        
                altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image1");
                altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box1");
                altUnityDriver.swipe(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 5);
        
                Thread.sleep(6000);
        
                String imageSource = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image1").getComponentProperty("UnityEngine.UI.Image", "sprite");
                String imageSourceDropZone= altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Image").getComponentProperty("UnityEngine.UI.Image", "sprite");
                assertNotSame(imageSource, imageSourceDropZone);
        
                imageSource = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image2").getComponentProperty("UnityEngine.UI.Image", "sprite");
                imageSourceDropZone = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop").getComponentProperty("UnityEngine.UI.Image", "sprite");
                assertNotSame(imageSource, imageSourceDropZone);
        
            }



    .. code-tab:: py
        def test_multiple_swipes(self):
                self.altdriver.load_scene('Scene 3 Drag And Drop')
         
                image1 = self.altdriver.find_element('Drag Image1')
                box1 = self.altdriver.find_element('Drop Box1')
        
                self.altdriver.swipe(image1.x, image1.y, box1.x, box1.y, 5)
        
        
                image2 = self.altdriver.find_element('Drag Image2')
                box2 = self.altdriver.find_element('Drop Box2')
        
                self.altdriver.swipe(image2.x, image2.y, box2.x, box2.y, 2)
        
        
                image3 = self.altdriver.find_element('Drag Image3')
                box1 = self.altdriver.find_element('Drop Box1')
        
                self.altdriver.swipe(image3.x, image3.y, box1.x, box1.y, 3)
        
                time.sleep(6)
        
                image_source = image1.get_component_property('UnityEngine.UI.Image', 'sprite')
                image_source_drop_zone = self.altdriver.find_element('Drop Image').get_component_property('UnityEngine.UI.Image', 'sprite')
                self.assertNotEqual(image_source, image_source_drop_zone)
        
                image_source = image2.get_component_property('UnityEngine.UI.Image', 'sprite')
                image_source_drop_zone = self.altdriver.find_element('Drop').get_component_property('UnityEngine.UI.Image', 'sprite')
                self.assertNotEqual(image_source, image_source_drop_zone)

```
