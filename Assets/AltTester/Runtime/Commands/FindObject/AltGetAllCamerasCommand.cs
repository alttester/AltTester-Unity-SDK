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
using System.Linq;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltGetAllCamerasCommand : AltCommand<CommandParams, List<AltObject>>
    {
        private readonly bool onlyActiveCameras;

        public AltGetAllCamerasCommand(AltGetAllCamerasParams cmdParams) : base(cmdParams)
        {
            this.onlyActiveCameras = false;
        }
        public AltGetAllCamerasCommand(AltGetAllActiveCamerasParams cmdParams) : base(cmdParams)
        {
            this.onlyActiveCameras = true;
        }
        public override List<AltObject> Execute()
        {
            var cameras = Object.FindObjectsOfType<Camera>();
            var cameraObjects = new List<AltObject>();
            if (onlyActiveCameras)
            {
                cameraObjects.AddRange(from Camera camera in cameras
                                       where camera.enabled == true
                                       select AltRunner._altRunner.GameObjectToAltObject(camera.gameObject));
            }
            else
            {
                cameraObjects.AddRange(from Camera camera in cameras
                                       select AltRunner._altRunner.GameObjectToAltObject(camera.gameObject));
            }
            return cameraObjects;
        }
    }
}
