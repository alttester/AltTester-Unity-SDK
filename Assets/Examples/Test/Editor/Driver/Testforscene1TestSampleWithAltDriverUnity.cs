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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace AltTester.AltTesterUnitySDK.Driver.Tests
{
    
    [TestFixture]
    [Parallelizable]
    [Timeout(30000)]
    public class TestForScene1TestSampleWithAltDriverUnity : TestBaseAltDriverUnity
    {

        public TestForScene1TestSampleWithAltDriverUnity()
        {
            sceneName = "Scene 1 AltDriverTestScene";
        }

        // UI disable elements are needed for this because at the moment, the test uses the AltDialog objects
        [TestCase("/AltTesterPrefab/AltDialog/Dialog", "Dialog")]
        [TestCase("/AltTesterPrefab/AltDialog/Dialog/Title", "Title")]
        [TestCase("/Cube", "Cube")]
        public void TestFindDisabledObject(string path, string name)
        {
            var altObject = altDriver.FindObject(AltBy.Path(path), enabled: false);
            Assert.NotNull(altObject);
            Assert.AreEqual(name, altObject.name);
        }

        [TestCase(By.COMPONENT, "CapsuleColl", "//Capsule")]
        [TestCase(By.ID, "13b211d0-eafa-452d-8708-cc70f5075e93", "//Capsule")]
        [TestCase(By.LAYER, "Wat", "//Capsule")]
        [TestCase(By.NAME, "Cap", "//Capsule")]
        [TestCase(By.TAG, "pla", "//Plane")]
        [TestCase(By.TEXT, "Change Camera", "/Canvas/Button/Text")]
        public void TestFindObjectWhichContains(By by, string value, string path)
        {
            var expectedObject = altDriver.FindObject(AltBy.Path(path));
            var altObject = altDriver.FindObjectWhichContains(new AltBy(by, value));
            Assert.NotNull(altObject);
            Assert.AreEqual(expectedObject.id, altObject.id);
        }

        [Test]
        public void TestFindElementThatContainsText()
        {
            const string text = "Change Camera";
            var altElement = altDriver.FindObject(AltBy.Path("//*[contains(@text," + text + ")]"));
            Assert.NotNull(altElement);
        }

        [TestCase(By.COMPONENT, "NonExistent")]
        [TestCase(By.ID, "NonExistent")]
        [TestCase(By.LAYER, "NonExistent")]
        [TestCase(By.NAME, "NonExistent")]
        [TestCase(By.TAG, "NonExistent")]
        [TestCase(By.TEXT, "NonExistent")]
        public void TestFindNonExistentObjectWhichContains(By by, string value)
        {
            AltBy altBy = new AltBy(by, value);
            Assert.Throws<NotFoundException>(() => altDriver.FindObjectWhichContains(altBy));
        }

        [TestCase(By.COMPONENT, "CapsuleColl", "//Capsule")]
        [TestCase(By.ID, "13b211d0-eafa-452d-8708-cc70f5075e93", "//Capsule")]
        [TestCase(By.LAYER, "Wat", "//Capsule")]
        [TestCase(By.TEXT, "Change Camera", "/Canvas/Button/Text")]
        public void TestFindObjectsWhichContain(By by, string value, string path)
        {
            var expectedObject = altDriver.FindObject(AltBy.Path(path));
            var altObjects = altDriver.FindObjectsWhichContain(new AltBy(by, value));
            Assert.AreEqual(1, altObjects.Count());
            Assert.AreEqual(expectedObject.id, altObjects[0].id);
        }

        [TestCase(By.NAME, "Cap", "//Capsule", "//CapsuleInfo")]
        [TestCase(By.TAG, "pla", "//Plane", "/UIWithWorldSpace/Plane")]
        public void TestFindObjectsWhichContain2(By by, string value, string path1, string path2)
        {
            var expectedObject1 = altDriver.FindObject(AltBy.Path(path1));
            var expectedObject2 = altDriver.FindObject(AltBy.Path(path2));
            var altObjects = altDriver.FindObjectsWhichContain(new AltBy(by, value));
            Assert.AreEqual(2, altObjects.Count());
            Assert.AreEqual(expectedObject1.id, altObjects[0].id);
            Assert.AreEqual(expectedObject2.id, altObjects[1].id);
        }

        [TestCase(By.COMPONENT, "NonExistent")]
        [TestCase(By.ID, "NonExistent")]
        [TestCase(By.LAYER, "NonExistent")]
        [TestCase(By.NAME, "NonExistent")]
        [TestCase(By.TAG, "NonExistent")]
        [TestCase(By.TEXT, "NonExistent")]
        public void TestFindNonExistentObjectsWhichContain(By by, string value)
        {
            var altObjects = altDriver.FindObjectsWhichContain(new AltBy(by, value));
            Assert.IsEmpty(altObjects);
        }

        [TestCase(By.COMPONENT, "AltRunner", "//AltTesterPrefab")]
        [TestCase(By.COMPONENT, "CapsuleCollider", "//Capsule")] //Unity component
        [TestCase(By.COMPONENT, "AltTester.AltDriver.AltRunner", "//AltTesterPrefab")] // namespace
        [TestCase(By.ID, "13b211d0-eafa-452d-8708-cc70f5075e93", "//Capsule")]
        [TestCase(By.LAYER, "Water", "//Capsule")]
        [TestCase(By.NAME, "Capsule", "//Capsule")]
        [TestCase(By.PATH, "/Sphere", "//Sphere")]
        [TestCase(By.PATH, "//PlaneS/..", "//Sphere")]
        [TestCase(By.PATH, "//Sphere/*", "//PlaneS")]
        [TestCase(By.PATH, "//*[@tag=plane]", "//Plane")]
        [TestCase(By.PATH, "//*[@layer=Water]", "//Capsule")]
        [TestCase(By.PATH, "//*[@name=Capsule]", "//Capsule")]
        [TestCase(By.PATH, "//*[@component=CapsuleCollider]", "//Capsule")]
        [TestCase(By.PATH, "//*[@id=13b211d0-eafa-452d-8708-cc70f5075e93]", "//Capsule")]
        [TestCase(By.PATH, "//*[@text=Change Camera Mode]", "/Canvas/Button/Text")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default]", "//Plane")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default][@name=Plane]", "//Plane")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default][@name=Plane][@component=MeshCollider]", "//Plane")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default][@name=Plane][@component=MeshCollider][@id=58af4167-0971-415f-901c-7c5226c3c170]", "//Plane")]
        [TestCase(By.PATH, "//*[@tag=Untagged][@layer=UI][@name=Text][@component=CanvasRenderer][@id=0ffed8a8-3d77-4b03-965b-5ae094ba9511][@text=Change Camera Mode]", "/Canvas/Button/Text")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)]", "//Plane")]
        [TestCase(By.PATH, "//*[contains(@layer,Wat)]", "//Capsule")]
        [TestCase(By.PATH, "//*[contains(@name,Cap)]", "//Capsule")]
        [TestCase(By.PATH, "//*[contains(@component,CapsuleColl)]", "//Capsule")]
        [TestCase(By.PATH, "//*[contains(@id,13b211d0-eafa-452d-8708-cc70f5075e93)]", "//Capsule")]
        [TestCase(By.PATH, "//*[contains(@text,Change Camera)]", "/Canvas/Button/Text")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)]", "//Plane")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,Pla)]", "//Plane")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,Pla)][contains(@component,MeshColl)]", "//Plane")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,Pla)][contains(@component,MeshColl)][contains(@id,58af4167-0971-415f-901c-7c5226c3c170)]", "//Plane")]
        [TestCase(By.PATH, "//*[contains(@tag,Untag)][contains(@layer,U)][contains(@name,Tex)][contains(@component,CanvasRender)][contains(@id,0ffed8a8-3d77-4b03-965b-5ae094ba9511)][contains(@text,Change Camera)]", "/Canvas/Button/Text")]
        [TestCase(By.TAG, "plane", "//Plane")]
        [TestCase(By.TEXT, "Capsule Info", "//CapsuleInfo")] // text area
        [TestCase(By.TEXT, "Change Camera Mode", "//Canvas/Button/Text")] // button with text
        public void TestFindObject(By by, string value, string path)
        {
            int referenceId = altDriver.FindObject(AltBy.Path(path)).id;
            var altObject = altDriver.FindObject(new AltBy(by, value));
            Assert.NotNull(altObject);
            Assert.AreEqual(referenceId, altObject.id);
        }


        [Test]
        public void TestDifferentCamera()
        {
            var altButton = altDriver.FindObject(AltBy.Name("Button"), AltBy.Name("Main Camera"));
            altButton.Click();
            altButton.Click();
            var altElement = altDriver.FindObject(AltBy.Name("Capsule"), AltBy.Name("Main Camera"));
            var altElement2 = altDriver.FindObject(AltBy.Name("Capsule"), AltBy.Name("Camera"));;
            AltVector2 pozOnScreenFromMainCamera = new AltVector2(altElement.x, altElement.y);
            AltVector2 pozOnScreenFromSecondaryCamera = new AltVector2(altElement2.x, altElement2.y);

            Assert.AreNotEqual(pozOnScreenFromSecondaryCamera, pozOnScreenFromMainCamera);

        }


        [TestCase(By.COMPONENT, "NonExistent")]
        [TestCase(By.ID, "NonExistent")]
        [TestCase(By.LAYER, "NonExistent")]
        [TestCase(By.NAME, "NonExistent")]
        [TestCase(By.PATH, "/NonExistent")]
        [TestCase(By.PATH, "//NonExistent/..")]
        [TestCase(By.PATH, "//NonExistent/*")]
        [TestCase(By.PATH, "//*[@tag=NonExistent]")]
        [TestCase(By.PATH, "//*[@layer=NonExistent]")]
        [TestCase(By.PATH, "//*[@name=NonExistent]")]
        [TestCase(By.PATH, "//*[@component=NonExistent]")]
        [TestCase(By.PATH, "//*[@id=NonExistent]")]
        [TestCase(By.PATH, "//*[@text=NonExistent]")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=NonExistent]")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default][@name=NonExistent]")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default][@name=Plane][@component=NonExistent]")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default][@name=Plane][@component=MeshCollider][@id=NonExistent]")]
        [TestCase(By.PATH, "//*[@tag=Untagged][@layer=UI][@name=Text][@component=CanvasRenderer][@id=f9dc3b3c-2791-42dc-9c0f-87ef7ff5e11d][@text=NonExistent]")]
        [TestCase(By.PATH, "//*[contains(@tag,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@layer,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@name,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@component,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@id,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@text,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,Pla)][contains(@component,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,Pla)][contains(@component,MeshColl)][contains(@id,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@tag,Untag)][contains(@layer,U)][contains(@name,Tex)][contains(@component,CanvasRender)][contains(@id,f9dc3b3c-2791-42dc-9c0f-87ef7ff5e11d)][contains(@text,NonExistent)]")]
        [TestCase(By.PATH, "//Canvas[100]")]
        [TestCase(By.PATH, "//Canvas[-100]")]
        [TestCase(By.TAG, "NonExistent")]
        [TestCase(By.TEXT, "NonExistent")]
        [TestCase(By.PATH, "//DisabledObject")]
        [TestCase(By.PATH, "//DisabledObject/DisabledChild")]
        public void TestFindNonExistentObject(By by, string value)
        {
            Assert.Throws<NotFoundException>(() => altDriver.FindObject(new AltBy(by, value)));
        }

       
        [Test]
        public void TestWaitForObjectToNotExist()
        {
            altDriver.WaitForObjectNotBePresent(AltBy.Name("ObjectDestroyedIn5Secs"));
            altDriver.WaitForObjectNotBePresent(AltBy.Name("Capsulee"), timeout: 1, interval: 0.5f);
        }

        [Test]
        public void TestWaitForObjectToNotExistFail()
        {
            try
            {
                altDriver.WaitForObjectNotBePresent(AltBy.Name("Capsule"), timeout: 1, interval: 0.5f);
                Assert.Fail();
            }
            catch (WaitTimeOutException exception)
            {
                Assert.AreEqual("Element //Capsule still found after 1 seconds", exception.Message);
            }
        }
       
        [Test]
        public void TestFindObjectWithCameraId()
        {
            var altButton = altDriver.FindObject(AltBy.Path("//Button"));
            altButton.Click();
            altButton.Click();
            var camera = altDriver.FindObject(AltBy.Path("//Camera"));
            var altElement = altDriver.FindObject(AltBy.Component("CapsuleCollider"), AltBy.Id(camera.id.ToString()));
            Assert.True(altElement.name.Equals("Capsule"));
            var camera2 = altDriver.FindObject(AltBy.Path("//Main Camera"));
            var altElement2 = altDriver.FindObject(AltBy.Component("CapsuleCollider"), AltBy.Id(camera2.id.ToString()));
            Assert.AreNotEqual(altElement.GetScreenPosition(), altElement2.GetScreenPosition());
        }

        [Test]
        public void TestWaitForObjectWithCameraId()
        {
            var altButton = altDriver.FindObject(AltBy.Path("//Button"));
            altButton.Click();
            altButton.Click();
            var camera = altDriver.FindObject(AltBy.Path("//Camera"));
            var altElement = altDriver.WaitForObject(AltBy.Component("CapsuleCollider"), AltBy.Id(camera.id.ToString()));
            Assert.True(altElement.name.Equals("Capsule"));
            var camera2 = altDriver.FindObject(AltBy.Path("//Main Camera"));
            var altElement2 = altDriver.WaitForObject(AltBy.Component("CapsuleCollider"), AltBy.Id(camera2.id.ToString()));
            Assert.AreNotEqual(altElement.GetScreenPosition(), altElement2.GetScreenPosition());
        }

        [Test]
        public void TestFindObjectsWithCameraId()
        {
            var altButton = altDriver.FindObject(AltBy.Path("//Button"));
            altButton.Click();
            altButton.Click();
            var camera = altDriver.FindObject(AltBy.Path("//Camera"));
            var altElement = altDriver.FindObjects(AltBy.Name("Plane"), AltBy.Id(camera.id.ToString()));
            Assert.True(altElement[0].name.Equals("Plane"));
            var camera2 = altDriver.FindObject(AltBy.Path("//Main Camera"));
            var altElement2 = altDriver.FindObjects(AltBy.Name("Plane"), AltBy.Id(camera2.id.ToString()));
            Assert.AreNotEqual(altElement[0].GetScreenPosition(), altElement2[0].GetScreenPosition());
        }

        [Test]
        public void TestWaitForObjectNotBePresentWithCameraId()
        {
            var camera2 = altDriver.FindObject(AltBy.Path("//Main Camera"));
            altDriver.WaitForObjectNotBePresent(AltBy.Name("ObjectDestroyedIn5Secs"), AltBy.Id(camera2.id.ToString()));

            var allObjectsInTheScene = altDriver.GetAllElements(cameraAltBy: null);
            Assert.IsTrue(!allObjectsInTheScene.Any(obj => obj.name.Equals("ObjectDestroyedIn5Secs")));
        }

        [Test]
        public void TestWaitForObjectWhichContainsWithCameraId()
        {
            var camera2 = altDriver.FindObject(AltBy.Path("//Main Camera"));
            var altElement = altDriver.WaitForObjectWhichContains(AltBy.Name("Canva"), AltBy.Id(camera2.id.ToString()));
            Assert.AreEqual("Canvas", altElement.name);

        }

        [Test]
        public void TestWaitForObjectWhichContainsWithTag()
        {
            var altElement = altDriver.WaitForObjectWhichContains(AltBy.Name("Canva"), AltBy.Tag("MainCamera"));
            Assert.AreEqual("Canvas", altElement.name);

        }

        [Test]
        public void TestWaitForObjectWhichContainsNonExistingCriteria()
        {
            Assert.Throws<WaitTimeOutException>(() => altDriver.WaitForObjectWhichContains(AltBy.Name("Unexisting"), AltBy.Tag("MainCamera"), timeout: 2));
        }

        [Test]
        public void TestWaitForObjectWhichContainsExistingCriteriaButNonExistingCamera()
        {
            Assert.Throws<AltCameraNotFoundException>(() => altDriver.WaitForObjectWhichContains(AltBy.Name("Canva"), AltBy.Tag("Unexisting"), timeout: 2));
        }

        [TestCase(By.NAME, "Main Camera")]
        [TestCase(By.COMPONENT, "Camera")]
        [TestCase(By.TAG, "MainCamera")]
        [TestCase(By.PATH, "/Main Camera")]
        [TestCase(By.LAYER, "Default")]
        [TestCase(By.ID, "4eb39f50-3403-473c-b684-915f7a40c393")]
        public void TestFindObjectByCamera(By cameraBy, string cameraValue)
        {
            int referenceId = altDriver.FindObject(AltBy.Path("//Capsule")).id;
            var altObject = altDriver.FindObject(AltBy.Path("//Capsule"), new AltBy(cameraBy, cameraValue));
            Assert.NotNull(altObject);
            Assert.AreEqual(referenceId, altObject.id);
        }

        [Test]
        public void TestWaitForObjectByCamera()
        {
            var altButton = altDriver.FindObject(AltBy.Path("//Button"));
            altButton.Click();
            altButton.Click();
            var altElement = altDriver.WaitForObject(AltBy.Component("CapsuleCollider"), AltBy.Name("Camera"));
            Assert.True(altElement.name.Equals("Capsule"));
            var altElement2 = altDriver.WaitForObject(AltBy.Component("CapsuleCollider"), AltBy.Name("Main Camera"));
            Assert.AreNotEqual(altElement.GetScreenPosition(), altElement2.GetScreenPosition());
        }

        [Test]
        public void TestFindObjectsByCamera()
        {
            var altButton = altDriver.FindObject(AltBy.Path("//Button"));
            altButton.Click();
            altButton.Click();
            var altElement = altDriver.FindObjects(AltBy.Name("Plane"), AltBy.Name("Camera"));
            Assert.True(altElement[0].name.Equals("Plane"));
            var altElement2 = altDriver.FindObjects(AltBy.Name("Plane"), AltBy.Name("Main Camera"));
            Assert.AreNotEqual(altElement[0].GetScreenPosition(), altElement2[0].GetScreenPosition());
        }

        [Test]
        public void TestWaitForObjectNotBePresentByCamera()
        {
            altDriver.WaitForObjectNotBePresent(AltBy.Name("ObjectDestroyedIn5Secs"), AltBy.Name("Main Camera"));

            var allObjectsInTheScene = altDriver.GetAllElements(cameraAltBy: null);
            Assert.IsTrue(!allObjectsInTheScene.Any(obj => obj.name.Equals("ObjectDestroyedIn5Secs")));
        }

      
        [Test]
        public void TestGetAltObjectWithCanvasParentButOnlyTransform()
        {
            var altObject = altDriver.FindObject(AltBy.Name("UIWithWorldSpace/Plane"));
            Assert.NotNull(altObject);
        }


        [TestCase("//Dialog[0]", "Dialog", false)]
        [TestCase("//Text[-1]", "Text", true)]
        public void TestFindIndexer(string path, string expectedResult, bool enabled)
        {
            var altElement = altDriver.FindObject(AltBy.Path(path), enabled: enabled);
            Assert.AreEqual(expectedResult, altElement.name);
        }

        [Test]
        public void TestGetObjectWithNumberAsName()
        {
            var numberObject = altDriver.FindObject(AltBy.Name("1234"), enabled: false);
            Assert.NotNull(numberObject);
            numberObject = altDriver.FindObject(AltBy.Path("//1234"), enabled: false);
            Assert.NotNull(numberObject);
        }

        [Test]
        public void TestInvalidPaths()
        {
            Assert.Throws<InvalidPathException>(() => altDriver.FindObject(AltBy.Path("//[1]")));
            Assert.Throws<InvalidPathException>(() => altDriver.FindObject(AltBy.Path("CapsuleInfo[@tag=UI]")));
            Assert.Throws<InvalidPathException>(() => altDriver.FindObject(AltBy.Path("//CapsuleInfo[@tag=UI/Text")));
            Assert.Throws<InvalidPathException>(() => altDriver.FindObject(AltBy.Path("//CapsuleInfo[0/Text")));
        }
    }
}
