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

using System.Collections.Generic;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    //TODO remove this class after OldFindObject is no longer supported
    class AltOldFindObjectsCommand : AltOldBaseFindObjetsCommand<List<AltObject>>
    {
        public AltOldFindObjectsCommand(BaseFindObjectsParams cmdParams) : base(cmdParams) { }

        public override List<AltObject> Execute()
        {
            UnityEngine.Debug.Log("OlfFindObject " + CommandParams.path);
            UnityEngine.Camera camera = null;
            if (!CommandParams.cameraPath.Equals("//"))
            {
                camera = GetCamera(CommandParams.cameraBy, CommandParams.cameraPath);
                if (camera == null) throw new CameraNotFoundException();
            }
            var path = new OldPathSelector(CommandParams.path);
            var foundObjects = new List<AltObject>();
            foreach (UnityEngine.GameObject testableObject in FindObjects(null, path.FirstBound, false, CommandParams.enabled))
            {
                foundObjects.Add(AltRunner._altRunner.GameObjectToAltObject(testableObject, camera));
            }

            return foundObjects;
        }
    }
}