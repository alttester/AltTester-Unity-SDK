using UnityEngine;

namespace Altom.AltUnityTester
{
    [DisallowMultipleComponent]
    public class AltUnityId : MonoBehaviour
    {

        public string altID;
        protected void OnValidate()
        {
            if (altID == null)
                altID = System.Guid.NewGuid().ToString();
        }
    }
}