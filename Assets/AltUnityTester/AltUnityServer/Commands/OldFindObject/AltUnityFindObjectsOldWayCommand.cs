namespace Assets.AltUnityTester.AltUnityServer
{
    class AltUnityFindObjectsOldWayCommand :AltUnityCommand
    {
       
        protected UnityEngine.GameObject FindObjectInScene(string objectName, bool enabled)
        {
            string[] pathList = objectName.Split('/');
            UnityEngine.GameObject foundGameObject = null;
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                foreach (UnityEngine.GameObject rootGameObject in UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).GetRootGameObjects())
                {
                    foundGameObject = CheckPath(rootGameObject, pathList, 0, enabled);
                    if (foundGameObject != null)
                        return foundGameObject;
                    else
                    {
                        foundGameObject = CheckChildren(rootGameObject, pathList, enabled);
                        if (foundGameObject != null)
                            return foundGameObject;
                    }
                }
            }
            foreach (var destroyOnLoadObject in AltUnityRunner.GetDontDestroyOnLoadObjects())
            {
                foundGameObject = CheckPath(destroyOnLoadObject, pathList, 0, enabled);
                if (foundGameObject != null)
                    return foundGameObject;
                else
                {
                    foundGameObject = CheckChildren(destroyOnLoadObject, pathList, enabled);
                    if (foundGameObject != null)
                        return foundGameObject;
                }
            }
            return foundGameObject;
        }
        private UnityEngine.GameObject CheckChildren(UnityEngine.GameObject obj, string[] pathList, bool enabled)
        {
            UnityEngine.GameObject objectReturned = null;
            foreach (UnityEngine.Transform childrenTransform in obj.transform)
            {
                objectReturned = CheckPath(childrenTransform.gameObject, pathList, 0, enabled);
                if (objectReturned != null)
                    return objectReturned;
                else
                {
                    objectReturned = CheckChildren(childrenTransform.gameObject, pathList, enabled);
                    if (objectReturned != null)
                        return objectReturned;
                }
            }
            return objectReturned;
        }
        private UnityEngine.GameObject CheckPath(UnityEngine.GameObject obj, string[] pathList, int pathListStep, bool enabled)
        {
            int option = CheckOption(pathList, pathListStep);

            switch (option)
            {
                case 2://..

                    if (pathListStep == pathList.Length - 1)
                    {
                        if (obj.transform.parent == null || (enabled && obj.activeInHierarchy == false)) return null;
                        return obj.transform.parent.gameObject;
                    }
                    else
                    {
                        int nextStep = pathListStep + 1;
                        return CheckNextElementInPath(obj.transform.parent.gameObject, pathList, nextStep, enabled);
                    }
                case 3://children
                    if (pathListStep == pathList.Length - 1)
                    {
                        if (enabled && obj.activeInHierarchy == false) return null;
                        return obj;
                    }
                    else
                    {
                        return CheckNextElementInPath(obj, pathList, pathListStep, enabled);
                    }
                case 4://id
                    var id = System.Convert.ToInt32(pathList[pathListStep].Substring(4, pathList[pathListStep].Length - 4));
                    if (obj.GetInstanceID() != id)
                    {
                        return null;
                    }
                    else
                    {
                        return CheckNextElementInPath(obj, pathList, pathListStep, enabled);
                    }
                case 5://tag
                    var tagName = pathList[pathListStep].Substring(5, pathList[pathListStep].Length - 5);
                    if (!obj.CompareTag(tagName))
                    {
                        return null;
                    }
                    else
                    {
                        return CheckNextElementInPath(obj, pathList, pathListStep, enabled);
                    }
                case 6://layer
                    var layerName = pathList[pathListStep].Substring(7, pathList[pathListStep].Length - 7);
                    int layerId = UnityEngine.LayerMask.NameToLayer(layerName);
                    if (!obj.layer.Equals(layerId))
                    {
                        return null;
                    }
                    else
                    {
                        return CheckNextElementInPath(obj, pathList, pathListStep, enabled);
                    }
                case 7://component
                    var componentName = pathList[pathListStep].Substring(11, pathList[pathListStep].Length - 11);
                    var list = obj.GetComponents(typeof(UnityEngine.Component));
                    for (int i = 0; i < list.Length; i++)
                    {
                        if (componentName.Equals(list[i].GetType().Name))
                        {
                            return CheckNextElementInPath(obj, pathList, pathListStep, enabled);
                        }
                    }
                    return null;
                case 8://name contains
                    var substringOfName = pathList[pathListStep].Substring(10, pathList[pathListStep].Length - 10);
                    if (!obj.name.Contains(substringOfName))
                    {
                        return null;
                    }
                    else
                    {
                        return CheckNextElementInPath(obj, pathList, pathListStep, enabled);
                    }
                default://name
                    var name = pathList[pathListStep];
                    if (option == 10)
                        name = pathList[pathListStep].Substring(6, pathList[pathListStep].Length - 6);
                    if (!obj.name.Equals(name))
                        return null;
                    else
                    {
                        return CheckNextElementInPath(obj, pathList, pathListStep, enabled);
                    }
            }
        }
        private UnityEngine.GameObject CheckNextElementInPath(UnityEngine.GameObject obj, string[] pathList, int pathListStep, bool enabled)
        {
            if (pathListStep == pathList.Length - 1)//Checks if it is at the end of the path
                if (enabled && obj.activeInHierarchy == false) return null;//Checks if it respects enable conditions
                else
                {
                    return obj;
                }
            else
            {
                int nextStep = pathListStep + 1;
                foreach (UnityEngine.Transform childrenObject in obj.transform)
                {
                    var objectReturned = CheckPath(childrenObject.gameObject, pathList, nextStep, enabled);
                    if (objectReturned != null)
                        return objectReturned;
                }
                return null;
            }
        }
        protected System.Collections.Generic.List<UnityEngine.GameObject> FindObjectsInScene(string objectName, bool enabled)
        {
            System.Collections.Generic.List<UnityEngine.GameObject> objectsFound = new System.Collections.Generic.List<UnityEngine.GameObject>();
            string[] pathList = objectName.Split('/');
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                foreach (UnityEngine.GameObject obj in UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).GetRootGameObjects())
                {
                    System.Collections.Generic.List<UnityEngine.GameObject> listGameObjects = CheckPathForMultipleElements(obj.gameObject, pathList, 0, enabled);
                    if (listGameObjects != null)
                        objectsFound.AddRange(listGameObjects);
                    listGameObjects = CheckChildrenForMultipleElements(obj.gameObject, pathList, enabled);
                    if (listGameObjects != null)
                        objectsFound.AddRange(listGameObjects);
                }
            }
            foreach (var destroyOnLoadObject in AltUnityRunner.GetDontDestroyOnLoadObjects())
            {
                System.Collections.Generic.List<UnityEngine.GameObject> listGameObjects = CheckPathForMultipleElements(destroyOnLoadObject.gameObject, pathList, 0, enabled);
                if (listGameObjects != null)
                    objectsFound.AddRange(listGameObjects);
                listGameObjects = CheckChildrenForMultipleElements(destroyOnLoadObject.gameObject, pathList, enabled);
                objectsFound.AddRange(listGameObjects);
            }
            return objectsFound;
        }
        private System.Collections.Generic.List<UnityEngine.GameObject> CheckChildrenForMultipleElements(UnityEngine.GameObject obj, string[] pathList, bool enabled)
        {
            System.Collections.Generic.List<UnityEngine.GameObject> objectsFound = new System.Collections.Generic.List<UnityEngine.GameObject>();
            foreach (UnityEngine.Transform childrenTransform in obj.transform)
            {
                System.Collections.Generic.List<UnityEngine.GameObject> listGameObjects = CheckPathForMultipleElements(childrenTransform.gameObject, pathList, 0, enabled);
                if (listGameObjects != null)
                    objectsFound.AddRange(listGameObjects);
                listGameObjects = CheckChildrenForMultipleElements(childrenTransform.gameObject, pathList, enabled);
                if (listGameObjects != null)
                    objectsFound.AddRange(listGameObjects);

            }
            return objectsFound;
        }
        private System.Collections.Generic.List<UnityEngine.GameObject> CheckPathForMultipleElements(UnityEngine.GameObject obj, string[] pathList, int pathListStep, bool enabled)
        {
            System.Collections.Generic.List<UnityEngine.GameObject> objectsFound = new System.Collections.Generic.List<UnityEngine.GameObject>();
            int option = CheckOption(pathList, pathListStep);
            switch (option)
            {
                case 2://..
                    if (pathListStep == pathList.Length - 1)
                    {
                        if (obj.transform.parent == null || (enabled && obj.activeInHierarchy == false)) return null;
                        objectsFound.Add(obj.transform.parent.gameObject);
                        return objectsFound;
                    }
                    else
                    {
                        int nextStep = pathListStep + 1;
                        return CheckPathForMultipleElements(obj.transform.parent.gameObject, pathList, nextStep, enabled);
                    }
                case 3://children
                    if (pathListStep == pathList.Length - 1)
                    {
                        if (obj.transform.childCount == 0 || (enabled && obj.activeInHierarchy == false)) return null;
                        var parent = obj.transform.parent;
                        for (int i = 0; i <= obj.transform.parent.childCount; i++)
                            objectsFound.Add(parent.GetChild(i).gameObject);
                        return objectsFound;
                    }
                    else
                    {
                        int nextStep = pathListStep + 1;
                        return CheckPathForMultipleElements(obj.transform.parent.gameObject, pathList, nextStep, enabled);
                    }

                case 4://id old version
                    var id = System.Convert.ToInt32(pathList[pathListStep].Substring(3, pathList[pathListStep].Length - 4));
                    if (obj.GetInstanceID() != id)
                    {
                        return null;
                    }
                    else
                    {
                        return CheckNextElementInPathForMultipleElements(obj, pathList, pathListStep, enabled);
                    }
                case 5://tag
                    var tagName = pathList[pathListStep].Substring(5, pathList[pathListStep].Length - 5);
                    if (!obj.CompareTag(tagName))
                    {
                        return null;
                    }
                    else
                    {
                        return CheckNextElementInPathForMultipleElements(obj, pathList, pathListStep, enabled);
                    }
                case 6://layer
                    var layerName = pathList[pathListStep].Substring(7, pathList[pathListStep].Length - 7);
                    int layerId = UnityEngine.LayerMask.NameToLayer(layerName);
                    if (!obj.layer.Equals(layerId))
                    {
                        return null;
                    }
                    else
                    {
                        return CheckNextElementInPathForMultipleElements(obj, pathList, pathListStep, enabled);
                    }
                case 7://component
                    var componentName = pathList[pathListStep].Substring(11, pathList[pathListStep].Length - 11);
                    var list = obj.GetComponents(typeof(UnityEngine.Component));
                    for (int i = 0; i < list.Length; i++)
                    {
                        if (componentName.Equals(list[i].GetType().Name))
                        {
                            return CheckNextElementInPathForMultipleElements(obj, pathList, pathListStep, enabled);
                        }
                    }
                    return null;
                case 8://name contains
                    var substringOfName = pathList[pathListStep].Substring(10, pathList[pathListStep].Length - 10);
                    if (!obj.name.Contains(substringOfName))
                    {
                        return null;
                    }
                    else
                    {
                        return CheckNextElementInPathForMultipleElements(obj, pathList, pathListStep, enabled);
                    }
                case 9://id new version
                    id = System.Convert.ToInt32(pathList[pathListStep].Substring(4, pathList[pathListStep].Length - 4));
                    if (obj.GetInstanceID() != id)
                    {
                        return null;
                    }
                    else
                    {
                        return CheckNextElementInPathForMultipleElements(obj, pathList, pathListStep, enabled);
                    }
                default://name
                    var name = pathList[pathListStep];
                    if (option == 10)
                        name = pathList[pathListStep].Substring(6, pathList[pathListStep].Length - 6);
                    if (!(obj.name.Equals(name) || (name.Equals("") && pathList.Length == 1)))
                        return null;
                    else
                    {
                        return CheckNextElementInPathForMultipleElements(obj, pathList, pathListStep, enabled);
                    }
            }
        }

        private static int CheckOption(string[] pathList, int pathListStep)
        {
            int option = 1;
            if (pathList[pathListStep].Equals(".."))
                option = 2;
            if (pathList[pathListStep].Equals("*"))
                option = 3;
            else
                if (pathList[pathListStep].StartsWith("id("))
                option = 4;
            else
                if (pathList[pathListStep].StartsWith("@tag="))
                option = 5;
            else
                if (pathList[pathListStep].StartsWith("@layer="))
                option = 6;
            else
                if (pathList[pathListStep].StartsWith("@component="))
                option = 7;
            else
                if (pathList[pathListStep].StartsWith("@contains="))
                option = 8;
            else
                if (pathList[pathListStep].StartsWith("@id="))
                option = 9;
            else if (pathList[pathListStep].StartsWith("@name="))
                option = 10;
            return option;
        }

        private System.Collections.Generic.List<UnityEngine.GameObject> CheckNextElementInPathForMultipleElements(UnityEngine.GameObject obj, string[] pathList, int pathListStep, bool enabled)
        {
            System.Collections.Generic.List<UnityEngine.GameObject> objectsFound = new System.Collections.Generic.List<UnityEngine.GameObject>();
            if (pathListStep == pathList.Length - 1)
                if (enabled && obj.activeInHierarchy == false) return null;
                else
                {
                    objectsFound.Add(obj);
                    return objectsFound;
                }
            else
            {
                int nextStep = pathListStep + 1;
                foreach (UnityEngine.Transform childrenObject in obj.transform)
                {
                    System.Collections.Generic.List<UnityEngine.GameObject> listGameObjects = CheckPathForMultipleElements(childrenObject.gameObject, pathList, nextStep, enabled);
                    if (listGameObjects != null)
                        objectsFound.AddRange(listGameObjects);
                }
                return objectsFound;
            }
        }

        public override string Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
