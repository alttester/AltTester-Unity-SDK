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

namespace AltTester.AltTesterUnitySDK.Driver
{
    public class AltObjectLight
    {
        public string name;
        public int id;
        public bool enabled;
        public int idCamera;
        public int transformParentId;
        public int transformId;

        public AltObjectLight(string name, int id = 0, bool enabled = true, int idCamera = 0, int transformParentId = 0, int transformId = 0)
        {
            this.name = name;
            this.id = id;
            this.enabled = enabled;
            this.idCamera = idCamera;
            this.transformParentId = transformParentId;
            this.transformId = transformId;
        }
    }
}
