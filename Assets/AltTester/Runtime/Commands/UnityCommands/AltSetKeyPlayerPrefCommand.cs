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
    class AltSetKeyPlayerPrefCommand : AltCommand<AltSetKeyPlayerPrefParams, string>
    {
        public AltSetKeyPlayerPrefCommand(AltSetKeyPlayerPrefParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            switch (CommandParams.keyType)
            {
                case PlayerPrefKeyType.String:
                    UnityEngine.PlayerPrefs.SetString(CommandParams.keyName, CommandParams.stringValue);
                    break;
                case PlayerPrefKeyType.Float:
                    UnityEngine.PlayerPrefs.SetFloat(CommandParams.keyName, CommandParams.floatValue);
                    break;
                case PlayerPrefKeyType.Int:
                    UnityEngine.PlayerPrefs.SetInt(CommandParams.keyName, CommandParams.intValue);
                    break;
            }
            return "Ok";
        }
    }
}
