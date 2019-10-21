# Command: GetText

## Description:

Get text value from a Button, Text, InputField. This also works with TextMeshPro elements.


## Examples
<!-- Language Specific -->
<div>
    <button class="language-btn active">C#</button>
    <button class="language-btn">Java</button>
    <button class="language-btn">Python</button>
</div>
<div id="language-c" class="languageContent" markdown=1 style="display:block;">

``` c#

    [Test]
    public void TestWaitForElementWithText()
    {
        const string name = "CapsuleInfo";
        string text = altUnityDriver.FindObject(By.NAME,name).GetText();
        var timeStart = DateTime.Now;
        var altElement = altUnityDriver.WaitForObjectWithText(By.NAME, name, text);
        var timeEnd = DateTime.Now;
        var time = timeEnd - timeStart;
        Assert.Less(time.TotalSeconds, 20);
        Assert.NotNull(altElement);
        Assert.AreEqual(altElement.GetText(), text);

    }


```

</div>
<div id="language-python" class="languageContent" markdown=1>

``` python

    def test_call_component_method(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        result = self.altdriver.find_element("Capsule").call_component_method("Capsule", "Jump", "setFromMethod")
        self.assertEqual(result,"null")
        self.altdriver.wait_for_element_with_text('CapsuleInfo', 'setFromMethod')
        self.assertEqual('setFromMethod', self.altdriver.find_element('CapsuleInfo').get_text())

```

</div>
<div id="language-java" class="languageContent" markdown=1>

``` java
  @Test
    public void testWaitForElementWithText() throws Exception {
        String name = "CapsuleInfo";
        String text = altUnityDriver.findObject(AltUnityDriver.By.NAME,name).getText();
        long timeStart = System.currentTimeMillis();
        AltUnityObject altElement = altUnityDriver.waitForObjectWithText(AltUnityDriver.By.NAME,name, text);
        long timeEnd = System.currentTimeMillis();
        long time = timeEnd - timeStart;
        assertTrue(time / 1000 < 20);
        assertNotNull(altElement);
        assertEquals(altElement.getText(), text);

    }

```
</div>