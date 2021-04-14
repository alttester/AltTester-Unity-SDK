using System;
using System.Collections.Generic;
using System.Linq;
using Altom.AltUnityDriver;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityBaseClassFindObjectsCommand : AltUnityCommand
    {
        protected string ObjectName;
        protected By CameraBy;
        protected string CameraPath;
        protected bool Enabled;

        protected AltUnityBaseClassFindObjectsCommand(params string[] parameters) : base(parameters, 6)
        {
            ObjectName = parameters[2];
            CameraBy = (By)Enum.Parse(typeof(By), parameters[3]);
            CameraPath = parameters[4];
            Enabled = JsonConvert.DeserializeObject<bool>(parameters[5].ToLower());
        }

        protected List<List<string>> ProcessPath(string path)
        {
            List<char> escapeCharacters;
            var text = eliminateEscapedCharacters(path, out escapeCharacters);
            var list = separateAxesAndSelectors(text);
            var pathSetCorrectly = setCondition(list);
            pathSetCorrectly = addEscapedCharactersBack(pathSetCorrectly, escapeCharacters);
            return pathSetCorrectly;
        }


        private List<List<string>> addEscapedCharactersBack(List<List<string>> pathSetCorrectly, List<char> escapeCharacters)
        {
            int counter = 0;
            for (int i = 0; i < pathSetCorrectly.Count; i++)
            {
                for (int j = 0; j < pathSetCorrectly[i].Count; j++)
                {
                    do
                    {
                        if (pathSetCorrectly[i][j].Contains("!"))
                        {
                            int index = pathSetCorrectly[i][j].IndexOf('!');
                            pathSetCorrectly[i][j] = pathSetCorrectly[i][j].Remove(index, 1);
                            pathSetCorrectly[i][j] = pathSetCorrectly[i][j].Insert(index, escapeCharacters[counter].ToString());
                            counter++;

                        }

                    } while (pathSetCorrectly[i][j].Contains("!"));

                }
            }
            return pathSetCorrectly;
        }

        private string getText(UnityEngine.GameObject objectToCheck)
        {
            var textComponent = objectToCheck.GetComponent<UnityEngine.UI.Text>();
            if (textComponent != null)
            {
                return textComponent.text;
            }

            var inputFieldComponent = objectToCheck.GetComponent<UnityEngine.UI.InputField>();
            if (inputFieldComponent != null)
            {
                return inputFieldComponent.text;
            }

            var tmpTextComponent = objectToCheck.GetComponent<TMPro.TMP_Text>();
            if (tmpTextComponent != null)
            {
                return tmpTextComponent.text;
            }

            var tmpInputFieldComponent = objectToCheck.GetComponent<TMPro.TMP_InputField>();
            if (tmpInputFieldComponent != null)
            {
                return tmpInputFieldComponent.text;
            }

            return "";
        }

        private List<List<string>> setCondition(List<string> list)
        {
            List<List<string>> conditions = new List<List<string>>();
            for (int i = 0; i < list.Count; i++)
            {
                if (i % 2 == 0)
                {
                    if (!(list[i].Equals("/") || list[i].Equals("//")))
                        throw new InvalidPathException("Expected / or // before " + list[i]);
                    conditions.Add(new List<string>() { list[i] });

                }
                else
                {
                    conditions.Add(parseSelector(list[i]));
                }

            }
            return conditions;
        }

        private List<string> parseSelector(string selector)
        {
            List<string> conditions = new List<string>();
            var substrings = selector.Split('[');
            for (int i = 0; i < substrings.Length; i++)
            {
                if (i == 0)
                {
                    if (String.IsNullOrEmpty(substrings[i]))
                    {
                        throw new InvalidPathException("Expected object name or * for " + selector);
                    }
                    conditions.Add(substrings[i]);
                }
                else
                {
                    if (!substrings[i].EndsWith("]"))
                    {
                        throw new InvalidPathException("Condition didn't end with ] for " + selector);
                    }
                    conditions.Add(substrings[i].Substring(0, substrings[i].Length - 1));

                }
            }
            return conditions;

        }

        private string eliminateEscapedCharacters(string text, out List<char> escapedCharacters)
        {
            escapedCharacters = new List<char>();
            var textWithoutEscapeCharacters = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Equals('\\'))
                {
                    escapedCharacters.Add(text[i + 1]);
                    textWithoutEscapeCharacters += "!";
                    i++;
                    continue;
                }
                if (text[i].Equals('!'))
                {
                    escapedCharacters.Add(text[i]);
                    textWithoutEscapeCharacters += "!";
                    continue;
                }
                textWithoutEscapeCharacters += text[i];
            }
            return textWithoutEscapeCharacters;
        }

        private List<string> separateAxesAndSelectors(string path)
        {
            string[] substrings = System.Text.RegularExpressions.Regex.Split(path, "(/)");
            List<string> listOfSubstring = new List<string>();
            foreach (var str in substrings)
                if (!str.Equals(""))
                    listOfSubstring.Add(str);
            for (int i = 0; i <= listOfSubstring.Count - 2; i++)
            {
                if (listOfSubstring[i].Equals("/") && listOfSubstring[i + 1].Equals("/"))
                {
                    listOfSubstring[i] += listOfSubstring[i + 1];
                    listOfSubstring[i + 1] = "";
                    continue;
                }
            }
            List<string> listOfSubstring2 = new List<string>();
            foreach (var str in listOfSubstring)
                if (!str.Equals(""))
                    listOfSubstring2.Add(str);
            return listOfSubstring2;

        }

        public List<UnityEngine.GameObject> FindObjects(UnityEngine.GameObject gameObject, List<List<string>> conditions, int step, bool singleObject, bool directChildren, bool enabled)
        {

            if (checkConditionIfParent(conditions[step]))
            {
                if (step == conditions.Count - 1)//if last condition is .. then it will return the parent
                {
                    return new List<UnityEngine.GameObject>() { gameObject.transform.parent.gameObject };
                }
                var directChild = IsNextElementDirectChild(conditions[step + 1]);
                return FindObjects(gameObject.transform.parent.gameObject, conditions, step + 2, singleObject, directChild, enabled);

            }
            List<UnityEngine.GameObject> objectsToCheck = getGameObjectsToCheck(gameObject);
            List<UnityEngine.GameObject> objectsFound = new List<UnityEngine.GameObject>();
            foreach (var objectToCheck in objectsToCheck)
            {
                int childNumber = -1;
                if ((!enabled || (enabled && objectToCheck.activeInHierarchy)) && checkCondition(objectToCheck, conditions[step], ref childNumber))
                {
                    //Pass the condition
                    UnityEngine.GameObject nextObjectToCheck = childNumber != -1 ? objectToCheck.transform.GetChild(childNumber).gameObject : objectToCheck;
                    if (step != conditions.Count - 1)
                    {
                        var directChild = IsNextElementDirectChild(conditions[step + 1]);
                        if (singleObject)
                        {
                            return FindObjects(nextObjectToCheck, conditions, step + 2, singleObject, directChild, enabled);
                        }
                        else
                        {
                            objectsFound.AddRange(FindObjects(nextObjectToCheck, conditions, step + 2, singleObject, directChild, enabled));
                            continue;
                        }
                    }
                    objectsFound.Add(nextObjectToCheck);
                    if (singleObject)
                    {
                        return objectsFound;
                    }

                }


                if (directChildren)
                {
                    continue;
                }

                objectsFound.AddRange(FindObjects(objectToCheck, conditions, step, singleObject, false, enabled));
                if (objectsFound.Count != 0 && singleObject)//Don't search further if you already found an object
                {
                    return objectsFound;
                }
                continue;
            }
            return objectsFound;

        }

        private bool checkCondition(UnityEngine.GameObject objectToCheck, List<string> listOfConditions, ref int childNumber)
        {
            bool valid = true;
            foreach (var condition in listOfConditions)
            {
                var option = (condition.Equals(listOfConditions[0]) && !condition.Equals("*")) ? 1 : checkOption(condition);
                switch (option)
                {
                    case 1://name
                        var name = condition;
                        valid = objectToCheck.name.Equals(name);
                        break;
                    case 2://tag
                        var tagName = condition.Substring(5, condition.Length - 5);
                        valid = objectToCheck.CompareTag(tagName);
                        break;
                    case 3://layer
                        var layerName = condition.Substring(7, condition.Length - 7);
                        int layerId = UnityEngine.LayerMask.NameToLayer(layerName);
                        valid = objectToCheck.layer.Equals(layerId);
                        break;
                    case 4://component
                        var componentName = condition.Substring(11, condition.Length - 11);
                        componentName = componentName.Split(new string[] { "." }, System.StringSplitOptions.None).Last();
                        var list = objectToCheck.GetComponents(typeof(UnityEngine.Component));
                        valid = false;

                        for (int i = 0; i < list.Length; i++)
                        {
                            if (componentName.Equals(list[i].GetType().Name))
                            {
                                valid = true;
                                break;
                            }
                        }
                        break;
                    case 5://id
                        string idAsString = condition.Substring(4, condition.Length - 4);
                        if (System.Text.RegularExpressions.Regex.Match(idAsString, "^([1-9]{1}[0-9]*|-[1-9]{1}[0-9]*|0)$").Success)
                        {
                            var id = System.Convert.ToInt32(condition.Substring(4, condition.Length - 4));
                            valid = objectToCheck.GetInstanceID() == id;
                            break;
                        }
                        var component = objectToCheck.GetComponent<AltUnityId>();
                        if (component != null)
                        {
                            valid = component.altID.Equals(idAsString);
                            break;
                        }
                        valid = false;
                        break;
                    case 6://contains
                        var substring = condition.Substring(9, condition.Length - 10);
                        var splitedValue = substring.Split(',');
                        var selector = splitedValue[0];
                        var value = splitedValue[1];
                        var optionContains = checkOption(selector);
                        switch (optionContains)
                        {
                            case 2:
                                valid = objectToCheck.tag.Contains(value);
                                break;
                            case 3:
                                var layerNm = UnityEngine.LayerMask.LayerToName(objectToCheck.layer);
                                valid = layerNm.Contains(value);
                                break;
                            case 4:
                                componentName = value;
                                list = objectToCheck.GetComponents(typeof(UnityEngine.Component));
                                valid = false;

                                for (int i = 0; i < list.Length; i++)
                                {
                                    if (componentName.Contains(list[i].GetType().Name))
                                    {
                                        valid = true;
                                        break;
                                    }
                                }
                                break;
                            case 5:
                                var stringId = objectToCheck.GetInstanceID().ToString();
                                valid = stringId.Contains(value);
                                break;
                            case 8:
                                valid = objectToCheck.name.Contains(value);
                                break;
                            case 10:
                                valid = getText(objectToCheck).Contains(value);
                                break;

                            default:
                                throw new System.Exception("No such selector is implemented");


                        }
                        break;
                    case 9:
                        childNumber = int.Parse(condition);
                        if (childNumber < 0)
                        {
                            childNumber = objectToCheck.transform.childCount + childNumber;
                        }
                        valid = childNumber >= 0 && childNumber <= objectToCheck.transform.childCount;
                        break;

                    case 10:
                        var text = condition.Substring(6, condition.Length - 6);
                        valid = getText(objectToCheck).Equals(text);

                        break;
                }
                if (!valid)
                    break;
            }
            return valid;
        }
        private static int checkOption(string condition)
        {
            int option = 1;
            if (condition.StartsWith("@tag"))
                option = 2;
            else if (condition.StartsWith("@layer"))
                option = 3;
            else if (condition.StartsWith("@component"))
                option = 4;
            else if (condition.StartsWith("@id"))
                option = 5;
            else if (condition.StartsWith("contains"))
                option = 6;
            else if (condition.Equals("*"))
                option = 7;
            else if (condition.Equals("@name"))
                option = 8;
            else if (System.Text.RegularExpressions.Regex.Match(condition, "([1-9]{1}[0-9]*|-[1-9]{1}[0-9]*|0)").Success)
                option = 9;
            else if (condition.StartsWith("@text"))
                option = 10;

            return option;
        }

        private bool checkConditionIfParent(List<string> list)
        {
            return list.Count == 1 && list[0].Equals("..");
        }

        protected bool IsNextElementDirectChild(List<string> list)
        {
            if (list.Count == 1 && list[0].Equals("/"))
                return true;
            else
                if (list.Count == 1 && list[0].Equals("//"))
                return false;
            throw new System.Exception("Invalid path. Expected / or // but got " + list.ToString());
        }

        private List<UnityEngine.GameObject> getGameObjectsToCheck(UnityEngine.GameObject gameObject)
        {
            List<UnityEngine.GameObject> objectsToCheck;
            if (gameObject == null)
            {
                objectsToCheck = getAllRootObjects();
            }
            else
            {
                objectsToCheck = getAllChildren(gameObject);
            }
            return objectsToCheck;
        }

        private List<UnityEngine.GameObject> getAllChildren(UnityEngine.GameObject gameObject)
        {
            List<UnityEngine.GameObject> objectsToCheck = new List<UnityEngine.GameObject>();
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                objectsToCheck.Add(gameObject.transform.GetChild(i).gameObject);
            }
            return objectsToCheck;
        }

        private List<UnityEngine.GameObject> getAllRootObjects()
        {
            List<UnityEngine.GameObject> objectsToCheck = new List<UnityEngine.GameObject>();
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                foreach (UnityEngine.GameObject rootGameObject in UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).GetRootGameObjects())
                {
                    objectsToCheck.Add(rootGameObject);
                }
            }
            foreach (var destroyOnLoadObject in AltUnityRunner.GetDontDestroyOnLoadObjects())
            {
                objectsToCheck.Add(destroyOnLoadObject);

            }
            return objectsToCheck;
        }

        public override string Execute()
        {
            throw new System.NotImplementedException();
        }
        protected UnityEngine.Camera GetCamera(By cameraBy, string cameraPath)
        {

            if (cameraBy == By.NAME)
            {
                var cameraPathSplited = cameraPath.Split('/');
                var cameraName = cameraPathSplited[cameraPathSplited.Length - 1];
                return UnityEngine.Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));

            }
            else
            {
                var cameraPathProcessed = ProcessPath(cameraPath);
                var isDirectChildCamera = IsNextElementDirectChild(cameraPathProcessed[0]);
                var gameObjectsCameraFound = FindObjects(null, cameraPathProcessed, 1, false, isDirectChildCamera, true);
                return UnityEngine.Camera.allCameras.ToList().Find(c => gameObjectsCameraFound.Find(d => c.gameObject.GetInstanceID() == d.GetInstanceID()));
            }

        }
    }
}
