[System.Serializable]
public class AltUnityAxis
{
    public string name;
    public string negativeButton;
    public string positiveButton;
    public string altPositiveButton;
    public string altNegativeButton;

    public AltUnityAxis(string name, string negativeButton, string positiveButton, string altPositiveButton, string altNegativeButton)
    {
        this.name = name;
        this.negativeButton = negativeButton;
        this.positiveButton = positiveButton;
        this.altPositiveButton = altPositiveButton;
        this.altNegativeButton = altNegativeButton;
    }


}