using System.Text.RegularExpressions;
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
    [Ignore("These tests will be run from the Unreal Engine SDK Test repo")]
    [TestFixture]
    [Parallelizable]
    [Timeout(30000)]
    public class TestForScene1TestSampleWithAltDriverUnreal : TestBaseAltDriverUnreal
    {
        public TestForScene1TestSampleWithAltDriverUnreal()
        {
            sceneName = "Scene1";
        }

        // working tests 
        // ---------------------------------------------------------------------------------------------------------------

        [Test]
        public void TestGetCurrentLevel()
        {
            Assert.That(altDriver.GetCurrentLevel(), Is.EqualTo(sceneName));
        }

        [TestCase("//*[contains(@name,AltTesterConnectionOverlay)]/*[contains(@name,CanvasPanel_)]/ConnectionMenu", "ConnectionMenu")]
        [TestCase("//*[contains(@name,AltTesterConnectionOverlay)]/*[contains(@name,CanvasPanel_)]/ConnectionMenu/AltTesterInfoText", "AltTesterInfoText")]
        [TestCase("/BP_Cube", "BP_Cube")]
        public void TestFindDisabledObject(string path, string name)
        {
            AltObjectUnreal altObject = altDriver.FindObject(AltBy.Path(path), enabled: false);
            Assert.That(altObject, Is.Not.Null);
            Assert.That(altObject.name, Does.Match($"^{Regex.Escape(name)}.*"));
        }

        // [TestCase(By.ID, "13b211d0-eafa-452d-8708-cc70f5075e93", "//Capsule")] - AltID not implemented
        [TestCase(By.LAYER, "Wat", "//BP_Capsule")]
        [TestCase(By.COMPONENT, "Capsule", "//BP_Capsule")]
        [TestCase(By.NAME, "Cap", "//BP_Capsule")]
        [TestCase(By.TAG, "MainCamera", "//MainCamera")]
        [TestCase(By.TEXT, "Change Camera", "//*[contains(@name,W_UserInterface_C_)]/*[contains(@name,CanvasPanel_)]/*[contains(@name,Button_)]/*[contains(@name,TextBlock_)]")]
        public void TestFindObjectWhichContains(By by, string value, string path)
        {
            AltObjectUnreal expectedObject = altDriver.FindObject(AltBy.Path(path));
            AltObjectUnreal altObject = altDriver.FindObjectWhichContains(new AltBy(by, value));

            Assert.That(altObject, Is.Not.Null);
            Assert.That(altObject.id, Is.EqualTo(expectedObject.id));
        }

        [Test]
        public void TestFindElementThatContainsText()
        {
            const string text = "Change Camera";
            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Path("//*[contains(@text," + text + ")]"));
            Assert.That(altElement, Is.Not.Null);
        }

        [TestCase(By.COMPONENT, "NonExistent")]
        [TestCase(By.ID, "NonExistent")]
        [TestCase(By.LAYER, "NonExistent")]
        [TestCase(By.NAME, "NonExistent")]
        [TestCase(By.TAG, "NonExistent")]
        [TestCase(By.TEXT, "NonExistent")]
        public void TestFindNonExistentObjectWhichContains(By by, string value)
        {
            Assert.Throws<NotFoundException>(() => altDriver.FindObjectWhichContains(new AltBy(by, value)));
        }

        [TestCase(By.NAME, "Cap", "//BP_Capsule", "//CapsuleInfo")]
        [TestCase(By.TAG, "camera", "//MainCamera", "//SecondaryCamera")]
        public void TestFindObjectsWhichContain2(By by, string value, string path1, string path2)
        {
            AltObjectUnreal expectedObject1 = altDriver.FindObject(AltBy.Path(path1));
            AltObjectUnreal expectedObject2 = altDriver.FindObject(AltBy.Path(path2));
            List<AltObjectUnreal> altObjects = altDriver.FindObjectsWhichContain(new AltBy(by, value));

            Assert.That(altObjects, Has.Count.EqualTo(2));
            Assert.That(
                (altObjects[0].id == expectedObject1.id && altObjects[1].id == expectedObject2.id) ||
                (altObjects[0].id == expectedObject2.id && altObjects[1].id == expectedObject1.id),
                Is.True, "The ids do not match the expected values in either order.");
        }
        [TestCase(By.COMPONENT, "NonExistent")]
        [TestCase(By.ID, "NonExistent")]
        [TestCase(By.LAYER, "NonExistent")]
        [TestCase(By.NAME, "NonExistent")]
        [TestCase(By.TAG, "NonExistent")]
        [TestCase(By.TEXT, "NonExistent")]
        public void TestFindNonExistentObjectsWhichContain(By by, string value)
        {
            List<AltObjectUnreal> altObjects = altDriver.FindObjectsWhichContain(new AltBy(by, value));
            Assert.That(altObjects, Is.Empty);
        }

        [Test]
        public void TestWaitForCurrentLevelToBe()
        {
            DateTime timeStart = DateTime.Now;
            altDriver.WaitForCurrentLevelToBe(sceneName);
            DateTime timeEnd = DateTime.Now;
            TimeSpan time = timeEnd - timeStart;
            Assert.That(time.TotalSeconds, Is.LessThan(20));

            string currentScene = altDriver.GetCurrentLevel();
            Assert.That(currentScene, Is.EqualTo(sceneName));
        }

        // [TestCase(By.ID, "13b211d0-eafa-452d-8708-cc70f5075e93", "//Capsule")]
        [TestCase(By.LAYER, "Wat", "//BP_Capsule")]
        [TestCase(By.COMPONENT, "Capsule", "//BP_Capsule")]
        [TestCase(By.TEXT, "Change Camera", "//*[contains(@name,W_UserInterface_C_)]/*[contains(@name,CanvasPanel_)]/*[contains(@name,Button_)]/*[contains(@name,TextBlock_)]")]
        public void TestFindObjectsWhichContain(By by, string value, string path)
        {
            AltObjectUnreal expectedObject = altDriver.FindObject(AltBy.Path(path));
            List<AltObjectUnreal> altObjects = altDriver.FindObjectsWhichContain(new AltBy(by, value));
            Assert.That(altObjects, Has.Count.EqualTo(1));
            Assert.That(altObjects[0].id, Is.EqualTo(expectedObject.id));
        }

        // [TestCase(By.ID, "13b211d0-eafa-452d-8708-cc70f5075e93", "//Capsule")]
        // [TestCase(By.PATH, "//*[@id=13b211d0-eafa-452d-8708-cc70f5075e93]", "//Capsule")]
        // [TestCase(By.PATH, "//*[@tag=plane][@layer=Default][@name=Plane][@component=MeshCollider][@id=58af4167-0971-415f-901c-7c5226c3c170]", "//Plane")]
        // [TestCase(By.PATH, "//*[@tag=Untagged][@layer=UI][@name=Text][@component=CanvasRenderer][@id=0ffed8a8-3d77-4b03-965b-5ae094ba9511][@text=Change Camera Mode]", "/Canvas/Button/Text")]
        // [TestCase(By.PATH, "//*[contains(@id,13b211d0-eafa-452d-8708-cc70f5075e93)]", "//Capsule")]
        // [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,Pla)][contains(@component,MeshColl)][contains(@id,58af4167-0971-415f-901c-7c5226c3c170)]", "//Plane")]
        // [TestCase(By.PATH, "//*[contains(@tag,Untag)][contains(@layer,U)][contains(@name,Tex)][contains(@component,CanvasRender)][contains(@id,0ffed8a8-3d77-4b03-965b-5ae094ba9511)][contains(@text,Change Camera)]", "/Canvas/Button/Text")]
        [TestCase(By.LAYER, "Water", "//BP_Capsule")]
        [TestCase(By.PATH, "//*[@layer=Water]", "//BP_Capsule")]
        [TestCase(By.COMPONENT, "Capsule", "//BP_Capsule")]
        [TestCase(By.COMPONENT, "SkyAtmosphereComponent", "//SkyAtmosphere")]
        [TestCase(By.NAME, "BP_Capsule", "//BP_Capsule")]
        [TestCase(By.PATH, "/Sphere", "//Sphere")]
        [TestCase(By.PATH, "//PlaneS/..", "//Sphere")]
        [TestCase(By.PATH, "//Sphere/*", "//PlaneS")]
        [TestCase(By.PATH, "//*[@tag=MainCamera]", "//MainCamera")]
        [TestCase(By.PATH, "//*[@name=BP_Capsule]", "//BP_Capsule")]
        [TestCase(By.PATH, "//*[@component=Capsule]", "//BP_Capsule")]
        [TestCase(By.PATH, "//*[@text=Change Camera Mode]", "//*[contains(@name,W_UserInterface_C_)]/*[contains(@name,CanvasPanel_)]/*[contains(@name,Button_)]/*[contains(@name,TextBlock_)]")]
        [TestCase(By.PATH, "//*[contains(@tag,MainCam)]", "//MainCamera")]
        [TestCase(By.PATH, "//*[contains(@name,Cap)]", "//BP_Capsule")]
        [TestCase(By.PATH, "//*[contains(@component,Capsule)]", "//BP_Capsule")]
        [TestCase(By.PATH, "//*[contains(@text,Change Camera)]", "//*[contains(@name,W_UserInterface_C_)]/*[contains(@name,CanvasPanel_)]/*[contains(@name,Button_)]/*[contains(@name,TextBlock_)]")]
        [TestCase(By.PATH, "//*[contains(@layer,Wat)]", "//BP_Capsule")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default]", "//Plane")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default][@name=Plane]", "//Plane")]
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default][@name=Plane][@component=StaticMeshComponent0]", "//Plane")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)]", "//Plane")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,Pla)]", "//Plane")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,Pla)][contains(@component,StaticMeshComp)]", "//Plane")]
        [TestCase(By.TAG, "MainCamera", "//MainCamera")]
        [TestCase(By.TEXT, "Capsule Info", "//CapsuleInfo")]
        [TestCase(By.TEXT, "Change Camera Mode", "//*[contains(@name,W_UserInterface_C_)]/*[contains(@name,CanvasPanel_)]/*[contains(@name,Button_)]/*[contains(@name,TextBlock_)]")]
        public void TestFindObject(By by, string value, string path)
        {
            int referenceId = altDriver.FindObject(AltBy.Path(path)).id;
            AltObjectUnreal altObject = altDriver.FindObject(new AltBy(by, value));
            Assert.That(altObject, Is.Not.Null);
            Assert.That(altObject.id, Is.EqualTo(referenceId));
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
        [TestCase(By.PATH, "//*[@tag=plane][@layer=Default][@name=Plane][@component=StaticMeshComponent0][@id=NonExistent]")]
        // [TestCase(By.PATH, "//*[@tag=Untagged][@layer=UI][@name=Text][@component=CanvasRenderer][@id=f9dc3b3c-2791-42dc-9c0f-87ef7ff5e11d][@text=NonExistent]")]
        [TestCase(By.PATH, "//*[contains(@tag,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@layer,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@name,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@component,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@id,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@text,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,Pla)][contains(@component,NonExistent)]")]
        [TestCase(By.PATH, "//*[contains(@tag,pla)][contains(@layer,Def)][contains(@name,Pla)][contains(@component,StaticMeshComp)][contains(@id,NonExistent)]")]
        // [TestCase(By.PATH, "//*[contains(@tag,Untag)][contains(@layer,U)][contains(@name,Tex)][contains(@component,CanvasRender)][contains(@id,f9dc3b3c-2791-42dc-9c0f-87ef7ff5e11d)][contains(@text,NonExistent)]")]
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
        public void TestGetComponentPropertyPrivate()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "privateVariable";

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            Assert.That(altElement, Is.Not.Null);

            int propertyValue = altElement.GetComponentProperty<int>(componentName, propertyName);
            Assert.That(propertyValue, Is.EqualTo(0));
        }

        [Test]
        public void TestGetComponentPropertyStatic()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "privateStaticVariable";

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            Assert.That(altElement, Is.Not.Null);

            // Can't access static variables from Unreal
            // int propertyValue = altElement.GetComponentProperty<int>(componentName, propertyName, "");
            // Assert.That(propertyValue, Is.EqualTo(0));

            try
            {
                altElement.GetComponentProperty<int>(componentName, propertyName);
                Assert.Fail();
            }
            catch (PropertyNotFoundException exception)
            {
                Assert.That(exception.Message, Is.EqualTo($"Property {propertyName} not found"));
            }
        }

        [Test]
        public void TestGetComponentPropertyStaticPublic()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "PublicStaticVariable";

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            Assert.That(altElement, Is.Not.Null);

            // Can't access static variables from Unreal
            // int propertyValue = altElement.GetComponentProperty<int>(componentName, propertyName, "");
            // Assert.That(propertyValue, Is.EqualTo(0));

            try
            {
                altElement.GetComponentProperty<int>(componentName, propertyName);
                Assert.Fail();
            }
            catch (PropertyNotFoundException exception)
            {
                Assert.That(exception.Message, Is.EqualTo($"Property {propertyName} not found"));
            }
        }

        [Test]
        public void TestSetComponentProperty()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "stringToSetFromTests";

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            Assert.That(altElement, Is.Not.Null);
            altElement.SetComponentProperty(componentName, propertyName, "2");

            string propertyValue = altElement.GetComponentProperty<string>(componentName, propertyName);
            Assert.That(propertyValue, Is.EqualTo("2"));
        }

        [Test]
        public void TestCallMethodWithOptionalParameters()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "TestMethodWithOptionalIntParameters";
            string[] parameters = new string[] {"1", "2"};

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            string data = altElement.CallComponentMethod<string>(componentName, methodName, parameters);
            Assert.That(data, Is.EqualTo("3"));
        }

        [Test]
        public void TestCallMethodWithOptionalParametersString()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "TestMethodWithOptionalStringParameters";

            object[] parameters = new object[] { "FirstParameter", "SecondParameter" };
            string[] typeOfParameters = new string[] { "System.String", "System.String" };

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            string data = altElement.CallComponentMethod<string>(componentName, methodName, parameters, typeOfParameters);
            Assert.That(data, Is.EqualTo("FirstParameterSecondParameter"));
        }

        [Test]
        public void TestCallMethodWithOptionalParametersString2()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "TestMethodWithOptionalStringParameters";

            object[] parameters = new object[] {"FirstParameter", ""};
            string[] typeOfParameters = new string[] {"System.String", "System.String"};

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            string data = altElement.CallComponentMethod<string>(componentName, methodName, parameters, typeOfParameters);
            Assert.That(data, Is.EqualTo("FirstParameter"));
        }

        [Test]
        public void TestWaitForObjectToNotExist()
        {
            altDriver.WaitForObjectNotBePresent(AltBy.Name("ObjectDestroyedIn5Secs"));
            altDriver.WaitForObjectNotBePresent(AltBy.Name("BP_Capsulee"), timeout: 1, interval: 0.5f);
        }

        [Test]
        public void TestWaitForObjectToNotExistFail()
        {
            try
            {
                altDriver.WaitForObjectNotBePresent(AltBy.Name("BP_Capsule"), timeout: 1, interval: 0.5f);
                Assert.Fail();
            }
            catch (WaitTimeOutException exception)
            {
                Assert.That(exception.Message, Is.EqualTo("Element //BP_Capsule still found after 1 seconds"));
            }
        }

        [Test]
        public void TestWaitForCurrentLevelToBeANonExistingScene()
        {
            const string name = "AltDriverTestScene";
            try
            {
                altDriver.WaitForCurrentLevelToBe(name, 1);
                Assert.Fail();
            }
            catch (WaitTimeOutException exception)
            {
                Assert.That(exception.Message, Is.EqualTo("Scene AltDriverTestScene not loaded after 1 seconds"));
            }
        }

        [Test]
        public void TestGetAllComponents()
        {
            List<AltComponent> components = altDriver.FindObject(AltBy.Name("MainCamera")).GetAllComponents();
            Assert.That(components, Has.Count.EqualTo(3));
            Assert.That(components[0].componentName, Is.EqualTo("SceneComponent"));
            Assert.That(components[0].assemblyName, Is.EqualTo("SceneComponent"));
        }

        [TestCase("ChineseLetters", "哦伊娜哦")]
        [TestCase("NonEnglishText", "BJÖRN'S PASS")]
        public void TestGetChineseLetters(string name, string nonEnglishText)
        {
            string text = altDriver.FindObject(AltBy.Name(name)).GetText();
            Assert.That(text, Is.EqualTo(nonEnglishText));
        }

        [Test]
        public void TestForSetText()
        {
            AltObjectUnreal text = altDriver.FindObject(AltBy.Name("NonEnglishText"));
            string originalText = text.GetText();
            string afterText = text.SetText("ModifiedText").GetText();
            Assert.That(afterText, Is.Not.EqualTo(originalText));
        }

        [Test]
        public void TestGetVersion()
        {
            Assert.That(altDriver.GetServerVersion(), Is.EqualTo("1.0.0"));
        }

        [Test]
        public void TestInvalidPaths()
        {
            Assert.Throws<InvalidPathException>(() => altDriver.FindObject(AltBy.Path("//[1]")));
            Assert.Throws<InvalidPathException>(() => altDriver.FindObject(AltBy.Path("CapsuleInfo[@tag=UI]")));
            Assert.Throws<InvalidPathException>(() => altDriver.FindObject(AltBy.Path("//CapsuleInfo[@tag=UI/Text")));
            Assert.Throws<InvalidPathException>(() => altDriver.FindObject(AltBy.Path("//CapsuleInfo[0/Text")));
        }

        [Test]
        public void TestCallPrivateMethod()
        {
            AltObjectUnreal altObject = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            altObject.CallComponentMethod<string>("AltExampleScriptCapsule", "CallJump", Array.Empty<object>());
            AltObjectUnreal capsuleInfo = altDriver.FindObject(AltBy.Name("CapsuleInfo"));
            string text = capsuleInfo.GetText();
            Assert.That(text, Is.EqualTo("Capsule jumps!"));
        }

        [Test]
        public void TestKeysDown()
        {
            AltObjectUnreal altObject = altDriver.FindObject(AltBy.Name("BP_Capsule"));

            // click in scene
            altDriver.Click(new AltVector2());
            
            // keys down
            AltKeyCode[] keys = new AltKeyCode[] {AltKeyCode.K, AltKeyCode.L};
            altDriver.KeysDown(keys);
            altDriver.KeysUp(keys);

            string finalPropertyValue = altObject.UpdateObject().GetComponentProperty<string>("AltExampleScriptCapsule", "stringToSetFromTests");
            Assert.That(finalPropertyValue, Is.EqualTo("multiple keys pressed"));
        }

        [Test]
        public void TestPressKeys()
        {
            AltObjectUnreal altObject = altDriver.FindObject(AltBy.Name("BP_Capsule"));

            // click in scene
            altDriver.Click(new AltVector2());

            // press keys
            AltKeyCode[] keys = new AltKeyCode[] {AltKeyCode.K, AltKeyCode.L};
            altDriver.PressKeys(keys);
            
            string finalPropertyValue = altObject.UpdateObject().GetComponentProperty<string>("AltExampleScriptCapsule", "stringToSetFromTests");
            Assert.That(finalPropertyValue, Is.EqualTo("multiple keys pressed"));
        }


        [Test]
        public void TestGetObjectWithNumberAsName()
        {
            AltObjectUnreal numberObject = altDriver.FindObject(AltBy.Name("1234"), enabled: false);
            Assert.That(numberObject, Is.Not.Null);

            numberObject = altDriver.FindObject(AltBy.Path("//1234"), enabled: false);
            Assert.That(numberObject, Is.Not.Null);
        }

        [TestCase("//ConnectionMenu[0]", "ConnectionMenu", false)]
        [TestCase("//TextBlock[-1]", "TextBlock", true)]
        public void TestFindIndexer(string path, string expectedResult, bool enabled)
        {
            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Path(path), enabled: enabled);
            Assert.That(altElement.name, Is.EqualTo(expectedResult));
        }

        [TestCase("TextMeshInputField")]
        [TestCase("UnrealUIInputField")]
        public void TestSetTextForUnrealUIInputField(string fieldName)
        {
            string text = "exampleUnrealUIInputField";
            AltObjectUnreal inputField = altDriver.FindObject(AltBy.Name(fieldName)).SetText(text);
            Assert.That(inputField.UpdateObject().GetText(), Is.EqualTo(text));
        }

        [Test]
        public void TestSetTimeScale()
        {
            altDriver.SetGlobalTimeDilation(0.1f);
            Thread.Sleep(1000);

            float timeScaleFromGame = altDriver.GetGlobalTimeDilation();
            Assert.That(timeScaleFromGame, Is.EqualTo(0.1f));
            altDriver.SetGlobalTimeDilation(1);
        }

        [Test]
        public void TestFindElementAtCoordinates()
        {
            AltObjectUnreal counterButtonText = altDriver.FindObject(AltBy.Name("ButtonCounter_Text"));
            AltObjectUnreal element = altDriver.FindObjectAtCoordinates(new AltVector2(counterButtonText.x, counterButtonText.y));
            Assert.That(element.name, Is.EqualTo("ButtonCounter_Text"));
        }

        [Test]
        public void TestFindElementAtCoordinates_NoElement()
        {
            AltObjectUnreal element = altDriver.FindObjectAtCoordinates(new AltVector2(-1, -1));
            Assert.That(element, Is.Null);
        }

        [Test]
        // [TestCase("//BP_Capsule", @"^BP_Capsule_C_\d+$")]
        [TestCase("//Plane", @"^StaticMeshActor_\d+$")]
        public void TestFindElementAtCoordinates_3dElement(string objectPath, string expectedPatternName)
        {
            AltObjectUnreal altObject = altDriver.FindObject(AltBy.Path(objectPath));
            AltObjectUnreal element = altDriver.FindObjectAtCoordinates(altObject.GetScreenPosition());
            Assert.That(element, Is.Not.Null);
            Assert.That(element.name, Does.Match(expectedPatternName));
        }

        [Test]
        public void TestLoadNonExistentScene()
        {
            Assert.Throws<SceneNotFoundException>(() => altDriver.LoadLevel("NonExistentScene"));
        }

        [Test]
        public void TestGetComponentPropertyArray()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "arrayOfInts";

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            Assert.That(altElement, Is.Not.Null);

            int[] propertyValue = altElement.GetComponentProperty<int[]>(componentName, propertyName);
            Assert.That(propertyValue, Has.Length.EqualTo(3));

            Assert.That(propertyValue[0], Is.EqualTo(1));
            Assert.That(propertyValue[1], Is.EqualTo(2));
            Assert.That(propertyValue[2], Is.EqualTo(3));
        }

        [Test]
        public void TestGetComponentPropertyNullValue()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "fieldNullValue";

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            Assert.That(altElement, Is.Not.Null);

            object propertyValue = altElement.GetComponentProperty<object>(componentName, propertyName);
            Assert.That(propertyValue, Is.EqualTo(null));
        }

        [Test]
        public void TestSetNonExistingComponentProperty()
        {
            const string unexistingComponent = "unexisting";
            const string propertyName = "stringToSetFromTests";

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            Assert.That(altElement, Is.Not.Null);
            try
            {
                altElement.SetComponentProperty(unexistingComponent, propertyName, "2");
                Assert.Fail();
            }
            catch (ComponentNotFoundException exception)
            {
                Assert.That(exception.Message, Does.StartWith("Component not found"), exception.Message);
            }
        }

        [Test]
        public void TestSetNonExistingProperty()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string unexistingPropertyName = "unexisting";

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            Assert.That(altElement, Is.Not.Null);
            try
            {
                altElement.SetComponentProperty(componentName, unexistingPropertyName, "2");
                Assert.Fail();
            }
            catch (PropertyNotFoundException exception)
            {
                Assert.That(exception.Message, Does.StartWith($"Property {unexistingPropertyName} not found"), exception.Message);
            }
        }

        [Test]
        public void TestCallMethodWithNoParameters()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "UIButtonClicked";

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            string data = altElement.CallComponentMethod<string>(componentName, methodName, Array.Empty<object>());
            Assert.That(data, Is.Null);
        }

        [Test]
        public void TestCallMethodWithParameters()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "Jump";
            string[] parameters = new string[] {"New Text"};

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            string data = altElement.CallComponentMethod<string>(componentName, methodName, parameters);
            Assert.That(data, Is.Null);
        }

        [Test]
        public void TestCallMethodWithManyParameters()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "TestMethodWithManyParameters";
            object[] parameters = new object[] { 1, "stringparam", 0.5, new int[] { 1, 2, 3 } };

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            string data = altElement.CallComponentMethod<string>(componentName, methodName, parameters);
            Assert.That(data, Is.Null);
        }

        [Test]
        public void TestCallMethodWithIncorrectNumberOfParameters()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "TestMethodWithManyParameters";
            object[] parameters = new object[] { 1, "stringparam", new int[] { 1, 2, 3 } };

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            try
            {
                altElement.CallComponentMethod<string>(componentName, methodName, parameters);
                Assert.Fail();
            }
            catch (MethodWithGivenParametersNotFoundException exception)
            {
                Assert.That(exception.Message, Does.StartWith("No method found with 3 parameters matching signature: TestMethodWithManyParameters()"), exception.Message);
            }
        }

        [Test]
        public void TestCallMethodWithIncorrectNumberOfParameters2()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "TestMethodWithManyParameters";
            object[] parameters = new object[] { "a", "stringparam", new int[] { 1, 2, 3 } };

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            try
            {
                altElement.CallComponentMethod<string>(componentName, methodName, parameters);
                Assert.Fail();
            }
            catch (MethodWithGivenParametersNotFoundException exception)
            {
                Assert.That(exception.Message, Does.StartWith("No method found with 3 parameters matching signature: TestMethodWithManyParameters()"), exception.Message);
            }
        }

        [Test]
        public void TestCallMethodInvalidMethodArgumentTypes()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "TestMethodWithManyParameters";
            object[] parameters = new object[] { "stringnoint", "stringparam", 0.5, new int[] { 1, 2, 3 } };

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            try
            {
                altElement.CallComponentMethod<string>(componentName: componentName, methodName: methodName, parameters: parameters);
                Assert.Fail();
            }
            catch (FailedToParseArgumentsException exception)
            {
                Assert.That(exception.Message, Does.StartWith("Could not parse parameter"), exception.Message);
            }
        }

        [Test]
        public void TestCallMethodInvalidParameterType()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string methodName = "TestMethodWithManyParameters";
            object[] parameters = new object[] { 1, "stringparam", 0.5, new int[] { 1, 2, 3 } };

            AltObjectUnreal altElement = altDriver.FindObject(AltBy.Name("BP_Capsule"));
            try
            {
                altElement.CallComponentMethod<string>(componentName, methodName, parameters, new string[] { "System.Stringggggg" });
                Assert.Fail();
            }
            catch (InvalidParameterTypeException exception)
            {
                Assert.That(exception.Message, Does.StartWith("Number of parameters different than number of types of parameters"), exception.Message);
            }
        }

        [Test]
        public void TestGetScreenshot()
        {
            string path = "test.png";
            altDriver.GetPNGScreenshot(path);
            FileAssert.Exists(path);
        }

        [Test]
        public void TestCallStaticNonExistentMethod()
        {
            Assert.Throws<MethodNotFoundException>(() => altDriver.CallStaticMethod<string>("GameUserSettings", "UNEXISTING", new object[] { "Test", "2" }));
        }

        [Test]
        public void TestCallStaticMethodNonExistingTypeName()
        {
            Assert.Throws<ComponentNotFoundException>(() => altDriver.CallStaticMethod<int>("UNEXISTING", "GetScreenResolution", new object[] { "Test", "2" }));
        }
    }
}
