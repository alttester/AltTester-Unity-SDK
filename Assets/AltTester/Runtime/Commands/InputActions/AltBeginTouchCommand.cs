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
using AltTester.AltTesterUnitySDK.InputModule;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltBeginTouchCommand : AltCommand<AltBeginTouchParams, int>
    {
        public AltBeginTouchCommand(AltBeginTouchParams cmdParams) : base(cmdParams)
        {

        }
        public override int Execute()
        {
            return InputController.BeginTouch(new UnityEngine.Vector2(CommandParams.coordinates.x, CommandParams.coordinates.y));
        }
    }
}
