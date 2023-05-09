namespace AltTester.AltTesterUnitySDK.Driver
{
    public enum InputType
    {
        KeyOrMouseButton,
        MouseMovement,
        JoystickAxis,
    };
    [System.Serializable]
    public class AltAxis
    {
        public string name;
        public string negativeButton;
        public string positiveButton;
        public string altPositiveButton;
        public string altNegativeButton;
        public InputType type;
        public int axisDirection;

        public AltAxis(string name, InputType type, string negativeButton, string positiveButton, string altPositiveButton, string altNegativeButton, int axisDirection)
        {
            this.name = name;
            this.type = type;
            this.negativeButton = negativeButton;
            this.positiveButton = positiveButton;
            this.altPositiveButton = altPositiveButton;
            this.altNegativeButton = altNegativeButton;
            this.axisDirection = axisDirection;
        }
    }
}
