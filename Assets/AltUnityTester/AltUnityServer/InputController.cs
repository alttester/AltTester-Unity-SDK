using System;
using System.Collections;
using System.Collections.Generic;
using Altom.AltUnityDriver;
using UnityEngine;

namespace Altom.AltUnityTester
{
    public class InputController
    {
        protected static IEnumerator runThrowingIterator(
               List<IEnumerator> enumerators,
               Action<Exception> done)
        {
            Exception err = null;
            while (true)
            {
                object current;
                try
                {
                    bool isDone = true;
                    for (int i = 0; i < enumerators.Count; i++)
                    {
                        if (enumerators[i].MoveNext() != false)
                        {
                            current = enumerators[i];
                            isDone = false;
                            break;
                        }
                    }
                    if (isDone)
                        break;

                    current = enumerators[0];

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

        public static void Scroll(float speedVertical, float speedHorizontal, float duration, Action<Exception> onFinish)
        {
#if ALTUNITYTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.ScrollLifeCycle(speedVertical, speedHorizontal, duration));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.ScrollLifeCycle(speedVertical, speedHorizontal, duration));
#endif
            AltUnityRunner._altUnityRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }

        public static void MoveMouse(UnityEngine.Vector2 location, float duration, Action<Exception> onFinish)
        {
#if ALTUNITYTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.MoveMouseCycle(location, duration));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.MoveMouseCycle(location, duration));
#endif
            AltUnityRunner._altUnityRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
        public static void TapElement(UnityEngine.GameObject target, int count, float interval, Action<Exception> onFinish)
        {
#if ALTUNITYTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.TapElementCycle(target, count, interval));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.tapClickElementLifeCycle(target, count, interval, true));
#endif
            AltUnityRunner._altUnityRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
        throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }

        public static void TapCoordinates(UnityEngine.Vector2 coordinates, int count, float interval, Action<Exception> onFinish)
        {
#if ALTUNITYTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.TapCoordinatesCycle(coordinates, count, interval));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.tapClickCoordinatesLifeCycle(coordinates, count, interval, true));
#endif
            AltUnityRunner._altUnityRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
        throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }

        public static void ClickElement(UnityEngine.GameObject target, int count, float interval, Action<Exception> onFinish)
        {
#if ALTUNITYTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.ClickElementLifeCycle(target, count, interval));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.tapClickElementLifeCycle(target, count, interval, false));
#endif
            AltUnityRunner._altUnityRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }

        public static void ClickCoordinates(UnityEngine.Vector2 screenPosition, int count, float interval, Action<Exception> onFinish)
        {
#if ALTUNITYTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.ClickCoordinatesLifeCycle(screenPosition, count, interval));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.tapClickCoordinatesLifeCycle(screenPosition, count, interval, false));
#endif
            AltUnityRunner._altUnityRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
        public static void Tilt(Vector3 accelerationValue, float duration, Action<Exception> onFinish)
        {
#if ALTUNITYTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.AccelerationLifeCycle(accelerationValue, duration));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.AccelerationLifeCycle(accelerationValue, duration));
#endif
            AltUnityRunner._altUnityRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif  
        }

    }
}