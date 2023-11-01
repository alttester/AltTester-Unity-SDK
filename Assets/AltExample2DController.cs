using UnityEngine;

public class AltExample2DController : MonoBehaviour
{
    private TMPro.TMP_Text text;
    void Awake() => text = GameObject.Find("Text").GetComponent<TMPro.TMP_Text>();
    public void OnMouseDown()
    {
        text.text = $"Clicked on {gameObject.name}";

    }

}
