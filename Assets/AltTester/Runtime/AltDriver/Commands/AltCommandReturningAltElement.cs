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
using Newtonsoft.Json;

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltCommandReturningAltElement : AltBaseCommand
    {
        public AltCommandReturningAltElement(IDriverCommunication commHandler) : base(commHandler)
        {
        }

        protected AltObject ReceiveAltObject(CommandParams cmdParams)
        {
            var altElement = CommHandler.Recvall<AltObject>(cmdParams);
            if (altElement != null) altElement.CommHandler = CommHandler;

            return altElement;
        }
        protected List<AltObject> ReceiveListOfAltObjects(CommandParams cmdParams)
        {
            var altElements = CommHandler.Recvall<List<AltObject>>(cmdParams);

            foreach (var altElement in altElements)
            {
                altElement.CommHandler = CommHandler;
            }

            return altElements;
        }
    }
}
