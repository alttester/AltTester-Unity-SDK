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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AltTester.AltTesterUnitySDK.Driver;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Commands
{
    //TODO remove this file after OldFindObject is no longer supported
    public enum OldBoundType
    {
        DirectChildren,
        AnyChildren,
        Parent
    }

    public enum OldSelectorType
    {
        Any, // *
        Name,  // name
        Function,//[functionName(@propertyName,propertyValue)] 
        PropertyEquals, //[@propertyName=propertyValue]
        Indexer, // [n]
    }

    public enum OldFunctionType
    {
        contains
    }
    public enum OldPropertyType
    {
        id,
        name,
        tag,
        layer,
        component,
        text
    }

    public class OldBoundCondition
    {
        public OldBoundCondition(string selector, OldBoundType type, OldBoundCondition previousBound)
        {
            PrevBound = previousBound;
            Type = type;
            Selector = selector;

            if (previousBound != null)
            {
                previousBound.NextBound = this;
            }
        }
        public string Selector { get; set; }
        public OldBoundType Type { get; protected set; }
        public OldIndexerCondition Indexer { get; set; }


        public OldBoundCondition NextBound { get; private set; }
        public OldBoundCondition PrevBound { get; private set; }
        public OldSelectorCondition FirstSelector { get; set; }

    }
    public abstract class OldSelectorCondition
    {

        public OldSelectorCondition(string selector, OldSelectorType type, OldSelectorCondition previousSelector)
        {
            Selector = selector;
            Type = type;
            PrevSelector = previousSelector;
            if (previousSelector != null)
                previousSelector.NextSelector = this;

        }
        public string Selector { get; set; }
        public OldSelectorType Type { get; protected set; }

        public OldSelectorCondition NextSelector { get; set; }
        public OldSelectorCondition PrevSelector { get; set; }

        public abstract GameObject MatchCondition(GameObject gameObjectToCheck, bool enabled);

        protected static PropertyType GetPropertyType(string property)
        {
            PropertyType parsed;
            if (!Enum.TryParse<PropertyType>(property, out parsed))
            {
                throw new InvalidPathException("Invalid property type '" + property + "'. Expected one of: " + string.Join(",", Enum.GetNames(typeof(PropertyType))));
            }
            return parsed;
        }
        protected static FunctionType GetFunctionType(string functionName)
        {
            FunctionType parsed;
            if (!Enum.TryParse<FunctionType>(functionName, out parsed))
            {
                throw new InvalidPathException("Invalid function '" + functionName + "'. Expected one of: " + string.Join(",", Enum.GetNames(typeof(FunctionType))));
            }
            return parsed;
        }

        protected string GetText(UnityEngine.GameObject objectToCheck)
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
    }

    public class OldAnyCondition : OldSelectorCondition
    {
        public OldAnyCondition(string selector, OldSelectorCondition previousSelector) : base(selector, OldSelectorType.Any, previousSelector)
        {
        }

        public override GameObject MatchCondition(GameObject gameObjectToCheck, bool enabled)
        {
            return gameObjectToCheck;
        }
    }

    public class OldNameCondition : OldSelectorCondition
    {
        public OldNameCondition(string selector, OldSelectorCondition previousSelector) : base(selector, OldSelectorType.Name, previousSelector)
        {
            this.Name = selector;
        }
        public string Name { get; set; }

        public override GameObject MatchCondition(GameObject gameObjectToCheck, bool enabled)
        {
            return gameObjectToCheck.name.Equals(Name) ? gameObjectToCheck : null;
        }
    }

    public class OldPropertyEqualsCondition : OldSelectorCondition
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector">@propertyName=propertyvalue</param>
        /// <returns></returns>
        public OldPropertyEqualsCondition(string selector, OldSelectorCondition previousSelector) : base(selector, OldSelectorType.PropertyEquals, previousSelector)
        {
            var delimiterPos = selector.IndexOf("=");
            if (delimiterPos < 0) throw new InvalidPathException("Expected property selector format `@propertyName=propertyvalue` Got " + selector);
            var propvalue = selector.Substring(1, delimiterPos - 1);
            this.Property = GetPropertyType(propvalue);
            this.PropertyValue = selector.Substring(delimiterPos + 1);
        }
        public PropertyType Property { get; private set; }
        public string PropertyValue { get; set; }

        public override GameObject MatchCondition(GameObject gameObjectToCheck, bool enabled)
        {
            switch (Property)
            {
                case PropertyType.id:
                    if (System.Text.RegularExpressions.Regex.Match(PropertyValue, "^([1-9]{1}[0-9]*|-[1-9]{1}[0-9]*|0)$").Success)
                    {
                        var id = System.Convert.ToInt32(PropertyValue);
                        return gameObjectToCheck.GetInstanceID() == id ? gameObjectToCheck : null;
                    }
                    var component = gameObjectToCheck.GetComponent<AltId>();
                    if (component != null)
                    {
                        return component.altID.Equals(PropertyValue) ? gameObjectToCheck : null;
                    }
                    return null;
                case PropertyType.name:
                    return gameObjectToCheck.name.Equals(PropertyValue) ? gameObjectToCheck : null;
                case PropertyType.tag:
                    try
                    {
                        return gameObjectToCheck.tag.Equals(PropertyValue) ? gameObjectToCheck : null;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                case PropertyType.layer:
                    int layerId = LayerMask.NameToLayer(PropertyValue);
                    return gameObjectToCheck.layer.Equals(layerId) ? gameObjectToCheck : null;
                case PropertyType.component:
                    var componentName = PropertyValue.Split(new string[] { "." }, System.StringSplitOptions.None).Last();
                    var list = gameObjectToCheck.GetComponents(typeof(UnityEngine.Component));
                    for (int i = 0; i < list.Length; i++)
                    {
                        try
                        {
                            if (componentName.Equals(list[i].GetType().Name))
                            {
                                return gameObjectToCheck;
                            }
                        }
                        catch (System.NullReferenceException)
                        {
                            continue;
                        }
                    }
                    return null;
                case PropertyType.text:
                    return GetText(gameObjectToCheck).Equals(PropertyValue) ? gameObjectToCheck : null;
            }
            return null;
        }
    }

    public class OldFunctionCondition : OldSelectorCondition
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector">functionName(@propertyName=propertyvalue)</param>
        /// <returns></returns>
        public OldFunctionCondition(string selector, OldSelectorCondition previousSelector) : base(selector, OldSelectorType.Function, previousSelector)
        {
            var delimiterPos = selector.IndexOf(")");
            if (delimiterPos != selector.Length - 1) throw new InvalidPathException("Expected property selector format `function(@propertyName,propertyvalue)` Got " + selector);

            delimiterPos = selector.IndexOf("(");
            if (delimiterPos < 0) throw new InvalidPathException("Expected property selector format `function(@propertyName,propertyvalue)` Got " + selector);

            var functionName = selector.Substring(0, delimiterPos);
            this.Function = GetFunctionType(functionName);

            string condition = selector.Substring(delimiterPos + 1, selector.Length - delimiterPos - 2);

            delimiterPos = condition.IndexOf(",");
            if (delimiterPos < 0) throw new InvalidPathException("Expected property selector format `function(@propertyName,propertyvalue)` Got " + selector);

            var propname = condition.Substring(1, delimiterPos - 1);
            this.Property = GetPropertyType(propname);
            this.PropertyValue = condition.Substring(delimiterPos + 1);
        }
        public PropertyType Property { get; private set; }
        public string PropertyValue { get; set; }
        public FunctionType Function { get; private set; }

        public override GameObject MatchCondition(GameObject gameObjectToCheck, bool enabled)
        {
            switch (Function)
            {
                case FunctionType.contains:
                    switch (Property)
                    {
                        case PropertyType.id:
                            if (System.Text.RegularExpressions.Regex.Match(PropertyValue, "^([1-9]{1}[0-9]*|-[1-9]{1}[0-9]*|0)$").Success)
                            {
                                return gameObjectToCheck.GetInstanceID().ToString().Contains(PropertyValue) ? gameObjectToCheck : null;
                            }
                            var component = gameObjectToCheck.GetComponent<AltId>();
                            if (component != null)
                            {
                                return component.altID.Contains(PropertyValue) ? gameObjectToCheck : null;
                            }
                            return null;
                        case PropertyType.name:
                            return gameObjectToCheck.name.Contains(PropertyValue) ? gameObjectToCheck : null;
                        case PropertyType.tag:
                            return gameObjectToCheck.tag.Contains(PropertyValue) ? gameObjectToCheck : null;
                        case PropertyType.layer:
                            string layerNm = LayerMask.LayerToName(gameObjectToCheck.layer);
                            return layerNm.Contains(PropertyValue) ? gameObjectToCheck : null;
                        case PropertyType.component:
                            var componentName = PropertyValue.Split(new string[] { "." }, System.StringSplitOptions.None).Last();
                            UnityEngine.Debug.LogWarning("Component Name: " + componentName);//TODO remove when PR ready
                            var list = gameObjectToCheck.GetComponents(typeof(UnityEngine.Component));
                            UnityEngine.Debug.LogWarning("List count: " + list.Length.ToString());//TODO remove when PR ready
                            for (int i = 0; i < list.Length; i++)
                            {
                                UnityEngine.Debug.LogWarning("i : " + i);//TODO remove when PR ready
                                if (list[i] != null && list[i].GetType().Name.Contains(componentName))
                                {
                                    UnityEngine.Debug.LogWarning("gameObjectToCheck : " + gameObjectToCheck.name);//TODO remove when PR ready
                                    return gameObjectToCheck;
                                }
                            }
                            return null;
                        case PropertyType.text:
                            return GetText(gameObjectToCheck).Contains(PropertyValue) ? gameObjectToCheck : null;
                    }
                    break;
            }
            return null;
        }

    }

    public class OldIndexerCondition : OldSelectorCondition
    {
        public OldIndexerCondition(string selector, OldSelectorCondition previousSelector) : base(selector, OldSelectorType.Indexer, previousSelector)
        {
            int index;
            if (!Regex.Match(selector, "([1-9]{1}[0-9]*|-[1-9]{1}[0-9]*|0)").Success || !int.TryParse(selector, out index))
                throw new InvalidPathException("Expected index as a number got [" + selector + "]");
            this.Index = index;
        }
        public int Index { get; private set; }
        public int CurrentIndexCountDown { get; set; }

        public override GameObject MatchCondition(GameObject gameObjectToCheck, bool enabled)
        {
            CurrentIndexCountDown--;
            return CurrentIndexCountDown >= 0 ? null : gameObjectToCheck;
        }
    }
    public class OldPathSelector
    {
        public OldPathSelector(string path)
        {
            this.FirstBound = processPath(path);
        }

        public OldBoundCondition FirstBound { get; private set; }

        private OldBoundCondition processPath(string path)
        {
            List<char> escapedCharacters;
            path = eliminateEscapedCharacters(path, out escapedCharacters);

            var rawConditions = getRawConditions(path);
            OldBoundCondition prevBoundCondition = null;
            OldSelectorCondition prevSelectorCondition = null;

            var parsedConditions = rawConditions.Select(rawCondition =>
             {
                 var boundCondition = processBoundCondition(rawCondition, prevBoundCondition);

                 if (boundCondition == null)
                 {
                     var selector = processSelectorCondition(rawCondition, prevSelectorCondition);
                     if (selector is OldIndexerCondition)
                     {
                         selector.PrevSelector.NextSelector = null;
                         prevBoundCondition.Indexer = (OldIndexerCondition)selector;

                     }
                     else
                     {
                         if (selector.PrevSelector == null) // is first selector
                         {
                             prevBoundCondition.FirstSelector = selector;
                         }

                     }

                     prevSelectorCondition = selector;
                 }
                 else
                 {
                     prevBoundCondition = boundCondition;
                     prevSelectorCondition = null;
                 }

                 return boundCondition;
             }).ToList();
            parsedConditions.RemoveAll(Condition => Condition == null);

            addEscapedCharactersBack(parsedConditions, escapedCharacters);
            return parsedConditions[0];
        }

        private OldBoundCondition processBoundCondition(string rawCondition, OldBoundCondition prevBoundCondition)
        {
            switch (rawCondition)
            {
                case "/": return new OldBoundCondition(rawCondition, OldBoundType.DirectChildren, prevBoundCondition);
                case "//": return new OldBoundCondition(rawCondition, OldBoundType.AnyChildren, prevBoundCondition);
                case "/..": return new OldBoundCondition(rawCondition, OldBoundType.Parent, prevBoundCondition);
                default: return null;
            }
        }
        private OldSelectorCondition processSelectorCondition(string rawCondition, OldSelectorCondition prevSelectorCondition)
        {
            if (rawCondition == "*")
            {
                return new OldAnyCondition(rawCondition, prevSelectorCondition);
            }

            if (rawCondition.StartsWith("["))
            {
                rawCondition = rawCondition.Substring(1, rawCondition.Length - 2);

                if (rawCondition.StartsWith("@"))
                {
                    return new OldPropertyEqualsCondition(rawCondition, prevSelectorCondition);
                }

                if (Enum.GetNames(typeof(FunctionType)).Any(functionName => rawCondition.StartsWith(functionName)))
                {
                    return new OldFunctionCondition(rawCondition, prevSelectorCondition);
                }
                return new OldIndexerCondition(rawCondition, prevSelectorCondition);
            }
            return new OldNameCondition(rawCondition, prevSelectorCondition);
        }

        private string eliminateEscapedCharacters(string text, out List<char> escapedCharacters)
        {
            escapedCharacters = new List<char>();
            StringBuilder cleanedText = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Equals('\\'))
                {
                    if (i == text.Length - 1)
                    {
                        throw new InvalidPathException("Final \\ must be escaped. Add another \\ at the end to escape it");
                    }
                    escapedCharacters.Add(text[i + 1]);
                    cleanedText.Append('!');
                    i++;
                    continue;
                }
                if (text[i].Equals('!'))
                {
                    escapedCharacters.Add(text[i]);
                    cleanedText.Append('!');
                    continue;
                }
                cleanedText.Append(text[i]);
            }
            return cleanedText.ToString();
        }

        private List<string> getRawConditions(string path)
        {
            string[] substrings = Regex.Split(path, "(//)|(/\\.\\.)|(/)");

            // paths that start with / or // first match have first match empty string

            if (!string.IsNullOrEmpty(substrings[0]) || substrings.Length == 1 || substrings[1] == "/..")
            {
                throw new InvalidPathException("Path must start with delimiter / or //");
            }

            if (substrings[substrings.Length - 1] == "/" || substrings[substrings.Length - 1] == "//")
            {
                throw new InvalidPathException("Path must not end with delimiter / or //");
            }
            var rawConditions = new List<string>();
            for (int i = 0; i < substrings.Length; i++)
            {
                if (substrings[i].Equals("//") && substrings[i + 1].StartsWith(".."))
                {
                    throw new InvalidPathException("Expected /, // or /..; got " + substrings[i] + substrings[i + 1]);
                }
                if (substrings[i].Equals("/.."))
                {
                    rawConditions.Add(substrings[i]);
                    continue;
                }
                if (substrings[i].Equals("//") || substrings[i].Equals("/"))
                {
                    rawConditions.Add(substrings[i]);
                    rawConditions.AddRange(parseSelector(substrings[i + 1]));
                    i++;
                    continue;
                }
                if (substrings[i].Equals(""))
                    continue;

                throw new InvalidPathException("Expected /, // or /..; got " + substrings[i]);
            }
            return rawConditions;
        }

        private void addEscapedCharactersBack(List<OldBoundCondition> pathSetCorrectly, List<char> escapedCharacters)
        {
            int counter = 0;
            for (int i = 0; i < pathSetCorrectly.Count; i++)
            {
                var SelectorCondition = pathSetCorrectly[i].FirstSelector;
                while (SelectorCondition != null)
                {
                    if (SelectorCondition.Type == OldSelectorType.PropertyEquals)
                    {
                        var propValue = (SelectorCondition as OldPropertyEqualsCondition).PropertyValue;
                        changeEscapedCharacterAndIncreaseCounter(escapedCharacters, counter, ref propValue);
                        (SelectorCondition as OldPropertyEqualsCondition).PropertyValue = propValue;
                    }
                    else if (SelectorCondition.Type == OldSelectorType.Function)
                    {
                        var propValue = (SelectorCondition as OldFunctionCondition).PropertyValue;
                        changeEscapedCharacterAndIncreaseCounter(escapedCharacters, counter, ref propValue);
                        (SelectorCondition as OldFunctionCondition).PropertyValue = propValue;
                    }
                    else if (SelectorCondition.Type == OldSelectorType.Name)
                    {
                        var propValue = (SelectorCondition as OldNameCondition).Name;
                        changeEscapedCharacterAndIncreaseCounter(escapedCharacters, counter, ref propValue);
                        (SelectorCondition as OldNameCondition).Name = propValue;
                    }

                    var selector = SelectorCondition.Selector;
                    counter = changeEscapedCharacterAndIncreaseCounter(escapedCharacters, counter, ref selector);
                    SelectorCondition.Selector = selector;

                    SelectorCondition = SelectorCondition.NextSelector;
                }
            }
        }

        private static int changeEscapedCharacterAndIncreaseCounter(List<char> escapedCharacters, int propertyValueCounter, ref string propValue)
        {
            for (int j = 0; j < propValue.Length; j++)
            {
                if (propValue[j].Equals('!'))
                {
                    propValue = propValue.Remove(j, 1).Insert(j, escapedCharacters[propertyValueCounter].ToString());
                    propertyValueCounter++;
                }
            }

            return propertyValueCounter;

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
                        throw new InvalidPathException("Expected object name or *. Got `" + selector + "`");
                    }
                    conditions.Add(substrings[i]);
                }
                else
                {
                    if (!substrings[i].EndsWith("]"))
                    {
                        throw new InvalidPathException("Condition didn't end with ]. Got `" + selector + "`");
                    }
                    conditions.Add("[" + substrings[i]);
                }
            }
            return conditions;
        }
    }
}