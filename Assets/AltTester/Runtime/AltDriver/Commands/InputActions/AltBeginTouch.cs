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
    public class AltBeginTouch : AltBaseCommand
    {
        AltBeginTouchParams cmdParams;

        public AltBeginTouch(IDriverCommunication commHandler, AltVector2 coordinates) : base(commHandler)
        {
            this.cmdParams = new AltBeginTouchParams(coordinates);
        }
        public int Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<int>(cmdParams);  //finger id
            //TODO: add handling for "Finished"
        }
    }
}
