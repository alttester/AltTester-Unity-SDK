using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using NUnit.Framework;
using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public class MyFirstTest
{
    private AltUnityDriver altUnityDriver; 

    [OneTimeSetUp]
    //This method will be called only once no matter how many test you have in this class
    public void SetUp()
    {
        altUnityDriver = new AltUnityDriver(); 
    }
    
    [OneTimeTearDown]
    //This method will be called only once no matter how many test you have in this class
    public void TearDown()
    {
        altUnityDriver.Stop(); 
    }
    
    [Test]
    public void TestStartGame() 
        altUnityDriver.LoadScene("Scene 2 Draggable Panel");

        
        var closePanelButton = altUnityDriver.FindObject(By.NAME, "Close Button");
        closePanelButton.Tap(); 
        var togglePanelButton = altUnityDriver.FindObject(By.NAME, "Button").Tap();
        var panelElement = altUnityDriver.FindObject(By.NAME, "Panel");

        Assert.IsTrue(altUnityDriver.WaitForObject(By.NAME, "Panel", timeout: 2).enabled);
    }
}
