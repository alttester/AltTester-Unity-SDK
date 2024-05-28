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

using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltGetStringKeyPlayerPrefCommand : AltCommand<AltGetKeyPlayerPrefParams, string>
    {
        public AltGetStringKeyPlayerPrefCommand(AltGetKeyPlayerPrefParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            if (UnityEngine.PlayerPrefs.HasKey(CommandParams.keyName))
            {
                return UnityEngine.PlayerPrefs.GetString(CommandParams.keyName);
            }
            throw new NotFoundException(string.Format("PlayerPrefs key {0} not found", CommandParams.keyName));
        }
    }

    class AltGetFloatKeyPlayerPrefCommand : AltCommand<AltGetKeyPlayerPrefParams, float>
    {
        public AltGetFloatKeyPlayerPrefCommand(AltGetKeyPlayerPrefParams cmdParams) : base(cmdParams)
        {
        }

        public override float Execute()
        {
            if (UnityEngine.PlayerPrefs.HasKey(CommandParams.keyName))
            {
                return UnityEngine.PlayerPrefs.GetFloat(CommandParams.keyName);
            }
            throw new NotFoundException(string.Format("PlayerPrefs key {0} not found", CommandParams.keyName));
        }
    }

    class AltGetIntKeyPlayerPrefCommand : AltCommand<AltGetKeyPlayerPrefParams, int>
    {
        public AltGetIntKeyPlayerPrefCommand(AltGetKeyPlayerPrefParams cmdParams) : base(cmdParams)
        {
        }

        public override int Execute()
        {
            if (UnityEngine.PlayerPrefs.HasKey(CommandParams.keyName))
            {
                return UnityEngine.PlayerPrefs.GetInt(CommandParams.keyName);
            }
            throw new NotFoundException(string.Format("PlayerPrefs key {0} not found", CommandParams.keyName));
        }
    }
}
