﻿/*
    Copyright(C) 2023  Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Communication;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltMoveMouseCommand : AltCommandWithWait<AltMoveMouseParams, string>
    {
        public AltMoveMouseCommand(ICommandHandler handler, AltMoveMouseParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
            InputController.MoveMouse(CommandParams.coordinates.ToUnity(), CommandParams.duration, onFinish);
            return "Ok";
        }
    }
}
