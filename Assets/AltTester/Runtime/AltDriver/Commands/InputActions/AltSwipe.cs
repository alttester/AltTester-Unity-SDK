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
    public class AltSwipe : AltBaseCommand
    {
        AltSwipeParams cmdParams;
        public AltSwipe(IDriverCommunication commHandler, AltVector2 start, AltVector2 end, float duration, bool wait) : base(commHandler)
        {
            cmdParams = new AltSwipeParams(start, end, duration, wait);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);

            if (cmdParams.wait)
            {
                data = CommHandler.Recvall<string>(cmdParams);
                ValidateResponse("Finished", data);
            }
        }
    }
}