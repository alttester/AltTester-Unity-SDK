/*
    Copyright(C) 2025 Altom Consulting

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

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltMoveToXRDevice : AltBaseCommand
    {
        AltMoveToXRDeviceParams cmdParams;
        public AltMoveToXRDevice(IDriverCommunication commHandler, AltXRDevice device, AltObject altObject, AltVector3 offset) : base(commHandler)
        {
            cmdParams = new AltMoveToXRDeviceParams(device, altObject, offset);
        }

        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);
        }

    }
}
