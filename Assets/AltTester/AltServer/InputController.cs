using System;
using System.Collections;
using System.Collections.Generic;
using Altom.AltDriver;
using AltTester.AltDriver;
using UnityEngine;


namespace AltTester
{
    public static class InputController
    {

        private static IEnumerator runThrowingIterator(
           List<IEnumerator> enumerators,
           Action<Exception> done)
        {
            Exception err = null;

            var CoroutineList = new List<Coroutine>();

            try
            {
                for (int i = 0; i < enumerators.Count; i++)
                {
                    CoroutineList.Add(AltRunner._altRunner.StartCoroutine(enumerators[i]));
                }
            }
            catch (Exception e)
            {
                err = e;
            }
            for (int i = 0; i < enumerators.Count; i++)
            {
                yield return CoroutineList[i];
            }

            done.Invoke(err);
        }

        public static void Scroll(float speedVertical, float speedHorizontal, float duration, Action<Exception> onFinish)
        {
#if ALTTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.ScrollLifeCycle(speedVertical, speedHorizontal, duration));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.ScrollLifeCycle(speedVertical, speedHorizontal, duration));
#endif
            AltRunner._altRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }

        public static void MoveMouse(UnityEngine.Vector2 location, float duration, Action<Exception> onFinish)
        {
#if ALTTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.MoveMouseCycle(location, duration));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.MoveMouseCycle(location, duration));
#endif
            AltRunner._altRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }
        public static void TapElement(UnityEngine.GameObject target, int count, float interval, Action<Exception> onFinish)
        {
#if ALTTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.TapElementCycle(target, count, interval));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.tapClickElementLifeCycle(target, count, interval, true));
#endif
            AltRunner._altRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }

        public static void TapCoordinates(UnityEngine.Vector2 coordinates, int count, float interval, Action<Exception> onFinish)
        {
#if ALTTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.TapCoordinatesCycle(coordinates, count, interval));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.tapClickCoordinatesLifeCycle(coordinates, count, interval, true));
#endif
            AltRunner._altRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }

        public static void ClickElement(UnityEngine.GameObject target, int count, float interval, Action<Exception> onFinish)
        {
#if ALTTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.ClickElementLifeCycle(target, count, interval));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.tapClickElementLifeCycle(target, count, interval, false));
#endif
            AltRunner._altRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }

        public static void ClickCoordinates(UnityEngine.Vector2 screenPosition, int count, float interval, Action<Exception> onFinish)
        {
#if ALTTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.ClickCoordinatesLifeCycle(screenPosition, count, interval));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.tapClickCoordinatesLifeCycle(screenPosition, count, interval, false));
#endif
            AltRunner._altRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }
        public static void Tilt(Vector3 accelerationValue, float duration, Action<Exception> onFinish)
        {
#if ALTTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.AccelerationLifeCycle(accelerationValue, duration));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.AccelerationLifeCycle(accelerationValue, duration));
#endif
            AltRunner._altRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }

        public static void KeyDown(KeyCode keyCode, float power)
        {
#if ALTTESTER
#if ENABLE_INPUT_SYSTEM
            NewInputSystem.KeyDown(keyCode, power);
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            AltRunner._altRunner.StartCoroutine(Input.KeyDownLifeCycle(keyCode, power));
#endif
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }

        public static void KeyUp(KeyCode keyCode)
        {
#if ALTTESTER
#if ENABLE_INPUT_SYSTEM
            NewInputSystem.KeyUp(keyCode);
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            AltRunner._altRunner.StartCoroutine(Input.KeyUpLifeCycle(keyCode));
#endif
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }

        public static void PressKey(KeyCode keyCode, float power, float duration, Action<Exception> onFinish)
        {
#if ALTTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.KeyPressLifeCycle(keyCode, power, duration));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.KeyPressLifeCycle(keyCode, power, duration));
#endif
            AltRunner._altRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }

        public static void SetMultipointSwipe(UnityEngine.Vector2[] positions, float duration, Action<Exception> onFinish)
        {
#if ALTTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.MultipointSwipeLifeCycle(positions, duration));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.MultipointSwipeLifeCycle(positions, duration));
#endif
            AltRunner._altRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }

        public static int BeginTouch(Vector3 screenPosition)
        {
#if ALTTESTER
            int newFingerId = 0, oldFingerId = -1;
#if ENABLE_INPUT_SYSTEM
            newFingerId = NewInputSystem.BeginTouch(screenPosition);
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            oldFingerId = Input.BeginTouch(screenPosition);
#endif
            if (newFingerId == 0)
                return oldFingerId + 1;
            if (oldFingerId == -1)
                return newFingerId;
            if (newFingerId - 1 == oldFingerId)
                return newFingerId;
            throw new Exception("FingerIds are not identical! OldInput fingerId: " + oldFingerId + " New Input fingerId: " + newFingerId);
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }

        public static void MoveTouch(int fingerId, Vector3 screenPosition)
        {
#if ALTTESTER
#if ENABLE_INPUT_SYSTEM
            NewInputSystem.MoveTouch(fingerId, screenPosition);
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            Input.MoveTouch(fingerId - 1, screenPosition);
#endif
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }

        public static void EndTouch(int fingerId, Action<Exception> onFinish)
        {
#if ALTTESTER
            List<IEnumerator> coroutines = new List<IEnumerator>();
#if ENABLE_INPUT_SYSTEM
            coroutines.Add(NewInputSystem.EndTouch(fingerId));
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            coroutines.Add(Input.EndTouch(fingerId - 1));
#endif
            AltRunner._altRunner.StartCoroutine(runThrowingIterator(coroutines, onFinish));
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif

        }
        public static void ResetInput()
        {
#if ALTTESTER
#if ENABLE_INPUT_SYSTEM
        NewInputSystem.Instance.ResetInput();
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
        Input._instance.ResetInput();
#endif
#endif

        }
    }
}