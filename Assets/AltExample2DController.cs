#if TMP_PRESENT

using TMPro;
#endif
using UnityEngine;
public class AltExample2DController : MonoBehaviour
{
#if TMP_PRESENT

    public TMPro.TMP_Text text;
    void Awake() => text = GameObject.Find("Text").GetComponent<TMPro.TMP_Text>();
#endif
    public void OnMouseDown()
    {
#if TMP_PRESENT
        text.text = $"Clicked on {gameObject.name}";
        if (gameObject.name.Equals("Square"))
        {
            Destroy(gameObject);
        }
#endif
    }

}
