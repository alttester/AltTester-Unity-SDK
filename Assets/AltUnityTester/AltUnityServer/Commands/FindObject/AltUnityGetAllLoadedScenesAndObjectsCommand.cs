using System.Collections.Generic;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityGetAllLoadedScenesAndObjectsCommand : AltUnityBaseClassFindObjectsCommand<List<AltUnityObjectLight>>
    {
        public AltUnityGetAllLoadedScenesAndObjectsCommand(BaseFindObjectsParams cmdParams) : base(cmdParams) { }

        public override List<AltUnityObjectLight> Execute()
        {
            List<AltUnityObjectLight> foundObjects = new List<AltUnityObjectLight>();
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                foundObjects.Add(new AltUnityObjectLight(scene.name));
                foreach (UnityEngine.GameObject rootGameObject in UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).GetRootGameObjects())
                {
                    if (CommandParams.enabled == false || rootGameObject.activeSelf)
                    {
                        foundObjects.Add(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObjectLight(rootGameObject));
                        foundObjects.AddRange(getAllChildren(rootGameObject));
                    }
                }
            }

            var doNotDestroyOnLoadObjects = AltUnityRunner.GetDontDestroyOnLoadObjects();
            if (doNotDestroyOnLoadObjects.Length != 0)
            {
                foundObjects.Add(new AltUnityObjectLight("DontDestroyOnLoad"));
            }
            foreach (var destroyOnLoadObject in AltUnityRunner.GetDontDestroyOnLoadObjects())
            {
                if (CommandParams.enabled == false || destroyOnLoadObject.activeSelf)
                {
                    foundObjects.Add(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObjectLight(destroyOnLoadObject));
                    foundObjects.AddRange(getAllChildren(destroyOnLoadObject));
                }
            }
            return foundObjects;

        }
        private List<AltUnityObjectLight> getAllChildren(UnityEngine.GameObject gameObject)
        {
            List<AltUnityObjectLight> children = new List<AltUnityObjectLight>();
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i).gameObject;
                if (CommandParams.enabled == false || child.activeSelf)
                {
                    children.Add(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObjectLight(child));
                    children.AddRange(getAllChildren(child));
                }

            }
            return children;
        }
    }
}