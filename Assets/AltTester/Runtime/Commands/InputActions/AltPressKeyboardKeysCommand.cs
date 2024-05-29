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

using AltTester.AltTesterUnitySDK.Driver.Commands;
using UnityEngine;
using AltTester.AltTesterUnitySDK.InputModule;
using AltTester.AltTesterUnitySDK.Driver;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltPressKeyboardKeysCommand : AltCommandWithWait<AltPressKeyboardKeysParams, string>
    {
        public AltPressKeyboardKeysCommand(ICommandHandler handler, AltPressKeyboardKeysParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
#if ALTTESTER
            var powerClamped = Mathf.Clamp01(CommandParams.power);
            foreach (var keyCode in CommandParams.keyCodes)
                InputController.PressKey((UnityEngine.KeyCode)keyCode, CommandParams.power, CommandParams.duration, onFinish);
            return "Ok";
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }
    }
}
