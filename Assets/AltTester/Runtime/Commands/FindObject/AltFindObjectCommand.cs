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

using System.Linq;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltFindObjectCommand : AltBaseFindObjectsCommand<AltObject>
    {
        public AltFindObjectCommand(BaseGameFindObjectParams cmdParam) : base(cmdParam) { }

        public override AltObject Execute()
        {

            var foundGameObject = FindObjects(null, CommandParams.objectConditions, 0, true, CommandParams.enabled);
            UnityEngine.Camera camera = null;
            if (IsCameraSpecified(CommandParams.cameraConditions))
            {
                camera = GetCamera(CommandParams.cameraConditions);
                if (camera == null) throw new CameraNotFoundException();
            }
            if (foundGameObject.Count() >= 1)
            {
                return
                    AltRunner._altRunner.GameObjectToAltObject(foundGameObject[0], camera);
            }
            throw new NotFoundException(string.Format("Object not found"));
        }
    }
}
