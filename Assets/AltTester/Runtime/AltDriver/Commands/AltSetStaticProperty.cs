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

namespace AltTester.AltTesterUnitySDK.Driver.Commands
{
    public class AltSetStaticProperty : AltBaseCommand
    {
        AltSetObjectComponentPropertyParams cmdParams;

        public AltSetStaticProperty(IDriverCommunication commHandler, string componentName, string propertyName, string assemblyName, object newValue) : base(commHandler)
        {
            var strValue = Newtonsoft.Json.JsonConvert.SerializeObject(newValue);
            cmdParams = new AltSetObjectComponentPropertyParams(null, componentName, propertyName, assemblyName, strValue);
        }

        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("valueSet", data);
        }
    }
}
