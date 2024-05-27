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

namespace AltTester.AltTesterUnitySDK.Driver.Commands
{
    public class AltBaseFindObjects : AltCommandReturningAltElement
    {
        public AltBaseFindObjects(IDriverCommunication commHandler) : base(commHandler)
        {
        }
        protected string SetPath(By by, string value)
        {
            string path = "";
            switch (by)
            {
                case By.TAG:
                    path = "//*[@tag=" + value + "]";
                    break;
                case By.LAYER:
                    path = "//*[@layer=" + value + "]";
                    break;
                case By.NAME:
                    path = "//" + value;
                    break;
                case By.COMPONENT:
                    path = "//*[@component=" + value + "]";
                    break;
                case By.PATH:
                    path = value;
                    break;
                case By.ID:
                    path = "//*[@id=" + value + "]";
                    break;
                case By.TEXT:
                    path = "//*[@text=" + value + "]";
                    break;
            }
            return path;
        }
        protected string SetPathContains(By by, string value)
        {
            string path = "";
            switch (by)
            {
                case By.TAG:
                    path = "//*[contains(@tag," + value + ")]";
                    break;
                case By.LAYER:
                    path = "//*[contains(@layer," + value + ")]";
                    break;
                case By.NAME:
                    path = "//*[contains(@name," + value + ")]";
                    break;
                case By.COMPONENT:
                    path = "//*[contains(@component," + value + ")]";
                    break;
                case By.PATH:
                    path = value;
                    break;
                case By.ID:
                    path = "//*[contains(@id," + value + ")]";
                    break;
                case By.TEXT:
                    path = "//*[contains(@text," + value + ")]";
                    break;
            }
            return path;
        }
    }
}
