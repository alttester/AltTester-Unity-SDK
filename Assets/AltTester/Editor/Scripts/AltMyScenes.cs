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

namespace AltTester.AltTesterUnitySDK.Editor
{
    [System.Serializable]
    public class AltMyScenes
    {
        public bool _toBeBuilt;
        public string _path;
        public int _buildIndex;

        public AltMyScenes(bool beBuilt, string path, int buildIndex)
        {
            _toBeBuilt = beBuilt;
            _path = path;
            _buildIndex = buildIndex;
        }

        public bool ToBeBuilt
        {
            get { return _toBeBuilt; }
            set { _toBeBuilt = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public int BuildScene
        {
            get { return _buildIndex; }
            set { _buildIndex = value; }
        }
    }
}
