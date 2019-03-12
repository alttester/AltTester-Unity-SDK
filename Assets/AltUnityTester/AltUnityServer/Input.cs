#if ALTUNITYTESTER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{
  
    private static bool UseCustomInput;
    public void Start()
    {
       
        instance = this;
        mockUpPointerInputModule = new MockUpPointerInputModule();

    }
    private void Update()
    {
        UseCustomInput = UnityEngine.Input.touchCount == 0;
    }
    public static Input instance;
    public static List<KeyCode> keyCodesPressed = new List<KeyCode>();
    public static List<KeyCode> keyCodesPressedDown = new List<KeyCode>();
    public static List<KeyCode> keyCodesPressedUp = new List<KeyCode>();
    private static MockUpPointerInputModule mockUpPointerInputModule;

    public static bool simulateMouseWithTouches
    {
        get { return UnityEngine.Input.simulateMouseWithTouches;}
        set { UnityEngine.Input.simulateMouseWithTouches = value; }
    }

    public static bool anyKey { get; set; }

    public static bool anyKeyDown { get; set; }

    public static string inputString { get; set; }

    public static Vector3 acceleration { get; set; }

    public static AccelerationEvent[] accelerationEvents { get; set; }

    public static int accelerationEventCount { get; set; }

    private static Touch[] _touches=new Touch[0];
    public static Touch[] touches
    {
        get { return UseCustomInput ? _touches : UnityEngine.Input.touches; }
        set
        {
            _touches =value;
        }
    }
    public Touch this[int i]
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

    public static bool mousePresent { get; set; }

    public static bool stylusTouchSupported { get; set; }

    public static bool touchSupported { get; set; }

    public static bool multiTouchEnabled { get; set; }

    public static LocationService location { get; set; }

    public static Compass compass { get; set; }

    public static DeviceOrientation deviceOrientation { get; set; }

    public static IMECompositionMode imeCompositionMode { get; set; }

    public static string compositionString { get; set; }

    public static bool imeIsSelected { get; set; }

    public static bool touchPressureSupported { get; set; }

    public static Vector2 mouseScrollDelta { get; set; }

    public static Vector3 mousePosition { get; set; }

    public static Gyroscope gyro { get; set; }

    public static Vector2 compositionCursorPos { get; set; }

    public static bool backButtonLeavesApp { get; set; }

    public static bool isGyroAvailable { get; set; }

    public static bool compensateSensors { get; set; }
    public static bool Finished { get; set; }


    public static AccelerationEvent GetAccelerationEvent(int index)
    {
        return UnityEngine.Input.GetAccelerationEvent(index);
    }

    public static float GetAxis(string axisName)
    {
        return UnityEngine.Input.GetAxis(axisName);


    }

    public static float GetAxisRaw(string axisName)
    {
        return 0;
    }
    public static bool GetButton(string buttonName)
    {
        KeyCode keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), buttonName);
        return keyCodesPressed.Contains(keyCode);
    }

    public static bool GetButtonDown(string buttonName)
    {
        KeyCode keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), buttonName);
        return keyCodesPressedDown.Contains(keyCode);
    }

    public static bool GetButtonUp(string buttonName)
    {
        KeyCode keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), buttonName);
        return keyCodesPressedUp.Contains(keyCode);
    }

    public static string[] GetJoystickNames()
    {
        return UnityEngine.Input.GetJoystickNames();
    }

    public static bool GetKey(string name)
    {
        return UnityEngine.Input.GetKey(name);
    }

    public static bool GetKey(KeyCode key)
    {
        return UnityEngine.Input.GetKey(key);
    }

    public static bool GetKeyDown(KeyCode key)
    {
        return UnityEngine.Input.GetKeyDown(key);
    }

    public static bool GetKeyDown(string name)
    {
        return UnityEngine.Input.GetKeyDown(name);
    }

    public static bool GetKeyUp(KeyCode key)
    {
        return UnityEngine.Input.GetKeyUp(key);
    }

    public static bool GetKeyUp(string name)
    {
        return UnityEngine.Input.GetKeyUp(name);
    }

    public static bool GetMouseButton(int button)
    {
        //method not tested
        return touches.Length > button;
    }

    public static bool GetMouseButtonDown(int button)
    {
        //method not tested
        return touches.Length > button && touches[button].phase != TouchPhase.Began;
    }

    public static bool GetMouseButtonUp(int button)
    {
        //method not tested
        return touches.Length > button && touches[button].phase == TouchPhase.Ended;
    }

    public static Touch GetTouch(int index)
    {
         return UseCustomInput ? _touches[index] : UnityEngine.Input.GetTouch(index);

    }

    public static void ResetInputAxes()
    {
        UnityEngine.Input.ResetInputAxes();
    }
    public static void SetMovingTouch(Touch touch, Vector2 destination, float duration)
    {
        Finished = false;
        instance.StartCoroutine(MovingTouchLifeCycle(touch, destination, duration));
    }

    public static IEnumerator MovingTouchLifeCycle(Touch touch, Vector2 destination, float duration)
    {

        float xDistance = (destination.x - touch.position.x);
        float yDistance = (destination.y - touch.position.y);

        Input.touchCount++;
        var touchListCopy = new Touch[touchCount];
        for (int i = 0; i < Input.touches.Length; i++)
        {
            touchListCopy[i] = Input.touches[i];
        }

        touchListCopy[Input.touchCount - 1] = touch;
        Input.touches = touchListCopy;
        mousePosition = new Vector3(touches[0].position.x, touches[0].position.y, 0);
        var pointerEventData = mockUpPointerInputModule.ExecuteTouchEvent(touch);
        yield return null;
        float time = 0;
        do
        {
            float deltaX;
            float deltaY;
            if (time + Time.deltaTime < duration)
            {
                deltaX = xDistance * Time.deltaTime / duration;
                deltaY = yDistance * Time.deltaTime / duration;
            }
            else
            {

                deltaX = xDistance * (duration - time) / duration;
                deltaY = yDistance * (duration - time) / duration;
            }
            time += Time.deltaTime;
            touch.position = new Vector2(touch.position.x + deltaX, touch.position.y + deltaY);
            touch.deltaPosition = new Vector2(deltaX, deltaY);
            touch.phase = touch.deltaPosition != Vector2.zero ? TouchPhase.Moved : TouchPhase.Stationary;
            for (int i = 0; i < touches.Length; i++)
            {
                if (touches[i].fingerId == touch.fingerId)
                {
                    touches[i] = touch;
                }
            }
            mousePosition = new Vector3(touches[0].position.x, touches[0].position.y, 0);


            //index = touches.IndexOf(touch);
            //touches[index] = touch;

            pointerEventData = mockUpPointerInputModule.ExecuteTouchEvent(touch, pointerEventData);
            Debug.Log(time + "  " + duration);

            yield return null;

        } while (time <= duration);

        touch.phase = TouchPhase.Ended;
        for (int i = 0; i < touches.Length; i++)
        {
            if (touches[i].fingerId == touch.fingerId)
            {
                touches[i] = touch;
            }
        }
        //touches[index] = touch;
        mockUpPointerInputModule.ExecuteTouchEvent(touch, pointerEventData);
        yield return null;
        var touches2 = new Touch[touchCount - 1];
        int contor = 0;
        foreach (var a in Input.touches)
        {
            if (a.fingerId != touch.fingerId)
            {
                touches2[contor] = a;
                contor++;
            }
        }

        Input.touches = touches2;
        Input.touchCount--;
        Finished = true;


    }
    public static void SetKeyDown(KeyCode keyCode, float duration)
    {
        KeyDownLifeCycle(keyCode, duration);
    }

    private static IEnumerator KeyDownLifeCycle(KeyCode keyCode, float duration)
    {
        float time = Time.time;
        keyCodesPressedDown.Add(keyCode);
        yield return null;
        keyCodesPressedDown.Remove(keyCode);
        keyCodesPressed.Add(keyCode);
        yield return new WaitForSeconds(duration);
        keyCodesPressed.Remove(keyCode);
        keyCodesPressedUp.Add(keyCode);
        yield return null;
        keyCodesPressedUp.Remove(keyCode);

    }
    private static IEnumerator proprietesSetter()
    {
        while (true)
        {
            if (keyCodesPressed.Count == 0 && keyCodesPressedDown.Count == 0 && keyCodesPressedUp.Count == 0)
            {
                anyKey = false;
                anyKeyDown = false;
            }
            else
            {
                if (keyCodesPressedDown.Count != 0)
                {
                    anyKeyDown = true;
                }
                anyKey = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }
   
}

#endif
