using AltTester.AltTesterSDK.Driver.Commands;

namespace AltTester.AltTesterSDK.Driver
{
    public class AltXRCommands
    {
        private IDriverCommunication communicationHandler;

        public AltXRCommands(IDriverCommunication communicationHandler)
        {
            this.communicationHandler = communicationHandler;
        }

        /// <summary>
        /// Simulates pressing multiple XR controller buttons.
        /// </summary>
        /// <param name="buttons">The buttons to press.</param>
        /// <param name="power">A value between [0,1] used to indicate how hard the button was pressed. Defaults to <c>1</c>.</param>
        /// <param name="duration">The time measured in seconds from the key press to the key release. Defaults to <c>0.1</c></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        /// 
        public void PressButtons(AltXRControllerButton[] buttons, float power = 1, float duration = 0.1f, bool wait = true)
        {
            new AltPressXRButtons(communicationHandler, buttons, power, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulates pressing a single XR controller button.
        /// </summary>
        /// <param name="button">The button to press.</param>
        /// <param name="power">A value between [0,1] used to indicate how hard the button was pressed. Defaults to <c>1</c>.</param>
        /// <param name="duration">The time measured in seconds from the key press to the key release. Defaults to <c>0.1</c>.</param>
        /// <param name="wait">If set, wait for command to finish. Defaults to <c>True</c>.</param>
        public void PressButton(AltXRControllerButton button, float power = 1, float duration = 0.1f, bool wait = true)
        {
            new AltPressXRButtons(communicationHandler, new AltXRControllerButton[] { button }, power, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulates moving an XR device in a specified direction for a given duration.
        /// </summary>
        /// <param name="device">The XR device to move.</param>
        /// <param name="direction">The direction vector to move the controller.</param>
        /// <param name="duration">Duration of the movement in seconds (default is 0.1).</param>
        /// <param name="wait">If true, waits until the movement is complete (default is true).</param>
        public void Move(AltXRDevice device, AltVector3 direction, float duration = 0.1f, bool wait = true)
        {
            new AltMoveXRDevice(communicationHandler, device, direction, duration, wait);
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulates rotating an XR device by a given vector for a specific duration.
        /// </summary>
        /// <param name="device">The XR device to rotate.</param>
        /// <param name="rotation">Rotation vector to apply to the controller.</param>
        /// <param name="duration">Duration of the rotation in seconds (default is 0.1).</param>
        /// <param name="wait">If true, waits until the rotation is complete (default is true).</param>
        public void Rotate(AltXRDevice device, AltVector3 rotation, float duration = 0.1f, bool wait = true)
        {
            new AltRotateXRDevice(communicationHandler, device, rotation, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulates moving the joystick of an XR controller in a given direction for a specified duration.
        /// </summary>
        /// <param name="controller">The XR controller whose joystick to move.</param>
        /// <param name="direction">Direction to move the joystick (as a vector).</param>
        /// <param name="duration">Duration of the joystick movement in seconds (default is 0.1).</param>
        /// <param name="wait">If true, waits until the joystick movement is complete (default is true).</param>
        public void MoveJoystick(AltXRController controller, AltVector2 direction, float duration = 0.1f, bool wait = true)
        {
            new AltMoveXRControllerJoystick(communicationHandler, controller, direction, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Resets the specified XR device's rotation and/or position.
        /// </summary>
        /// <param name="device">The XR device to reset.</param>
        /// <param name="resetRotation">If true, resets the rotation.</param>
        /// <param name="resetPosition">If true, resets the position.</param>
        /// <param name="wait">If true, waits until the reset operation is complete (default is true).</param>
        public void Reset(AltXRDevice device, bool resetRotation, bool resetPosition, bool wait = true)
        {
            new AltResetXRDevice(communicationHandler, device, resetRotation, resetPosition, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }
    }
}