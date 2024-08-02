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

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltHighlightSelectedObjectCommand : AltBaseScreenshotCommand<AltHighlightObjectScreenshotParams, string>
    {

        public AltHighlightSelectedObjectCommand(ICommandHandler handler, AltHighlightObjectScreenshotParams cmdParams) : base(handler, cmdParams)
        {

        }

        public override string Execute()
        {
            var gameObject = AltRunner.GetGameObject(CommandParams.altObjectId);

            if (gameObject != null)
            {
                var color = new UnityEngine.Color(CommandParams.color.r, CommandParams.color.g, CommandParams.color.b, CommandParams.color.a);

                AltRunner._altRunner.StartCoroutine(SendScreenshotObjectHighlightedCoroutine(new UnityEngine.Vector2(CommandParams.size.x, CommandParams.size.y), CommandParams.quality, gameObject, color, CommandParams.width));
            }
            else
            {
                AltRunner._altRunner.StartCoroutine(SendTexturedScreenshotCoroutine(new UnityEngine.Vector2(CommandParams.size.x, CommandParams.size.y), CommandParams.quality));
            }
            return "Ok";
        }
    }
}
