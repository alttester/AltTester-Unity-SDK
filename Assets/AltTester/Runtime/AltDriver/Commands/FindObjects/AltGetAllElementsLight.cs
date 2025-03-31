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

using System.Collections.Generic;

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltGetAllElementsLight : AltBaseFindObjects
    {
        AltFindObjectsLightParams cmdParams;
        public AltGetAllElementsLight(IDriverCommunication commHandler, By cameraBy, string cameraValue, bool enabled) : base(commHandler)
        {
            cmdParams = new AltFindObjectsLightParams("//*", cameraBy, SetPath(cameraBy, cameraValue), enabled);
        }
        public List<AltObjectLight> Execute()
        {
            CommHandler.Send(cmdParams);

            return CommHandler.Recvall<List<AltObjectLight>>(cmdParams);
        }
    }
}
