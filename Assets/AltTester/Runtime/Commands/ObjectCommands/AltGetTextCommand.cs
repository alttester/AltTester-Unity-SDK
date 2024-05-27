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
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltGetTextCommand : AltReflectionMethodsCommand<AltGetTextParams, string>
    {
        static readonly AltObjectProperty[] textProperties =
        {
            new AltObjectProperty("UnityEngine.UI.Text", "text"),
            new AltObjectProperty("UnityEngine.UI.InputField", "text"),
            new AltObjectProperty("TMPro.TMP_Text", "text", "Unity.TextMeshPro"),
            new AltObjectProperty("TMPro.TMP_InputField", "text", "Unity.TextMeshPro")
        };

        public AltGetTextCommand(AltGetTextParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            Exception exception = null;

            foreach (var property in textProperties)
            {
                try
                {
                    System.Type type = GetType(property.Component, property.Assembly);
                    return GetValueForMember(CommandParams.altObject, property.Property.Split('.'), type) as string;
                }
                catch (PropertyNotFoundException ex)
                {
                    exception = ex;
                }
                catch (ComponentNotFoundException ex)
                {
                    exception = ex;
                }
                catch (AssemblyNotFoundException ex)
                {
                    exception = ex;
                }
            }

            if (exception != null) throw exception;
            throw new Exception("Something went wrong"); // should not reach this point
        }
    }
}
