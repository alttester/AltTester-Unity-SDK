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
using System.Text.RegularExpressions;

namespace AltTester.AltTesterUnitySDK.Driver
{
    public enum BoundType
    {
        DirectChildren,
        AnyChildren,
        Parent
    }
    public enum SelectorType
    {
        Any, // *
        Name,  // name
        Function,//[functionName(@propertyName,propertyValue)] 
        PropertyEquals, //[@propertyName=propertyValue]
        Indexer, // [n]
    }
    public enum FunctionType
    {
        contains
    }
    public enum PropertyType
    {
        id,
        name,
        tag,
        layer,
        component,
        text
    }
    public class BoundCondition
    {
        public BoundCondition(string selector, BoundType type)
        {
            Type = type;
            Selector = selector;
        }
        public string Selector { get; set; }
        public BoundType Type { get; protected set; }
        public IndexerCondition Indexer { get; set; }
        public List<SelectorCondition> Selectors { get; set; }
    }

    public class SelectorCondition
    {
        public SelectorCondition(string selector, SelectorType type)
        {
            Selector = selector;
            Type = type;
        }
        public string Selector { get; set; }
        public SelectorType Type { get; protected set; }
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
    }

    public class AnyCondition : SelectorCondition
    {
        public AnyCondition(string selector) : base(selector, SelectorType.Any)
        {
        }
    }

    public class NameCondition : SelectorCondition
    {
        public NameCondition(string selector) : base(selector, SelectorType.Name)
        {
            this.Name = selector;
        }
        public string Name { get; set; }
    }

    public class PropertyEqualsCondition : SelectorCondition
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector">@propertyName=propertyvalue</param>
        /// <returns></returns>
        public PropertyEqualsCondition(string selector) : base(selector, SelectorType.PropertyEquals)
        {
            var delimiterPos = selector.IndexOf("=");
            if (delimiterPos < 0) throw new InvalidPathException("Expected property selector format `@propertyName=propertyvalue` Got " + selector);
            var propvalue = selector.Substring(1, delimiterPos - 1);
            this.Property = GetPropertyType(propvalue);
            this.PropertyValue = selector.Substring(delimiterPos + 1);
        }
        public PropertyType Property { get; private set; }
        public string PropertyValue { get; set; }

    }

    public class FunctionCondition : SelectorCondition
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector">functionName(@propertyName=propertyvalue)</param>
        /// <returns></returns>
        public FunctionCondition(string selector) : base(selector, SelectorType.Function)
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

    }
    public class IndexerCondition : SelectorCondition
    {
        public IndexerCondition(string selector) : base(selector, SelectorType.Indexer)
        {
            int index;
            if (!Regex.Match(selector, "([1-9]{1}[0-9]*|-[1-9]{1}[0-9]*|0)").Success || !int.TryParse(selector, out index))
                throw new InvalidPathException("Expected index as a number got [" + selector + "]");
            this.Index = index;
        }
        public int Index { get; private set; }
        public int CurrentIndexCountDown { get; set; }

    }


}
