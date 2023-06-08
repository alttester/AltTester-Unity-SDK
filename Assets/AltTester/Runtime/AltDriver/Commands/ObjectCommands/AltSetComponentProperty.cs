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

namespace AltTester.AltTesterUnitySDK.Driver.Commands
{
    public class AltSetComponentProperty : AltBaseCommand
    {
        AltSetObjectComponentPropertyParams cmdParams;

        public AltSetComponentProperty(IDriverCommunication commHandler, string componentName, string propertyName, object value, string assemblyName, AltObject altObject) : base(commHandler)
        {
            var strValue = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            cmdParams = new AltSetObjectComponentPropertyParams(altObject, componentName, propertyName, assemblyName, strValue);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);

            ValidateResponse("valueSet", data);
        }
    }
}