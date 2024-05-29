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

using System;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltUnloadSceneCommand : AltCommand<AltUnloadSceneParams, string>
    {
        readonly ICommandHandler handler;

        public AltUnloadSceneCommand(ICommandHandler handler, AltUnloadSceneParams cmdParams) : base(cmdParams)
        {
            this.handler = handler;
        }

        public override string Execute()
        {
            try
            {
                var sceneLoadingOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(CommandParams.sceneName);
                if (sceneLoadingOperation == null)
                {
                    throw new CouldNotPerformOperationException("Cannot unload scene: " + CommandParams.sceneName);
                }
                sceneLoadingOperation.completed += sceneUnloaded;
            }
            catch (ArgumentException)
            {
                throw new CouldNotPerformOperationException("Cannot unload scene: " + CommandParams.sceneName);
            }

            return "Ok";
        }

        private void sceneUnloaded(UnityEngine.AsyncOperation obj)
        {
            handler.Send(ExecuteAndSerialize(() => "Scene Unloaded"));
        }
    }
}
