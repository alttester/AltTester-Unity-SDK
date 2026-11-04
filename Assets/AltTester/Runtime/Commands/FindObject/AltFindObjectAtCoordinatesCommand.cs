/*
    Copyright(C) 2026 Altom Consulting

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

using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltFindObjectAtCoordinatesCommand : AltCommand<AltFindObjectAtCoordinatesParams, AltObject>
    {
        public AltFindObjectAtCoordinatesCommand(AltFindObjectAtCoordinatesParams cmdParam) : base(cmdParam) { }

        public override AltObject Execute()
        {
            var result = FindObjectViaRayCast.FindObjectAtCoordinates(new UnityEngine.Vector2(CommandParams.coordinates.x, CommandParams.coordinates.y));

            if (result.obj == null) return null;

            if (result.isGameObject)
            {
                return AltRunner._altRunner.GameObjectToAltObject(result.obj as GameObject);
            }

            return null;
        }


    }
}
