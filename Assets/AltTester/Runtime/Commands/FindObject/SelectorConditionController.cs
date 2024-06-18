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
using System.Linq;
using AltTester.AltTesterUnitySDK.Driver;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public static class SelectorConditionController
    {

        public static GameObject MatchCondition(SelectorCondition selectorCondition, GameObject gameObjectToCheck, bool enabled)
        {
            switch (selectorCondition.Type)
            {
                case SelectorType.Any:
                    return matchConditionForAny(selectorCondition as AnyCondition, gameObjectToCheck, enabled);
                case SelectorType.Name:
                    NameCondition nameCondition = new NameCondition(selectorCondition.Selector);
                    return matchConditionForName(nameCondition, gameObjectToCheck, enabled);
                case SelectorType.Function:
                    FunctionCondition functionCondition = new FunctionCondition(selectorCondition.Selector);
                    return matchConditionForFunction(functionCondition, gameObjectToCheck, enabled);
                case SelectorType.PropertyEquals:
                    PropertyEqualsCondition propertyEqualsCondition = new PropertyEqualsCondition(selectorCondition.Selector);
                    return matchConditionForProperty(propertyEqualsCondition, gameObjectToCheck, enabled);
                case SelectorType.Indexer:
                    IndexerCondition indexerCondition = new IndexerCondition(selectorCondition.Selector);
                    return matchConditionForIndexer(indexerCondition, gameObjectToCheck, enabled);
                default:
                    return null;
            }
        }
        private static GameObject matchConditionForAny(AnyCondition anyCondition, GameObject gameObjectToCheck, bool enabled)
        {
            return gameObjectToCheck;
        }
        private static GameObject matchConditionForName(NameCondition nameCondition, GameObject gameObjectToCheck, bool enabled)
        {
            return gameObjectToCheck.name.Equals(nameCondition.Name) ? gameObjectToCheck : null;
        }
        private static GameObject matchConditionForProperty(PropertyEqualsCondition propertyEqualsCondition, GameObject gameObjectToCheck, bool enabled)
        {
            switch (propertyEqualsCondition.Property)
            {
                case PropertyType.id:
                    if (System.Text.RegularExpressions.Regex.Match(propertyEqualsCondition.PropertyValue, "^([1-9]{1}[0-9]*|-[1-9]{1}[0-9]*|0)$").Success)
                    {
                        var id = System.Convert.ToInt32(propertyEqualsCondition.PropertyValue);
                        return gameObjectToCheck.GetInstanceID() == id ? gameObjectToCheck : null;
                    }
                    var component = gameObjectToCheck.GetComponent<AltId>();
                    if (component != null)
                    {
                        return component.altID.Equals(propertyEqualsCondition.PropertyValue) ? gameObjectToCheck : null;
                    }
                    return null;
                case PropertyType.name:
                    return gameObjectToCheck.name.Equals(propertyEqualsCondition.PropertyValue) ? gameObjectToCheck : null;
                case PropertyType.tag:
                    try
                    {
                        return gameObjectToCheck.tag.Equals(propertyEqualsCondition.PropertyValue) ? gameObjectToCheck : null;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                case PropertyType.layer:
                    int layerId = LayerMask.NameToLayer(propertyEqualsCondition.PropertyValue);
                    return gameObjectToCheck.layer.Equals(layerId) ? gameObjectToCheck : null;
                case PropertyType.component:
                    var componentName = propertyEqualsCondition.PropertyValue.Split(new string[] { "." }, System.StringSplitOptions.None).Last();
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
                    return getText(gameObjectToCheck).Equals(propertyEqualsCondition.PropertyValue) ? gameObjectToCheck : null;
            }
            return null;
        }
        private static GameObject matchConditionForIndexer(IndexerCondition indexerCondition, GameObject gameObjectToCheck, bool enabled)
        {
            indexerCondition.CurrentIndexCountDown--;
            return indexerCondition.CurrentIndexCountDown >= 0 ? null : gameObjectToCheck;
        }
        private static GameObject matchConditionForFunction(FunctionCondition functionCondition, GameObject gameObjectToCheck, bool enabled)
        {
            switch (functionCondition.Function)
            {
                case FunctionType.contains:
                    switch (functionCondition.Property)
                    {
                        case PropertyType.id:
                            if (System.Text.RegularExpressions.Regex.Match(functionCondition.PropertyValue, "^([1-9]{1}[0-9]*|-[1-9]{1}[0-9]*|0)$").Success)
                            {
                                return gameObjectToCheck.GetInstanceID().ToString().Contains(functionCondition.PropertyValue) ? gameObjectToCheck : null;
                            }
                            var component = gameObjectToCheck.GetComponent<AltId>();
                            if (component != null)
                            {
                                return component.altID.Contains(functionCondition.PropertyValue) ? gameObjectToCheck : null;
                            }
                            return null;
                        case PropertyType.name:
                            return gameObjectToCheck.name.Contains(functionCondition.PropertyValue) ? gameObjectToCheck : null;
                        case PropertyType.tag:
                            return gameObjectToCheck.tag.Contains(functionCondition.PropertyValue) ? gameObjectToCheck : null;
                        case PropertyType.layer:
                            string layerNm = LayerMask.LayerToName(gameObjectToCheck.layer);
                            return layerNm.Contains(functionCondition.PropertyValue) ? gameObjectToCheck : null;
                        case PropertyType.component:
                            var componentName = functionCondition.PropertyValue.Split(new string[] { "." }, System.StringSplitOptions.None).Last();
                            var list = gameObjectToCheck.GetComponents(typeof(UnityEngine.Component));
                            for (int i = 0; i < list.Length; i++)
                            {
                                if (list[i].GetType().Name.Contains(componentName))
                                {
                                    return gameObjectToCheck;
                                }
                            }
                            return null;
                        case PropertyType.text:
                            return getText(gameObjectToCheck).Contains(functionCondition.PropertyValue) ? gameObjectToCheck : null;
                    }
                    break;
            }
            return null;
        }
        private static string getText(UnityEngine.GameObject objectToCheck)
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
}
