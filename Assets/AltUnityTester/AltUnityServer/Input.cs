#if ALTUNITYTESTER

using Assets.AltUnityTester.AltUnityDriver;
using System.Linq;

public class Input : UnityEngine.MonoBehaviour
{
  
    private static bool UseCustomInput;
    private static System.Collections.Generic.List<AltUnityAxis> AxisList;
    public void Start()
    {
       
        instance = this;
        mockUpPointerInputModule = new AltUnityMockUpPointerInputModule();
        string filePath = "AltUnityTester/AltUnityTesterInputAxisData";

        UnityEngine.TextAsset targetFile = UnityEngine.Resources.Load<UnityEngine.TextAsset>(filePath);
        string dataAsJson = targetFile.text;
        AxisList = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityAxis>>(dataAsJson);
    }
    private void Update()
    {
        UseCustomInput = UnityEngine.Input.touchCount == 0 && !UnityEngine.Input.anyKey;
    }
    
    public static Input instance;
    public static System.Collections.Generic.List<KeyStructure> keyCodesPressed = new System.Collections.Generic.List<KeyStructure>();
    public static System.Collections.Generic.List<KeyStructure> keyCodesPressedDown = new System.Collections.Generic.List<KeyStructure>();
    public static System.Collections.Generic.List<KeyStructure> keyCodesPressedUp = new System.Collections.Generic.List<KeyStructure>();
    private static AltUnityMockUpPointerInputModule mockUpPointerInputModule;

    public static bool simulateMouseWithTouches
    {
        get { return UnityEngine.Input.simulateMouseWithTouches;}
        set { UnityEngine.Input.simulateMouseWithTouches = value; }
    }

    public static bool anyKey {
        get {
            if (UseCustomInput)
            {
                if (keyCodesPressed.Count > 0 )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return UnityEngine.Input.anyKey;
            }
        }
    }

    public static bool anyKeyDown {
        get
        {
            if (UseCustomInput)
            {
                if (keyCodesPressedDown.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return UnityEngine.Input.anyKeyDown;
            }
        }
    }

    public static string inputString//WIP
    {
        get
        {
            if (UseCustomInput)
            {
                string charachtersPressedCurrentFrame = "";
                foreach(var keyCode in keyCodesPressedDown)
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
    }//Doable
    private static UnityEngine.Vector3 _acceleration;
    public static UnityEngine.Vector3 acceleration
    {
        get
        {
            if (UseCustomInput)
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
    private static UnityEngine.AccelerationEvent[] _accelerationEvents;
    public static UnityEngine.AccelerationEvent[] accelerationEvents
    {
        get
        {
            if (UseCustomInput)
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
            if (UseCustomInput)
            {
                return _accelerationEvents.Length;
            }
            else
            {
                return UnityEngine.Input.accelerationEventCount;
            }
        }
    }

    private static UnityEngine.Touch[] _touches=new UnityEngine.Touch[0];
    public static UnityEngine.Touch[] touches
    {
        get { return UseCustomInput ? _touches : UnityEngine.Input.touches; }
        set
        {
            _touches =value;
        }
    }
    public UnityEngine.Touch this[int i]
    {
        get { return UseCustomInput ? _touches[i] : UnityEngine.Input.GetTouch(i); }
        set { _touches[i] = value; }
    }

    private static int _touchCount;
    public static int touchCount
    {
        get { return UseCustomInput ? _touchCount : UnityEngine.Input.touchCount; }
        set { _touchCount = value; }
    }
    //NotImplementedForAltUnityTester
    public static bool mousePresent
    {
        get
        {
            return UnityEngine.Input.mousePresent;
        }
    }

    //NotImplementedForAltUnityTester
    public static bool stylusTouchSupported { get; set; }//?

    //NotImplementedForAltUnityTester
    public static bool touchSupported { get; set; }//?

    //NotImplementedForAltUnityTester
    public static bool multiTouchEnabled { get; set; }//?

    // //NotImplementedForAltUnityTester
    // public static LocationService location {
    //     get
    //     {
    //         return UnityEngine.Input.location;
    //     }
    // }

    //NotImplementedForAltUnityTester
    public static UnityEngine.Compass compass {
        get {
            return UnityEngine.Input.compass;
        }
    }//NotDoableAtThisMoment

    public static UnityEngine.DeviceOrientation deviceOrientation
    {
        get
        {
            return UnityEngine.Input.deviceOrientation;
        }
    }

    //NotImplementedForAltUnityTester
    public static UnityEngine.IMECompositionMode imeCompositionMode {
        get
        {
            return UnityEngine.Input.imeCompositionMode;
        }
    }//?

    //NotImplementedForAltUnityTester
    public static string compositionString { get; set; }

    //NotImplementedForAltUnityTester
    public static bool imeIsSelected { get; set; }

    //NotImplementedForAltUnityTester
    public static bool touchPressureSupported { get; set; }

    private static UnityEngine.Vector2 _mouseScrollDelta=new UnityEngine.Vector2();
    public static UnityEngine.Vector2 mouseScrollDelta
    {
        get
        {
            if (UseCustomInput)
            {
                return _mouseScrollDelta;
            }
            else
            {
                return UnityEngine.Input.mouseScrollDelta;
            }
        }
    }

    private static UnityEngine.Vector3 _mousePosition=new UnityEngine.Vector3();
    public static UnityEngine.Vector3 mousePosition {
        get
        {
            if (UseCustomInput)
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

    //NotImplementedForAltUnityTester
    public static UnityEngine.Gyroscope gyro { get
        {
            return UnityEngine.Input.gyro;
        }
    }

    //NotImplementedForAltUnityTester
    public static UnityEngine.Vector2 compositionCursorPos
    {
        get
        {
            return UnityEngine.Input.compositionCursorPos;
        }
    }

    //NotImplementedForAltUnityTester
    public static bool backButtonLeavesApp
    {
        get
        {
            return UnityEngine.Input.backButtonLeavesApp;
        }
    }

    //NotImplementedForAltUnityTester
    public static bool isGyroAvailable
    {
        get
        {
            return UnityEngine.Input.isGyroAvailable;
        }
    }

    public static bool compensateSensors
    {
        get
        {
            return UnityEngine.Input.compensateSensors;
        }
    }


    //Our
    public static bool Finished { get; set; }
    public static float LastAxisValue { get; set; }
    public static string LastAxisName { get; set; }
    public static string LastButtonDown { get; set; }
    public static string LastButtonPressed { get; set; }
    public static string LastButtonUp { get; set; }


    public static UnityEngine.AccelerationEvent GetAccelerationEvent(int index)
    {
        return UnityEngine.Input.GetAccelerationEvent(index);
    }
    public static float GetAxis(string axisName)
    {
        if (UseCustomInput)
        {
            var axis = AxisList.First(axle=>axle.name==axisName);
            if (axis==null)
            {
                throw new NotFoundException("No axis with this name was found");
            }
            foreach (var keyStructure in keyCodesPressed)
            {
                if ((axis.positiveButton!="" && keyStructure.KeyCode == ConvertStringToKeyCode(axis.positiveButton)) || (axis.altPositiveButton != "" && keyStructure.KeyCode == ConvertStringToKeyCode(axis.altPositiveButton)))
                {
                    LastAxisName = axisName;//DebugPurpose
                    LastAxisValue = keyStructure.Power;
                    return keyStructure.Power;
                }
                if ((axis.negativeButton!="" && keyStructure.KeyCode == ConvertStringToKeyCode(axis.negativeButton)) || (axis.altNegativeButton != "" && keyStructure.KeyCode == ConvertStringToKeyCode(axis.altNegativeButton)))
                {
                    LastAxisName = axisName;//DebugPurpose
                    LastAxisValue = -1*keyStructure.Power;
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
        if (UseCustomInput)
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
        if (UseCustomInput)
        {
            var axis = AxisList.First(axle => axle.name == buttonName);
            
            if (axis == null)
            {
                throw new NotFoundException("No button with this name was found");
            }
            
            foreach (var keyStructure in keyCodesPressed)
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
        
        if (UseCustomInput)
        {
            var axis = AxisList.First(axle => axle.name == buttonName);
            if (axis == null)
            {
                throw new NotFoundException("No button with this name was found");
            }
            foreach (var keyStructure in keyCodesPressedDown)
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
        
        if (UseCustomInput)
        {
            var axis = AxisList.First(axle => axle.name == buttonName);
            if (axis == null)
            {
                throw new NotFoundException("No button with this name was found");
            }
            foreach (var keyStructure in keyCodesPressedUp)
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

    public static string[] GetJoystickNames()
    {
        return UnityEngine.Input.GetJoystickNames();
    }

    public static bool GetKey(string name)
    {
        if (UseCustomInput)
        {
            UnityEngine.KeyCode keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), name);
            return 0 != keyCodesPressedDown.FindAll(key => key.KeyCode == keyCode).Count;
        }
        else
        {
            return UnityEngine.Input.GetKey(name);
        }
    }

    public static bool GetKey(UnityEngine.KeyCode key)
    {
        if (UseCustomInput)
        {
            return 0 != keyCodesPressed.FindAll(keyFromList => keyFromList.KeyCode == key).Count;
        }
        else
        {
            return UnityEngine.Input.GetKey(key);
        }
    }

    public static bool GetKeyDown(UnityEngine.KeyCode key)
    {
        if (UseCustomInput)
        {
            return 0 != keyCodesPressedDown.FindAll(keyFromList => keyFromList.KeyCode == key).Count;
        }
        else
        {
            return UnityEngine.Input.GetKeyDown(key);
        }
    }

    public static bool GetKeyDown(string name)
    {
        
        if (UseCustomInput)
        {
            UnityEngine.KeyCode keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), name);
            return 0 != keyCodesPressedDown.FindAll(key => key.KeyCode == keyCode).Count;
        }
        else
        {
            return UnityEngine.Input.GetKeyDown(name);
        }
    }

    public static bool GetKeyUp(UnityEngine.KeyCode key)
    {
        if (UseCustomInput)
        {
            return 0 != keyCodesPressedUp.FindAll(keyFromList => keyFromList.KeyCode == key).Count;
        }
        else
        {
            return UnityEngine.Input.GetKeyUp(key);
        }
    }

    public static bool GetKeyUp(string name)
    {
        if (UseCustomInput)
        {
            UnityEngine.KeyCode keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), name);
            return 0 != keyCodesPressedUp.FindAll(key => key.KeyCode == keyCode).Count;
        }
        else
        {
            return UnityEngine.Input.GetKeyUp(name);
        }
    }

    public static bool GetMouseButton(int button)
    {
        if (UseCustomInput)
        {
            var keyCode=(UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), "Mouse" + button);
            return 0 != keyCodesPressed.FindAll(key => key.KeyCode == keyCode).Count || touches.Length > button ;
        }
        else
        {
            return UnityEngine.Input.GetMouseButton(button);
        }
    }

    public static bool GetMouseButtonDown(int button)
    {
        //method not tested
        if (UseCustomInput)
        {
            var keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), "Mouse" + button);
            return 0 != keyCodesPressedDown.FindAll(key => key.KeyCode == keyCode).Count || touches.Length > button && touches[button].phase != UnityEngine.TouchPhase.Began;
        }
        else
        {
            return UnityEngine.Input.GetMouseButtonDown(button);
        }
    }

    public static bool GetMouseButtonUp(int button)
    {
        //method not tested
        if (UseCustomInput)
        {
            var keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), "Mouse" + button);
            return 0 != keyCodesPressedUp.FindAll(key => key.KeyCode == keyCode).Count || touches.Length > button && touches[button].phase == UnityEngine.TouchPhase.Ended;
        }
        else
        {
            return UnityEngine.Input.GetMouseButtonUp(button);
        }
    }

    public static UnityEngine.Touch GetTouch(int index)
    {
         return UseCustomInput ? _touches[index] : UnityEngine.Input.GetTouch(index);
    }

    public static void ResetInputAxes()
    {
        UnityEngine.Input.ResetInputAxes();
    }
    
    public static void SetMultipointSwipe(UnityEngine.Vector2[] positions, float duration)
    {
        Finished = false;
        instance.StartCoroutine(MultipointSwipeLifeCycle(positions, duration));
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
        var pointerEventData = mockUpPointerInputModule.ExecuteTouchEvent(touch);
        var markId = AltUnityRunner._altUnityRunner.ShowInput(touch.position);
        
        yield return null;
        
        var oneInputDuration = duration / (positions.Length - 1);
        for (var i = 1; i < positions.Length; i++)
        {
            var dest = positions[i];
            float xDistance = (dest.x - touch.position.x);
            float yDistance = (dest.y - touch.position.y);
            float time = 0;
            do
            {
                float deltaX;
                float deltaY;
                if (time + UnityEngine.Time.deltaTime < oneInputDuration)
                {
                    deltaX = xDistance * UnityEngine.Time.deltaTime / oneInputDuration;
                    deltaY = yDistance * UnityEngine.Time.deltaTime / oneInputDuration;
                }
                else
                {
                    deltaX = xDistance * (oneInputDuration - time) / oneInputDuration;
                    deltaY = yDistance * (oneInputDuration - time) / oneInputDuration;
                }

                touch.phase = touch.deltaPosition != UnityEngine.Vector2.zero ? UnityEngine.TouchPhase.Moved : UnityEngine.TouchPhase.Stationary;
                time += UnityEngine.Time.deltaTime;
                touch.position = new UnityEngine.Vector2(touch.position.x + deltaX, touch.position.y + deltaY);
                touch.deltaPosition = new UnityEngine.Vector2(deltaX, deltaY);

                for (var t = 0; t < touches.Length; t++)
                {
                    if (touches[t].fingerId == touch.fingerId)
                    {
                        touches[t] = touch;
                    }
                }

                mousePosition = new UnityEngine.Vector3(touches[0].position.x, touches[0].position.y, 0);
                pointerEventData = mockUpPointerInputModule.ExecuteTouchEvent(touch, pointerEventData);

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

        mockUpPointerInputModule.ExecuteTouchEvent(touch, pointerEventData);
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

    public static void SetKeyDown(UnityEngine.KeyCode keyCode,float power, float duration)
    {
       Finished = false;
       instance.StartCoroutine(KeyDownLifeCycle(keyCode,power, duration));
    }

    private static System.Collections.IEnumerator KeyDownLifeCycle(UnityEngine.KeyCode keyCode,float power, float duration)
    {

        float time = UnityEngine.Time.time;
        var keyStructure = new KeyStructure(keyCode, power);
        keyCodesPressedDown.Add(keyStructure);
        yield return null;
        keyCodesPressedDown.Remove(keyStructure);
        keyCodesPressed.Add(keyStructure);
        if (duration != 0)
        {
            yield return new UnityEngine.WaitForSeconds(duration);
        }
        keyCodesPressed.Remove(keyStructure);
        keyCodesPressedUp.Add(keyStructure);
        yield return null;
        keyCodesPressedUp.Remove(keyStructure);
        Finished = true;

    }   
    public static void MoveMouse(UnityEngine.Vector2 location,float duration)
    {
        Finished = false;
        instance.StartCoroutine(MoveMouseCycle(location, duration));
    }
    public static System.Collections.IEnumerator MoveMouseCycle(UnityEngine.Vector2 location, float duration)
    {
        float time = 0;
        var debugPurpose = mousePosition;
        var distance = location - new UnityEngine.Vector2(mousePosition.x,mousePosition.y);
        do
        {
            float deltaX;
            float deltaY;
            if (time + UnityEngine.Time.deltaTime < duration)
            {
                deltaX = distance.x * UnityEngine.Time.deltaTime / duration;
                deltaY = distance.y * UnityEngine.Time.deltaTime / duration;
            }
            else
            {

                deltaX = distance.x * (duration - time) / duration;
                deltaY = distance.y * (duration - time) / duration;
            }
            mousePosition = new UnityEngine.Vector3(mousePosition.x + deltaX, mousePosition.y + deltaY, 0);
            yield return null;
            time += UnityEngine.Time.deltaTime;
        } while (time <= duration);
        Finished = true;
    }
    public static void Scroll(float scrollValue,float duration)
    {
        Finished = false;
        instance.StartCoroutine(ScrollLifeCycle(scrollValue, duration));
    }
    private static System.Collections.IEnumerator ScrollLifeCycle(float scrollValue, float duration)
    {
        float timeSpent = 0;
        while (timeSpent < duration)
        {
            _mouseScrollDelta = new UnityEngine.Vector2(0, scrollValue);//x value is not taken in consideration
            yield return null;
            timeSpent += UnityEngine.Time.deltaTime;
        }
        Finished = true;

    }
    private static UnityEngine.KeyCode ConvertStringToKeyCode(string keyName)
    {
        if(keyName.Length==1 && IsEnglishLetter(keyName[0]))
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
        if(keyName.Equals("right alt"))
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
            return (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), "Joystick"+splitedString[1]+"Button" + number);
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

#endif
