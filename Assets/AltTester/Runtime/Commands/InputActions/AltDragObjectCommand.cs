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

using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;



namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltDragObjectCommand : AltCommand<AltDragObjectParams, AltObject>
    {
        public AltDragObjectCommand(AltDragObjectParams cmdParams) : base(cmdParams)
        {
        }

        public override AltObject Execute()
        {
            var mockUp = new AltMockUpPointerInputModule();

            var pointerEventData = mockUp.ExecuteTouchEvent(new UnityEngine.Touch() { position = new UnityEngine.Vector2(CommandParams.position.x, CommandParams.position.y) });
            UnityEngine.GameObject gameObject = AltRunner.GetGameObject(CommandParams.altObject.id);
            UnityEngine.Camera viewingCamera = AltRunner._altRunner.FoundCameraById(CommandParams.altObject.idCamera);
            UnityEngine.Vector3 gameObjectPosition = viewingCamera.WorldToScreenPoint(gameObject.transform.position);
            pointerEventData.delta = pointerEventData.position - new UnityEngine.Vector2(gameObjectPosition.x, gameObjectPosition.y);
            UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.dragHandler);
            var camera = AltRunner._altRunner.FoundCameraById(CommandParams.altObject.idCamera);

            return camera != null ? AltRunner._altRunner.GameObjectToAltObject(gameObject, camera) : AltRunner._altRunner.GameObjectToAltObject(gameObject);
        }
    }
}
