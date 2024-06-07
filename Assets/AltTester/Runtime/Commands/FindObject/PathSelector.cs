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
    public class SelectorConditionController
    {

        public GameObject MatchCondition(SelectorCondition selectorCondition, GameObject gameObjectToCheck, bool enabled)
        {
            if (selectorCondition.GetType() == typeof(IndexerCondition))
            {

            }
            return null;
        }
        public GameObject MatchConditionForIndexer(IndexerCondition indexerCondition, GameObject gameObjectToCheck, bool enabled)
        {
            indexerCondition.CurrentIndexCountDown--;
            return indexerCondition.CurrentIndexCountDown >= 0 ? null : gameObjectToCheck;
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

    // public class AnyCondition : SelectorCondition
    // {
    //     public AnyCondition(string selector, SelectorCondition previousSelector) : base(selector, SelectorType.Any, previousSelector)
    //     {
    //     }

    //     public override GameObject MatchCondition(GameObject gameObjectToCheck, bool enabled)
    //     {
    //         return gameObjectToCheck;
    //     }
    // }

    // public class NameCondition : SelectorCondition
    // {
    //     public NameCondition(string selector, SelectorCondition previousSelector) : base(selector, SelectorType.Name, previousSelector)
    //     {
    //         this.Name = selector;
    //     }
    //     public string Name { get; set; }

    //     public override GameObject MatchCondition(GameObject gameObjectToCheck, bool enabled)
    //     {
    //         return gameObjectToCheck.name.Equals(Name) ? gameObjectToCheck : null;
    //     }
    // }

    // public class PropertyEqualsCondition : SelectorCondition
    // {
    //     /// <summary>
    //     /// 
    //     /// </summary>
    //     /// <param name="selector">@propertyName=propertyvalue</param>
    //     /// <returns></returns>
    //     public PropertyEqualsCondition(string selector, SelectorCondition previousSelector) : base(selector, SelectorType.PropertyEquals, previousSelector)
    //     {
    //         var delimiterPos = selector.IndexOf("=");
    //         if (delimiterPos < 0) throw new InvalidPathException("Expected property selector format `@propertyName=propertyvalue` Got " + selector);
    //         var propvalue = selector.Substring(1, delimiterPos - 1);
    //         this.Property = GetPropertyType(propvalue);
    //         this.PropertyValue = selector.Substring(delimiterPos + 1);
    //     }
    //     public PropertyType Property { get; private set; }
    //     public string PropertyValue { get; set; }

    //     public override GameObject MatchCondition(GameObject gameObjectToCheck, bool enabled)
    //     {
    //         switch (Property)
    //         {
    //             case PropertyType.id:
    //                 if (System.Text.RegularExpressions.Regex.Match(PropertyValue, "^([1-9]{1}[0-9]*|-[1-9]{1}[0-9]*|0)$").Success)
    //                 {
    //                     var id = System.Convert.ToInt32(PropertyValue);
    //                     return gameObjectToCheck.GetInstanceID() == id ? gameObjectToCheck : null;
    //                 }
    //                 var component = gameObjectToCheck.GetComponent<AltId>();
    //                 if (component != null)
    //                 {
    //                     return component.altID.Equals(PropertyValue) ? gameObjectToCheck : null;
    //                 }
    //                 return null;
    //             case PropertyType.name:
    //                 return gameObjectToCheck.name.Equals(PropertyValue) ? gameObjectToCheck : null;
    //             case PropertyType.tag:
    //                 try
    //                 {
    //                     return gameObjectToCheck.tag.Equals(PropertyValue) ? gameObjectToCheck : null;
    //                 }
    //                 catch (Exception)
    //                 {
    //                     return null;
    //                 }
    //             case PropertyType.layer:
    //                 int layerId = LayerMask.NameToLayer(PropertyValue);
    //                 return gameObjectToCheck.layer.Equals(layerId) ? gameObjectToCheck : null;
    //             case PropertyType.component:
    //                 var componentName = PropertyValue.Split(new string[] { "." }, System.StringSplitOptions.None).Last();
    //                 var list = gameObjectToCheck.GetComponents(typeof(UnityEngine.Component));
    //                 for (int i = 0; i < list.Length; i++)
    //                 {
    //                     try
    //                     {
    //                         if (componentName.Equals(list[i].GetType().Name))
    //                         {
    //                             return gameObjectToCheck;
    //                         }
    //                     }
    //                     catch (System.NullReferenceException)
    //                     {
    //                         continue;
    //                     }
    //                 }
    //                 return null;
    //             case PropertyType.text:
    //                 return GetText(gameObjectToCheck).Equals(PropertyValue) ? gameObjectToCheck : null;
    //         }
    //         return null;
    //     }
    // }

    // public class FunctionCondition : SelectorCondition
    // {
    //     /// <summary>
    //     /// 
    //     /// </summary>
    //     /// <param name="selector">functionName(@propertyName=propertyvalue)</param>
    //     /// <returns></returns>
    //     public FunctionCondition(string selector, SelectorCondition previousSelector) : base(selector, SelectorType.Function, previousSelector)
    //     {
    //         var delimiterPos = selector.IndexOf(")");
    //         if (delimiterPos != selector.Length - 1) throw new InvalidPathException("Expected property selector format `function(@propertyName,propertyvalue)` Got " + selector);

    //         delimiterPos = selector.IndexOf("(");
    //         if (delimiterPos < 0) throw new InvalidPathException("Expected property selector format `function(@propertyName,propertyvalue)` Got " + selector);

    //         var functionName = selector.Substring(0, delimiterPos);
    //         this.Function = GetFunctionType(functionName);

    //         string condition = selector.Substring(delimiterPos + 1, selector.Length - delimiterPos - 2);

    //         delimiterPos = condition.IndexOf(",");
    //         if (delimiterPos < 0) throw new InvalidPathException("Expected property selector format `function(@propertyName,propertyvalue)` Got " + selector);

    //         var propname = condition.Substring(1, delimiterPos - 1);
    //         this.Property = GetPropertyType(propname);
    //         this.PropertyValue = condition.Substring(delimiterPos + 1);
    //     }
    //     public PropertyType Property { get; private set; }
    //     public string PropertyValue { get; set; }
    //     public FunctionType Function { get; private set; }

    //     public override GameObject MatchCondition(GameObject gameObjectToCheck, bool enabled)
    //     {
    //         switch (Function)
    //         {
    //             case FunctionType.contains:
    //                 switch (Property)
    //                 {
    //                     case PropertyType.id:
    //                         if (System.Text.RegularExpressions.Regex.Match(PropertyValue, "^([1-9]{1}[0-9]*|-[1-9]{1}[0-9]*|0)$").Success)
    //                         {
    //                             return gameObjectToCheck.GetInstanceID().ToString().Contains(PropertyValue) ? gameObjectToCheck : null;
    //                         }
    //                         var component = gameObjectToCheck.GetComponent<AltId>();
    //                         if (component != null)
    //                         {
    //                             return component.altID.Contains(PropertyValue) ? gameObjectToCheck : null;
    //                         }
    //                         return null;
    //                     case PropertyType.name:
    //                         return gameObjectToCheck.name.Contains(PropertyValue) ? gameObjectToCheck : null;
    //                     case PropertyType.tag:
    //                         return gameObjectToCheck.tag.Contains(PropertyValue) ? gameObjectToCheck : null;
    //                     case PropertyType.layer:
    //                         string layerNm = LayerMask.LayerToName(gameObjectToCheck.layer);
    //                         return layerNm.Contains(PropertyValue) ? gameObjectToCheck : null;
    //                     case PropertyType.component:
    //                         var componentName = PropertyValue.Split(new string[] { "." }, System.StringSplitOptions.None).Last();
    //                         var list = gameObjectToCheck.GetComponents(typeof(UnityEngine.Component));
    //                         for (int i = 0; i < list.Length; i++)
    //                         {
    //                             if (list[i].GetType().Name.Contains(componentName))
    //                             {
    //                                 return gameObjectToCheck;
    //                             }
    //                         }
    //                         return null;
    //                     case PropertyType.text:
    //                         return GetText(gameObjectToCheck).Contains(PropertyValue) ? gameObjectToCheck : null;
    //                 }
    //                 break;
    //         }
    //         return null;
    //     }

    // }

    // public class IndexerCondition : SelectorCondition
    // {
    //     public IndexerCondition(string selector, SelectorCondition previousSelector) : base(selector, SelectorType.Indexer, previousSelector)
    //     {
    //         int index;
    //         if (!Regex.Match(selector, "([1-9]{1}[0-9]*|-[1-9]{1}[0-9]*|0)").Success || !int.TryParse(selector, out index))
    //             throw new InvalidPathException("Expected index as a number got [" + selector + "]");
    //         this.Index = index;
    //     }
    //     public int Index { get; private set; }
    //     public int CurrentIndexCountDown { get; set; }

    //     public override GameObject MatchCondition(GameObject gameObjectToCheck, bool enabled)
    //     {
    //         CurrentIndexCountDown--;
    //         return CurrentIndexCountDown >= 0 ? null : gameObjectToCheck;
    //     }
    // }

}
