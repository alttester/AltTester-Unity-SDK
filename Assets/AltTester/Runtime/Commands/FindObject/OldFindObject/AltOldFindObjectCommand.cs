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

using System.Linq;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    //TODO remove this class after OldFindObject is no longer supported
    class AltOldFindObjectCommand : AltOldBaseFindObjetsCommand<AltObject>
    {
        public AltOldFindObjectCommand(BaseFindObjectsParams cmdParam) : base(cmdParam) { }

        public override AltObject Execute()
        {
            UnityEngine.Debug.Log("OlfFindObject " + CommandParams.path);
            var path = new OldPathSelector(CommandParams.path);
            var foundGameObject = FindObjects(null, path.FirstBound, true, CommandParams.enabled);
            UnityEngine.Camera camera = null;
            if (!CommandParams.cameraPath.Equals("//"))
            {
                camera = GetCamera(CommandParams.cameraBy, CommandParams.cameraPath);
                if (camera == null) throw new CameraNotFoundException();
            }
            if (foundGameObject.Count() >= 1)
            {
                return
                    AltRunner._altRunner.GameObjectToAltObject(foundGameObject[0], camera);
            }
            throw new NotFoundException(string.Format("Object {0} not found", CommandParams.path));
        }
    }
}