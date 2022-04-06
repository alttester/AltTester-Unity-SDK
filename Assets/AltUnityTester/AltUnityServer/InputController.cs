using System;
using System.Collections;
using System.Collections.Generic;
using Altom.AltUnityDriver;
using Altom.AltUnityTester;
using UnityEngine;

namespace Altom.AltUnityTester
{
    public static class InputController
    {
        private static IEnumerator runThrowingIterator(
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

        public static void Scroll(float scrollValue, float duration, Action<Exception> onFinish)
        {
#if ALTUNITYTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.ScrollLifeCycle(scrollValue, duration));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.ScrollLifeCycle(scrollValue, duration));
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
#if ENABLE_LEGACY_INPUT_MANAGER
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


        public static void KeyDown(KeyCode keyCode, float power)
        {
#if ALTUNITYTESTER
#if ENABLE_INPUT_SYSTEM
            NewInputSystem.KeyDown(keyCode, power);
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            AltUnityRunner._altUnityRunner.StartCoroutine(Input.KeyDownLifeCycle(keyCode, power));
#endif
#else
        throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }

        public static void KeyUp(KeyCode keyCode)
        {
#if ALTUNITYTESTER
#if ENABLE_INPUT_SYSTEM
            NewInputSystem.KeyUp(keyCode);
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            AltUnityRunner._altUnityRunner.StartCoroutine(Input.KeyUpLifeCycle(keyCode));
#endif
#else
        throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }

        public static void PressKey(KeyCode keyCode, float power, float duration, Action<Exception> onFinish)
        {
#if ALTUNITYTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.KeyPressLifeCycle(keyCode, power, duration));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.KeyPressLifeCycle(keyCode, power, duration));
#endif
            AltUnityRunner._altUnityRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
        throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }

    }
}
