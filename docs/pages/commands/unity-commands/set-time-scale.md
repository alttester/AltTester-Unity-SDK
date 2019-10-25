# Command: SetTimeScale

## Description:

Modify the current time scale of the game. 1 is normal speed. If you want to slow down the game give a value between \[0.1\) or to make it faster a value >1.


## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| timeScale         | float       |   false  | Value to which the time scale will be changed|

## Return 
- Nothing
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

