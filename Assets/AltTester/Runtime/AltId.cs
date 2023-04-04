using UnityEngine;

namespace AltTester.AltTesterUnitySDK
{
    [DisallowMultipleComponent]
    public class AltId : MonoBehaviour
    {

        public string altID;
        protected void OnValidate()
        {
            if (altID == null)
                altID = System.Guid.NewGuid().ToString();
        }
    }
}