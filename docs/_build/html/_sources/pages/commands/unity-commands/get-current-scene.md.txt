# Command: GetCurrentScene

## Description:

Get the current active scene.


## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
|None|

## Return
- String

## Examples
```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetCurrentScene()
        {
            Assert.AreEqual("Scene 1 AltUnityDriverTestScene", altUnityDriver.GetCurrentScene());
        }
    .. code-tab:: java

        @Test
            public void testGetCurrentScene() throws Exception {
                assertEquals("Scene 1 AltUnityDriverTestScene", altUnityDriver.getCurrentScene());
            }


    .. code-tab:: py

       //TODO
```

