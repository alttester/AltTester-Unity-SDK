# Command: WaitForCurrentSceneToBe

## Description:

Wait for a specific scene to load

## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| sceneName      |     string   |   false   | The name of the desired scene to be loaded|
| timeout         | double        |   true   | number of seconds that it will wait for object|
| interval        | double        |   true   | number of seconds after which it will try to find the object again. interval should be smaller than timeout |

## Return
- string 

## Examples
```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestWaitForCurrentSceneToBe()
        {
            const string name = "Scene 1 AltUnityDriverTestScene";
            var timeStart = DateTime.Now;
            var currentScene = altUnityDriver.WaitForCurrentSceneToBe(name);
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.Less(time.TotalSeconds, 20);
            Assert.NotNull(currentScene);
            Assert.AreEqual("Scene 1 AltUnityDriverTestScene", currentScene);
        }
    .. code-tab:: java

        @Test
            public void testWaitForCurrentSceneToBe() throws Exception {
                String name = "Scene 1 AltUnityDriverTestScene";
                long timeStart = System.currentTimeMillis();
                String currentScene = altUnityDriver.waitForCurrentSceneToBe(name);
                long timeEnd = System.currentTimeMillis();
                long time = timeEnd - timeStart;
                assertTrue(time / 1000 < 20);
                assertNotNull(currentScene);
                assertEquals("Scene 1 AltUnityDriverTestScene", currentScene);
            }


    .. code-tab:: py

       def test_load_and_wait_for_scene(self):
               self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
               self.altdriver.wait_for_current_scene_to_be('Scene 1 AltUnityDriverTestScene',1)
               self.altdriver.load_scene('Scene 2 Draggable Panel')
               self.altdriver.wait_for_current_scene_to_be('Scene 2 Draggable Panel',1)
```

