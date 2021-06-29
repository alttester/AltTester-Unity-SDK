#if ALTUNITYTESTER

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Altom.AltUnityDriver;
using UnityEngine;
using UnityEngine.EventSystems;

public class Input : UnityEngine.MonoBehaviour
{
    private static bool _useCustomInput;
    private static UnityEngine.Vector3 _acceleration;
    private static UnityEngine.AccelerationEvent[] _accelerationEvents;
    private static int _touchCount;
    private static UnityEngine.Touch[] _touches = new UnityEngine.Touch[0];
    private static UnityEngine.Vector2 _mouseScrollDelta = new UnityEngine.Vector2();
    private static UnityEngine.Vector3 _mousePosition = new UnityEngine.Vector3();
    private static System.Collections.Generic.List<AltUnityAxis> AxisList;

    private static AltUnityMockUpPointerInputModule _mockUpPointerInputModule;
    private static Input _instance;
    private static System.Collections.Generic.List<KeyStructure> _keyCodesPressed = new System.Collections.Generic.List<KeyStructure>();
    private static System.Collections.Generic.List<KeyStructure> _keyCodesPressedDown = new System.Collections.Generic.List<KeyStructure>();
    private static System.Collections.Generic.List<KeyStructure> _keyCodesPressedUp = new System.Collections.Generic.List<KeyStructure>();

    public static bool Finished { get; set; }
    public static float LastAxisValue { get; set; }
    public static string LastAxisName { get; set; }
    public static string LastButtonDown { get; set; }
    public static string LastButtonPressed { get; set; }
    public static string LastButtonUp { get; set; }

    public static AltUnityMockUpPointerInputModule AltUnityMockUpPointerInputModule
    {
        get
        {
            if (_mockUpPointerInputModule == null)
            {
                if (EventSystem.current != null)
                {
                    _mockUpPointerInputModule = EventSystem.current.gameObject.AddComponent<AltUnityMockUpPointerInputModule>();
                }
                else
                {
                    var newEventSystem = new GameObject("EventSystem");
                    _mockUpPointerInputModule = newEventSystem.AddComponent<AltUnityMockUpPointerInputModule>();
                }
            }
            return _mockUpPointerInputModule;
        }

    }

    public void Start()
    {
        _instance = this;
        string filePath = "AltUnityTester/AltUnityTesterInputAxisData";

        UnityEngine.TextAsset targetFile = UnityEngine.Resources.Load<UnityEngine.TextAsset>(filePath);
        string dataAsJson = targetFile.text;
        AxisList = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityAxis>>(dataAsJson);
    }
    private void Update()
    {
        _useCustomInput = UnityEngine.Input.touchCount == 0 && !UnityEngine.Input.anyKey && UnityEngine.Input.mouseScrollDelta == UnityEngine.Vector2.zero;
    }

    #region UnityEngine.Input.AltUnityTester.NotImplemented

    public static bool simulateMouseWithTouches
    {
        get { return UnityEngine.Input.simulateMouseWithTouches; }
        set { UnityEngine.Input.simulateMouseWithTouches = value; }
    }
    public static bool mousePresent
    {
        get
        {
            return UnityEngine.Input.mousePresent;
        }
    }

    public static bool stylusTouchSupported
    {
        get { return UnityEngine.Input.stylusTouchSupported; }
    }

    public static bool touchSupported
    {
        get { return UnityEngine.Input.touchSupported; }
    }

    public static bool multiTouchEnabled
    {
        get { return UnityEngine.Input.multiTouchEnabled; }
        set { UnityEngine.Input.multiTouchEnabled = value; }
    }

    public static UnityEngine.LocationService location
    {
        get { return UnityEngine.Input.location; }
    }

    public static UnityEngine.Compass compass
    {
        get { return UnityEngine.Input.compass; }
    }

    public static UnityEngine.DeviceOrientation deviceOrientation
    {
        get { return UnityEngine.Input.deviceOrientation; }
    }

    public static UnityEngine.IMECompositionMode imeCompositionMode
    {
        get { return UnityEngine.Input.imeCompositionMode; }
        set { UnityEngine.Input.imeCompositionMode = value; }
    }

    public static string compositionString
    {
        get { return UnityEngine.Input.compositionString; }
    }
    public static bool imeIsSelected
    {
        get { return UnityEngine.Input.imeIsSelected; }
    }

    public static bool touchPressureSupported
    {
        get { return UnityEngine.Input.touchPressureSupported; }
    }

    public static UnityEngine.Gyroscope gyro
    {
        get { return UnityEngine.Input.gyro; }
    }

    public static UnityEngine.Vector2 compositionCursorPos
    {
        get { return UnityEngine.Input.compositionCursorPos; }
        set { UnityEngine.Input.compositionCursorPos = value; }
    }

    public static bool backButtonLeavesApp
    {
        get { return UnityEngine.Input.backButtonLeavesApp; }
        set { UnityEngine.Input.backButtonLeavesApp = value; }
    }

    [System.Obsolete]
    public static bool isGyroAvailable
    {
        get { return UnityEngine.Input.isGyroAvailable; }
    }

    public static bool compensateSensors
    {
        get { return UnityEngine.Input.compensateSensors; }
        set { UnityEngine.Input.compensateSensors = value; }
    }

    public static UnityEngine.AccelerationEvent GetAccelerationEvent(int index)
    {
        return UnityEngine.Input.GetAccelerationEvent(index);
    }

    public static string[] GetJoystickNames()
    {
        return UnityEngine.Input.GetJoystickNames();
    }
    public static void ResetInputAxes()
    {
        UnityEngine.Input.ResetInputAxes();
    }

    #endregion


    #region UnityEngine.Input.AltUnityTester

    public static bool anyKey
    {
        get
        {
            if (_useCustomInput)
            {
                return _keyCodesPressed.Count > 0;
            }
            else
            {
                return UnityEngine.Input.anyKey;
            }
        }
    }

    public static bool anyKeyDown
    {
        get
        {
            if (_useCustomInput)
            {
                return _keyCodesPressedDown.Count > 0;
            }
            else
            {
                return UnityEngine.Input.anyKeyDown;
            }
        }
    }

    //WIP
    public static string inputString
    {
        get
        {
            if (_useCustomInput)
            {
                string charachtersPressedCurrentFrame = "";
                foreach (var keyCode in _keyCodesPressedDown)
                {
                    //need a Parser from keycode to character every characher from keyboard + backspace and enter
                }
                return charachtersPressedCurrentFrame;

            }
            else
            {
                return UnityEngine.Input.inputString;
            }
        }
    }//TODO: Doable 

    public static UnityEngine.Vector3 acceleration
    {
        get
        {
            if (_useCustomInput)
            {
                return _acceleration;
            }
            else
            {
                return UnityEngine.Input.acceleration;
            }
        }
        set
        {
            _acceleration = acceleration;
        }
    }
    public static UnityEngine.AccelerationEvent[] accelerationEvents
    {
        get
        {
            if (_useCustomInput)
            {
                return _accelerationEvents;
            }
            else
            {
                return UnityEngine.Input.accelerationEvents;
            }
        }
        set
        {
            _accelerationEvents = accelerationEvents;
        }
    }
    public static int accelerationEventCount
    {
        get
        {
            if (_useCustomInput)
            {
                return _accelerationEvents.Length;
            }
            else
            {
                return UnityEngine.Input.accelerationEventCount;
            }
        }
    }


    public static UnityEngine.Touch[] touches
    {
        get { return _useCustomInput ? _touches : UnityEngine.Input.touches; }
        set
        {
            _touches = value;
        }
    }
    public UnityEngine.Touch this[int i]
    {
        get { return _useCustomInput ? _touches[i] : UnityEngine.Input.GetTouch(i); }
        set { _touches[i] = value; }
    }

    public static int touchCount
    {
        get { return _useCustomInput ? _touchCount : UnityEngine.Input.touchCount; }
        set { _touchCount = value; }
    }

    public static UnityEngine.Vector2 mouseScrollDelta
    {
        get
        {
            if (_useCustomInput)
            {
                return _mouseScrollDelta;
            }
            else
            {
                return UnityEngine.Input.mouseScrollDelta;
            }
        }
    }

    public static UnityEngine.Vector3 mousePosition
    {
        get
        {
            if (_useCustomInput)
            {
                return _mousePosition;
            }
            else
            {
                return UnityEngine.Input.mousePosition;
            }
        }
        set
        {
            _mousePosition = value;
        }
    }//Doable

    public static float GetAxis(string axisName)
    {
        if (_useCustomInput)
        {
            var axis = AxisList.First(axle => axle.name == axisName);
            if (axis == null)
            {
                throw new NotFoundException("No axis with this name was found");
            }
            foreach (var keyStructure in _keyCodesPressed)
            {
                if ((axis.positiveButton != "" && keyStructure.KeyCode == ConvertStringToKeyCode(axis.positiveButton)) || (axis.altPositiveButton != "" && keyStructure.KeyCode == ConvertStringToKeyCode(axis.altPositiveButton)))
                {
                    LastAxisName = axisName;//DebugPurpose
                    LastAxisValue = keyStructure.Power;
                    return keyStructure.Power;
                }
                if ((axis.negativeButton != "" && keyStructure.KeyCode == ConvertStringToKeyCode(axis.negativeButton)) || (axis.altNegativeButton != "" && keyStructure.KeyCode == ConvertStringToKeyCode(axis.altNegativeButton)))
                {
                    LastAxisName = axisName;//DebugPurpose
                    LastAxisValue = -1 * keyStructure.Power;
                    return -1 * keyStructure.Power;
                }
            }
            return 0;
        }
        else
        {
            return UnityEngine.Input.GetAxis(axisName);
        }
    }

    public static float GetAxisRaw(string axisName)
    {
        if (_useCustomInput)
        {
            return GetAxis(axisName);
        }
        else
        {
            return UnityEngine.Input.GetAxisRaw(axisName);
        }
    }
    public static bool GetButton(string buttonName)
    {
        if (_useCustomInput)
        {
            var axis = AxisList.First(axle => axle.name == buttonName);

            if (axis == null)
            {
                throw new NotFoundException("No button with this name was found");
            }

            foreach (var keyStructure in _keyCodesPressed)
            {
                if (keyStructure.KeyCode == ConvertStringToKeyCode(axis.positiveButton) || keyStructure.KeyCode == ConvertStringToKeyCode(axis.altPositiveButton))
                {
                    LastAxisName = axis.name;//Debug purpose
                    return true;
                }
                if (keyStructure.KeyCode == ConvertStringToKeyCode(axis.negativeButton) || keyStructure.KeyCode == ConvertStringToKeyCode(axis.altNegativeButton))
                {
                    LastAxisName = axis.name;//Debug purpose
                    return true;
                }
            }
            return false;
        }
        else
        {
            return UnityEngine.Input.GetButton(buttonName);
        }
    }

    public static bool GetButtonDown(string buttonName)
    {

        if (_useCustomInput)
        {
            var axis = AxisList.First(axle => axle.name == buttonName);
            if (axis == null)
            {
                throw new NotFoundException("No button with this name was found");
            }
            foreach (var keyStructure in _keyCodesPressedDown)
            {
                if (keyStructure.KeyCode == ConvertStringToKeyCode(axis.positiveButton) || keyStructure.KeyCode == ConvertStringToKeyCode(axis.altPositiveButton))
                {
                    LastAxisName = axis.name;//Debug purpose
                    return true;
                }
                if (keyStructure.KeyCode == ConvertStringToKeyCode(axis.negativeButton) || keyStructure.KeyCode == ConvertStringToKeyCode(axis.altNegativeButton))
                {
                    LastAxisName = axis.name;//Debug purpose
                    return true;
                }
            }
            return false;
        }
        else
        {
            return UnityEngine.Input.GetButtonDown(buttonName);

        }
    }

    public static bool GetButtonUp(string buttonName)
    {

        if (_useCustomInput)
        {
            var axis = AxisList.First(axle => axle.name == buttonName);
            if (axis == null)
            {
                throw new NotFoundException("No button with this name was found");
            }
            foreach (var keyStructure in _keyCodesPressedUp)
            {
                if (keyStructure.KeyCode == ConvertStringToKeyCode(axis.positiveButton) || keyStructure.KeyCode == ConvertStringToKeyCode(axis.altPositiveButton))
                {
                    LastAxisName = axis.name;//Debug purpose
                    return true;
                }
                if (keyStructure.KeyCode == ConvertStringToKeyCode(axis.negativeButton) || keyStructure.KeyCode == ConvertStringToKeyCode(axis.altNegativeButton))
                {
                    LastAxisName = axis.name;//Debug purpose
                    return true;
                }
            }
            return false;
        }
        else
        {
            return UnityEngine.Input.GetButtonUp(buttonName);
        }
    }

    public static bool GetKey(string name)
    {
        if (_useCustomInput)
        {
            UnityEngine.KeyCode keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), name);
            return 0 != _keyCodesPressed.FindAll(key => key.KeyCode == keyCode).Count;
        }
        else
        {
            return UnityEngine.Input.GetKey(name);
        }
    }

    public static bool GetKey(UnityEngine.KeyCode key)
    {
        if (_useCustomInput)
        {
            return 0 != _keyCodesPressed.FindAll(keyFromList => keyFromList.KeyCode == key).Count;
        }
        else
        {
            return UnityEngine.Input.GetKey(key);
        }
    }

    public static bool GetKeyDown(UnityEngine.KeyCode key)
    {
        if (_useCustomInput)
        {
            return 0 != _keyCodesPressedDown.FindAll(keyFromList => keyFromList.KeyCode == key).Count;
        }
        else
        {
            return UnityEngine.Input.GetKeyDown(key);
        }
    }

    public static bool GetKeyDown(string name)
    {

        if (_useCustomInput)
        {
            UnityEngine.KeyCode keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), name);
            return 0 != _keyCodesPressedDown.FindAll(key => key.KeyCode == keyCode).Count;
        }
        else
        {
            return UnityEngine.Input.GetKeyDown(name);
        }
    }

    public static bool GetKeyUp(UnityEngine.KeyCode key)
    {
        if (_useCustomInput)
        {
            return 0 != _keyCodesPressedUp.FindAll(keyFromList => keyFromList.KeyCode == key).Count;
        }
        else
        {
            return UnityEngine.Input.GetKeyUp(key);
        }
    }

    public static bool GetKeyUp(string name)
    {
        if (_useCustomInput)
        {
            UnityEngine.KeyCode keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), name);
            return 0 != _keyCodesPressedUp.FindAll(key => key.KeyCode == keyCode).Count;
        }
        else
        {
            return UnityEngine.Input.GetKeyUp(name);
        }
    }

    public static bool GetMouseButton(int button)
    {
        if (_useCustomInput)
        {
            var keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), "Mouse" + button);
            return 0 != _keyCodesPressed.FindAll(key => key.KeyCode == keyCode).Count || touches.Length > button;
        }
        else
        {
            return UnityEngine.Input.GetMouseButton(button);
        }
    }

    public static bool GetMouseButtonDown(int button)
    {
        //method not tested
        if (_useCustomInput)
        {
            var keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), "Mouse" + button);
            return 0 != _keyCodesPressedDown.FindAll(key => key.KeyCode == keyCode).Count || touches.Length > button && touches[button].phase != UnityEngine.TouchPhase.Began;
        }
        else
        {
            return UnityEngine.Input.GetMouseButtonDown(button);
        }
    }

    public static bool GetMouseButtonUp(int button)
    {
        //method not tested
        if (_useCustomInput)
        {
            var keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), "Mouse" + button);
            return 0 != _keyCodesPressedUp.FindAll(key => key.KeyCode == keyCode).Count || touches.Length > button && touches[button].phase == UnityEngine.TouchPhase.Ended;
        }
        else
        {
            return UnityEngine.Input.GetMouseButtonUp(button);
        }
    }

    public static UnityEngine.Touch GetTouch(int index)
    {
        return _useCustomInput ? _touches[index] : UnityEngine.Input.GetTouch(index);
    }

    #endregion

    private static UnityEngine.Touch beginTouch(UnityEngine.Vector3 screenPosition)
    {
        var touch = new UnityEngine.Touch
        {
            phase = UnityEngine.TouchPhase.Began,
            position = screenPosition,
            rawPosition = screenPosition,
            pressure = 1.0f,
            maximumPossiblePressure = 1.0f,
        };

        List<int> fingerIds = touches.Select(t => t.fingerId).ToList();
        fingerIds.Sort();
        int fingerId = 0;
        foreach (var iter in fingerIds)
        {
            if (iter != fingerId)
                break;
            fingerId++;
        }
        touch.fingerId = fingerId;


        touchCount++;
        var touchListCopy = new UnityEngine.Touch[touches.Length + 1];
        System.Array.Copy(touches, 0, touchListCopy, 0, touches.Length);
        touchListCopy[touches.Length] = touch;
        touches = touchListCopy;

        return touch;
    }

    private static void endTouch(UnityEngine.Touch touch)
    {
        var newTouches = new UnityEngine.Touch[touches.Length - 1];
        int contor = 0;
        foreach (var t in touches)
        {
            if (t.fingerId != touch.fingerId)
            {
                newTouches[contor] = t;
                contor++;
            }
        }

        touches = newTouches;
        touchCount--;
    }

    /// <summary>
    /// Finds element at given pointerEventData for which we raise EventSystem input events
    /// </summary>
    /// <param name="pointerEventData"></param>
    /// <returns>the found gameObject</returns>
    private static UnityEngine.GameObject findEventSystemObject(UnityEngine.EventSystems.PointerEventData pointerEventData)
    {
        UnityEngine.EventSystems.RaycastResult firstRaycastResult;
        AltUnityMockUpPointerInputModule.GetFirstRaycastResult(pointerEventData, out firstRaycastResult);
        pointerEventData.pointerCurrentRaycast = firstRaycastResult;
        pointerEventData.pointerPressRaycast = firstRaycastResult;
        return firstRaycastResult.gameObject;
    }

    /// <summary>
    /// Finds element(s) at given coordinates for which we raise MonoBehaviour input events
    /// </summary>
    /// <param name="coordinates"></param>
    /// <returns>the found gameObject</returns>
    private static UnityEngine.GameObject findMonoBehaviourObject(UnityEngine.Vector2 coordinates)
    {
        var target = AltUnityMockUpPointerInputModule.GetGameObjectHitMonoBehaviour(coordinates);
        if (target == null)
            return null;

        var rigidBody = target.GetComponentInParent<UnityEngine.Rigidbody>();
        if (rigidBody != null)
            return rigidBody.gameObject;
        var rigidBody2D = target.GetComponentInParent<UnityEngine.Rigidbody2D>();
        if (rigidBody2D != null)
            return rigidBody2D.gameObject;
        return target;
    }

    private static IEnumerator tapClickCoordinatesLifeCycle(UnityEngine.Vector2 screenPosition, int count, float interval, bool tap, Action onFinish)
    {
        var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current)
        {
            position = screenPosition,
            button = UnityEngine.EventSystems.PointerEventData.InputButton.Left,
            eligibleForClick = true,
            pressPosition = screenPosition
        };
        var eventSystemTarget = findEventSystemObject(pointerEventData);
        var monoBehaviourTarget = findMonoBehaviourObject(screenPosition);


        yield return null; //new frame


        pointerEventData.pointerEnter = UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerEnterHandler);
        if (monoBehaviourTarget != null) monoBehaviourTarget.SendMessage("OnMouseEnter", UnityEngine.SendMessageOptions.DontRequireReceiver);
        pointerEventData.pointerEnter = eventSystemTarget;

        for (int i = 0; i < count; i++)
        {
            float time = 0;
            AltUnityRunner._altUnityRunner.ShowClick(screenPosition);

            /* pointer/touch down */
            UnityEngine.Touch touch = new UnityEngine.Touch();
            int pointerId = 0;
            if (tap)
            {
                touch = beginTouch(screenPosition);
                pointerId = touch.fingerId;
            }
            pointerEventData.pointerId = pointerId;

            var keyStructure = new KeyStructure(UnityEngine.KeyCode.Mouse0, 1.0f);//power 1
            _keyCodesPressedDown.Add(keyStructure);
            _keyCodesPressed.Add(keyStructure);

            pointerEventData.pointerPress = UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);
            if (monoBehaviourTarget != null) monoBehaviourTarget.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);
            yield return null;
            time += UnityEngine.Time.unscaledDeltaTime;

            /* pointer/touch up */
            if (tap) endTouch(touch);
            _keyCodesPressedDown.Remove(keyStructure);
            _keyCodesPressed.Remove(keyStructure);
            _keyCodesPressedUp.Add(keyStructure);

            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
            if (monoBehaviourTarget != null) monoBehaviourTarget.SendMessage("OnMouseUp", UnityEngine.SendMessageOptions.DontRequireReceiver);

            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
            if (monoBehaviourTarget != null) monoBehaviourTarget.SendMessage("OnMouseUpAsButton", UnityEngine.SendMessageOptions.DontRequireReceiver);
            yield return null;
            time += UnityEngine.Time.unscaledDeltaTime;

            _keyCodesPressedUp.Remove(keyStructure);

            if (i != count - 1 && time < interval)//do not wait at last click/tap
                yield return new UnityEngine.WaitForSecondsRealtime(interval - time);
        }
        // mouse position doesn't change  but we fire on mouse exit
        UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
        if (monoBehaviourTarget != null) monoBehaviourTarget.SendMessage("OnMouseExit", UnityEngine.SendMessageOptions.DontRequireReceiver);

        onFinish();
    }

    private static IEnumerator tapClickElementLifeCycle(UnityEngine.GameObject target, int count, float interval, bool tap, Action<UnityEngine.GameObject> onFinish)
    {
        UnityEngine.Vector3 screenPosition;
        AltUnityRunner._altUnityRunner.findCameraThatSeesObject(target, out screenPosition);
        yield return null; //new frame

        var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current)
        {
            position = screenPosition,
            button = UnityEngine.EventSystems.PointerEventData.InputButton.Left,
            eligibleForClick = true,
            pressPosition = screenPosition
        };

        mousePosition = screenPosition;
        UnityEngine.EventSystems.ExecuteEvents.Execute(target, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerEnterHandler);
        target.SendMessage("OnMouseEnter", UnityEngine.SendMessageOptions.DontRequireReceiver);
        pointerEventData.pointerEnter = target;

        //repeat
        for (int i = 0; i < count; i++)
        {
            float time = 0;
            AltUnityRunner._altUnityRunner.ShowClick(screenPosition);

            /* pointer/touch down */
            UnityEngine.Touch touch = new UnityEngine.Touch();
            int pointerId = 0;
            if (tap)
            {
                touch = beginTouch(screenPosition);
                pointerId = touch.fingerId;
            }
            pointerEventData.pointerId = pointerId;

            var keyStructure = new KeyStructure(UnityEngine.KeyCode.Mouse0, 1.0f);//power 1
            _keyCodesPressedDown.Add(keyStructure);
            _keyCodesPressed.Add(keyStructure);

            UnityEngine.EventSystems.ExecuteEvents.Execute(target, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);
            target.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);
            pointerEventData.pointerPress = target;
            yield return null;
            time += UnityEngine.Time.unscaledDeltaTime;

            /* pointer/touch up */
            if (tap) endTouch(touch);
            _keyCodesPressedDown.Remove(keyStructure);
            _keyCodesPressed.Remove(keyStructure);
            _keyCodesPressedUp.Add(keyStructure);

            UnityEngine.EventSystems.ExecuteEvents.Execute(target, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
            target.SendMessage("OnMouseUp", UnityEngine.SendMessageOptions.DontRequireReceiver);

            UnityEngine.EventSystems.ExecuteEvents.Execute(target, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
            target.SendMessage("OnMouseUpAsButton", UnityEngine.SendMessageOptions.DontRequireReceiver);

            yield return null;
            time += UnityEngine.Time.unscaledDeltaTime;
            _keyCodesPressedUp.Remove(keyStructure);

            if (i != count - 1 && time < interval)//do not wait at last click/tap
                yield return new UnityEngine.WaitForSecondsRealtime(interval - time);
        }

        // mouse position doesn't change  but we fire on mouse exit
        UnityEngine.EventSystems.ExecuteEvents.Execute(target, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
        target.SendMessage("OnMouseExit", UnityEngine.SendMessageOptions.DontRequireReceiver);
        onFinish(target);
    }
    public static void TapElement(UnityEngine.GameObject target, int count, float interval, Action<UnityEngine.GameObject> onFinish)
    {
        _instance.StartCoroutine(tapClickElementLifeCycle(target, count, interval, true, onFinish));
    }
    public static void ClickElement(UnityEngine.GameObject target, int count, float interval, Action<UnityEngine.GameObject> onFinish)
    {
        _instance.StartCoroutine(tapClickElementLifeCycle(target, count, interval, false, onFinish));
    }

    public static void TapCoordinates(UnityEngine.Vector2 coordinates, int count, float interval, Action onFinish)
    {
        _instance.StartCoroutine(tapClickCoordinatesLifeCycle(coordinates, count, interval, true, onFinish));
    }
    public static void ClickCoordinates(UnityEngine.Vector2 coordinates, int count, float interval, Action onFinish)
    {
        _instance.StartCoroutine(tapClickCoordinatesLifeCycle(coordinates, count, interval, false, onFinish));
    }

    public static void SetMultipointSwipe(UnityEngine.Vector2[] positions, float duration)
    {
        Finished = false;
        _instance.StartCoroutine(MultipointSwipeLifeCycle(positions, duration));
    }

    public static System.Collections.IEnumerator MultipointSwipeLifeCycle(UnityEngine.Vector2[] positions, float duration)
    {
        var touch = new UnityEngine.Touch
        {
            phase = UnityEngine.TouchPhase.Began,
            position = positions[0]
        };

        System.Collections.Generic.List<UnityEngine.Touch> currentTouches = touches.ToList();
        currentTouches.Sort((touch1, touch2) => (touch1.fingerId.CompareTo(touch2.fingerId)));
        int fingerId = 0;
        foreach (var iter in currentTouches)
        {
            if (iter.fingerId != fingerId)
                break;
            fingerId++;
        }

        touch.fingerId = fingerId;
        touchCount++;

        var touchListCopy = new UnityEngine.Touch[touchCount];
        System.Array.Copy(touches, 0, touchListCopy, 0, touches.Length);
        touchListCopy[touchCount - 1] = touch;
        touches = touchListCopy;
        mousePosition = new UnityEngine.Vector3(touches[0].position.x, touches[0].position.y, 0);
        var pointerEventData = AltUnityMockUpPointerInputModule.ExecuteTouchEvent(touch);
        var markId = AltUnityRunner._altUnityRunner.ShowInput(touch.position);

        yield return null;

        var oneInputDuration = duration / (positions.Length - 1);
        for (var i = 1; i < positions.Length; i++)
        {
            var wholeDelta = positions[i] - touch.position;
            var deltaPerSecond = wholeDelta / oneInputDuration;
            float time = 0;
            do
            {
                UnityEngine.Vector2 previousPosition = touch.position;
                if (time + UnityEngine.Time.unscaledDeltaTime < oneInputDuration)
                {
                    touch.position += deltaPerSecond * UnityEngine.Time.unscaledDeltaTime;
                }
                else
                {
                    touch.position = positions[i];
                }

                touch.phase = touch.deltaPosition != UnityEngine.Vector2.zero ? UnityEngine.TouchPhase.Moved : UnityEngine.TouchPhase.Stationary;
                time += UnityEngine.Time.unscaledDeltaTime;
                touch.deltaPosition = touch.position - previousPosition;

                for (var t = 0; t < touches.Length; t++)
                {
                    if (touches[t].fingerId == touch.fingerId)
                    {
                        touches[t] = touch;
                    }
                }
                mousePosition = new UnityEngine.Vector3(touches[0].position.x, touches[0].position.y, 0);
                pointerEventData = AltUnityMockUpPointerInputModule.ExecuteTouchEvent(touch, pointerEventData);

                AltUnityRunner._altUnityRunner.ShowInput(touch.position, markId);
                yield return null;

            } while (time <= oneInputDuration);
        }

        yield return null;

        touch.phase = UnityEngine.TouchPhase.Ended;
        for (var i = 0; i < touches.Length; i++)
        {
            if (touches[i].fingerId == touch.fingerId)
            {
                touches[i] = touch;
            }
        }

        AltUnityMockUpPointerInputModule.ExecuteTouchEvent(touch, pointerEventData);
        yield return null;
        var newTouches = new UnityEngine.Touch[touchCount - 1];
        int contor = 0;
        foreach (var t in touches)
        {
            if (t.fingerId != touch.fingerId)
            {
                newTouches[contor] = t;
                contor++;
            }
        }

        touches = newTouches;
        touchCount--;
        Finished = true;
    }

    public static void TapAtCoordinates(UnityEngine.Vector2 position, int count, float interval)
    {
        Finished = false;
        _instance.StartCoroutine(CustomTapLifeCycle(position, count, interval));
    }

    public static void TapAtCoordinates(UnityEngine.Vector2 position, out UnityEngine.GameObject gameObject, out UnityEngine.Camera camera)
    {
        AltUnityRunner._altUnityRunner.ShowClick(position);
        var mockUp = Input.AltUnityMockUpPointerInputModule;
        var touch = new UnityEngine.Touch { position = position, phase = UnityEngine.TouchPhase.Began };
        var pointerEventData = mockUp.ExecuteTouchEvent(touch);
        if (pointerEventData.pointerPress == null &&
            pointerEventData.pointerEnter == null &&
            pointerEventData.pointerDrag == null)
        {
            gameObject = null;
            camera = null;
            return;
        }
        gameObject = pointerEventData.pointerPress.gameObject;
        triggerMonobehaviourEventsForClick(gameObject);
        touch.phase = UnityEngine.TouchPhase.Ended;
        mockUp.ExecuteTouchEvent(touch, pointerEventData);
        camera = pointerEventData.enterEventCamera;
    }

    private static void triggerMonobehaviourEventsForClick(GameObject gameObject)
    {
        gameObject.SendMessage("OnMouseEnter", UnityEngine.SendMessageOptions.DontRequireReceiver);
        gameObject.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);
        gameObject.SendMessage("OnMouseOver", UnityEngine.SendMessageOptions.DontRequireReceiver);
        gameObject.SendMessage("OnMouseUp", UnityEngine.SendMessageOptions.DontRequireReceiver);
        gameObject.SendMessage("OnMouseUpAsButton", UnityEngine.SendMessageOptions.DontRequireReceiver);
        gameObject.SendMessage("OnMouseExit", UnityEngine.SendMessageOptions.DontRequireReceiver);
    }

    public static void TapObject(UnityEngine.GameObject targetGameObject, int count)
    {
        var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);

        UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(targetGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerEnterHandler);
        targetGameObject.SendMessage("OnMouseEnter", UnityEngine.SendMessageOptions.DontRequireReceiver);

        for (var i = 0; i < count; i++)
            initiateTap(targetGameObject, pointerEventData);

        UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(targetGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
        targetGameObject.SendMessage("OnMouseExit", UnityEngine.SendMessageOptions.DontRequireReceiver);
    }

    public static void ClickObject(UnityEngine.GameObject targetGameObject)
    {
        var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        UnityEngine.EventSystems.ExecuteEvents.Execute(targetGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerEnterHandler);
        targetGameObject.SendMessage("OnMouseEnter", UnityEngine.SendMessageOptions.DontRequireReceiver);
        UnityEngine.EventSystems.ExecuteEvents.Execute(targetGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);
        targetGameObject.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);
        UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(targetGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.initializePotentialDrag);
        targetGameObject.SendMessage("OnMouseOver", UnityEngine.SendMessageOptions.DontRequireReceiver);
        UnityEngine.EventSystems.ExecuteEvents.Execute(targetGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
        targetGameObject.SendMessage("OnMouseUp", UnityEngine.SendMessageOptions.DontRequireReceiver);
        UnityEngine.EventSystems.ExecuteEvents.Execute(targetGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
        targetGameObject.SendMessage("OnMouseUpAsButton", UnityEngine.SendMessageOptions.DontRequireReceiver);
        UnityEngine.EventSystems.ExecuteEvents.Execute(targetGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
        targetGameObject.SendMessage("OnMouseExit", UnityEngine.SendMessageOptions.DontRequireReceiver);
    }

    private static void initiateTap(UnityEngine.GameObject targetGameObject, UnityEngine.EventSystems.PointerEventData pointerEventData)
    {
        pointerEventData.clickTime = UnityEngine.Time.unscaledTime;

        UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(targetGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);
        targetGameObject.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);
        UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(targetGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.initializePotentialDrag);
        targetGameObject.SendMessage("OnMouseOver", UnityEngine.SendMessageOptions.DontRequireReceiver);
        UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(targetGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
        targetGameObject.SendMessage("OnMouseUp", UnityEngine.SendMessageOptions.DontRequireReceiver);
        UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(targetGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
        targetGameObject.SendMessage("OnMouseUpAsButton", UnityEngine.SendMessageOptions.DontRequireReceiver);
    }
    private static System.Collections.IEnumerator CustomTapLifeCycle(UnityEngine.Vector2 position, int count, float interval)
    {
        var mockUp = AltUnityMockUpPointerInputModule;
        var touch = new UnityEngine.Touch { position = position };

        for (var i = 0; i < count; i++)
        {
            AltUnityRunner._altUnityRunner.ShowClick(position);

            touch.phase = UnityEngine.TouchPhase.Began;
            var pointerEventData = mockUp.ExecuteTouchEvent(touch);

            if (pointerEventData.pointerPress != null)
            {
                UnityEngine.GameObject targetGameObject = pointerEventData.pointerPress.gameObject;

                triggerMonobehaviourEventsForClick(targetGameObject);
                touch.phase = UnityEngine.TouchPhase.Ended;
                mockUp.ExecuteTouchEvent(touch, pointerEventData);
            }

            yield return new UnityEngine.WaitForSecondsRealtime(interval);
        }
        Finished = true;
    }



    public static void SetKeyDown(UnityEngine.KeyCode keyCode, float power, float duration)
    {
        Finished = false;
        _instance.StartCoroutine(KeyDownLifeCycle(keyCode, power, duration));
    }

    private static System.Collections.IEnumerator KeyDownLifeCycle(UnityEngine.KeyCode keyCode, float power, float duration)
    {

        float time = 0;
        var keyStructure = new KeyStructure(keyCode, power);
        yield return null;
        _keyCodesPressedDown.Add(keyStructure);
        _keyCodesPressed.Add(keyStructure);
        yield return null;
        _keyCodesPressedDown.Remove(keyStructure);
        if (keyCode == UnityEngine.KeyCode.Mouse0)
        {
            yield return _instance.StartCoroutine(mouseEventTrigger(PointerEventData.InputButton.Left, power, duration));
        }
        else if (keyCode == UnityEngine.KeyCode.Mouse1)
        {
            yield return _instance.StartCoroutine(mouseEventTrigger(PointerEventData.InputButton.Right, power, duration));
        }
        else if (keyCode == UnityEngine.KeyCode.Mouse2)
        {
            yield return _instance.StartCoroutine(mouseEventTrigger(PointerEventData.InputButton.Middle, power, duration));
        }
        else
        {
            if (duration != 0)
            {
                yield return new UnityEngine.WaitForSecondsRealtime(duration);
            }
        }
        _keyCodesPressed.Remove(keyStructure);
        _keyCodesPressedUp.Add(keyStructure);
        yield return null;
        _keyCodesPressedUp.Remove(keyStructure);
        Finished = true;

    }
    private static IEnumerator mouseEventTrigger(PointerEventData.InputButton mouseButton, float power, float duration)
    {
        var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current)
        {
            position = mousePosition,
            button = mouseButton,
            eligibleForClick = true,
            pressPosition = mousePosition
        };
        var eventSystemTarget = findEventSystemObject(pointerEventData);
        var monoBehaviourTarget = findMonoBehaviourObject(mousePosition);

        pointerEventData.pointerEnter = UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerEnterHandler);
        if (monoBehaviourTarget != null) monoBehaviourTarget.SendMessage("OnMouseEnter", UnityEngine.SendMessageOptions.DontRequireReceiver);
        pointerEventData.pointerEnter = eventSystemTarget;

        AltUnityRunner._altUnityRunner.ShowClick(mousePosition);

        /* pointer/touch down */
        int pointerId = 0;
        pointerEventData.pointerId = pointerId;

        pointerEventData.pointerPress = UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);
        if (mouseButton == PointerEventData.InputButton.Left && monoBehaviourTarget != null) monoBehaviourTarget.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);

        yield return new WaitForSecondsRealtime(duration);

        /* pointer/touch up */
        UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
        if (mouseButton == PointerEventData.InputButton.Left && monoBehaviourTarget != null) monoBehaviourTarget.SendMessage("OnMouseUpAsButton", UnityEngine.SendMessageOptions.DontRequireReceiver);

        UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
        if (mouseButton == PointerEventData.InputButton.Left && monoBehaviourTarget != null) monoBehaviourTarget.SendMessage("OnMouseUp", UnityEngine.SendMessageOptions.DontRequireReceiver);

        // mouse position doesn't change  but we fire on mouse exit
        UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
        if (monoBehaviourTarget != null) monoBehaviourTarget.SendMessage("OnMouseExit", UnityEngine.SendMessageOptions.DontRequireReceiver);

    }
    public static void MoveMouse(UnityEngine.Vector2 location, float duration)
    {
        Finished = false;
        _instance.StartCoroutine(MoveMouseCycle(location, duration));
    }
    public static System.Collections.IEnumerator MoveMouseCycle(UnityEngine.Vector2 location, float duration)
    {
        float time = 0;
        var distance = location - new UnityEngine.Vector2(mousePosition.x, mousePosition.y);

        do
        {
            UnityEngine.Vector3 delta;

            if (time + UnityEngine.Time.unscaledDeltaTime < duration)
            {
                delta = distance * UnityEngine.Time.unscaledDeltaTime / duration;
            }
            else
            {
                delta = location - new UnityEngine.Vector2(mousePosition.x, mousePosition.y);
            }

            mousePosition += delta;
            yield return null;
            time += UnityEngine.Time.unscaledDeltaTime;
        } while (time < duration);
        Finished = true;
    }
    public static void Scroll(float scrollValue, float duration)
    {
        Finished = false;
        _instance.StartCoroutine(ScrollLifeCycle(scrollValue, duration));
    }
    private static System.Collections.IEnumerator ScrollLifeCycle(float scrollValue, float duration)
    {
        float timeSpent = 0;

        while (timeSpent < duration)
        {
            yield return null;
            timeSpent += UnityEngine.Time.unscaledDeltaTime;
            float scrollStep = scrollValue * UnityEngine.Time.unscaledDeltaTime / duration;

            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current)
            {
                position = _mousePosition,
                button = UnityEngine.EventSystems.PointerEventData.InputButton.Left,
                eligibleForClick = true,
            };
            var eventSystemTarget = findEventSystemObject(pointerEventData);
            _mouseScrollDelta = new UnityEngine.Vector2(0, scrollStep);//x value is not taken in consideration
            pointerEventData.scrollDelta = _mouseScrollDelta;
            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.scrollHandler);
        }
        _mouseScrollDelta = UnityEngine.Vector2.zero;//reset the value after scroll ended
        Finished = true;
    }

    public static void Acceleration(UnityEngine.Vector3 accelarationValue, float duration)
    {
        Finished = false;
        _instance.StartCoroutine(AccelerationLifeCycle(accelarationValue, duration));
    }
    private static System.Collections.IEnumerator AccelerationLifeCycle(UnityEngine.Vector3 accelarationValue, float duration)
    {
        float timeSpent = 0;
        while (timeSpent < duration)
        {
            _acceleration = accelarationValue;
            yield return null;
            timeSpent += UnityEngine.Time.unscaledDeltaTime;
        }
        _acceleration = UnityEngine.Vector3.zero;//reset the value after acceleration ended
        Finished = true;

    }
    private static UnityEngine.KeyCode ConvertStringToKeyCode(string keyName)
    {
        if (keyName.Length == 1 && IsEnglishLetter(keyName[0]))
        {
            return (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), System.Char.ToUpper(keyName[0]).ToString());
        }
        if (keyName.Equals("left"))
        {
            return UnityEngine.KeyCode.LeftArrow;
        }
        if (keyName.Equals("right"))
        {
            return UnityEngine.KeyCode.RightArrow;
        }
        if (keyName.Equals("down"))
        {
            return UnityEngine.KeyCode.DownArrow;
        }
        if (keyName.Equals("up"))
        {
            return UnityEngine.KeyCode.UpArrow;
        }
        if (keyName.Length == 0 && char.IsDigit(keyName[0]))
        {
            return (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), "Alpha" + keyName);
        }
        if (System.Text.RegularExpressions.Regex.Match(keyName, @"\[[0-9]{1}\]").Success)
        {
            return (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), "Keypad" + keyName);
        }
        if (keyName == "[+]")
        {
            return UnityEngine.KeyCode.KeypadPlus;
        }
        if (keyName == "[equals]")
        {
            return UnityEngine.KeyCode.KeypadEquals;
        }
        if (System.Text.RegularExpressions.Regex.Match(keyName, "f[0-9]{1,2}").Success)
        {
            return (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), keyName.ToUpper());
        }
        if (keyName.Equals("right shift"))
        {
            return UnityEngine.KeyCode.RightShift;
        }
        if (keyName.Equals("left shift"))
        {
            return UnityEngine.KeyCode.LeftShift;
        }
        if (keyName.Equals("right ctrl"))
        {
            return UnityEngine.KeyCode.RightControl;
        }
        if (keyName.Equals("left ctrl"))
        {
            return UnityEngine.KeyCode.LeftControl;
        }
        if (keyName.Equals("right alt"))
        {
            return UnityEngine.KeyCode.RightAlt;
        }
        if (keyName.Equals("left alt"))
        {
            return UnityEngine.KeyCode.LeftAlt;
        }
        if (keyName.Equals("right cmd"))
        {
            return UnityEngine.KeyCode.RightCommand;
        }
        if (keyName.Equals("left cmd"))
        {
            return UnityEngine.KeyCode.LeftCommand;
        }
        if (System.Text.RegularExpressions.Regex.Match(keyName, @"mouse [0-6]").Success)
        {
            return (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), "Mouse" + keyName[6]);
        }
        if (keyName.Equals("backspace"))
        {
            return UnityEngine.KeyCode.Backspace;
        }
        if (keyName.Equals("tab"))
        {
            return UnityEngine.KeyCode.Tab;
        }
        if (keyName.Equals("return"))
        {
            return UnityEngine.KeyCode.Return;
        }
        if (keyName.Equals("escape"))
        {
            return UnityEngine.KeyCode.Escape;
        }
        if (keyName.Equals("space"))
        {
            return UnityEngine.KeyCode.Space;
        }
        if (keyName.Equals("delete"))
        {
            return UnityEngine.KeyCode.Delete;
        }
        if (keyName.Equals("enter"))
        {
            return UnityEngine.KeyCode.KeypadEnter;
        }
        if (keyName.Equals("insert"))
        {
            return UnityEngine.KeyCode.Insert;
        }
        if (keyName.Equals("home"))
        {
            return UnityEngine.KeyCode.Home;
        }
        if (keyName.Equals("end"))
        {
            return UnityEngine.KeyCode.End;
        }
        if (keyName.Equals("page up"))
        {
            return UnityEngine.KeyCode.PageUp;
        }
        if (keyName.Equals("page down"))
        {
            return UnityEngine.KeyCode.Home;
        }
        if (System.Text.RegularExpressions.Regex.Match(keyName, "joystick button [0-9]{1,2}").Success)
        {
            var splitedString = keyName.Split(' ');
            var number = System.Int32.Parse(splitedString[2]);
            if (number >= 20)
            {
                throw new NotFoundException("Key not recognized");
            }
            return (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), "JoystickButton" + number);
        }
        if (System.Text.RegularExpressions.Regex.Match(keyName, "joystick [1-8] button [0-9]{1,2}").Success)
        {
            var splitedString = keyName.Split(' ');
            var number = System.Int32.Parse(splitedString[3]);
            if (number >= 20)
            {
                throw new NotFoundException("Key not recognized");
            }
            return (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), "Joystick" + splitedString[1] + "Button" + number);
        }
        throw new NotFoundException("Key not recognized");
    }
    private static bool IsEnglishLetter(char c)
    {
        return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
    }


}

public class KeyStructure
{
    public KeyStructure(UnityEngine.KeyCode keyCode, float power)
    {
        KeyCode = keyCode;
        Power = power;
    }

    public UnityEngine.KeyCode KeyCode { get; set; }

    public float Power { get; set; }

}
#else
using UnityEngine;

namespace Altom.Server.Input
{
    public class Input : MonoBehaviour
    {

    }
}
#endif