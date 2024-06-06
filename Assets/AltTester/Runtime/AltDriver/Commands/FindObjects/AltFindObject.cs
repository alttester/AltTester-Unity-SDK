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

namespace AltTester.AltTesterUnitySDK.Driver.Commands
{
    public class AltFindObject : AltBaseFindObjects
    {
        AltFindObjectParams cmdParams;

        public AltFindObject(IDriverCommunication commHandler, By by, string value, By cameraBy, string cameraValue, bool enabled) : base(commHandler)
        {
            cameraValue = SetPath(cameraBy, cameraValue);
            string path = SetPath(by, value);
            cmdParams = new AltFindObjectParams(path, cameraBy, cameraValue, enabled);
        }

        public AltObject Execute()
        {
            CommHandler.Send(cmdParams);
            var altTesterObject = CommHandler.Recvall<AltObject>(cmdParams);
            altTesterObject.CommHandler = CommHandler;
            return altTesterObject;
        }
    }
}
