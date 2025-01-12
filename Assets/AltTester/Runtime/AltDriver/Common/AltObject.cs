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

using System;
using System.Threading;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using Newtonsoft.Json;

namespace AltTester.AltTesterUnitySDK.Driver
{
    public class AltObject: AltObjectBase
    {
        
        [JsonConstructor]
        public AltObject(string name, int id = 0, int x = 0, int y = 0, int z = 0, int mobileY = 0, string type = "", bool enabled = true, float worldX = 0, float worldY = 0, float worldZ = 0, int idCamera = 0, int transformParentId = 0, int transformId = 0)
            : base(name, id, x, y, z, mobileY, type, enabled, worldX, worldY, worldZ, idCamera, transformParentId, transformId)
        {
        }

        public AltObject(AltObjectBase altObjectBase)
            : base(altObjectBase.name, altObjectBase.id, altObjectBase.x, altObjectBase.y, altObjectBase.z, altObjectBase.mobileY, altObjectBase.type, altObjectBase.enabled, altObjectBase.worldX, altObjectBase.worldY, altObjectBase.worldZ, altObjectBase.idCamera, altObjectBase.transformParentId, altObjectBase.transformId)
        {
            this.name = altObjectBase.name;
            this.id = altObjectBase.id;
            this.x = altObjectBase.x;
            this.y = altObjectBase.y;
            this.z = altObjectBase.z;
            this.mobileY = altObjectBase.mobileY;
            this.type = altObjectBase.type;
            this.enabled = altObjectBase.enabled;
            this.worldX = altObjectBase.worldX;
            this.worldY = altObjectBase.worldY;
            this.worldZ = altObjectBase.worldZ;
            this.idCamera = altObjectBase.idCamera;
            this.transformParentId = altObjectBase.transformParentId;
            this.transformId = altObjectBase.transformId;
            this.CommHandler = altObjectBase.CommHandler;
        }
   
        public new AltObject UpdateObject()
        {
            return base.UpdateObject();
        }

        public new AltObject FindObjectFromObject(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            return base.FindObjectFromObject(by, value, cameraBy, cameraValue, enabled);
        }
    
        public new T GetComponentProperty<T>(string componentName, string propertyName, string assemblyName, int maxDepth = 2)
        {
            return base.GetComponentProperty<T>(componentName, propertyName, assemblyName, maxDepth);
        }
        public new T WaitForComponentProperty<T>(string componentName, string propertyName, T propertyValue, string assemblyName, double timeout = 20, double interval = 0.5, bool getPropertyAsString = false, int maxDepth = 2)
        {
            return base.WaitForComponentProperty(componentName, propertyName, propertyValue, assemblyName, timeout, interval, getPropertyAsString, maxDepth);
        }
        public new void SetComponentProperty(string componentName, string propertyName, object value, string assemblyName)
        {
            base.SetComponentProperty(componentName, propertyName, value, assemblyName);
        }

        public new T CallComponentMethod<T>(string componentName, string methodName, string assemblyName, object[] parameters, string[] typeOfParameters = null)
        {
            return base.CallComponentMethod<T>(componentName, methodName, assemblyName, parameters, typeOfParameters);
        }

        public new AltObject SetText(string text, bool submit = false)
        {
            return base.SetText(text, submit);
        }

        /// <summary>
        /// Click current object
        /// </summary>
        /// <param name="count">Number of times to click</param>
        /// <param name="interval">Interval between clicks in seconds</param>
        /// <param name="wait">Wait for command to finish</param>
        /// <returns>The clicked object</returns>
        public new AltObject Click(int count = 1, float interval = 0.1f, bool wait = true)
        {
            return base.Click(count, interval, wait);
        }

        public new AltObject PointerUp()
        {
            return base.PointerUp();
        }

        public new AltObject PointerDown()
        {
            return base.PointerDown();
        }

        public new AltObject PointerEnter()
        {
            return base.PointerEnter();
        }

        public new AltObject PointerExit()
        {
            return base.PointerExit();
        }

        /// <summary>
        /// Tap current object
        /// </summary>
        /// <param name="count">Number of taps</param>
        /// <param name="interval">Interval in seconds</param>
        /// <param name="wait">Wait for command to finish</param>
        /// <returns>The tapped object</returns>
        public new AltObject Tap(int count = 1, float interval = 0.1f, bool wait = true)
        {
            return base.Tap(count, interval, wait);
        }

        public new System.Collections.Generic.List<AltComponent> GetAllComponents()
        {
           return base.GetAllComponents();
        }

        public new System.Collections.Generic.List<AltProperty> GetAllProperties(AltComponent altComponent, AltPropertiesSelections altPropertiesSelections = AltPropertiesSelections.ALLPROPERTIES)
        {
            return base.GetAllProperties(altComponent, altPropertiesSelections);
        }

        public new System.Collections.Generic.List<AltProperty> GetAllFields(AltComponent altComponent, AltFieldsSelections altFieldsSelections = AltFieldsSelections.ALLFIELDS)
        {
            return base.GetAllFields(altComponent, altFieldsSelections);
        }

        public new System.Collections.Generic.List<string> GetAllMethods(AltComponent altComponent, AltMethodSelection methodSelection = AltMethodSelection.ALLMETHODS)
        {
            return base.GetAllMethods(altComponent, methodSelection);
        }
        public new T GetVisualElementProperty<T>(string propertyName)
        {
           return base.GetVisualElementProperty<T>(propertyName);
        }
    }
}
