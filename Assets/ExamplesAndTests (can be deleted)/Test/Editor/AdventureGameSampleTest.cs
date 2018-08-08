//using NUnit.Framework;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading;
//using UnityEngine;

//public class AdventureGameSampleTest {

//    private AltUnityDriver altUnityDriver;
//    [OneTimeSetUp]
//    public void SetUp()
//    {
//        altUnityDriver = new AltUnityDriver();
//    }

//    [OneTimeTearDown]
//    public void TearDown()
//    {
//        altUnityDriver.Stop();
//    }
//    [Test]
//    public void TestClickObject()
//    {
//        const string name = "SecurityRoom";
//        var player = altUnityDriver.FindElement("Player");
//        var altElement1 = altUnityDriver.FindElement(name);
//        var altElement=altElement1.ClickObject();
//        Debug.Log(altElement.x + " " + altElement.y);
//        Thread.Sleep(2000);
//        var playerAfterMove = altUnityDriver.FindElement("Player");
//        Assert.AreNotEqual(playerAfterMove.x, player.x);
//        Assert.AreNotEqual(playerAfterMove.y, player.y);
//        Vector2 vectorPlayer = new Vector2(player.x,player.y);
//        Vector2 vectorPlayerAfterMove = new Vector2(playerAfterMove.x, playerAfterMove.y);
//        Vector2 vectorAltElement = new Vector2(altElement.x,altElement.y);
//        Assert.Less(Mathf.Abs(Vector2.Distance(vectorAltElement, vectorPlayerAfterMove)),Mathf.Abs( Vector2.Distance(vectorAltElement, vectorPlayer)));
       
//    }
//    [Test]
//    public void TestClickScreen()
//    {
//        const float X = 480;
//        const float Y = 100;
//        Vector2 screenTapPosition = new Vector2(X, Y);
//        var player = altUnityDriver.FindElement("Player");
//        var altElement = altUnityDriver.ClickScreen(X,Y);
//        Thread.Sleep(2000);
//        var playerAfterMove = altUnityDriver.FindElement("Player");
//        Assert.AreNotEqual(playerAfterMove.x, player.x);
//        Assert.AreNotEqual(playerAfterMove.y, player.y);
//        Vector2 vectorPlayer = new Vector2(player.x, player.y);
//        Vector2 vectorPlayerAfterMove = new Vector2(playerAfterMove.x, playerAfterMove.y);
//        Assert.Less(Vector2.Distance(screenTapPosition, vectorPlayerAfterMove), Vector2.Distance(screenTapPosition, vectorPlayer));
//        Assert.AreEqual("SecurityRoom", altElement.name);
//    }
//}
