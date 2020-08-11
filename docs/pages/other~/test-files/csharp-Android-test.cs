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
        //It is very important to be before the connection part
        AltUnityPortHandler.ForwardAndroid();
        altUnityDriver = new AltUnityDriver(); //Connect the driver to the server
    }
    
    [OneTimeTearDown]
    //This method will be called only once no matter how many test you have in this class
    public void TearDown()
    {
        altUnityDriver.Stop(); //Disconnects the driver from the server
        //Remove port forwarding made with your android device
        AltUnityPortHandler.RemoveForwardAndroid(); 
    }
    
    [Test]
    public void TestStartGame() //Example of a test
    {
        altUnityDriver.LoadScene("Scene 2 Draggable Panel");

        //Find a game object in the game
        var closePanelButton = altUnityDriver.FindObject(By.NAME, "Close Button");
        closePanelButton.Tap(); //Taps the found object
        togglePanelButton = altUnityDriver.FindObject(By.NAME, "Button").Tap();
        var panelElement = altUnityDriver.FindObject(By.NAME, "Panel");
        
        Assert.IsTrue(altUnityDriver.WaitForObject(By.NAME, "Panel", timeout: 2).enabled);
    }
}
