# Writing and running tests

It is very simple to write tests with AltUnityTester. You can use any testing framework and all you need to do is to import the driver for the specific language to your test file.

After that in your setup method you will need to make an instance of the driver and in you tear-down method you have to invoke the stop method of the driver.

If you do this in your test method you could access all the commands that AltUnityTester offers to test your game. 


## C#

If you are writing tests in c# then you could use create your tests direct from Unity.

1.  Create an folder named Editor
2.  Right-click and select to create a new AltUnityTest file.(This will create a template file in which you could start write your test)
3.  Name the file however you want.
4.  Open AltUnityTester window
5.  You should be able to see your test in the left column
6.  Run by pressing one of the 3 options on the right column

Check the this video for more details
<iframe width="560" height="315" src="https://www.youtube.com/embed/-KK7CO4uoxM?start=135" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>