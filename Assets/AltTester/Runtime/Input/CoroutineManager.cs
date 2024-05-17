using UnityEngine;
using System.Collections;
namespace AltTester.AltTesterUnitySDK.InputModule
{

    public class CoroutineManager : MonoBehaviour
    {
        private static CoroutineManager instance;

        public static CoroutineManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("CoroutineManager").AddComponent<CoroutineManager>();
                    DontDestroyOnLoad(instance.gameObject);
                }
                return instance;
            }
        }

        public void StartCoroutineFromExternal(IEnumerator routine)
        {
            StartCoroutine(routine);
        }
    }
}