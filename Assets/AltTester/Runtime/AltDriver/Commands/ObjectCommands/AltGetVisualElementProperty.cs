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
    public class AltGetVisualElementProperty<T> : AltBaseCommand
    {
        AltGetVisualElementPropertyParams cmdParams;
        public AltGetVisualElementProperty(IDriverCommunication commHandler, string propertyName, AltObject altObject) : base(commHandler)
        {
            cmdParams = new AltGetVisualElementPropertyParams(altObject, propertyName);
        }
        public T Execute()
        {
            CommHandler.Send(cmdParams);
            T result = CommHandler.Recvall<T>(cmdParams);
            
            // If the result is an AltObject, ensure its CommHandler is set
            if (result is AltObject altObject)
            {
                altObject.CommHandler = CommHandler;
            }
            
            return result;
        }
    }
}
