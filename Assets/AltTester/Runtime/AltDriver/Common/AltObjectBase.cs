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

namespace AltTester.AltTesterUnitySDK.Driver
{
    public class AltObjectBase
    {
        public string name;
        public int id;
        public int x;
        public int y;
        public int z;
        public int mobileY;
        public string type;
        public bool enabled;
        public float worldX;
        public float worldY;
        public float worldZ;
        public int idCamera;
        public int transformParentId;
        public int transformId;
        [Newtonsoft.Json.JsonIgnore]
        public IDriverCommunication CommHandler;

        public AltObjectBase(string name, int id = 0, int x = 0, int y = 0, int z = 0, int mobileY = 0, string type = "", bool enabled = true, float worldX = 0, float worldY = 0, float worldZ = 0, int idCamera = 0, int transformParentId = 0, int transformId = 0)
        {
            this.name = name;
            this.id = id;
            this.x = x;
            this.y = y;
            this.z = z;
            this.mobileY = mobileY;
            this.type = type;
            this.enabled = enabled;
            this.worldX = worldX;
            this.worldY = worldY;
            this.worldZ = worldZ;
            this.idCamera = idCamera;
            this.transformParentId = transformParentId;
            this.transformId = transformId;
        }

        protected AltObject UpdateObject()
        {
            var altObject = new AltFindObject(CommHandler, By.ID, this.id.ToString(), By.NAME, "", this.enabled).Execute();
            x = altObject.x;
            y = altObject.y;
            z = altObject.z;
            id = altObject.id;
            name = altObject.name;
            mobileY = altObject.mobileY;
            type = altObject.type;
            enabled = altObject.enabled;
            worldX = altObject.worldX;
            worldY = altObject.worldY;
            worldZ = altObject.worldZ;
            idCamera = altObject.idCamera;
            transformParentId = altObject.transformParentId;
            transformId = altObject.transformId;

            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return new AltObject(this);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            AltObject altObject = (AltObject)obj;
            return id == altObject.id;
        }

        public override int GetHashCode()
        {
            return id;
        }        

        public AltObject GetParent()
        {
            var altObject = new AltFindObject(CommHandler, By.PATH, "//*[@id=" + this.id + "]/..", By.NAME, "", true).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altObject;
        }

        [Obsolete("getParent is deprecated, please use GetParent instead.")]
        protected AltObject getParent()
        {
            var altObject = new AltFindObject(CommHandler, By.PATH, "//*[@id=" + this.id + "]/..", By.NAME, "", true).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altObject;
        }
        protected AltObject FindObjectFromObject(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            var findObject = new AltFindObjectFromObject(CommHandler, by, value, cameraBy, cameraValue, enabled, new AltObject(this)).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return findObject;
        }
        public AltVector2 GetScreenPosition()
        {
            return new AltVector2(x, y);
        }

        [Obsolete("getScreenPosition is deprecated, please use GetScreenPosition instead.")]
        protected AltVector2 getScreenPosition()
        {
            return new AltVector2(x, y);
        }
        public AltVector3 GetWorldPosition()
        {
            return new AltVector3(worldX, worldY, worldZ);
        }

        [Obsolete("getWorldPosition is deprecated, please use GetWorldPosition instead.")]
        protected AltVector3 getWorldPosition()
        {
            return new AltVector3(worldX, worldY, worldZ);
        }
        protected T GetComponentProperty<T>(string componentName, string propertyName, string assemblyName, int maxDepth = 2)
        {
            var propertyValue = new AltGetComponentProperty<T>(CommHandler, componentName, propertyName, assemblyName, maxDepth, new AltObject(this)).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return propertyValue;
        }
        protected T WaitForComponentProperty<T>(string componentName, string propertyName, T propertyValue, string assemblyName, double timeout = 20, double interval = 0.5, bool getPropertyAsString = false, int maxDepth = 2)
        {
            var propertyFound = new AltWaitForComponentProperty<T>(CommHandler, componentName, propertyName, propertyValue, assemblyName, timeout, interval, getPropertyAsString, maxDepth, new AltObject(this)).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return propertyFound;
        }
        protected void SetComponentProperty(string componentName, string propertyName, object value, string assemblyName)
        {
            new AltSetComponentProperty(CommHandler, componentName, propertyName, value, assemblyName, new AltObject(this)).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
        }

        protected T CallComponentMethod<T>(string componentName, string methodName, string assemblyName, object[] parameters, string[] typeOfParameters = null)
        {
            var result = new AltCallComponentMethod<T>(CommHandler, componentName, methodName, parameters, typeOfParameters, assemblyName, new AltObject(this)).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return result;
        }

        public string GetText()
        {
            var text = new AltGetText(CommHandler, new AltObject(this)).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return text;
        }

        protected AltObject SetText(string text, bool submit = false)
        {
            var altObject = new AltSetText(CommHandler, new AltObject(this), text, submit).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altObject;
        }

        /// <summary>
        /// Click current object
        /// </summary>
        /// <param name="count">Number of times to click</param>
        /// <param name="interval">Interval between clicks in seconds</param>
        /// <param name="wait">Wait for command to finish</param>
        /// <returns>The clicked object</returns>
        protected AltObject Click(int count = 1, float interval = 0.1f, bool wait = true)
        {
            var altObject = new AltClickElement(CommHandler, new AltObject(this), count, interval, wait).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altObject;
        }

        [Obsolete("PointerUpFromObject is deprecated, please use PointerUp instead.")]
        protected AltObject PointerUpFromObject()
        {
            return PointerUp();
        }

        protected AltObject PointerUp()
        {
            var altObject = new AltPointerUpFromObject(CommHandler, new AltObject(this)).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altObject;
        }

        [Obsolete("PointerDownFromObject is deprecated, please use PointerDown instead.")]
        protected AltObject PointerDownFromObject()
        {
            return PointerDown();
        }

        protected AltObject PointerDown()
        {
            var altObject = new AltPointerDownFromObject(CommHandler, new AltObject(this)).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altObject;
        }

        [Obsolete("PointerEnterObject is deprecated, please use PointerEnter instead.")]
        protected AltObject PointerEnterObject()
        {
            return PointerEnter();
        }

        protected AltObject PointerEnter()
        {
            var altObject = new AltPointerEnterObject(CommHandler, new AltObject(this)).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altObject;
        }

        [Obsolete("PointerExitObject is deprecated, please use PointerExit instead.")]
        protected AltObject PointerExitObject()
        {
            return PointerExit();
        }

        protected AltObject PointerExit()
        {
            var altObject = new AltPointerExitObject(CommHandler, new AltObject(this)).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altObject;
        }

        /// <summary>
        /// Tap current object
        /// </summary>
        /// <param name="count">Number of taps</param>
        /// <param name="interval">Interval in seconds</param>
        /// <param name="wait">Wait for command to finish</param>
        /// <returns>The tapped object</returns>
        protected AltObject Tap(int count = 1, float interval = 0.1f, bool wait = true)
        {
            var altObject = new AltTapElement(CommHandler, new AltObject(this), count, interval, wait).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altObject;
        }

        public System.Collections.Generic.List<AltComponent> GetAllComponents()
        {
            var altObject = new AltGetAllComponents(CommHandler, new AltObject(this)).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altObject;
        }

        protected System.Collections.Generic.List<AltProperty> GetAllProperties(AltComponent altComponent, AltPropertiesSelections altPropertiesSelections = AltPropertiesSelections.ALLPROPERTIES)
        {
            var altObject = new AltGetAllProperties(CommHandler, altComponent, new AltObject(this), altPropertiesSelections).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altObject;
        }

        protected System.Collections.Generic.List<AltProperty> GetAllFields(AltComponent altComponent, AltFieldsSelections altFieldsSelections = AltFieldsSelections.ALLFIELDS)
        {
            var altObject = new AltGetAllFields(CommHandler, altComponent, new AltObject(this), altFieldsSelections).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altObject;
        }

        protected System.Collections.Generic.List<string> GetAllMethods(AltComponent altComponent, AltMethodSelection methodSelection = AltMethodSelection.ALLMETHODS)
        {
            var altObject = new AltGetAllMethods(CommHandler, altComponent, methodSelection).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altObject;
        }
        protected T GetVisualElementProperty<T>(string propertyName)
        {
            if (type != "UIToolkit")
            {
                throw new WrongAltObjectTypeException("This method is only available for VisualElement objects");
            }
            var propertyValue = new AltGetVisualElementProperty<T>(CommHandler, propertyName, new AltObject(this)).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return propertyValue;
        }
    }
}
