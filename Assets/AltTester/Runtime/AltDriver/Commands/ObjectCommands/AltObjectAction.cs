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

namespace AltTester.AltTesterUnitySDK.Driver.Commands
{
    [Obsolete]
    public struct AltObjectAction
    {
        public string Component;
        public string Method;
        public string Parameters;
        public string TypeOfParameters;

        public AltObjectAction(string component = "", string method = "", string parameters = "", string typeOfParameters = "", string assembly = "")
        {
            Component = component;
            Method = method;
            Parameters = parameters;
            TypeOfParameters = typeOfParameters;
            Assembly = assembly;
        }


        public string Assembly;
    }
}
