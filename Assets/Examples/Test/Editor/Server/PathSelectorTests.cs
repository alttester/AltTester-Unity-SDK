/*
    Copyright(C) 2023 Altom Consulting

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

using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Commands;
using NUnit.Framework;

public class PathSelectorTests
{
    [Test]
    public void TestValidSelectors_Any()
    {
        PathSelector selector = new PathSelector("//*");

        Assert.AreEqual(BoundType.AnyChildren, selector.FirstBound.Type);
        Assert.AreEqual("//", selector.FirstBound.Selector);

        var firstCondition = selector.FirstBound.FirstSelector;
        Assert.True(firstCondition is SelectorCondition);
        Assert.AreEqual("*", firstCondition.Selector);
        Assert.AreEqual(SelectorType.Any, firstCondition.Type);
    }
    [Test]
    public void TestValidSelectors_Name()
    {
        PathSelector selector = new PathSelector("//name");

        Assert.AreEqual(BoundType.AnyChildren, selector.FirstBound.Type);
        Assert.AreEqual("//", selector.FirstBound.Selector);

        var firstCondition = selector.FirstBound.FirstSelector;
        Assert.True(firstCondition is SelectorCondition);
        Assert.AreEqual("name", firstCondition.Selector);
        Assert.AreEqual(SelectorType.Name, firstCondition.Type);
    }

    [Test]
    public void TestValidSelectors_Property()
    {
        PathSelector selector = new PathSelector("//name[@id=123]");

        Assert.AreEqual(BoundType.AnyChildren, selector.FirstBound.Type);
        Assert.AreEqual("//", selector.FirstBound.Selector);

        var firstCondition = selector.FirstBound.FirstSelector;
        Assert.True(firstCondition is SelectorCondition);
        Assert.AreEqual("name", firstCondition.Selector);
        Assert.AreEqual(SelectorType.Name, firstCondition.Type);

        var secondCondition = firstCondition.NextSelector;
        Assert.True(secondCondition is SelectorCondition);
        Assert.AreEqual(SelectorType.PropertyEquals, (secondCondition as SelectorCondition).Type);
        Assert.AreEqual("@id=123", secondCondition.Selector);
        Assert.AreEqual(PropertyType.id, (secondCondition as PropertyEqualsCondition).Property);
        Assert.AreEqual("123", (secondCondition as PropertyEqualsCondition).PropertyValue);
    }

    [Test]
    public void TestValidSelectors_Function()
    {
        PathSelector selector = new PathSelector("//name[contains(@text,abc)]");

        Assert.AreEqual(BoundType.AnyChildren, selector.FirstBound.Type);
        Assert.AreEqual("//", selector.FirstBound.Selector);

        var firstCondition = selector.FirstBound.FirstSelector;
        Assert.True(firstCondition is SelectorCondition);
        Assert.AreEqual("name", firstCondition.Selector);
        Assert.AreEqual(SelectorType.Name, firstCondition.Type);

        var secondCondition = firstCondition.NextSelector;
        Assert.True(secondCondition is SelectorCondition);
        Assert.AreEqual(SelectorType.Function, (secondCondition as SelectorCondition).Type);
        Assert.AreEqual("contains(@text,abc)", secondCondition.Selector);
        Assert.AreEqual(PropertyType.text, (secondCondition as FunctionCondition).Property);
        Assert.AreEqual("abc", (secondCondition as FunctionCondition).PropertyValue);
        Assert.AreEqual(FunctionType.contains, (secondCondition as FunctionCondition).Function);
    }

    [Test]
    public void TestValidSelectors_Indexer()
    {
        PathSelector selector = new PathSelector("//name[1]");

        Assert.AreEqual(BoundType.AnyChildren, selector.FirstBound.Type);
        Assert.AreEqual("//", selector.FirstBound.Selector);

        var firstCondition = selector.FirstBound.FirstSelector;
        Assert.True(firstCondition is SelectorCondition);
        Assert.AreEqual("name", firstCondition.Selector);
        Assert.AreEqual(SelectorType.Name, firstCondition.Type);
        Assert.IsNull(firstCondition.NextSelector);


        Assert.AreEqual("1", selector.FirstBound.Indexer.Selector);
        Assert.AreEqual(1, selector.FirstBound.Indexer.Index);
    }

    [TestCase("", "Path must start with delimiter / or //")]
    [TestCase("///", "Expected object name or *. Got ``")]
    [TestCase("//[@text=123]", "Expected object name or *. Got `[@text=123]`")]
    [TestCase("//name[contains(@invalid,2)", "Condition didn't end with ]. Got `name[contains(@invalid,2)`")]
    [TestCase("//name[invalid]", "Expected index as a number got [invalid]")]
    [TestCase("//name[@invalid=22]", "Invalid property type 'invalid'. Expected one of: id,name,tag,layer,component,text")]
    [TestCase("//[1]", "Expected object name or *. Got `[1]`")]
    [TestCase("//test////name", "Expected object name or *. Got ``")]
    [TestCase("//test///name", "Expected object name or *. Got ``")]
    [TestCase("//name//..", "Expected /, // or /..; got //..")]
    [TestCase("//name\\", "Final \\ must be escaped. Add another \\ at the end to escape it")]
    [TestCase("//*[contains(@name,Pla]", "Expected property selector format `function(@propertyName,propertyvalue)` Got contains(@name,Pla")]
    public void TestInvalidPathSelectors(string path, string exceptionMessage)
    {
        var invalidPathException = Assert.Throws<InvalidPathException>(() => new PathSelector(path));
        Assert.That(invalidPathException.Message, Is.EqualTo(exceptionMessage));

    }
    [TestCase("//\\[name\\]", "[name]")]
    [TestCase("//\\/name", "/name")]
    [TestCase("//na\\/me", "na/me")]
    [TestCase("//name\\/", "name/")]
    [TestCase("//na\\;me", "na;me")]
    [TestCase("//\\;name", ";name")]
    [TestCase("//name\\;", "name;")]
    [TestCase("//\\\\na\\\\me\\\\", "\\na\\me\\")]
    [TestCase("//\\&na\\&me\\&", "&na&me&")]
    [TestCase("//\\!na\\!me\\!", "!na!me!")]
    public void TestEscapedCharactersInsideName(string path, string value)
    {
        PathSelector selector = new PathSelector(path);


        Assert.AreEqual(BoundType.AnyChildren, selector.FirstBound.Type);
        Assert.AreEqual("//", selector.FirstBound.Selector);

        var firstCondition = selector.FirstBound.FirstSelector;
        Assert.True(firstCondition is SelectorCondition);
        Assert.AreEqual(value, firstCondition.Selector);
        Assert.AreEqual(value, (firstCondition as NameCondition).Name);
        Assert.AreEqual(SelectorType.Name, firstCondition.Type);
    }

    [TestCase("//*[@layer=\\[name\\]]", "[name]")]
    [TestCase("//*[@tag=\\/na\\/me\\/]", "/na/me/")]
    [TestCase("//*[@text=\\;na\\;me\\;]", ";na;me;")]
    [TestCase("//*[@component=\\\\na\\\\me\\\\]", "\\na\\me\\")]
    [TestCase("//*[@name=\\&na\\&me\\&]", "&na&me&")]
    [TestCase("//*[@tag=\\!na\\!me\\!]", "!na!me!")]

    public void TestEscapedCharactersInsideSelectors(string path, string expectedResult)
    {
        PathSelector selector = new PathSelector(path);

        Assert.AreEqual(BoundType.AnyChildren, selector.FirstBound.Type);
        Assert.AreEqual("//", selector.FirstBound.Selector);

        var firstCondition = selector.FirstBound.FirstSelector;
        Assert.True(firstCondition is SelectorCondition);
        Assert.AreEqual("*", firstCondition.Selector);
        Assert.AreEqual(SelectorType.Any, firstCondition.Type);

        var secondCondition = firstCondition.NextSelector;
        Assert.True(secondCondition is SelectorCondition);
        Assert.AreEqual(SelectorType.PropertyEquals, (secondCondition as SelectorCondition).Type);
        Assert.AreEqual(expectedResult, (secondCondition as PropertyEqualsCondition).PropertyValue);

    }
    [TestCase("//*[contains(@layer,\\[name\\])]", "[name]")]
    [TestCase("//*[contains(@tag,\\/na\\/me\\/)]", "/na/me/")]
    [TestCase("//*[contains(@text,\\;na\\;me\\;)]", ";na;me;")]
    [TestCase("//*[contains(@component,\\\\na\\\\me\\\\)]", "\\na\\me\\")]
    [TestCase("//*[contains(@name,\\&na\\&me\\&)]", "&na&me&")]
    [TestCase("//*[contains(@tag,\\!na\\!me\\!)]", "!na!me!")]
    [TestCase("//*[contains(@tag,\\(na\\)me\\()]", "(na)me(")]

    public void TestEscapedCharactersInsideFunctions(string path, string expectedResult)
    {
        PathSelector selector = new PathSelector(path);

        Assert.AreEqual(BoundType.AnyChildren, selector.FirstBound.Type);
        Assert.AreEqual("//", selector.FirstBound.Selector);

        var firstCondition = selector.FirstBound.FirstSelector;

        Assert.AreEqual("*", firstCondition.Selector);
        Assert.AreEqual(SelectorType.Any, firstCondition.Type);

        var secondCondition = firstCondition.NextSelector;
        Assert.True(secondCondition is SelectorCondition);
        Assert.AreEqual(SelectorType.Function, (secondCondition as SelectorCondition).Type);
        Assert.AreEqual(expectedResult, (secondCondition as FunctionCondition).PropertyValue);

    }

}
