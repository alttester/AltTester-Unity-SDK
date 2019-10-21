# Command: GetTimeScale

## Description:

Return the value of the current time scale


## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
|None|

## Return
- float
## Examples
```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
            public void TestSetTimeScale() {
                altUnityDriver.SetTimeScale(0.1f);
                Thread.Sleep(1000);
                var timeScaleFromGame = altUnityDriver.GetTimeScale();
                Assert.AreEqual(0.1f, timeScaleFromGame);
                altUnityDriver.SetTimeScale(1);
            }
    .. code-tab:: java

        @Test
            public void TestGetSetTimeScale(){
                altUnityDriver.setTimeScale(0.1f);
                float timeScale = altUnityDriver.getTimeScale();
                assertEquals(0.1f, timeScale,0);
                altUnityDriver.setTimeScale(1f);
            }


    .. code-tab:: py

       def test_set_and_get_time_scale(self):
               self.altdriver.set_time_scale(0.1)
               time.sleep(1)
               time_scale=self.altdriver.get_time_scale()
               self.assertEquals(0.1, time_scale)
               self.altdriver.set_time_scale(1)
```

