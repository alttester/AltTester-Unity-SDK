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
        public static UnityEngine.Vector3 Acceleration;
        public static UnityEngine.AccelerationEvent[] AccelerationEvents;
        public static int TouchCount;
        public static UnityEngine.Touch[] Touches = new UnityEngine.Touch[0];
        public static UnityEngine.Vector2 MouseScrollDelta = new UnityEngine.Vector2();
        public static UnityEngine.Vector3 MousePosition = new UnityEngine.Vector3();
        public static UnityEngine.Vector3 MouseDelta = new Vector3();
        public static System.Collections.Generic.List<AltAxis> AxisList;
        public static GameObject EventSystemTargetMouseDown;
        public static GameObject MonoBehaviourTargetMouseDown;
        public static UnityEngine.Vector3 PreviousMousePosition = new UnityEngine.Vector3();
        public static UnityEngine.GameObject MonoBehaviourPreviousTarget = null;
        public static UnityEngine.GameObject PreviousEventSystemTarget = null;

        public static AltMockUpPointerInputModule MockUpPointerInputModule;
        public static AltInput Instance;
        public static System.Collections.Generic.List<KeyStructure> KeyCodesPressed = new System.Collections.Generic.List<KeyStructure>();
        public static System.Collections.Generic.List<KeyStructure> KeyCodesPressedDown = new System.Collections.Generic.List<KeyStructure>();
        public static System.Collections.Generic.List<KeyStructure> KeyCodesPressedUp = new System.Collections.Generic.List<KeyStructure>();
        public static System.Collections.Generic.Dictionary<int, PointerEventData> PointerEventsDataDictionary = new System.Collections.Generic.Dictionary<int, PointerEventData>();
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
            Touches = new UnityEngine.Touch[0];
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
            string filePath = "AltTester/AltTesterInputAxisData";

            UnityEngine.TextAsset targetFile = Resources.Load<UnityEngine.TextAsset>(filePath);
            string dataAsJson = targetFile.text;
            AxisList = JsonConvert.DeserializeObject<System.Collections.Generic.List<AltAxis>>(dataAsJson, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                Culture = CultureInfo.InvariantCulture,
                Formatting = Formatting.Indented
            });
        }

        private void FixedUpdate()
        {
            var monoBehaviourTarget = FindObjectViaRayCast.GetGameObjectHitMonoBehaviour(MousePosition);
            if (MonoBehaviourPreviousTarget != monoBehaviourTarget)
            {
                if (MonoBehaviourPreviousTarget ?? false) MonoBehaviourPreviousTarget.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
                if (monoBehaviourTarget ?? false && PreviousMousePosition != MousePosition) monoBehaviourTarget.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
                MonoBehaviourPreviousTarget = monoBehaviourTarget;
            }
            if (monoBehaviourTarget ?? false) monoBehaviourTarget.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);

            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(EventSystem.current)
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
                        if (PreviousEventSystemTarget ?? false) ExecuteHierarchy(PreviousEventSystemTarget, pointerEventData, pointerExitHandler);
                        if (eventSystemTarget ?? false && PreviousMousePosition != MousePosition) ExecuteHierarchy(eventSystemTarget, pointerEventData, pointerEnterHandler);
                        PreviousEventSystemTarget = eventSystemTarget;
                    }
                }
            if (PreviousMousePosition != MousePosition)
            {
                if (eventSystemTarget ?? false) ExecuteHierarchy(PreviousEventSystemTarget, pointerEventData, pointerMoveHandler);
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
                    if ((transform ?? false) && (transform.gameObject ?? false))
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
        private static UnityEngine.GameObject findEventSystemObject(UnityEngine.EventSystems.PointerEventData pointerEventData)
        {
            UnityEngine.EventSystems.RaycastResult firstRaycastResult;
            AltMockUpPointerInputModule.GetFirstRaycastResult(pointerEventData, out firstRaycastResult);
            pointerEventData.pointerCurrentRaycast = firstRaycastResult;
            pointerEventData.pointerPressRaycast = firstRaycastResult;
            return firstRaycastResult.gameObject;
        }

        #region public commands interface
        public static int BeginTouch(UnityEngine.Vector3 screenPosition)
        {
            var touch = createTouch(screenPosition);

            var pointerEventData = AltMockUpPointerInputModule.ExecuteTouchEvent(touch);
            if (touch.fingerId == 0)
            {
                MousePosition = screenPosition;
                mouseTriggerInit(PointerEventData.InputButton.Left, out PointerEventData _, out GameObject eventSystemTarget, out GameObject monoBehaviourTarget);
                mouseDownTrigger(PointerEventData.InputButton.Left, pointerEventData, eventSystemTarget, monoBehaviourTarget);
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
                yield return new UnityEngine.WaitForEndOfFrame();

            var touch = findTouch(fingerId);
            var previousPointerEventData = PointerEventsDataDictionary[touch.fingerId];
            PointerEventsDataDictionary.Remove(touch.fingerId);

            var keyStructure = new KeyStructure(KeyCode.Mouse0, 1);
            beginKeyUpTouchEndedLifecycle(keyStructure, true, ref touch);
            AltMockUpPointerInputModule.ExecuteTouchEvent(touch, previousPointerEventData);

            yield return null;
            endKeyUpTouchEndedLifecycle(keyStructure, true, touch);
        }



        public static System.Collections.IEnumerator MultipointSwipeLifeCycle(UnityEngine.Vector2[] positions, float duration)
        {
            var touch = createTouch(positions[0]);


            var pointerEventData = AltMockUpPointerInputModule.ExecuteTouchEvent(touch);
            if (touch.fingerId == 0)
            {
                MousePosition = new UnityEngine.Vector3(Touches[0].position.x, Touches[0].position.y, 0);
                mouseTriggerInit(PointerEventData.InputButton.Left, out PointerEventData _, out GameObject eventSystemTarget, out GameObject monoBehaviourTarget);
                mouseDownTrigger(PointerEventData.InputButton.Left, pointerEventData, eventSystemTarget, monoBehaviourTarget);
                MouseDownPointerEventData = pointerEventData;
            }
            var keyStructure = new KeyStructure(KeyCode.Mouse0, 1.0f);
            if (Application.isBatchMode)
            {
                yield return null;
            }
            else
                yield return new UnityEngine.WaitForEndOfFrame();
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

            touch.phase = UnityEngine.TouchPhase.Ended;
            updateTouchInTouchList(touch);
            beginKeyUpTouchEndedLifecycle(keyStructure, true, ref touch);
            AltMockUpPointerInputModule.ExecuteTouchEvent(touch, pointerEventData);
            yield return null;
            endKeyUpTouchEndedLifecycle(keyStructure, true, touch);
        }

        public static void MoveMouse(UnityEngine.Vector2 location, float duration, Action<Exception> onFinish)
        {
            Instance.StartCoroutine(runThrowingIterator(MoveMouseCycle(location, duration), onFinish));
        }

        public static System.Collections.IEnumerator MoveMouseCycle(UnityEngine.Vector2 location, float duration)
        {
            float time = 0;
            var distance = location - new UnityEngine.Vector2(MousePosition.x, MousePosition.y);

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

        internal static System.Collections.IEnumerator ScrollLifeCycle(float scrollVertical, float scrollHorizontal, float duration)
        {
            float timeSpent = 0;

            while (timeSpent < duration)
            {
                yield return null;
                timeSpent += UnityEngine.Time.unscaledDeltaTime;
                float scrollVerticalStep = scrollVertical * UnityEngine.Time.unscaledDeltaTime / duration;
                float scrollHorizontalStep = scrollHorizontal * UnityEngine.Time.unscaledDeltaTime / duration;

                var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current)
                {
                    position = MousePosition,
                    button = UnityEngine.EventSystems.PointerEventData.InputButton.Left,
                    eligibleForClick = true,
                };
                var eventSystemTarget = findEventSystemObject(pointerEventData);
                MouseScrollDelta = new UnityEngine.Vector2(scrollHorizontalStep, scrollVerticalStep);
                pointerEventData.scrollDelta = MouseScrollDelta;
#if ENABLE_INPUT_SYSTEM
                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
#endif
                    if (eventSystemTarget != null ? eventSystemTarget : false) UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.scrollHandler);
            }
            MouseScrollDelta = UnityEngine.Vector2.zero;//reset the value after scroll ended
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
                    UnityEngine.Debug.LogError(ex.ToString());
                    err = ex;
                    yield break;
                }
                yield return current;
            }
            done.Invoke(err);
        }
        #endregion

        #region private interface
        private static UnityEngine.Touch createTouch(UnityEngine.Vector3 screenPosition)
        {
            var touch = new UnityEngine.Touch
            {
                phase = UnityEngine.TouchPhase.Began,
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
            var touchListCopy = new UnityEngine.Touch[Touches.Length + 1];
            System.Array.Copy(Touches, 0, touchListCopy, 0, Touches.Length);
            touchListCopy[Touches.Length] = touch;
            Touches = touchListCopy;
            return touch;
        }

        private static void destroyTouch(UnityEngine.Touch touch)
        {
            var newTouches = new UnityEngine.Touch[Touches.Length - 1];
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
                yield return new UnityEngine.WaitForEndOfFrame();
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



        internal static IEnumerator tapClickCoordinatesLifeCycle(UnityEngine.Vector2 screenPosition, int count, float interval, bool tap)
        {
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current)
            {
                position = screenPosition,
                button = UnityEngine.EventSystems.PointerEventData.InputButton.Left,
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
                yield return new UnityEngine.WaitForEndOfFrame();//run after Update

            MousePosition = screenPosition;
            pointerEventData.pointerEnter = eventSystemTarget;

            for (int i = 0; i < count; i++)
            {
                float time = 0;

                /* pointer/touch down */
                UnityEngine.Touch touch = new UnityEngine.Touch();
                int pointerId = 0;
                if (tap)
                {
                    touch = createTouch(screenPosition);
                    pointerId = touch.fingerId;
                }
                pointerEventData.pointerId = pointerId;

                var keyStructure = new KeyStructure(UnityEngine.KeyCode.Mouse0, 1.0f);//power 1
                KeyCodesPressedDown.Add(keyStructure);
                KeyCodesPressed.Add(keyStructure);
#if ENABLE_INPUT_SYSTEM

                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
                {
#endif
                    if (eventSystemTarget ?? false)
                        ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.initializePotentialDrag);
                    if (eventSystemTarget ?? false)
                        pointerEventData.pointerPress = ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);
#if ENABLE_INPUT_SYSTEM
                }
#endif
                if (monoBehaviourTarget ?? false)
                {
                    monoBehaviourTarget.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);
                }
                time += UnityEngine.Time.unscaledDeltaTime;
                yield return null;

                KeyCodesPressedDown.Remove(keyStructure);
                beginKeyUpTouchEndedLifecycle(keyStructure, tap, ref touch);

#if ENABLE_INPUT_SYSTEM
                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
                {
#endif
                    if (eventSystemTarget ?? false)
                        ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
                    if (eventSystemTarget ?? false)
                        ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
#if ENABLE_INPUT_SYSTEM
                }
#endif
                if (monoBehaviourTarget ?? false)
                    monoBehaviourTarget.SendMessage("OnMouseUp", UnityEngine.SendMessageOptions.DontRequireReceiver);
                if (monoBehaviourTarget ?? false)
                    monoBehaviourTarget.SendMessage("OnMouseUpAsButton", UnityEngine.SendMessageOptions.DontRequireReceiver);

                time += UnityEngine.Time.unscaledDeltaTime;
                yield return null;

                endKeyUpTouchEndedLifecycle(keyStructure, tap, touch);

                if (i != count - 1 && time < interval)//do not wait at last click/tap
                {
                    float elapsedTime = 0;
                    while (elapsedTime < interval - time)
                    {
                        elapsedTime += UnityEngine.Time.unscaledDeltaTime;
                        yield return null;
                    }
                }
            }

            // mouse position doesn't change  but we fire on mouse exit
#if ENABLE_INPUT_SYSTEM
            if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
#endif
                if (eventSystemTarget ?? false) ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
            if (monoBehaviourTarget ?? false) monoBehaviourTarget.SendMessage("OnMouseExit", UnityEngine.SendMessageOptions.DontRequireReceiver);
        }

        internal static IEnumerator tapClickElementLifeCycle(UnityEngine.GameObject target, int count, float interval, bool tap)
        {
            UnityEngine.Vector3 screenPosition;
            FindObjectViaRayCast.FindCameraThatSeesObject(target, out screenPosition);
            if (Application.isBatchMode)
            {
                yield return null;
            }
            else
                yield return new UnityEngine.WaitForEndOfFrame();//run after Update

            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current)
            {
                position = screenPosition,
                button = UnityEngine.EventSystems.PointerEventData.InputButton.Left,
                eligibleForClick = true,
                pressPosition = screenPosition
            };
            MousePosition = screenPosition;
            pointerEventData.pointerEnter = target;
            //repeat
            for (int i = 0; i < count; i++)
            {
                float time = 0;

                /* pointer/touch down */
                UnityEngine.Touch touch = new UnityEngine.Touch();
                int pointerId = 0;
                if (tap)
                {
                    touch = createTouch(screenPosition);
                    pointerId = touch.fingerId;
                }
                pointerEventData.pointerId = pointerId;

                var keyStructure = new KeyStructure(UnityEngine.KeyCode.Mouse0, 1.0f);//power 1
                KeyCodesPressedDown.Add(keyStructure);
                KeyCodesPressed.Add(keyStructure);
#if ENABLE_INPUT_SYSTEM
                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
                {
#endif
                    if (target ?? false) ExecuteHierarchy(target, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.initializePotentialDrag);
                    if (target ?? false) ExecuteHierarchy(target, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);
#if ENABLE_INPUT_SYSTEM
                }
#endif
                if (target ?? false) target.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);
                pointerEventData.pointerPress = target;

                time += UnityEngine.Time.unscaledDeltaTime;
                yield return null;

                KeyCodesPressedDown.Remove(keyStructure);
                beginKeyUpTouchEndedLifecycle(keyStructure, tap, ref touch);

#if ENABLE_INPUT_SYSTEM
                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
                {
#endif
                    if (target ?? false) ExecuteHierarchy(target, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
                    if (target ?? false) ExecuteHierarchy(target, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
#if ENABLE_INPUT_SYSTEM
                }
#endif

                if (target ?? false) target.SendMessage("OnMouseUp", UnityEngine.SendMessageOptions.DontRequireReceiver);
                if (target ?? false) target.SendMessage("OnMouseUpAsButton", UnityEngine.SendMessageOptions.DontRequireReceiver);

                time += UnityEngine.Time.unscaledDeltaTime;
                yield return null;

                endKeyUpTouchEndedLifecycle(keyStructure, tap, touch);

                if (i != count - 1 && time < interval)//do not wait at last click/tap
                {
                    float elapsedTime = 0;
                    while (elapsedTime < interval - time)
                    {
                        elapsedTime += UnityEngine.Time.unscaledDeltaTime;
                        yield return null;
                    }
                }
            }

            // mouse position doesn't change  but we fire on mouse exit
#if ENABLE_INPUT_SYSTEM
            if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
#endif
                if (target ?? false) ExecuteHierarchy(target, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
            if (target ?? false) target.SendMessage("OnMouseExit", UnityEngine.SendMessageOptions.DontRequireReceiver);
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
                mouseDownTrigger(inputButton, pointerEventData, eventSystemTarget, monoBehaviourTarget);
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
                        elapsedTime += UnityEngine.Time.unscaledDeltaTime;
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
                pressPosition = MousePosition
            };
            eventSystemTarget = findEventSystemObject(pointerEventData);
            monoBehaviourTarget = FindObjectViaRayCast.FindMonoBehaviourObject(MousePosition);

        }

        private static void mouseDownTrigger(PointerEventData.InputButton mouseButton, PointerEventData pointerEventData, GameObject eventSystemTarget, GameObject monoBehaviourTarget)
        {

            /* pointer/touch down */
            pointerEventData.pointerId = PointerIds[mouseButton];
#if ENABLE_INPUT_SYSTEM
            if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
#endif
                if (eventSystemTarget ?? false) pointerEventData.pointerPress = ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);


            if (mouseButton == PointerEventData.InputButton.Left && (monoBehaviourTarget ?? false)) monoBehaviourTarget.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);

            if (MouseButtons.Contains(mouseButton))
            {
#if ENABLE_INPUT_SYSTEM
                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
                {
#endif
                    if (eventSystemTarget ?? false) pointerEventData.pointerDrag = ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, ExecuteEvents.initializePotentialDrag);
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
                    if (eventSystemTarget ?? false) ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, ExecuteEvents.pointerClickHandler);
                }
                if (EventSystemTargetMouseDown ?? false) ExecuteEvents.ExecuteHierarchy(EventSystemTargetMouseDown, pointerEventData, ExecuteEvents.pointerUpHandler);
#if ENABLE_INPUT_SYSTEM
            }
#endif

            if (mouseButton == PointerEventData.InputButton.Left && (monoBehaviourTarget ?? false)) monoBehaviourTarget.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
            /* pointer/touch up */
            if (monoBehaviourTarget == MonoBehaviourTargetMouseDown && mouseButton == PointerEventData.InputButton.Left && (monoBehaviourTarget ?? false))
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
            mouseDownTrigger(mouseButton, pointerEventData, eventSystemTarget, monoBehaviourTarget);
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                elapsedTime += UnityEngine.Time.unscaledDeltaTime;
                yield return null;
            }

            mouseUpTrigger(mouseButton, pointerEventData, eventSystemTarget, monoBehaviourTarget);
        }

        public static System.Collections.IEnumerator AccelerationLifeCycle(UnityEngine.Vector3 accelarationValue, float duration)
        {
            float timeSpent = 0;
            while (timeSpent < duration)
            {
                Acceleration = accelarationValue;
                timeSpent += UnityEngine.Time.unscaledDeltaTime;
                yield return null;
            }
            Acceleration = UnityEngine.Vector3.zero;//reset the value after acceleration ended
        }


        public static UnityEngine.KeyCode ConvertStringToKeyCode(string keyName)
        {
            if (keyName.Length == 0)
            {
                return KeyCode.None;
            }
            if (keyName.Length == 1 && Char.IsLetter(keyName[0]))
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
            if (keyName.Length == 1 && char.IsDigit(keyName[0]))
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
        #endregion
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
