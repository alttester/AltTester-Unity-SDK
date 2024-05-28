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

using System.Collections.Generic;
using Newtonsoft.Json;

namespace AltTester.AltTesterUnitySDK.Driver.Commands
{
    public class AltGetAllFields : AltBaseCommand
    {
        AltGetAllFieldsParams cmdParams;
        public AltGetAllFields(IDriverCommunication commHandler, AltComponent altComponent, AltObject altObject, AltFieldsSelections altFieldsSelections = AltFieldsSelections.ALLFIELDS) : base(commHandler)
        {
            cmdParams = new AltGetAllFieldsParams(altObject.id, altComponent, altFieldsSelections);
        }
        public List<AltProperty> Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<List<AltProperty>>(cmdParams);
        }
    }
}
