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

using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltKeysDownCommand : AltCommand<AltKeysDownParams, string>
    {
        public AltKeysDownCommand(AltKeysDownParams cmdParams) : base(cmdParams)
        {

        }

        public override string Execute()
        {

#if ALTTESTER
            var powerClamped = Mathf.Clamp(CommandParams.power, -1, 1);
            foreach (var keyCode in CommandParams.keyCodes)
                InputController.KeyDown((UnityEngine.KeyCode)keyCode, powerClamped);
            return "Ok";
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }
    }
}
