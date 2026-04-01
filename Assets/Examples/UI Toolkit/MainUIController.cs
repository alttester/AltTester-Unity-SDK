using UnityEngine;
using UnityEngine.UIElements;

public class MainUIController : MonoBehaviour
{
    private UIDocument _uiDocument;

    // Tabs
    private Button _tabA, _tabB, _tabC;
    private VisualElement _viewA, _viewB, _viewC;

    // View A Elements
    private Label _counterLabel;
    private int _counter = 0;

    // View B Elements
    private Label _sliderLabel;

    // View C Elements
    private Button _longPressBtn;
    private IVisualElementScheduledItem _longPressTask;
    private bool _isPointerDown = false;

    // Drag elements
    private VisualElement _draggableItem;
    private VisualElement _dragArea;
    private bool _isDragging = false;
    private Vector2 _dragOffset;

    // Scroll Drag elements
    private ScrollView _scrollView;
    private bool _isScrolling = false;
    private Vector2 _scrollStartPosition;
    private Vector2 _scrollStartOffset;

    void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        var root = _uiDocument.rootVisualElement;

        // --- BIND TABS ---
        _tabA = root.Q<Button>("tab-A");
        _tabB = root.Q<Button>("tab-B");
        _tabC = root.Q<Button>("tab-C");

        _viewA = root.Q<VisualElement>("viewA");
        _viewB = root.Q<VisualElement>("viewB");
        _viewC = root.Q<VisualElement>("viewC");

        _tabA.clicked += () => SwitchTab(_tabA, _viewA);
        _tabB.clicked += () => SwitchTab(_tabB, _viewB);
        _tabC.clicked += () => SwitchTab(_tabC, _viewC);

        // --- BIND VIEW A (Clicks, Toggles, Text) ---
        _counterLabel = root.Q<Label>("counter-label");
        root.Q<Button>("btn-minus").clicked += () => UpdateCounter(-1);
        root.Q<Button>("btn-plus").clicked += () => UpdateCounter(1);

        var themeToggle = root.Q<Toggle>("toggle-theme");
        themeToggle.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue)
            {
                // Add the light-theme class to the root element
                root.AddToClassList("light-theme");
            }
            else
            {
                // Remove it to revert to the default dark theme
                root.RemoveFromClassList("light-theme");
            }
        });
        root.Q<TextField>("textfield-name").RegisterValueChangedCallback(evt => Debug.Log($"Name changed to: {evt.newValue}"));

        // --- BIND VIEW B (Slider & ScrollView) ---
        _sliderLabel = root.Q<Label>("slider-label");
        var slider = root.Q<Slider>("main-slider");
        slider.RegisterValueChangedCallback(evt =>
        {
            _sliderLabel.text = Mathf.RoundToInt(evt.newValue).ToString();
        });

        _scrollView = root.Q<ScrollView>("scroll-view");
        PopulateScrollView(_scrollView);

        // Register custom drag-to-scroll events
        _scrollView.RegisterCallback<PointerDownEvent>(OnScrollPointerDown);
        _scrollView.RegisterCallback<PointerMoveEvent>(OnScrollPointerMove);
        _scrollView.RegisterCallback<PointerUpEvent>(OnScrollPointerUp);
        _scrollView.RegisterCallback<PointerCaptureOutEvent>(OnScrollPointerUp);

        // --- BIND VIEW C (Long Press & Drag) ---
        _longPressBtn = root.Q<Button>("btn-longpress");

        // Use TrickleDown to catch the event BEFORE the Button consumes it natively
        _longPressBtn.RegisterCallback<PointerDownEvent>(OnPointerDown, TrickleDown.TrickleDown);
        _longPressBtn.RegisterCallback<PointerUpEvent>(OnPointerUp, TrickleDown.TrickleDown);
        _longPressBtn.RegisterCallback<PointerLeaveEvent>(OnPointerUp, TrickleDown.TrickleDown);


        _draggableItem = root.Q<VisualElement>("draggable-item");
        _dragArea = root.Q<VisualElement>("drag-area");

        _draggableItem.RegisterCallback<PointerDownEvent>(OnDragStart);
        _draggableItem.RegisterCallback<PointerMoveEvent>(OnDragUpdate);
        _draggableItem.RegisterCallback<PointerUpEvent>(OnDragEnd);
        _draggableItem.RegisterCallback<PointerCaptureOutEvent>(OnDragEnd);
    }

    private void SwitchTab(Button activeTab, VisualElement activeView)
    {
        // Reset tabs
        _tabA.RemoveFromClassList("active");
        _tabB.RemoveFromClassList("active");
        _tabC.RemoveFromClassList("active");

        _viewA.style.display = DisplayStyle.None;
        _viewB.style.display = DisplayStyle.None;
        _viewC.style.display = DisplayStyle.None;

        // Set active
        activeTab.AddToClassList("active");
        activeView.style.display = DisplayStyle.Flex;
    }

    private void UpdateCounter(int amount)
    {
        _counter += amount;
        _counterLabel.text = _counter.ToString();
    }

    private void PopulateScrollView(ScrollView scrollView)
    {
        // Add dummy content to test scrolling (mouse wheel or drag on mobile)
        for (int i = 1; i <= 20; i++)
        {
            var label = new Label($"Scroll Item {i}");
            label.style.color = Color.white;
            label.style.paddingTop = 10;
            label.style.paddingBottom = 10;
            label.style.borderBottomWidth = 1;
            label.style.borderBottomColor = new Color(0.3f, 0.3f, 0.3f);
            scrollView.Add(label);
        }
    }

    // --- LONG PRESS LOGIC ---
    private void OnPointerDown(PointerDownEvent evt)
    {
        _isPointerDown = true;
        UnityEngine.Debug.LogWarning("Pointer down on long press button. Starting long press timer...");
        // Schedule a task to execute after 1000ms (1 second)
        _longPressTask = _longPressBtn.schedule.Execute(() =>
        {
            if (_isPointerDown)
            {
                Debug.Log("Long Press Triggered!");
                _longPressBtn.text = "Success!";
                _longPressBtn.style.backgroundColor = new Color(0.2f, 0.6f, 0.2f); // Turn green
            }
        }).StartingIn(1000);
    }

    private void OnPointerUp(EventBase evt)
    {
        UnityEngine.Debug.LogWarning("Pointer up on long press button. Stopping long press timer...");
        _isPointerDown = false;

        _longPressTask?.Pause();
        _longPressBtn.text = "Hold Me (Long Press)";
        _longPressBtn.style.backgroundColor = new StyleColor(StyleKeyword.Null); // Reset color
    }

    // --- DRAG AND DROP LOGIC ---
    private void OnDragStart(PointerDownEvent evt)
    {
        _isDragging = true;
        _draggableItem.CapturePointer(evt.pointerId); // Capture pointer so we don't lose it if moving fast

        // Calculate the offset between the pointer and the top-left of the visual element
        _dragOffset = evt.localPosition;

        _draggableItem.style.backgroundColor = new Color(0.8f, 0.4f, 0.4f); // Highlight while dragging
    }

    private void OnDragUpdate(PointerMoveEvent evt)
    {
        if (!_isDragging || !_draggableItem.HasPointerCapture(evt.pointerId)) return;

        // Calculate new position relative to the drag area
        Vector2 newPos = (Vector2)evt.position - _dragArea.worldBound.position - _dragOffset;

        // Optional: Clamp position to keep it inside the drag area
        newPos.x = Mathf.Clamp(newPos.x, 0, _dragArea.layout.width - _draggableItem.layout.width);
        newPos.y = Mathf.Clamp(newPos.y, 0, _dragArea.layout.height - _draggableItem.layout.height);

        _draggableItem.style.left = newPos.x;
        _draggableItem.style.top = newPos.y;
    }

    private void OnDragEnd(EventBase evt)
    {
        if (!_isDragging) return;

        _isDragging = false;
        if (evt is PointerUpEvent pointerUpEvent)
            _draggableItem.ReleasePointer(pointerUpEvent.pointerId);

        _draggableItem.style.backgroundColor = new StyleColor(StyleKeyword.Null); // Reset color
        Debug.Log("Dropped item at: " + _draggableItem.style.left.value + ", " + _draggableItem.style.top.value);
    }

    // --- SCROLL DRAG LOGIC ---
    private void OnScrollPointerDown(PointerDownEvent evt)
    {
        // Only trigger on left mouse button
        if (evt.button != 0) return;

        _isScrolling = true;
        _scrollStartPosition = evt.position;
        _scrollStartOffset = _scrollView.scrollOffset;

        _scrollView.CapturePointer(evt.pointerId);
    }

    private void OnScrollPointerMove(PointerMoveEvent evt)
    {
        if (!_isScrolling || !_scrollView.HasPointerCapture(evt.pointerId)) return;

        // Calculate the difference between where we started dragging and where we are now
        Vector2 delta = (Vector2)evt.position - _scrollStartPosition;

        // Apply the difference to the scroll offset. 
        // We subtract delta.y because moving the mouse UP (negative delta) should scroll the view DOWN (positive offset)
        _scrollView.scrollOffset = new Vector2(
            _scrollStartOffset.x,
            _scrollStartOffset.y - delta.y
        );
    }

    private void OnScrollPointerUp(EventBase evt)
    {
        if (!_isScrolling) return;

        _isScrolling = false;
        if (evt is PointerUpEvent pointerUpEvent)
            _scrollView.ReleasePointer(pointerUpEvent.pointerId);
    }
}