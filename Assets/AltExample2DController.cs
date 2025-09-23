using UnityEngine;
using UnityEngine.UI;
public class AltExample2DController : MonoBehaviour
{

    public Text text;
    void Awake() => text = GameObject.Find("Text").GetComponent<Text>();
    public void OnMouseDown()
    {
        text.text = $"Clicked on {gameObject.name}";
        if (gameObject.name.Equals("Square"))
        {
            Destroy(gameObject);
        }
    }

}
