using UnityEngine;

namespace AltTester
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