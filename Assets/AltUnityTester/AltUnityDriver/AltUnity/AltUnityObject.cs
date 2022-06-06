using System;
using System.Threading;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityDriver
{
    public class AltUnityObject
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

        public AltUnityObject(string name, int id = 0, int x = 0, int y = 0, int z = 0, int mobileY = 0, string type = "", bool enabled = true, float worldX = 0, float worldY = 0, float worldZ = 0, int idCamera = 0, int transformParentId = 0, int transformId = 0)
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

        public AltUnityObject getParent()
        {
            var altUnityObject = new AltUnityFindObject(CommHandler, By.PATH, "//*[@id=" + this.id + "]/..", By.NAME, "", true).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altUnityObject;
        }

        public AltUnityVector2 getScreenPosition()
        {
            return new AltUnityVector2(x, y);
        }

        public AltUnityVector3 getWorldPosition()
        {
            return new AltUnityVector3(worldX, worldY, worldZ);
        }

        public T GetComponentProperty<T>(string componentName, string propertyName, string assemblyName = null, int maxDepth = 2)
        {
            var propertyValue = new AltUnityGetComponentProperty<T>(CommHandler, componentName, propertyName, assemblyName, maxDepth, this).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return propertyValue;
        }

        public void SetComponentProperty(string componentName, string propertyName, object value, string assemblyName = null)
        {
            new AltUnitySetComponentProperty(CommHandler, componentName, propertyName, value, assemblyName, this).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
        }

        public T CallComponentMethod<T>(string componentName, string methodName, object[] parameters, string[] typeOfParameters = null, string assemblyName = null)
        {
            var result = new AltUnityCallComponentMethod<T>(CommHandler, componentName, methodName, parameters, typeOfParameters, assemblyName, this).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return result;
        }

        public string GetText()
        {
            var text = new AltUnityGetText(CommHandler, this).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return text;
        }

        public AltUnityObject SetText(string text, bool submit = false)
        {
            var altUnityObject = new AltUnitySetText(CommHandler, this, text, submit).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altUnityObject;
        }

        /// <summary>
        /// Click current object
        /// </summary>
        /// <param name="count">Number of times to click</param>
        /// <param name="interval">Interval between clicks in seconds</param>
        /// <param name="wait">Wait for command to finish</param>
        /// <returns>The clicked object</returns>
        public AltUnityObject Click(int count = 1, float interval = 0.1f, bool wait = true)
        {
            var altUnityObject = new AltUnityClickElement(CommHandler, this, count, interval, wait).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altUnityObject;
        }

        public AltUnityObject PointerUpFromObject()
        {
            var altUnityObject = new AltUnityPointerUpFromObject(CommHandler, this).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altUnityObject;
        }

        public AltUnityObject PointerDownFromObject()
        {
            var altUnityObject = new AltUnityPointerDownFromObject(CommHandler, this).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altUnityObject;
        }

        public AltUnityObject PointerEnterObject()
        {
            var altUnityObject = new AltUnityPointerEnterObject(CommHandler, this).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altUnityObject;
        }

        public AltUnityObject PointerExitObject()
        {
            var altUnityObject = new AltUnityPointerExitObject(CommHandler, this).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altUnityObject;
        }

        /// <summary>
        /// Tap current object
        /// </summary>
        /// <param name="count">Number of taps</param>
        /// <param name="interval">Interval in seconds</param>
        /// <param name="wait">Wait for command to finish</param>
        /// <returns>The tapped object</returns>
        public AltUnityObject Tap(int count = 1, float interval = 0.1f, bool wait = true)
        {
            var altUnityObject = new AltUnityTapElement(CommHandler, this, count, interval, wait).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altUnityObject;
        }

        public System.Collections.Generic.List<AltUnityComponent> GetAllComponents()
        {
            var altUnityObject = new AltUnityGetAllComponents(CommHandler, this).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altUnityObject;
        }

        public System.Collections.Generic.List<AltUnityProperty> GetAllProperties(AltUnityComponent altUnityComponent, AltUnityPropertiesSelections altUnityPropertiesSelections = AltUnityPropertiesSelections.ALLPROPERTIES)
        {
            var altUnityObject = new AltUnityGetAllProperties(CommHandler, altUnityComponent, this, altUnityPropertiesSelections).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altUnityObject;
        }

        public System.Collections.Generic.List<AltUnityProperty> GetAllFields(AltUnityComponent altUnityComponent, AltUnityFieldsSelections altUnityFieldsSelections = AltUnityFieldsSelections.ALLFIELDS)
        {
            var altUnityObject = new AltUnityGetAllFields(CommHandler, altUnityComponent, this, altUnityFieldsSelections).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altUnityObject;
        }

        public System.Collections.Generic.List<string> GetAllMethods(AltUnityComponent altUnityComponent, AltUnityMethodSelection methodSelection = AltUnityMethodSelection.ALLMETHODS)
        {
            var altUnityObject = new AltUnityGetAllMethods(CommHandler, altUnityComponent, methodSelection).Execute();
            CommHandler.SleepFor(CommHandler.GetDelayAfterCommand());
            return altUnityObject;
        }
    }
}
