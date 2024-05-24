/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

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
