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
    public class AltGetPNGScreenshot : AltBaseCommand
    {
        string path;
        AltGetPNGScreenshotParams cmdParams;

        public AltGetPNGScreenshot(IDriverCommunication commHandler, string path) : base(commHandler)
        {
            this.path = path;
            this.cmdParams = new AltGetPNGScreenshotParams();
        }

        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var message = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", message);
            string screenshotData = CommHandler.Recvall<string>(cmdParams);
            System.IO.File.WriteAllBytes(path, System.Convert.FromBase64String(screenshotData));
        }
    }
}
