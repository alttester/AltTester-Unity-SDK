/*
    Copyright(C) 2025 Altom Consulting

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


using Newtonsoft.Json;

namespace AltTester.AltTesterUnitySDK.Driver
{
    public class AltObjectUnreal: AltObjectBase
    {
        [JsonConstructor]
        public AltObjectUnreal(string name, int id = 0, int x = 0, int y = 0, int z = 0, int mobileY = 0, string type = "", bool enabled = true, float worldX = 0, float worldY = 0, float worldZ = 0, int idCamera = 0, int transformParentId = 0, int transformId = 0)
            : base(name, id, x, y, z, mobileY, type, enabled, worldX, worldY, worldZ, idCamera, transformParentId, transformId)
        {
        }

        private AltObjectUnreal(AltObject altObject)
            : base(altObject.name, altObject.id, altObject.x, altObject.y, altObject.z, altObject.mobileY, altObject.type, altObject.enabled, altObject.worldX, altObject.worldY, altObject.worldZ, altObject.idCamera, altObject.transformParentId, altObject.transformId)
        {
            this.CommHandler = altObject.CommHandler;
        } 

        public new AltObjectUnreal UpdateObject()
        {
            return AltObjectUnreal.Create(base.UpdateObject());
        }

        public static AltObjectUnreal Create(AltObject altObject)
        {
            if (altObject == null)
            {
                return null;
            }

            return new AltObjectUnreal(altObject);
        }

        public AltObjectUnreal Click()
        {
            return AltObjectUnreal.Create(base.Click());
        }

        public T GetComponentProperty<T>(string componentName, string propertyName)
        {
            return base.GetComponentProperty<T>(componentName, propertyName, "", 2);
        }

        public T WaitForComponentProperty<T>(string componentName, string propertyName, T propertyValue, double timeout = 20, double interval = 0.5, bool getPropertyAsString = false)
        {
            return base.WaitForComponentProperty(componentName, propertyName, propertyValue, "", timeout, interval, getPropertyAsString, 2);
        }

        public void SetComponentProperty(string componentName, string propertyName, object value)
        {
            base.SetComponentProperty(componentName, propertyName, value, "");
        }

        public T CallComponentMethod<T>(string componentName, string methodName, object[] parameters, string[] typeOfParameters = null)
        {
            return base.CallComponentMethod<T>(componentName, methodName, "", parameters, typeOfParameters);
        }

        public AltObjectUnreal SetText(string text)
        {
            return AltObjectUnreal.Create(base.SetText(text, false));
        }
    }
}
