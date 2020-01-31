namespace Assets.AltUnityTester.AltUnityServer
{
    class AltUnityBaseClassFindObjectsCommand :AltUnityCommand
    {
        protected System.Collections.Generic.List<System.Collections.Generic.List<string>> ProcessPath(string path)
        {
            System.Collections.Generic.List<char> escapeCharacters;
            var text = EliminateEscapedCharacters(path, out escapeCharacters);
            var list = SeparateAxesAndSelectors(text);
            var pathSetCorrectly = SetCondition(list);
            pathSetCorrectly = AddEscapedCharactersBack(pathSetCorrectly, escapeCharacters);
            return pathSetCorrectly;
        }


        private System.Collections.Generic.List<System.Collections.Generic.List<string>> AddEscapedCharactersBack(System.Collections.Generic.List<System.Collections.Generic.List<string>> pathSetCorrectly, System.Collections.Generic.List<char> escapeCharacters)
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

        private System.Collections.Generic.List<System.Collections.Generic.List<string>> SetCondition(System.Collections.Generic.List<string> list)
        {
            System.Collections.Generic.List<System.Collections.Generic.List<string>> conditions = new System.Collections.Generic.List<System.Collections.Generic.List<string>>();
            for (int i = 0; i < list.Count; i++)
            {
                if (i % 2 == 0)
                {
                    if (!(list[i].Equals("/") || list[i].Equals("//")))
                        throw new System.Exception("Expected / or // instead of " + list[i]);
                    conditions.Add(new System.Collections.Generic.List<string>() { list[i] });

                }
                else
                {
                    conditions.Add(ParseSelector(list[i]));
                }

            }
            return conditions;
        }

        private System.Collections.Generic.List<string> ParseSelector(string selector)
        {
            System.Collections.Generic.List<string> conditions = new System.Collections.Generic.List<string>();
            if (System.Text.RegularExpressions.Regex.IsMatch(selector, "^.+\\[@.+=.+\\]$") || System.Text.RegularExpressions.Regex.IsMatch(selector, "^.+\\[.+(@.+,.+)\\]$"))
            {
                var substrings = selector.Split('[');
                conditions.Add(substrings[0]);
                conditions.Add(substrings[1].Substring(0, substrings[1].Length - 1));
                return conditions;
            }
            conditions.Add(selector);
            return conditions;
        }

        private string EliminateEscapedCharacters(string text, out System.Collections.Generic.List<char> escapedCharacters)
        {
            escapedCharacters = new System.Collections.Generic.List<char>();
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

        private System.Collections.Generic.List<string> SeparateAxesAndSelectors(string path)
        {
            string[] substrings = System.Text.RegularExpressions.Regex.Split(path, "(/)");
            System.Collections.Generic.List<string> listOfSubstring = new System.Collections.Generic.List<string>();
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
            System.Collections.Generic.List<string> listOfSubstring2 = new System.Collections.Generic.List<string>();
            foreach (var str in listOfSubstring)
                if (!str.Equals(""))
                    listOfSubstring2.Add(str);
            return listOfSubstring2;

        }

        public System.Collections.Generic.List<UnityEngine.GameObject> FindObjects(UnityEngine.GameObject gameObject, System.Collections.Generic.List<System.Collections.Generic.List<string>> conditions, int step, bool singleObject, bool directChildren, bool enabled)
        {

            if (CheckConditionIfParent(conditions[step]))
            {
                if (IsNextElementDirectChild(conditions[step + 1]))
                {
                    return FindObjects(gameObject.transform.parent.gameObject, conditions, step + 2, singleObject, true, enabled);
                }
                else
                {
                    return FindObjects(gameObject.transform.parent.gameObject, conditions, step + 2, singleObject, false, enabled);
                }

            }
            System.Collections.Generic.List<UnityEngine.GameObject> objectsToCheck = GetGameObjectsToCheck(gameObject);
            System.Collections.Generic.List<UnityEngine.GameObject> objectsFound = new System.Collections.Generic.List<UnityEngine.GameObject>();
            foreach (var objectToCheck in objectsToCheck)
            {

                if ((!enabled || (enabled && objectToCheck.activeInHierarchy)) && CheckCondition(objectToCheck, conditions[step]))
                {

                    //Pass the condition
                    if (step != conditions.Count - 1)
                    {
                        if (IsNextElementDirectChild(conditions[step + 1]))
                        {
                            if (singleObject)
                            {
                                return FindObjects(objectToCheck, conditions, step + 2, singleObject, true, enabled);
                            }
                            else
                            {
                                objectsFound.AddRange( FindObjects(objectToCheck, conditions, step + 2, singleObject, true, enabled));
                                continue;

                            }
                        }
                        else
                        {
                            if (singleObject)
                            {
                                return FindObjects(objectToCheck, conditions, step + 2, singleObject, false, enabled);
                            }
                            else
                            {
                                objectsFound.AddRange( FindObjects(objectToCheck, conditions, step + 2, singleObject, false, enabled));
                                continue;

                            }
                        }

                    }
                    objectsFound.Add(objectToCheck);
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

        private bool CheckCondition(UnityEngine.GameObject objectToCheck, System.Collections.Generic.List<string> listOfConditions)
        {
            bool valid = true;
            foreach (var condition in listOfConditions)
            {
                var option = CheckOption(condition);
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
                        var id = System.Convert.ToInt32(condition.Substring(4, condition.Length - 4));
                        valid = (objectToCheck.GetInstanceID() == id);
                        break;
                    case 6://contains
                        var substring = condition.Substring(9, condition.Length - 10);
                        var splitedValue = substring.Split(',');
                        var selector = splitedValue[0];
                        var value = splitedValue[1];
                        var optionContains = CheckOption(selector);
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
                            default:
                                throw new System.Exception("No such selector is implemented");


                        }
                        break;
                }
                if (!valid)
                    break;
            }
            return valid;
        }
        private static int CheckOption(string condition)
        {
            int option = 1;
            if (condition.StartsWith("@tag"))
                option = 2;
            else
                if (condition.StartsWith("@layer"))
                option = 3;
            else
                if (condition.StartsWith("@component"))
                option = 4;
            else
                if (condition.StartsWith("@id"))
                option = 5;
            else
                if (condition.StartsWith("contains"))
                option = 6;
            else
                if (condition.Equals("*"))
                option = 7;
            else if (condition.Equals("@name"))
                option = 8;
            return option;
        }

        private bool CheckConditionIfParent(System.Collections.Generic.List<string> list)
        {
            return list.Count == 1 && list[0].Equals("..");
        }

        protected bool IsNextElementDirectChild(System.Collections.Generic.List<string> list)
        {
            if (list.Count == 1 && list[0].Equals("/"))
                return true;
            else
                if (list.Count == 1 && list[0].Equals("//"))
                return false;
            throw new System.Exception("Invalid path. Expected / or // but got " + list.ToString());
        }

        private System.Collections.Generic.List<UnityEngine.GameObject> GetGameObjectsToCheck(UnityEngine.GameObject gameObject)
        {
            System.Collections.Generic.List<UnityEngine.GameObject> objectsToCheck = new System.Collections.Generic.List<UnityEngine.GameObject>();
            if (gameObject == null)
            {
                objectsToCheck = GetAllRootObjects();
            }
            else
            {
                objectsToCheck = GetAllChildren(gameObject);
            }
            return objectsToCheck;
        }

        private System.Collections.Generic.List<UnityEngine.GameObject> GetAllChildren(UnityEngine.GameObject gameObject)
        {
            System.Collections.Generic.List<UnityEngine.GameObject> objectsToCheck = new System.Collections.Generic.List<UnityEngine.GameObject>();
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                objectsToCheck.Add(gameObject.transform.GetChild(i).gameObject);
            }
            return objectsToCheck;
        }

        private System.Collections.Generic.List<UnityEngine.GameObject> GetAllRootObjects()
        {
            System.Collections.Generic.List<UnityEngine.GameObject> objectsToCheck = new System.Collections.Generic.List<UnityEngine.GameObject>();
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
    }
   
}
