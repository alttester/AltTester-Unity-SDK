using System.Security.Permissions;
using System.Threading;
using AltTester.AltTesterSDK.Driver;
using NUnit.Framework;

public class TestForScene15 : TestBase
{
    // View A
    public string BtnPlusPath = "//New Panel Settings//btn-plus";
    public string LabelAPath = "//New Panel Settings//counter-label";
    public string BtnMinusPath = "//New Panel Settings//btn-minus";
    public string ToggleThemePath = "//New Panel Settings//toggle-theme";
    public string TextFieldNamePath = "//New Panel Settings//textfield-name";

    // View B
    public string SliderLabelPath = "//New Panel Settings//slider-label";
    public string MainSliderPath = "//New Panel Settings//main-slider";
    public string ScrollViewPath = "//New Panel Settings//scroll-view";

    // View C
    public string BtnLongPressPath = "//New Panel Settings//btn-longpress";
    public string DragAreaPath = "//New Panel Settings//drag-area";
    public string DraggableItemPath = "//New Panel Settings//draggable-item";

    // Tab bar
    public string TabAPath = "//New Panel Settings//tab-A";
    public string TabBPath = "//New Panel Settings//tab-B";
    public string TabCPath = "//New Panel Settings//tab-C";

    public TestForScene15()
    {
        sceneName = "Scene 15 UIToolkit";
    }
    [SetUp]
    public override void SetUp()
    {
        base.SetUp();
    }

    // ── View A ──────────────────────────────────────────────────────────────

    [Test]
    public void Test_CounterIncrement()
    {
        var BtnPlus = altDriver.WaitForObject(By.PATH, BtnPlusPath, timeout: 5);
        BtnPlus.Click();
        var counterLabel = altDriver.WaitForObject(By.PATH, LabelAPath);
        Assert.AreEqual("1", counterLabel.GetText());

        BtnPlus.Click();
        Assert.AreEqual("2", counterLabel.GetText());
    }

    [Test]
    public void Test_CounterDecrement()
    {
        var btnMinus = altDriver.WaitForObject(By.PATH, BtnMinusPath, timeout: 5);
        btnMinus.Click();

        var counterLabel = altDriver.WaitForObject(By.PATH, LabelAPath, timeout: 5);
        Assert.AreEqual("-1", counterLabel.GetText());
    }

    [Test]
    public void Test_CounterIncrement_And_Decrement()
    {
        var btnPlus = altDriver.WaitForObject(By.PATH, BtnPlusPath, timeout: 5);
        var btnMinus = altDriver.WaitForObject(By.PATH, BtnMinusPath, timeout: 5);
        var counterLabel = altDriver.WaitForObject(By.PATH, LabelAPath, timeout: 5);

        btnPlus.Click();
        btnPlus.Click();
        Assert.AreEqual("2", counterLabel.GetText());

        btnMinus.Click();
        Assert.AreEqual("1", counterLabel.GetText());
    }

    [Test]
    public void Test_Counter_Initial_Value()
    {
        var counterLabel = altDriver.WaitForObject(By.PATH, LabelAPath, timeout: 5);
        Assert.AreEqual("0", counterLabel.GetText());
    }

    [Test]
    public void Test_Toggle_EnableLightTheme()
    {
        var toggle = altDriver.WaitForObject(By.PATH, ToggleThemePath, timeout: 5);
        Assert.IsNotNull(toggle);

        toggle.Click();
        var toggleAfterClick = altDriver.WaitForObject(By.PATH, ToggleThemePath, timeout: 5);
        var valueAfterClick = toggleAfterClick.GetVisualElementProperty<bool>("value");
        Assert.IsTrue(valueAfterClick);
        toggle.Click();
        var toggleAfterSecondClick = altDriver.WaitForObject(By.PATH, ToggleThemePath, timeout: 5);
        var valueAfterSecondClick = toggleAfterSecondClick.GetVisualElementProperty<bool>("value");
        Assert.IsFalse(valueAfterSecondClick);
    }

    [Test]
    public void Test_TextField_SetPlayerName()
    {
        var textField = altDriver.WaitForObject(By.PATH, TextFieldNamePath, timeout: 5);
        Assert.IsNotNull(textField);

        textField.SetText("AltTester");
        var updatedField = altDriver.WaitForObject(By.PATH, TextFieldNamePath, timeout: 5);
        Assert.AreEqual("AltTester", updatedField.GetText());
    }

    [Test]
    public void Test_TextField_ClearAndRetype()
    {
        var textField = altDriver.WaitForObject(By.PATH, TextFieldNamePath, timeout: 5);
        textField.SetText("First");
        textField.SetText("Second");

        var updatedField = altDriver.WaitForObject(By.PATH, TextFieldNamePath, timeout: 5);
        Assert.AreEqual("Second", updatedField.GetText());
    }


    [Test]
    public void Test_TabA_IsActiveByDefault()
    {
        var tabA = altDriver.WaitForObject(By.PATH, TabAPath, timeout: 5);
        Assert.IsNotNull(tabA);

        var viewA = altDriver.WaitForObject(By.PATH, "//New Panel Settings//viewA", timeout: 5);
        Assert.IsNotNull(viewA);
    }

    [Test]
    public void Test_TabNavigation_AllTabs_Exist()
    {
        var tabA = altDriver.WaitForObject(By.PATH, TabAPath, timeout: 5);
        var tabB = altDriver.WaitForObject(By.PATH, TabBPath, timeout: 5);
        var tabC = altDriver.WaitForObject(By.PATH, TabCPath, timeout: 5);

        Assert.AreEqual("View A", tabA.GetText());
        Assert.AreEqual("View B", tabB.GetText());
        Assert.AreEqual("View C", tabC.GetText());
    }


    [Test]
    public void Test_TabNavigation_To_ViewB_And_Swipe_Slider()
    {
        var navB = altDriver.WaitForObject(By.PATH, TabBPath, timeout: 5);
        navB.Tap();

        var sliderLabel = altDriver.WaitForObject(By.PATH, SliderLabelPath, timeout: 5);
        Assert.AreEqual("50", sliderLabel.GetText());

        var slider = altDriver.WaitForObject(By.PATH, MainSliderPath, timeout: 5);
        altDriver.Swipe(slider.GetScreenPosition(), slider.GetScreenPosition() + new AltVector2(5000, 0), 0.5f);
        Assert.AreEqual("100", sliderLabel.GetText());
    }

    [Test]
    public void Test_ViewB_Slider_InitialValue()
    {
        var tabB = altDriver.WaitForObject(By.PATH, TabBPath, timeout: 5);
        tabB.Tap();

        var sliderLabel = altDriver.WaitForObject(By.PATH, SliderLabelPath, timeout: 5);
        Assert.AreEqual("50", sliderLabel.GetText());
    }

    [Test]
    public void Test_ViewB_Slider_SwipeToMin()
    {
        var tabB = altDriver.WaitForObject(By.PATH, TabBPath, timeout: 5);
        tabB.Tap();

        var slider = altDriver.WaitForObject(By.PATH, MainSliderPath, timeout: 5);
        altDriver.Swipe(slider.GetScreenPosition(), new AltVector2(0, slider.GetScreenPosition().y), 0.5f);

        var sliderLabel = altDriver.WaitForObject(By.PATH, SliderLabelPath, timeout: 5);
        Assert.AreEqual("0", sliderLabel.GetText());
    }

    [Test]
    public void Test_ViewB_ScrollView_Exists()
    {
        var tabB = altDriver.WaitForObject(By.PATH, TabBPath, timeout: 5);
        tabB.Tap();

        var scrollView = altDriver.WaitForObject(By.PATH, ScrollViewPath, timeout: 5);
        Assert.IsNotNull(scrollView);
    }


    [Test]
    public void Test_TabNavigation_To_ViewC()
    {
        var tabC = altDriver.WaitForObject(By.PATH, TabCPath, timeout: 5);
        tabC.Tap();

        var btnLongPress = altDriver.WaitForObject(By.PATH, BtnLongPressPath, timeout: 5);
        Assert.IsNotNull(btnLongPress);
        Assert.AreEqual("Hold Me (Long Press)", btnLongPress.GetText());
    }

    [Test]
    [Ignore("This test is currently flaky and needs investigation. It may be related to how the long press is being simulated or how the UI updates after the long press.")]
    public void Test_ViewC_LongPress_Button()
    {
        var tabC = altDriver.WaitForObject(By.PATH, TabCPath, timeout: 5);
        tabC.Tap();

        var btnLongPress = altDriver.WaitForObject(By.PATH, BtnLongPressPath, timeout: 5);
        altDriver.MoveMouse(btnLongPress.GetScreenPosition());
        altDriver.KeyDown(AltKeyCode.Mouse0);
        // altDriver.HoldButton(btnLongPress.GetScreenPosition(), 5f, false);
        Thread.Sleep(3000); // Wait for any UI updates after long press

        var btnAfter = altDriver.WaitForObject(By.PATH, BtnLongPressPath, timeout: 5);
        var text = btnAfter.GetText();
        altDriver.KeyUp(AltKeyCode.Mouse0);
        Assert.AreEqual("Success!", text, "Button text should change to 'Success!' after long press. If it doesn't, the long press might not have been registered correctly.");

        // altDriver.WaitForObject(By.TEXT, "Hold Me (Long Press)", timeout: 5);// Wait for text to revert back after long press
    }

    [Test]
    public void Test_ViewC_DragArea_Exists()
    {
        var tabC = altDriver.WaitForObject(By.PATH, TabCPath, timeout: 5);
        tabC.Tap();

        var dragArea = altDriver.WaitForObject(By.PATH, DragAreaPath, timeout: 5);
        Assert.IsNotNull(dragArea);
    }

    [Test]
    public void Test_ViewC_DraggableItem_Exists()
    {
        var tabC = altDriver.WaitForObject(By.PATH, TabCPath, timeout: 5);
        tabC.Tap();

        var draggable = altDriver.WaitForObject(By.PATH, DraggableItemPath, timeout: 5);
        Assert.IsNotNull(draggable);
    }

    [Test]
    public void Test_ViewC_Drag_DraggableItem()
    {
        var tabC = altDriver.WaitForObject(By.PATH, TabCPath, timeout: 5);
        tabC.Tap();

        var draggable = altDriver.WaitForObject(By.PATH, DraggableItemPath, timeout: 5);
        var startPos = draggable.GetScreenPosition();
        var endPos = startPos + new AltVector2(100, 0);

        altDriver.Swipe(startPos, endPos, 0.5f);

        var draggableAfter = altDriver.WaitForObject(By.PATH, DraggableItemPath, timeout: 5);
        Assert.AreNotEqual(startPos.x, draggableAfter.GetScreenPosition().x);
    }
}

