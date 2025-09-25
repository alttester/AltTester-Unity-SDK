/*
    Copyright(C) 2025 Altom Consulting

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
        public static List<KeyStructure> KeyCodesPressed = new List<KeyStructure>();
        public static List<KeyStructure> KeyCodesPressedDown = new List<KeyStructure>();
        public static List<KeyStructure> KeyCodesPressedUp = new List<KeyStructure>();
        public static Dictionary<int, PointerEventData> PointerEventsDataDictionary = new Dictionary<int, PointerEventData>();
        public static readonly KeyCode[] MouseKeyCodes = { KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2 };
        public static readonly Dictionary<PointerEventData.InputButton, int> PointerIds = new Dictionary<PointerEventData.InputButton, int>
    {
        {PointerEventData.InputButton.Left, -1},
        {PointerEventData.InputButton.Right, -2},
        {PointerEventData.InputButton.Middle, -3}
    };
        public static PointerEventData MouseDownPointerEventData = null;
        public static PointerEventData.InputButton[] MouseButtons = { PointerEventData.InputButton.Left, PointerEventData.InputButton.Middle, PointerEventData.InputButton.Right };

        public static float LastAxisValue { get; set; }
        public static string LastAxisName { get; set; }
        public static string LastButtonDown { get; set; }
        public static string LastButtonPressed { get; set; }
        public static string LastButtonUp { get; set; }

        public static bool KeyDownFlag;

        public void ResetInput()
        {
            KeyCodesPressed.Clear();
            KeyCodesPressedDown.Clear();
            KeyCodesPressedUp.Clear();
            MousePosition = Vector3.zero;
            Touches = new Touch[0];
            TouchCount = 0;
            Acceleration = Vector3.zero;
            AccelerationEvents = new AccelerationEvent[0];
            PointerEventsDataDictionary.Clear();
            CoroutineManager.Instance.StopAllCoroutines();
        }

        public static AltMockUpPointerInputModule AltMockUpPointerInputModule
        {
            get
            {
                if (MockUpPointerInputModule == null)
                {
                    GameObject eventSystem = EventSystem.current != null ? EventSystem.current.gameObject : new GameObject("EventSystem");
                    MockUpPointerInputModule = eventSystem.AddComponent<AltMockUpPointerInputModule>();
                }
                return MockUpPointerInputModule;
            }
        }


        public void Start()
        {
            Instance = this;
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

            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = MousePosition,
                button = PointerEventData.InputButton.Left,
                eligibleForClick = true
            };
            var eventSystemTarget = findEventSystemObject(pointerEventData);
            pointerEventData.pointerEnter = eventSystemTarget;
            if (EventSystem.current.currentInputModule != null)
#if ENABLE_INPUT_SYSTEM
                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
#endif
                {
                    if (eventSystemTarget != PreviousEventSystemTarget)
                    {
                        if (PreviousEventSystemTarget != null) ExecuteHierarchy(PreviousEventSystemTarget, pointerEventData, pointerExitHandler);
                        if (eventSystemTarget != null && PreviousMousePosition != MousePosition) ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerEnterHandler);
                        PreviousEventSystemTarget = eventSystemTarget;
                    }
                }
            if (PreviousMousePosition != MousePosition)
            {
                if (eventSystemTarget != null) ExecuteHierarchy(PreviousEventSystemTarget, pointerEventData, pointerMoveHandler);
                PreviousMousePosition = MousePosition;
            }

        }
        public static GameObject ExecuteHierarchy<T>(GameObject root, BaseEventData eventData, EventFunction<T> callbackFunction) where T : IEventSystemHandler
        {
            IList<Transform> s_InternalTransformList = new List<Transform>();
            getEventChain(root, s_InternalTransformList);

            var internalTransformListCount = s_InternalTransformList.Count;
            for (var i = 0; i < internalTransformListCount; i++)
            {
                var transform = s_InternalTransformList[i];
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
            var previousPointerEventData = PointerEventsDataDictionary[touch.fingerId];
            var previousPosition = touch.position;
            touch.phase = TouchPhase.Moved;
            touch.position = screenPosition;
            touch.rawPosition = screenPosition;
            touch.deltaPosition = touch.position - previousPosition;
            if (fingerId == 0)
            {
                MousePosition = screenPosition;
            }
            updateTouchInTouchList(touch);
            AltMockUpPointerInputModule.ExecuteTouchEvent(touch, previousPointerEventData);

        }

        public static IEnumerator EndTouch(int fingerId)
        {
            if (Application.isBatchMode)
            {
                yield return null;
            }
            else
                yield return new WaitForEndOfFrame();

            var touch = findTouch(fingerId);
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
            var touch = createTouch(positions[0]);


            var pointerEventData = AltMockUpPointerInputModule.ExecuteTouchEvent(touch);
            if (touch.fingerId == 0)
            {
                MousePosition = new Vector3(Touches[0].position.x, Touches[0].position.y, 0);
                mouseTriggerInit(PointerEventData.InputButton.Left, out PointerEventData _, out GameObject eventSystemTarget, out GameObject monoBehaviourTarget);
                mouseDownTrigger(PointerEventData.InputButton.Left,ref  pointerEventData, eventSystemTarget, monoBehaviourTarget);
                MouseDownPointerEventData = pointerEventData;
            }
            var keyStructure = new KeyStructure(KeyCode.Mouse0, 1.0f);
            if (Application.isBatchMode)
            {
                yield return null;
            }
            else
                yield return new WaitForEndOfFrame();
            KeyCodesPressedDown.Add(keyStructure);
            KeyCodesPressed.Add(keyStructure);

            yield return null;
            KeyCodesPressedDown.Remove(keyStructure);
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
                    Vector2 previousPosition = touch.position;
                    time += Time.unscaledDeltaTime;
                    touch.position = time < oneInputDuration ? touch.position + deltaPerSecond * Time.unscaledDeltaTime : positions[i];

                    touch.phase = touch.deltaPosition != Vector2.zero ? TouchPhase.Moved : TouchPhase.Stationary;
                    touch.deltaPosition = touch.position - previousPosition;
                    updateTouchInTouchList(touch);
                    MousePosition = new Vector3(Touches[0].position.x, Touches[0].position.y, 0);
                    pointerEventData = AltMockUpPointerInputModule.ExecuteTouchEvent(touch, pointerEventData);
                } while (time <= oneInputDuration);
            }

            yield return null;

            touch.phase = TouchPhase.Ended;
            updateTouchInTouchList(touch);
            beginKeyUpTouchEndedLifecycle(keyStructure, true, ref touch);
            AltMockUpPointerInputModule.ExecuteTouchEvent(touch, pointerEventData);
            yield return null;
            endKeyUpTouchEndedLifecycle(keyStructure, true, touch);
        }

        public static void MoveMouse(Vector2 location, float duration, Action<Exception> onFinish)
        {
            Instance.StartCoroutine(runThrowingIterator(MoveMouseCycle(location, duration), onFinish));
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

                var pointerEventData = new PointerEventData(EventSystem.current)
                {
                    position = MousePosition,
                    button = PointerEventData.InputButton.Left,
                    eligibleForClick = true,
                };
                var eventSystemTarget = findEventSystemObject(pointerEventData);
                MouseScrollDelta = new Vector2(scrollHorizontalStep, scrollVerticalStep);
                pointerEventData.scrollDelta = MouseScrollDelta;
#if ENABLE_INPUT_SYSTEM
                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
#endif
                    if (eventSystemTarget != null ? eventSystemTarget : false) ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, scrollHandler);
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
            var newTouches = new Touch[Touches.Length - 1];
            int contor = 0;
            foreach (var t in Touches)
            {
                if (t.fingerId != touch.fingerId)
                {
                    newTouches[contor] = t;
                    contor++;
                }
            }

            Touches = newTouches;
            TouchCount--;
        }

        private static IEnumerator setMouse0KeyCodePressedDown()
        {
            var keyStructure = new KeyStructure(KeyCode.Mouse0, 1.0f);
            if (Application.isBatchMode)
            {
                yield return null;
            }
            else
                yield return new WaitForEndOfFrame();
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
            var pressedKeyStructure = KeyCodesPressed.Find(key => key.KeyCode == keyStructure.KeyCode);
            KeyCodesPressed.Remove(pressedKeyStructure);
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



        internal static IEnumerator tapClickCoordinatesLifeCycle(Vector2 screenPosition, int count, float interval, bool tap)
        {
            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = screenPosition,
                button = PointerEventData.InputButton.Left,
                eligibleForClick = true,
                pressPosition = screenPosition
            };
            var eventSystemTarget = findEventSystemObject(pointerEventData);
            var monoBehaviourTarget = FindObjectViaRayCast.FindMonoBehaviourObject(screenPosition);

            if (Application.isBatchMode)
            {
                yield return null;
            }
            else
                yield return new WaitForEndOfFrame();//run after Update

            MousePosition = screenPosition;
            pointerEventData.pointerEnter = eventSystemTarget;

            for (int i = 0; i < count; i++)
            {
                float time = 0;

                /* pointer/touch down */
                Touch touch = new Touch();
                int pointerId = 0;
                if (tap)
                {
                    touch = createTouch(screenPosition);
                    pointerId = touch.fingerId;
                }
                pointerEventData.pointerId = pointerId;

                var keyStructure = new KeyStructure(KeyCode.Mouse0, 1.0f);//power 1
                KeyCodesPressedDown.Add(keyStructure);
                KeyCodesPressed.Add(keyStructure);
#if ENABLE_INPUT_SYSTEM

                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
                {
#endif
                    if (eventSystemTarget != null)
                    {
                        ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerEnterHandler);
                        ExecuteHierarchy(eventSystemTarget, pointerEventData, initializePotentialDrag);
                        pointerEventData.pointerPress = ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerDownHandler);
                    }
#if ENABLE_INPUT_SYSTEM
                }
#endif
                if (monoBehaviourTarget != null)
                {
                    monoBehaviourTarget.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
                }
                time += Time.unscaledDeltaTime;
                yield return null;

                KeyCodesPressedDown.Remove(keyStructure);
                beginKeyUpTouchEndedLifecycle(keyStructure, tap, ref touch);

#if ENABLE_INPUT_SYSTEM
                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
                {
#endif
                    if (eventSystemTarget != null)
                    {
                        ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerUpHandler);
                        ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerClickHandler);
                    }
#if ENABLE_INPUT_SYSTEM
                }
#endif
                if (monoBehaviourTarget != null)
                {
                    monoBehaviourTarget.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
                    monoBehaviourTarget.SendMessage("OnMouseUpAsButton", SendMessageOptions.DontRequireReceiver);
                }
                time += Time.unscaledDeltaTime;
                yield return null;

                endKeyUpTouchEndedLifecycle(keyStructure, tap, touch);

                if (i != count - 1 && time < interval)//do not wait at last click/tap
                {
                    float elapsedTime = 0;
                    while (elapsedTime < interval - time)
                    {
                        elapsedTime += Time.unscaledDeltaTime;
                        yield return null;
                    }
                }
            }

            // mouse position doesn't change  but we fire on mouse exit
#if ENABLE_INPUT_SYSTEM
            if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
#endif
                if (eventSystemTarget != null) ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerExitHandler);
            if (monoBehaviourTarget != null) monoBehaviourTarget.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
        }

        internal static IEnumerator tapClickElementLifeCycle(GameObject target, int count, float interval, bool tap)
        {
            Vector3 screenPosition;
            FindObjectViaRayCast.FindCameraThatSeesObject(target, out screenPosition);
            if (Application.isBatchMode)
            {
                yield return null;
            }
            else
                yield return new WaitForEndOfFrame();//run after Update

            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = screenPosition,
                button = PointerEventData.InputButton.Left,
                eligibleForClick = true,
                pressPosition = screenPosition,
                pointerCurrentRaycast = new RaycastResult { gameObject = target }
            };
            MousePosition = screenPosition;
            pointerEventData.pointerEnter = target;
            //repeat
            for (int i = 0; i < count; i++)
            {
                float time = 0;

                /* pointer/touch down */
                Touch touch = new Touch();
                int pointerId = 0;
                if (tap)
                {
                    touch = createTouch(screenPosition);
                    pointerId = touch.fingerId;
                }
                pointerEventData.pointerId = pointerId;

                var keyStructure = new KeyStructure(KeyCode.Mouse0, 1.0f);//power 1
                KeyCodesPressedDown.Add(keyStructure);
                KeyCodesPressed.Add(keyStructure);
#if ENABLE_INPUT_SYSTEM
                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
                {
#endif
                    if (target != null)
                    {
                        ExecuteHierarchy(target, pointerEventData, pointerEnterHandler);
                        ExecuteHierarchy(target, pointerEventData, initializePotentialDrag);
                        ExecuteHierarchy(target, pointerEventData, pointerDownHandler);
                    }
#if ENABLE_INPUT_SYSTEM
                }
#endif
                if (target != null) target.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
                pointerEventData.pointerPress = target;

                time += Time.unscaledDeltaTime;
                yield return null;

                KeyCodesPressedDown.Remove(keyStructure);
                beginKeyUpTouchEndedLifecycle(keyStructure, tap, ref touch);

#if ENABLE_INPUT_SYSTEM
                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
                {
#endif
                    if (target != null)
                    {
                        ExecuteHierarchy(target, pointerEventData, pointerUpHandler);
                        ExecuteHierarchy(target, pointerEventData, pointerClickHandler);
                    }
#if ENABLE_INPUT_SYSTEM
                }
#endif

                if (target != null)
                {
                    target.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
                    target.SendMessage("OnMouseUpAsButton", SendMessageOptions.DontRequireReceiver);
                }

                time += Time.unscaledDeltaTime;
                yield return null;

                endKeyUpTouchEndedLifecycle(keyStructure, tap, touch);

                if (i != count - 1 && time < interval)//do not wait at last click/tap
                {
                    float elapsedTime = 0;
                    while (elapsedTime < interval - time)
                    {
                        elapsedTime += Time.unscaledDeltaTime;
                        yield return null;
                    }
                }
            }

            // mouse position doesn't change  but we fire on mouse exit
#if ENABLE_INPUT_SYSTEM
            if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
#endif
                if (target != null)
                {
                    ExecuteHierarchy(target, pointerEventData, pointerExitHandler);
                    target.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
                }
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
            KeyDownFlag = true;
            yield return new WaitForEndOfFrame();
            var keyStructure = new KeyStructure(keyCode, power);
            KeyCodesPressedDown.Add(keyStructure);
            KeyCodesPressed.Add(keyStructure);
            if (MouseKeyCodes.Contains(keyCode))
            {
                var inputButton = keyCodeToInputButton(keyCode);
                mouseTriggerInit(inputButton, out PointerEventData pointerEventData, out GameObject eventSystemTarget, out GameObject monoBehaviourTarget);
                mouseDownTrigger(inputButton,ref pointerEventData, eventSystemTarget, monoBehaviourTarget);
                MouseDownPointerEventData = pointerEventData;
            }
            yield return null;
            KeyCodesPressedDown.Remove(keyStructure);

            KeyDownFlag = false;
        }


        internal static IEnumerator KeyUpLifeCycle(KeyCode keyCode)
        {
            yield return null;
            while (KeyDownFlag)
            {
                yield return null;
            }
            if (MouseKeyCodes.Contains(keyCode))
            {
                var inputButton = keyCodeToInputButton(keyCode);
                mouseTriggerInit(inputButton, out PointerEventData pointerEventData, out GameObject eventSystemTarget, out GameObject monoBehaviourTarget);
                if(MouseDownPointerEventData != null)
                {
                    pointerEventData = MouseDownPointerEventData;
                }
                mouseUpTrigger(inputButton, pointerEventData, eventSystemTarget, monoBehaviourTarget);
            }
            var keyStructure = new KeyStructure(keyCode, 1);
            KeyCodesPressed.Remove(keyStructure);
            KeyCodesPressedUp.Add(keyStructure);
            yield return null;
            KeyCodesPressedUp.Remove(keyStructure);
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
            yield return null;
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
            KeyCodesPressed.Remove(keyStructure);
            KeyCodesPressedUp.Add(keyStructure);
            yield return null;
            KeyCodesPressedUp.Remove(keyStructure);
        }

        private static void mouseTriggerInit(PointerEventData.InputButton mouseButton, out PointerEventData pointerEventData, out GameObject eventSystemTarget, out GameObject monoBehaviourTarget)
        {

            pointerEventData = new PointerEventData(EventSystem.current)
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

        private static void mouseDownTrigger(PointerEventData.InputButton mouseButton,ref PointerEventData pointerEventData, GameObject eventSystemTarget, GameObject monoBehaviourTarget)
        {

            /* pointer/touch down */
            pointerEventData.pointerId = PointerIds[mouseButton];
#if ENABLE_INPUT_SYSTEM
            if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
#endif
                if (eventSystemTarget != null) pointerEventData.pointerPress = ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerDownHandler);

            if (mouseButton == PointerEventData.InputButton.Left && (monoBehaviourTarget != null)) monoBehaviourTarget.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);

            if (MouseButtons.Contains(mouseButton))
            {
#if ENABLE_INPUT_SYSTEM
                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
                {
#endif
                    if (eventSystemTarget != null) pointerEventData.pointerDrag = ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, initializePotentialDrag);
                    EventSystemTargetMouseDown = eventSystemTarget;
#if ENABLE_INPUT_SYSTEM
                }
#endif
                MonoBehaviourTargetMouseDown = monoBehaviourTarget;
            }
        }

        private static void mouseUpTrigger(PointerEventData.InputButton mouseButton, PointerEventData pointerEventData, GameObject eventSystemTarget, GameObject monoBehaviourTarget)
        {
#if ENABLE_INPUT_SYSTEM
            if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
            {
#endif
                if (eventSystemTarget == EventSystemTargetMouseDown && mouseButton == PointerEventData.InputButton.Left)
                {
                    if (eventSystemTarget != null) ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerClickHandler);
                }
                if (EventSystemTargetMouseDown != null) ExecuteEvents.ExecuteHierarchy(EventSystemTargetMouseDown, pointerEventData, pointerUpHandler);
#if ENABLE_INPUT_SYSTEM
            }
#endif

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


        public static KeyCode ConvertStringToKeyCode(string keyName)
        {
            if (keyName.Length == 0)
            {
                return KeyCode.None;
            }
            if (keyName.Length == 1 && Char.IsLetter(keyName[0]))
            {
                return (KeyCode)Enum.Parse(typeof(KeyCode), System.Char.ToUpper(keyName[0]).ToString());
            }
            if (keyName.Equals("left"))
            {
                return KeyCode.LeftArrow;
            }
            if (keyName.Equals("right"))
            {
                return KeyCode.RightArrow;
            }
            if (keyName.Equals("down"))
            {
                return KeyCode.DownArrow;
            }
            if (keyName.Equals("up"))
            {
                return KeyCode.UpArrow;
            }
            if (keyName.Length == 1 && char.IsDigit(keyName[0]))
            {
                return (KeyCode)Enum.Parse(typeof(KeyCode), "Alpha" + keyName);
            }
            if (System.Text.RegularExpressions.Regex.Match(keyName, @"\[[0-9]{1}\]").Success)
            {
                return (KeyCode)Enum.Parse(typeof(KeyCode), "Keypad" + keyName);
            }
            if (keyName == "[+]")
            {
                return KeyCode.KeypadPlus;
            }
            if (keyName == "[equals]")
            {
                return KeyCode.KeypadEquals;
            }
            if (System.Text.RegularExpressions.Regex.Match(keyName, "f[0-9]{1,2}").Success)
            {
                return (KeyCode)Enum.Parse(typeof(KeyCode), keyName.ToUpper());
            }
            if (keyName.Equals("right shift"))
            {
                return KeyCode.RightShift;
            }
            if (keyName.Equals("left shift"))
            {
                return KeyCode.LeftShift;
            }
            if (keyName.Equals("right ctrl"))
            {
                return KeyCode.RightControl;
            }
            if (keyName.Equals("left ctrl"))
            {
                return KeyCode.LeftControl;
            }
            if (keyName.Equals("right alt"))
            {
                return KeyCode.RightAlt;
            }
            if (keyName.Equals("left alt"))
            {
                return KeyCode.LeftAlt;
            }
            if (keyName.Equals("right cmd"))
            {
                return KeyCode.RightCommand;
            }
            if (keyName.Equals("left cmd"))
            {
                return KeyCode.LeftCommand;
            }
            if (System.Text.RegularExpressions.Regex.Match(keyName, @"mouse [0-6]").Success)
            {
                return (KeyCode)Enum.Parse(typeof(KeyCode), "Mouse" + keyName[6]);
            }
            if (keyName.Equals("backspace"))
            {
                return KeyCode.Backspace;
            }
            if (keyName.Equals("tab"))
            {
                return KeyCode.Tab;
            }
            if (keyName.Equals("return"))
            {
                return KeyCode.Return;
            }
            if (keyName.Equals("escape"))
            {
                return KeyCode.Escape;
            }
            if (keyName.Equals("space"))
            {
                return KeyCode.Space;
            }
            if (keyName.Equals("delete"))
            {
                return KeyCode.Delete;
            }
            if (keyName.Equals("enter"))
            {
                return KeyCode.KeypadEnter;
            }
            if (keyName.Equals("insert"))
            {
                return KeyCode.Insert;
            }
            if (keyName.Equals("home"))
            {
                return KeyCode.Home;
            }
            if (keyName.Equals("end"))
            {
                return KeyCode.End;
            }
            if (keyName.Equals("page up"))
            {
                return KeyCode.PageUp;
            }
            if (keyName.Equals("page down"))
            {
                return KeyCode.PageDown;
            }
            if (System.Text.RegularExpressions.Regex.Match(keyName, "joystick button [0-9]{1,2}").Success)
            {
                var splitedString = keyName.Split(' ');
                var number = System.Int32.Parse(splitedString[2]);
                if (number >= 20)
                {
                    throw new NotFoundException("Key not recognized");
                }
                return (KeyCode)Enum.Parse(typeof(KeyCode), "JoystickButton" + number);
            }
            if (System.Text.RegularExpressions.Regex.Match(keyName, "joystick [1-8] button [0-9]{1,2}").Success)
            {
                var splitedString = keyName.Split(' ');
                var number = System.Int32.Parse(splitedString[3]);
                if (number >= 20)
                {
                    throw new NotFoundException("Key not recognized");
                }
                return (KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + splitedString[1] + "Button" + number);
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
        public KeyCode KeyCode { get; set; }
        public float Power { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is KeyStructure))
                return false;
            var other = (KeyStructure)obj;
            return other.KeyCode == this.KeyCode;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
