/*
    Copyright(C) 2026 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AltTester.AltTesterUnitySDK;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterUnitySDK.InputModule;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif
using UnityEngine.Scripting;
using static UnityEngine.EventSystems.ExecuteEvents;
using UnityEngine.UIElements;


namespace AltTester.AltTesterUnitySDK.InputModule
{
    [Preserve]
    public class AltInput : MonoBehaviour
    {
        public static bool UseCustomInput;
        public static Vector3 Acceleration;
        public static AccelerationEvent[] AccelerationEvents;
        public static int TouchCount;
        public static Touch[] Touches = new Touch[0];
        public static Vector2 MouseScrollDelta = new Vector2();
        public static Vector3 MousePosition = new Vector3();
        public static Vector3 MouseDelta = new Vector3();
        public static List<AltAxis> AxisList;
        public static GameObject EventSystemTargetMouseDown;
        public static GameObject MonoBehaviourTargetMouseDown;
        public static Vector3 PreviousMousePosition = new Vector3();
        public static GameObject MonoBehaviourPreviousTarget = null;
        public static GameObject PreviousEventSystemTarget = null;

        public static AltMockUpPointerInputModule MockUpPointerInputModule;
        public static AltInput Instance;
        public readonly static List<KeyStructure> KeyCodesPressed = new List<KeyStructure>();
        public readonly static List<KeyStructure> KeyCodesPressedDown = new List<KeyStructure>();
        public readonly static List<KeyStructure> KeyCodesPressedUp = new List<KeyStructure>();
        public readonly static Dictionary<int, PointerEventData> PointerEventsDataDictionary = new Dictionary<int, PointerEventData>();
        public static readonly KeyCode[] MouseKeyCodes = { KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2 };
        public static readonly Dictionary<PointerEventData.InputButton, int> PointerIds = new Dictionary<PointerEventData.InputButton, int>
    {
        {PointerEventData.InputButton.Left, -1},
        {PointerEventData.InputButton.Right, -2},
        {PointerEventData.InputButton.Middle, -3}
    };
        public static PointerEventData MouseDownPointerEventData = null;
        public static readonly PointerEventData.InputButton[] MouseButtons = { PointerEventData.InputButton.Left, PointerEventData.InputButton.Middle, PointerEventData.InputButton.Right };

        public static float LastAxisValue { get; set; }
        public static string LastAxisName { get; set; }
        public static string LastButtonDown { get; set; }
        public static string LastButtonPressed { get; set; }
        public static string LastButtonUp { get; set; }


        public static List<KeyCode> KeyDownFlag = new List<KeyCode>();

        private static EventSystem eventSystem;


        internal static bool needsExtraInitFrame = false;

        private static bool ShouldHandleEventsManually
        {
#if ENABLE_INPUT_SYSTEM
            get => !(GetEventSystem().currentInputModule is InputSystemUIInputModule);
#else
            get => true;
#endif
        }

        public void ResetInput()
        {
            KeyCodesPressed.Clear();
            KeyCodesPressedDown.Clear();
            KeyCodesPressedUp.Clear();
            KeyDownFlag.Clear();
            MousePosition = Vector3.zero;
            Touches = new Touch[0];
            TouchCount = 0;
            Acceleration = Vector3.zero;
            AccelerationEvents = new AccelerationEvent[0];
            PointerEventsDataDictionary.Clear();
            MouseDownPointerEventData = null;
            EventSystemTargetMouseDown = null;
            MonoBehaviourTargetMouseDown = null;
            PreviousMousePosition = Vector3.zero;
            CoroutineManager.Instance.StopAllCoroutines();
        }
        public static EventSystem GetEventSystem()
        {
            var current = EventSystem.current;


            if (eventSystem != null && !eventSystem)
            {
                eventSystem = null;
                MockUpPointerInputModule = null;
            }

            if (current != null && current != eventSystem)
            {
                eventSystem = current;
                MockUpPointerInputModule = null; // force re-creation on the correct EventSystem
                // Signal that bridge components were just added; their Start() hasn't run yet.
                needsExtraInitFrame = true;
                return eventSystem;
            }
            if (eventSystem != null)
                return eventSystem;
            eventSystem = current;
            if (eventSystem != null)
                return eventSystem;
#if UNITY_6000_0_OR_NEWER
            eventSystem = FindAnyObjectByType<EventSystem>();
#else
            eventSystem = FindObjectOfType<EventSystem>();

#endif
            if (eventSystem != null)
                return eventSystem;
            GameObject eventSystemGameObject = new GameObject("EventSystem");
            eventSystem = eventSystemGameObject.AddComponent<EventSystem>();
            return eventSystem;
        }

        public static AltMockUpPointerInputModule AltMockUpPointerInputModule
        {
            get
            {
                if (MockUpPointerInputModule == null)
                {
                    MockUpPointerInputModule = GetEventSystem().gameObject.AddComponent<AltMockUpPointerInputModule>();
                }
                return MockUpPointerInputModule;
            }
        }

        public static string SimulatedInputString { get; private set; }

        public void Start()
        {
            Instance = this;
            MockUpPointerInputModule = AltMockUpPointerInputModule; //initialize
            LoadAxisList();
        }

        public static bool LoadAxisList()
        {
            try
            {
                string filePath = "AltTester/AltTesterInputAxisData";

                UnityEngine.TextAsset targetFile = UnityEngine.Resources.Load<UnityEngine.TextAsset>(filePath);
                string dataAsJson = targetFile.text;
                AxisList = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltAxis>>(dataAsJson, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver(),
                    Culture = CultureInfo.InvariantCulture,
                    Formatting = Formatting.Indented
                });
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }

            return AxisList != null;



        }

        private void FixedUpdate()
        {
            var monoBehaviourTarget = FindObjectViaRayCast.GetGameObjectHitMonoBehaviour(MousePosition);
            if (MonoBehaviourPreviousTarget != monoBehaviourTarget)
            {
                if (MonoBehaviourPreviousTarget != null) MonoBehaviourPreviousTarget.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
                if (monoBehaviourTarget != null && PreviousMousePosition != MousePosition) monoBehaviourTarget.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
                MonoBehaviourPreviousTarget = monoBehaviourTarget;
            }
            if (monoBehaviourTarget != null) monoBehaviourTarget.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);

            if (MousePosition == PreviousMousePosition) return;

            var eventSystem = GetEventSystem();
            var pointerEventData = new PointerEventData(eventSystem)
            {
                position = MousePosition,
                button = PointerEventData.InputButton.Left,
                eligibleForClick = true
            };
            var eventSystemTarget = findEventSystemObject(pointerEventData);
            pointerEventData.pointerEnter = eventSystemTarget;

            if (ShouldHandleEventsManually)
            {
                if (eventSystemTarget != PreviousEventSystemTarget)
                {
                    if (PreviousEventSystemTarget != null) ExecuteHierarchy(PreviousEventSystemTarget, pointerEventData, pointerExitHandler);
                    if (eventSystemTarget != null) ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerEnterHandler);
                    PreviousEventSystemTarget = eventSystemTarget;
                }
                if (eventSystemTarget != null) ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerMoveHandler);
            }

            PreviousMousePosition = MousePosition;
        }
        public static GameObject ExecuteHierarchy<T>(GameObject root, BaseEventData eventData, EventFunction<T> callbackFunction) where T : IEventSystemHandler
        {
            var chain = new List<Transform>();
            getEventChain(root, chain);

            var internalTransformListCount = chain.Count;
            for (var i = 0; i < internalTransformListCount; i++)
            {
                var transform = chain[i];
                if (Execute(transform.gameObject, eventData, callbackFunction))
                    if ((transform != null) && (transform.gameObject != null))
                        return transform.gameObject;
                    else
                        return null;

            }
            return null;
        }
        private static void getEventChain(GameObject root, IList<Transform> eventChain)
        {
            eventChain.Clear();
            if (root == null)
                return;

            var t = root.transform;
            while (t != null)
            {
                eventChain.Add(t);
                t = t.parent;
            }
        }


        /// <summary>
        /// Finds element at given pointerEventData for which we raise EventSystem input events
        /// </summary>
        /// <param name="pointerEventData"></param>
        /// <returns>the found gameObject</returns>
        private static GameObject findEventSystemObject(PointerEventData pointerEventData)
        {
            RaycastResult firstRaycastResult;
            AltMockUpPointerInputModule.GetFirstRaycastResult(pointerEventData, out firstRaycastResult);
            pointerEventData.pointerCurrentRaycast = firstRaycastResult;
            pointerEventData.pointerPressRaycast = firstRaycastResult;
            return firstRaycastResult.gameObject;
        }

        #region public commands interface
        public static int BeginTouch(Vector3 screenPosition)
        {
            var touch = createTouch(screenPosition);

            var pointerEventData = AltMockUpPointerInputModule.ExecuteTouchEvent(touch);
            if (touch.fingerId == 0)
            {
                MousePosition = screenPosition;
                mouseTriggerInit(PointerEventData.InputButton.Left, out PointerEventData _, out GameObject eventSystemTarget, out GameObject monoBehaviourTarget);
                mouseDownTrigger(PointerEventData.InputButton.Left, ref pointerEventData, eventSystemTarget, monoBehaviourTarget);
                MouseDownPointerEventData = pointerEventData;
            }
            PointerEventsDataDictionary.Add(touch.fingerId, pointerEventData);
            Instance.StartCoroutine(setMouse0KeyCodePressedDown());



            return touch.fingerId;
        }

        public static void MoveTouch(int fingerId, Vector3 screenPosition)
        {
            var touch = findTouch(fingerId);
            MoveTouch(touch, screenPosition);

        }
        public static void MoveTouch(Touch touch, Vector3 screenPosition)
        {
            var previousPointerEventData = PointerEventsDataDictionary[touch.fingerId];
            var previousPosition = touch.position;
            touch.phase = TouchPhase.Moved;
            touch.position = screenPosition;
            touch.rawPosition = screenPosition;
            touch.deltaPosition = touch.position - previousPosition;
            if (touch.fingerId == 0)
            {
                MousePosition = screenPosition;
            }
            updateTouchInTouchList(touch);
            AltMockUpPointerInputModule.ExecuteTouchEvent(touch, previousPointerEventData);
        }

        public static IEnumerator EndTouch(int fingerId)
        {
            yield return Instance.StartCoroutine(EndTouch(findTouch(fingerId)));
        }
        public static IEnumerator EndTouch(Touch touch)
        {
            yield return WaitForEndOfFrame();

            var previousPointerEventData = PointerEventsDataDictionary[touch.fingerId];
            PointerEventsDataDictionary.Remove(touch.fingerId);

            var keyStructure = new KeyStructure(KeyCode.Mouse0, 1);
            beginKeyUpTouchEndedLifecycle(keyStructure, true, ref touch);
            AltMockUpPointerInputModule.ExecuteTouchEvent(touch, previousPointerEventData);

            yield return null;
            endKeyUpTouchEndedLifecycle(keyStructure, true, touch);
        }



        public static IEnumerator MultipointSwipeLifeCycle(Vector2[] positions, float duration)
        {
            int touchId = BeginTouch(positions[0]);
            var touch = findTouch(touchId);
            yield return null;

            var oneInputDuration = duration / (positions.Length - 1);
            for (var i = 1; i < positions.Length; i++)
            {
                var wholeDelta = positions[i] - touch.position;
                var deltaPerSecond = wholeDelta / oneInputDuration;
                float time = 0;
                do
                {
                    yield return null;
                    time += Time.unscaledDeltaTime;
                    var newPosition = time < oneInputDuration ? touch.position + deltaPerSecond * Time.unscaledDeltaTime : positions[i];
                    MoveTouch(touch, newPosition);
                    touch = findTouch(touchId);
                } while (time <= oneInputDuration);
            }

            yield return CoroutineManager.Instance.StartCoroutine(EndTouch(touchId));

        }

        public static IEnumerator MoveMouseCycle(Vector2 location, float duration)
        {
            float time = 0;

            var distance = location - new Vector2(MousePosition.x, MousePosition.y);
            if (distance == Vector2.zero)
            {
                yield break;
            }

            do
            {
                if (time + Time.unscaledDeltaTime < duration)
                {
                    MouseDelta = distance * Time.unscaledDeltaTime / duration;
                }
                else
                {
                    MouseDelta = location - new Vector2(MousePosition.x, MousePosition.y);
                }
                MousePosition += MouseDelta;
                if (MouseDownPointerEventData != null)
                {
                    MockUpPointerInputModule.ExecuteDragPointerEvents(MouseDownPointerEventData);
                    MouseDownPointerEventData.position = MousePosition;
                    MouseDownPointerEventData.delta = MouseDelta;
                    findEventSystemObject(MouseDownPointerEventData);
                }
                time += Time.unscaledDeltaTime;
                yield return null;
            } while (time < duration);
        }

        internal static IEnumerator ScrollLifeCycle(float scrollVertical, float scrollHorizontal, float duration)
        {
            float timeSpent = 0;

            while (timeSpent < duration)
            {
                yield return null;
                timeSpent += Time.unscaledDeltaTime;
                float scrollVerticalStep = scrollVertical * Time.unscaledDeltaTime / duration;
                float scrollHorizontalStep = scrollHorizontal * Time.unscaledDeltaTime / duration;

                var pointerEventData = new PointerEventData(GetEventSystem())
                {
                    position = MousePosition,
                    button = PointerEventData.InputButton.Left,
                    eligibleForClick = true,
                };
                var eventSystemTarget = findEventSystemObject(pointerEventData);
                MouseScrollDelta = new Vector2(scrollHorizontalStep, scrollVerticalStep);
                pointerEventData.scrollDelta = MouseScrollDelta;
                if (ShouldHandleEventsManually && eventSystemTarget != null)
                    ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, scrollHandler);
            }
            MouseScrollDelta = Vector2.zero;//reset the value after scroll ended
        }

        internal static IEnumerator runThrowingIterator( //TODO Remove this method when all the input methods were implemented in InputController
                IEnumerator enumerator,
                Action<Exception> done)
        {
            Exception err = null;
            while (true)
            {
                object current;
                try
                {
                    if (enumerator.MoveNext() == false)
                    {
                        break;
                    }
                    current = enumerator.Current;
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.ToString());
                    err = ex;
                    yield break;
                }
                yield return current;
            }
            done.Invoke(err);
        }
        #endregion

        #region private interface

        /// <summary>
        /// Waits for the End of frame. In batch mode, where there is no rendering, it waits for the next update instead.
        /// </summary>
        /// <returns></returns>
        private static IEnumerator WaitForEndOfFrame()
        {
            if (Application.isBatchMode)
                yield return null;
            else
                yield return new WaitForEndOfFrame();
        }
        private static Touch createTouch(Vector3 screenPosition)
        {
            var touch = new Touch
            {
                phase = TouchPhase.Began,
                position = screenPosition,
                rawPosition = screenPosition,
                pressure = 1.0f,
                maximumPossiblePressure = 1.0f,
            };

            List<int> fingerIds = Touches.Select(t => t.fingerId).ToList();
            fingerIds.Sort();
            int fingerId = 0;
            foreach (var iter in fingerIds)
            {
                if (iter != fingerId)
                    break;
                fingerId++;
            }
            touch.fingerId = fingerId;

            TouchCount++;
            var touchListCopy = new Touch[Touches.Length + 1];
            Array.Copy(Touches, 0, touchListCopy, 0, Touches.Length);
            touchListCopy[Touches.Length] = touch;
            Touches = touchListCopy;
            return touch;
        }

        private static void destroyTouch(Touch touch)
        {
            Touches = Touches.Where(t => t.fingerId != touch.fingerId).ToArray();
            TouchCount--;
        }

        private static IEnumerator setMouse0KeyCodePressedDown()
        {
            var keyStructure = new KeyStructure(KeyCode.Mouse0, 1.0f);
            yield return WaitForEndOfFrame();
            KeyCodesPressedDown.Add(keyStructure);
            KeyCodesPressed.Add(keyStructure);

            yield return null;
            KeyCodesPressedDown.Remove(keyStructure);
        }

        private static void beginKeyUpTouchEndedLifecycle(KeyStructure keyStructure, bool tap, ref Touch touch)
        {
            if (tap)
            {
                touch.phase = TouchPhase.Ended;
                updateTouchInTouchList(touch);
            }
            KeyCodesPressed.Remove(keyStructure);
            KeyCodesPressedUp.Add(keyStructure);
        }

        private static void endKeyUpTouchEndedLifecycle(KeyStructure keyStructure, bool tap, Touch touch)
        {
            KeyCodesPressedUp.Remove(keyStructure);
            if (tap)
            {
                destroyTouch(touch);
            }
        }

        private static Touch findTouch(int fingerId)
        {
            return Touches.First(touch => touch.fingerId == fingerId);
        }

        public static IEnumerator DummyClickAt(Vector2 screenPosition)
        {
            yield return Instance.StartCoroutine(tapClickCoordinatesLifeCycle(screenPosition, 1, 0, true));
        }

        internal static IEnumerator tapClickCoordinatesLifeCycle(Vector2 screenPosition, int count, float interval, bool tap)
        {
            yield return tapClickLifeCycle(screenPosition, null, count, interval, tap);
        }

        internal static IEnumerator tapClickElementLifeCycle(GameObject target, int count, float interval, bool tap)
        {
            FindObjectViaRayCast.FindCameraThatSeesObject(target, out Vector3 screenPosition);
            yield return tapClickLifeCycle(screenPosition, target, count, interval, tap);
        }

        private static IEnumerator tapClickLifeCycle(Vector2 screenPosition, GameObject target, int count, float interval, bool tap)
        {
            var monoBehaviourTarget = target ?? FindObjectViaRayCast.FindMonoBehaviourObject(screenPosition);
            yield return WaitForEndOfFrame();

            var pointerEventData = new PointerEventData(GetEventSystem())
            {
                position = screenPosition,
                button = PointerEventData.InputButton.Left,
                eligibleForClick = true,
                pressPosition = screenPosition
            };

            GameObject eventSystemTarget;
            if (target != null)
            {
                pointerEventData.pointerCurrentRaycast = new RaycastResult { gameObject = target };
                eventSystemTarget = target;
            }
            else
            {
                eventSystemTarget = findEventSystemObject(pointerEventData);
            }

            MousePosition = screenPosition;
            pointerEventData.pointerEnter = eventSystemTarget;

            for (int i = 0; i < count; i++)
            {
                float time = 0;

                Touch touch = new Touch();
                int pointerId = 0;
                if (tap)
                {
                    touch = createTouch(screenPosition);
                    pointerId = touch.fingerId;
                }
                pointerEventData.pointerId = pointerId;

                var keyStructure = new KeyStructure(KeyCode.Mouse0, 1.0f);
                KeyCodesPressedDown.Add(keyStructure);
                KeyCodesPressed.Add(keyStructure);

                if (ShouldHandleEventsManually && eventSystemTarget != null)
                {
                    ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerEnterHandler);
                    ExecuteHierarchy(eventSystemTarget, pointerEventData, initializePotentialDrag);
                    pointerEventData.pointerPress = ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerDownHandler);
                }
                // When targeting a specific element, ensure pointerPress is set even if the handler returned null
                if (target != null)
                    pointerEventData.pointerPress = target;
                if (monoBehaviourTarget != null)
                    monoBehaviourTarget.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);

                time += Time.unscaledDeltaTime;
                yield return null;

                KeyCodesPressedDown.Remove(keyStructure);
                beginKeyUpTouchEndedLifecycle(keyStructure, tap, ref touch);

                if (ShouldHandleEventsManually && eventSystemTarget != null)
                {
                    ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerUpHandler);
                    ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerClickHandler);
                }
                if (monoBehaviourTarget != null)
                {
                    monoBehaviourTarget.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
                    monoBehaviourTarget.SendMessage("OnMouseUpAsButton", SendMessageOptions.DontRequireReceiver);
                }

                time += Time.unscaledDeltaTime;
                yield return null;

                endKeyUpTouchEndedLifecycle(keyStructure, tap, touch);

                if (i != count - 1 && time < interval)
                {
                    float elapsedTime = 0;
                    while (elapsedTime < interval - time)
                    {
                        elapsedTime += Time.unscaledDeltaTime;
                        yield return null;
                    }
                }
            }

            if (ShouldHandleEventsManually && eventSystemTarget != null)
                ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerExitHandler);
            if (monoBehaviourTarget != null)
                monoBehaviourTarget.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
        }

        private static void updateTouchInTouchList(Touch touch)
        {
            for (var t = 0; t < Touches.Length; t++)
            {
                if (Touches[t].fingerId == touch.fingerId)
                {
                    Touches[t] = touch;
                }
            }
        }

        internal static IEnumerator KeyDownLifeCycle(KeyCode keyCode, float power)
        {
            KeyDownFlag.Add(keyCode);
            yield return WaitForEndOfFrame();
            var keyStructure = new KeyStructure(keyCode, power);
            KeyCodesPressedDown.Add(keyStructure);
            KeyCodesPressed.Add(keyStructure);
            if (MouseKeyCodes.Contains(keyCode))
            {
                var inputButton = keyCodeToInputButton(keyCode);
                mouseTriggerInit(inputButton, out PointerEventData pointerEventData, out GameObject eventSystemTarget, out GameObject monoBehaviourTarget);
                mouseDownTrigger(inputButton, ref pointerEventData, eventSystemTarget, monoBehaviourTarget);
                MouseDownPointerEventData = pointerEventData;
            }
            var ch = KeyCodeToChar(keyCode);
            if (ch.HasValue) SimulatedInputString = ch.Value.ToString();
            yield return WaitForEndOfFrame();
            SimulatedInputString = "";
            KeyCodesPressedDown.Remove(keyStructure);

            KeyDownFlag.Remove(keyCode);
        }


        internal static IEnumerator KeyUpLifeCycle(KeyCode keyCode)
        {
            yield return WaitForEndOfFrame();
            while (KeyDownFlag.Contains(keyCode))
            {
                yield return null;
            }
            if (MouseKeyCodes.Contains(keyCode))
            {
                var inputButton = keyCodeToInputButton(keyCode);
                mouseTriggerInit(inputButton, out PointerEventData pointerEventData, out GameObject eventSystemTarget, out GameObject monoBehaviourTarget);
                if (MouseDownPointerEventData != null)
                {
                    pointerEventData = MouseDownPointerEventData;
                }
                mouseUpTrigger(inputButton, pointerEventData, eventSystemTarget, monoBehaviourTarget);
            }
            yield return KeyReleasedPhase(new KeyStructure(keyCode, 1));
        }

        private static PointerEventData.InputButton keyCodeToInputButton(KeyCode keyCode)
        {
            PointerEventData.InputButton[] inputButtons = { PointerEventData.InputButton.Left, PointerEventData.InputButton.Right, PointerEventData.InputButton.Middle };
            return inputButtons[Array.IndexOf(MouseKeyCodes, keyCode)];
        }


        internal static IEnumerator KeyPressLifeCycle(KeyCode keyCode, float power, float duration)
        {
            var keyStructure = new KeyStructure(keyCode, power);
            yield return null;
            KeyCodesPressedDown.Add(keyStructure);
            KeyCodesPressed.Add(keyStructure);
            var ch = KeyCodeToChar(keyCode);
            if (ch.HasValue) SimulatedInputString = ch.Value.ToString();
            yield return null;
            SimulatedInputString = "";
            KeyCodesPressedDown.Remove(keyStructure);
            if (MouseKeyCodes.Contains(keyCode))
            {
                var inputButton = keyCodeToInputButton(keyCode);
                yield return Instance.StartCoroutine(mouseEventTrigger(inputButton, duration));
            }
            else
            {
                if (duration != 0)
                {
                    float elapsedTime = 0;
                    while (elapsedTime < duration)
                    {
                        elapsedTime += Time.unscaledDeltaTime;
                        yield return null;
                    }
                }
            }
            yield return KeyReleasedPhase(keyStructure);
        }

        internal static char? KeyCodeToChar(KeyCode keyCode)
        {
            int code = (int)keyCode;
            // Standard printable ASCII range: space (32) through tilde (126)
            // Unity's KeyCode enum int values intentionally match ASCII for this range
            if (code >= 32 && code <= 126)
                return (char)code;
            if (keyCode == KeyCode.Backspace) return '\b';
            if (keyCode == KeyCode.Return || keyCode == KeyCode.KeypadEnter) return '\n';
            if (keyCode == KeyCode.Tab) return '\t';
            // Keypad 0-9: KeyCode.Keypad0=256 ... KeyCode.Keypad9=265
            if (code >= 256 && code <= 265) return (char)('0' + code - 256);
            return null;
        }



        private static IEnumerator KeyReleasedPhase(KeyStructure keyStructure)
        {
            KeyCodesPressed.Remove(keyStructure);
            KeyCodesPressedUp.Add(keyStructure);
            yield return null;
            KeyCodesPressedUp.Remove(keyStructure);
        }

        private static void mouseTriggerInit(PointerEventData.InputButton mouseButton, out PointerEventData pointerEventData, out GameObject eventSystemTarget, out GameObject monoBehaviourTarget)
        {

            pointerEventData = new PointerEventData(GetEventSystem())
            {
                position = MousePosition,
                button = mouseButton,
                eligibleForClick = true,
                pressPosition = MousePosition,
                dragging = false
            };
            eventSystemTarget = findEventSystemObject(pointerEventData);
            monoBehaviourTarget = FindObjectViaRayCast.FindMonoBehaviourObject(MousePosition);

        }

        private static void mouseDownTrigger(PointerEventData.InputButton mouseButton, ref PointerEventData pointerEventData, GameObject eventSystemTarget, GameObject monoBehaviourTarget)
        {
            pointerEventData.pointerId = PointerIds[mouseButton];
            if (ShouldHandleEventsManually && eventSystemTarget != null)
                pointerEventData.pointerPress = ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerDownHandler);

            if (mouseButton == PointerEventData.InputButton.Left && monoBehaviourTarget != null)
                monoBehaviourTarget.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);

            if (MouseButtons.Contains(mouseButton))
            {
                if (ShouldHandleEventsManually && eventSystemTarget != null)
                    pointerEventData.pointerDrag = ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, initializePotentialDrag);
                EventSystemTargetMouseDown = eventSystemTarget;
                MonoBehaviourTargetMouseDown = monoBehaviourTarget;
            }
        }

        private static void mouseUpTrigger(PointerEventData.InputButton mouseButton, PointerEventData pointerEventData, GameObject eventSystemTarget, GameObject monoBehaviourTarget)
        {
            if (ShouldHandleEventsManually)
            {
                if (eventSystemTarget == EventSystemTargetMouseDown && mouseButton == PointerEventData.InputButton.Left && eventSystemTarget != null)
                    ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerClickHandler);
                if (EventSystemTargetMouseDown != null)
                    ExecuteEvents.ExecuteHierarchy(EventSystemTargetMouseDown, pointerEventData, pointerUpHandler);
            }

            if (mouseButton == PointerEventData.InputButton.Left && (monoBehaviourTarget != null)) monoBehaviourTarget.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
            /* pointer/touch up */
            if (monoBehaviourTarget == MonoBehaviourTargetMouseDown && mouseButton == PointerEventData.InputButton.Left && (monoBehaviourTarget != null))
            {
                monoBehaviourTarget.SendMessage("OnMouseUpAsButton", SendMessageOptions.DontRequireReceiver);
            }


            if (MouseButtons.Contains(mouseButton) && (MouseDownPointerEventData != null))
                MockUpPointerInputModule.ExecuteEndDragPointerEvents(MouseDownPointerEventData);
            MonoBehaviourTargetMouseDown = null;

            MouseDownPointerEventData = null;
        }

        private static IEnumerator mouseEventTrigger(PointerEventData.InputButton mouseButton, float duration)
        {
            mouseTriggerInit(mouseButton, out PointerEventData pointerEventData, out GameObject eventSystemTarget, out GameObject monoBehaviourTarget);
            mouseDownTrigger(mouseButton, ref pointerEventData, eventSystemTarget, monoBehaviourTarget);
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            mouseUpTrigger(mouseButton, pointerEventData, eventSystemTarget, monoBehaviourTarget);
        }

        public static IEnumerator AccelerationLifeCycle(Vector3 accelarationValue, float duration)
        {
            float timeSpent = 0;
            while (timeSpent < duration)
            {
                Acceleration = accelarationValue;
                timeSpent += Time.unscaledDeltaTime;
                yield return null;
            }
            Acceleration = Vector3.zero;//reset the value after acceleration ended
        }


        private static readonly System.Text.RegularExpressions.Regex KeypadDigitRegex = new System.Text.RegularExpressions.Regex(@"^\[[0-9]\]$");
        private static readonly System.Text.RegularExpressions.Regex FunctionKeyRegex = new System.Text.RegularExpressions.Regex(@"^f[0-9]{1,2}$");
        private static readonly System.Text.RegularExpressions.Regex MouseButtonRegex = new System.Text.RegularExpressions.Regex(@"^mouse ([0-6])$");
        private static readonly System.Text.RegularExpressions.Regex JoystickButtonRegex = new System.Text.RegularExpressions.Regex(@"^joystick button ([0-9]{1,2})$");
        private static readonly System.Text.RegularExpressions.Regex JoystickNButtonRegex = new System.Text.RegularExpressions.Regex(@"^joystick ([1-8]) button ([0-9]{1,2})$");

        private static readonly Dictionary<string, KeyCode> KeyNameMap = new Dictionary<string, KeyCode>
        {
            { "left",        KeyCode.LeftArrow },
            { "right",       KeyCode.RightArrow },
            { "down",        KeyCode.DownArrow },
            { "up",          KeyCode.UpArrow },
            { "[+]",         KeyCode.KeypadPlus },
            { "[equals]",    KeyCode.KeypadEquals },
            { "right shift", KeyCode.RightShift },
            { "left shift",  KeyCode.LeftShift },
            { "right ctrl",  KeyCode.RightControl },
            { "left ctrl",   KeyCode.LeftControl },
            { "right alt",   KeyCode.RightAlt },
            { "left alt",    KeyCode.LeftAlt },
            { "right cmd",   KeyCode.RightCommand },
            { "left cmd",    KeyCode.LeftCommand },
            { "backspace",   KeyCode.Backspace },
            { "tab",         KeyCode.Tab },
            { "return",      KeyCode.Return },
            { "escape",      KeyCode.Escape },
            { "space",       KeyCode.Space },
            { "delete",      KeyCode.Delete },
            { "enter",       KeyCode.KeypadEnter },
            { "insert",      KeyCode.Insert },
            { "home",        KeyCode.Home },
            { "end",         KeyCode.End },
            { "page up",     KeyCode.PageUp },
            { "page down",   KeyCode.PageDown },
        };

        public static KeyCode ConvertStringToKeyCode(string keyName)
        {
            if (keyName.Length == 0)
                return KeyCode.None;

            if (keyName.Length == 1 && Char.IsLetter(keyName[0]))
                return (KeyCode)Enum.Parse(typeof(KeyCode), Char.ToUpper(keyName[0]).ToString());

            if (keyName.Length == 1 && char.IsDigit(keyName[0]))
                return (KeyCode)Enum.Parse(typeof(KeyCode), "Alpha" + keyName);

            if (KeyNameMap.TryGetValue(keyName, out var mapped))
                return mapped;

            if (KeypadDigitRegex.IsMatch(keyName))
                return (KeyCode)Enum.Parse(typeof(KeyCode), "Keypad" + keyName);

            if (FunctionKeyRegex.IsMatch(keyName))
                return (KeyCode)Enum.Parse(typeof(KeyCode), keyName.ToUpper());

            var mouseMatch = MouseButtonRegex.Match(keyName);
            if (mouseMatch.Success)
                return (KeyCode)Enum.Parse(typeof(KeyCode), "Mouse" + mouseMatch.Groups[1].Value);

            var joystickMatch = JoystickButtonRegex.Match(keyName);
            if (joystickMatch.Success)
            {
                var number = int.Parse(joystickMatch.Groups[1].Value);
                if (number >= 20) throw new NotFoundException("Key not recognized");
                return (KeyCode)Enum.Parse(typeof(KeyCode), "JoystickButton" + number);
            }

            var joystickNMatch = JoystickNButtonRegex.Match(keyName);
            if (joystickNMatch.Success)
            {
                var number = int.Parse(joystickNMatch.Groups[2].Value);
                if (number >= 20) throw new NotFoundException("Key not recognized");
                return (KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + joystickNMatch.Groups[1].Value + "Button" + number);
            }

            throw new NotFoundException("Key not recognized");
        }
        #endregion
    }



    public class KeyStructure
    {
        public KeyStructure(KeyCode keyCode, float power)
        {
            KeyCode = keyCode;
            Power = power;
        }
        public readonly KeyCode KeyCode;
        public readonly float Power;

        public override bool Equals(object obj)
        {
            if (!(obj is KeyStructure))
                return false;
            var other = (KeyStructure)obj;
            return other.KeyCode == this.KeyCode;
        }

        public override int GetHashCode()
        {
            return KeyCode.GetHashCode();
        }

        public override string ToString()
        {
            return KeyCode.ToString() + " with power " + Power;
        }
    }
}
