using Altom.AltUnityDriver;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllLoadedScenesAndObjectsCommand : AltUnityBaseClassFindObjectsCommand
    {

        public AltUnityGetAllLoadedScenesAndObjectsCommand(params string[] paramters) : base(paramters) { }

        public override string Execute()
        {
            System.Collections.Generic.List<AltUnityObjectLight> foundObjects = new System.Collections.Generic.List<AltUnityObjectLight>();
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                foundObjects.Add(new AltUnityObjectLight(scene.name));
                foreach (UnityEngine.GameObject rootGameObject in UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).GetRootGameObjects())
                {
                    if (Enabled == false || rootGameObject.activeSelf)
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
                if (Enabled == false || destroyOnLoadObject.activeSelf)
                {
                    foundObjects.Add(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObjectLight(destroyOnLoadObject));
                    foundObjects.AddRange(getAllChildren(destroyOnLoadObject));
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(foundObjects);

        }
        private System.Collections.Generic.List<AltUnityObjectLight> getAllChildren(UnityEngine.GameObject gameObject)
        {
            System.Collections.Generic.List<AltUnityObjectLight> children = new System.Collections.Generic.List<AltUnityObjectLight>();
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i).gameObject;
                if (Enabled == false || child.activeSelf)
                {
                    children.Add(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObjectLight(child));
                    children.AddRange(getAllChildren(child));
                }

            }
            return children;
        }
    }
}