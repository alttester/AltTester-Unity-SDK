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
    public class AltBy 
    {
        public By By { get; set; }
        public string Value { get; set; }

        public AltBy(By by, string value)
        {
            this.By = by;
            this.Value = value;
        }

        public static AltBy Text(string text) => new(By.TEXT, text);
        public static AltBy Name(string name) => new(By.NAME, name);
        public static AltBy Id(string id) => new(By.ID, id);
        public static AltBy Tag(string tag) => new(By.TAG, tag);
        public static AltBy Layer(string layer) => new(By.LAYER, layer);
        public static AltBy Component(string component) => new(By.COMPONENT, component);
        public static AltBy Path(string path) => new(By.PATH, path);        

    }
}
