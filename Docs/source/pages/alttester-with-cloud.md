# Running tests using device cloud services

In some cases you might want to run your tests on dozens or even hundreds of real devices, to test the compatibility of your app on many different device models and OS versions. There are multiple device farms which will enable you to do so, without having to own the devices yourself.

Some of these cloud services allow running Appium automated tests by giving you access to an Appium server running in the cloud that has access to all their mobile devices. These services will work with AltTester Unity SDK only if they also offer some solution for local testing / tunnelling that supports web sockets, so that the device in the cloud can access the AltTester Desktop app running locally on your machine. So far, we know that **BrowserStack** (with BrowerStack Local) and **Sauce Labs** work with AltTester.

However, some of these cloud services give you access to a virtual machine or a Docker container that has a cloud device attached, where you upload your tests, configure your environment and run your tests. 
<!-- This means you can configure your reverse port forwarding so that the tests can connect to the Web Socket opened by AltTester Unity SDK inside the app running on the device. -->

**AWS Device Farm** and **BitBar Cloud** both offer this type of "server-side" running, so they both support running AltTester tests. If you know of any other device cloud providers that might support this, please let us know and we will try them out.

## AWS Device Farm

Amazon offers another great alternative to cloud mobile testing, in the form of [**AWS Device Farm**](https://docs.aws.amazon.com/devicefarm/index.html). You can register for free and get a 1000 device minutes trial period (a credit card will be required for registration).

Because the application is instrumented with AltUnityTester SDK with a version ≥ v2.0.0, AltServer is no longer integrated into the instrumented application. In order to connect to AltServer, AltTester Desktop needs to be running and accessible from the devices running inside the AWS Device Farm. 

You can connect to AltTester Desktop in two ways:

**A. A remote connection**

- AltTester Desktop is opened in a remote location (e.g. a virtual machine in the cloud) from where it can be accessed through IP/URL.
- For the purpose of this example [AltTester Desktop was installed on an AWS instance](https://docs.google.com/document/d/1sZFnmYXsINdR5R0Szx1ce1Pao9_R0_jFiqNJZJv5BJU/edit#heading=h.o24rsr65ynnr). You can connect with the application through IP/URL. In this case, during instrumentation, you need to specify the IP/URL as the AltServerHost. 
- This works for both Android and iOS.

**B. A local connection**

- AltTester Desktop is installed on the AWS Device Farm VM. Therefore a localhost connection is established, so there is no need for setting the host during instrumentation. A license for running the application in batch mode is needed as well, which is stored separately in license.txt (Be aware to not make this file public). For an AltTester Desktop Linux build, please [contact us](https://alttester.com/about/#contact).
- The local connection works only for Android. Unfortunately, [IProxy does not have a way of setting up reverse port forwarding](https://alttester.com/docs/sdk/latest/pages/advanced-usage.html#in-case-of-ios).

You will need two files in order to run your tests:
* **.apk** file, with a build of your app containing the AltDriver;
* A **.zip** file containing your tests, the batchmode Linux AltTester Desktop build (in case of local connection), and other configuration files depending on the language you're running the tests on.

### AWS Device Farm python project example

You can download our example project from [here](https://github.com/alttester-test-examples/Python-AWS-AltTrashCat).

#### **Preparation steps**

**1. Prepare the application**
- Instrument the TrashCat application using AltTester Unity SDK `v2.0.2`. For additional information you can follow [this tutorial](https://alttester.com/walkthrough-tutorial-upgrading-trashcat-to-2-0-x/#Instrument%20TrashCat%20with%20AltTester%20Unity%20SDK%20v.2.0.x).
- Based on your option to connect to AltTester Desktop you need to set AltServer Host of the instrumented app to:
    - localhost (`127.0.0.1`) - for local connection 
    - IP/URL provided by the AWS Instance where AltTester Desktop is running - for remote connection

```eval_rst

.. note::
    If you want to use the AltTester Desktop Community edition (FREE plan) you need to create a Windows AWS Instance. You may only test on 1 device at a time.
    For the moment, the same applies if using the PRO edition too, as in the AWS Device Farm the tests run concurrently on devices and the instrumented app has the same AppName for all instances, which will not allow the AltServer to connect to all of them. In a future release, this problem will be handled so you may be able to test on more than 1 device (on the PRO edition). 

```

```eval_rst

.. note::
    If you are using the AltTester Desktop PRO edition for Linux batchmode you should take into account that when running tests on multiple devices for each device 1 license seat is required, as in the AWS Device Farm the tests run concurrently on devices and for each device, one instance of the AltTester Desktop is launched. In conclusion, you may only test on as many devices at a time as the available number of seats for your license key. 

```

**2. Prepare test code and dependencies**
- **for local connection** - from the cloned repository, create a **.zip** file containing:
    - the batchmode Linux build for AltTester Desktop (in the `AltTesterDesktopLinuxBatchmode` folder)
    - a `license.txt` file which will store your AltTester Desktop PRO license, needed to run batch mode commands. If you only have 1 seat per license please remove the activation in other places before using it here.
    - the `requirements.txt` file
    - the `tests` folder (which contains also the `pages` folder)
- **for remote connection** - from the cloned repository, create a **.zip** file containing:
    - the `requirements.txt` file
    - the `tests` folder (which contains also the `pages` folder) - don`t forget to add the IP/URL of the remote VM when defining AltDriver in ``base_test.py``
 
    Make sure to place all the files in the root directory.

**3. Prepare the configuration file**
- For a custom [test environment](https://docs.aws.amazon.com/devicefarm/latest/developerguide/custom-test-environments.html) you can edit the default configuration file by adding the needed commands. The commands are written as YAML so there are some validations for them
- Using the commands from the configuration file you can access the contents of the uploaded **.zip** folder, install applications, and ultimately run your tests

Keep in mind that the setup is different for Android and iOS. 

- **for local connection**
    - here are the commands needed in the Configuration file to run [AltServer in batchmode](https://alttester.com/docs/desktop/latest/pages/advanced-usage.html#running-altserver-in-terminal). You can choose to include these commands in your test code or add it in the Test Configuration file like in our example.

        ```
        export LICENSE_KEY=$(cat license.txt)
        cd AltTesterDesktopLinuxBatchmode
        ./AltTesterDesktop.x86_64 -batchmode -port 13000 -license $LICENSE_KEY -nographics -termsAndConditionsAccepted & 
        ```
        The `&` symbol is used to make the application run in the background. Failure to add the symbol will cause the commands following it to not be triggered. 
    - You also need to add port reverse forwarding when running on Android after Appium is started: 
        ```
        adb reverse -- remove-all
        adb reverse tcp:13000 tcp:13000
        ```
    - Don`t forget to remove the license activation after each run! 
        ```
        ./AltTesterDesktop.x86_64 -batchmode -removeActivation
        ```
- **for remote connection** - a way to connect to AltServer, within the AltTester Desktop application is by installing AltTester Desktop on an [Amazon EC2 Instance](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/Instances.html). The details of creating an EC2 Instance are out of scope, however, these are the main things to take into account for a successful connection: 
    - create a [Windows instance](https://docs.aws.amazon.com/AWSEC2/latest/WindowsGuide/EC2Win_Infrastructure.html) <br>  
    ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/aws-windows-instance.png
    ```
    - add [Inbound rule to Security Group](https://docs.aws.amazon.com/AWSEC2/latest/WindowsGuide/working-with-security-groups.html#changing-security-group) to make port `13000` accessible - the custom TCP on port `13000` is needed to have the connection to AltTester Desktop default `13000` port <br>  
    ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/aws-inbound-rule-to-security-group.png
    ``` 
    - [Connect to the Instance](https://docs.aws.amazon.com/AWSEC2/latest/WindowsGuide/connecting_to_windows_instance.html) through Remote Access Connection <br>  
    ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/aws-connect-to-instance.png
    ```  
    - [Download AltTester Desktop for Windows](https://alttester.com/app/uploads/AltTester/desktop/AltTesterDesktop__v2.0.2.exe) and install it on the Instance  
    - [Associate an Elastic IP](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/elastic-ip-addresses-eip.html#using-instance-addressing-eips-associating), so that the IP remains constant after each opening of the instance
    ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/aws-associate-elastic-ip.png
    ```
```eval_rst

.. note::
    Please make sure to deactivate any Firewalls on the Windows VM, as it might block the connection.
```

#### **Instructions for running tests on Android using the remote connection**

The instructions and resources will be for running tests on Android, for an application instrumented with AltUnityTester SDK v2.0.2 with a specific host (the elastic IP address provided when creating a remote connection), so that we can connect to it from an AWS Instance (remote connection).

```eval_rst

.. important::
    The remote connection needs to be opened and AltTester Desktop has to be running when starting a new automated run.
```
**1. Create a new project on AWS Device Farm**
- Access [AWS Device Farm](https://us-west-2.console.aws.amazon.com/devicefarm/home?region=us-east-1#/mobile/projects) of your account and create a new project like instructed [here](https://docs.aws.amazon.com/devicefarm/latest/developerguide/getting-started.html), making sure you are in `Mobile Device Testing` > `Projects`

**2. Create a new run**
- on the created project select `Create a new run`
- add the instrumented application build making sure you are on the `Mobile App` tab
- at the configuration step, choose *"Appium Python"* for the purpose of this example, then upload the **.zip** file
- the example project contains a **.yml** configuration file. Use it at the next step, by selecting *"Run your test in a custom environment"*. This will define how your test environment is set up and how the tests run
- the next step will allow you to select on which devices the tests will be executed. You can create your own device pool, or use the recommended top devices;
- for this example, no changes need to be done to the Device state configuration;
- at the last step, you can set the execution timeout for your devices, then start the tests.

Once the status is available the project screen will show the overall status of the tests execution progress and results. Selecting individual runs and devices will give detailed logs about the tests, together with a video recording of the run itself.

```eval_rst

.. note::
    The purpose of the example project is to offer a guide on how to run tests on AWS Device Farm, tests that are not adapted to pass on any type of Android device. In order for them to pass feel free to bring adjustments based on your needs.
```

### AWS Device Farm C# project example

AWS Device Farm does not offer an out-of-the-box solution for running tests written in C#, since it doesn't have an option for it, like in the case of Java or Python, for which there are Test Framework options available. However, using the Appium-ruby configuration from the test-type selection allows the upload of the tests written in C# as a zipped folder. 

<!-- to update here the article link-->
Check [this article](https://insert-article-link-here/) for details on how to create the setup for running C# tests on the AWS Device Farm. The example project can be found [here](https://github.com/alttester/EXAMPLES-CSharp-AWS-AltTrashCat)

## BitBar Cloud

``` note::

    This section is not yet updated to work with version 2.0.*. We will updated this ASAP. 

```

BitBar Cloud is a platform that provides access to hundreds of real iOS and Android devices. It supports client side test execution, but also server-side test execution which we need in order to make AltTester work.

You can create a free account at <https://cloud.bitbar.com> and try out the test examples detailed below for yourself.

In order to run tests on the BitBart Cloud, you will first need to create two files:

* **.ipa** (for iOS) / **.apk** (for Android) file, with a build of your app containing the AltDriver;

If you’re unsure how to generate an **.ipa** file please watch the first half of [this video](https://www.youtube.com/embed/rCwWhEeivjY?start=0&end=199) for iOS.
After you finish setting up the build, you need to use the **Archive** option to generate the standalone **.ipa**. The required steps for the archive option are described [here](https://docs.saucelabs.com/mobile-apps/automated-testing/ipa-files/#creating-ipa-files-for-appium-testing). Keep in mind that you need to select **Development** at step 6.


* **.zip** file containing your tests and a script that defines how those tests will be run.
For more details about the content of this file please see the BitBar documentation [here](https://docs.bitbar.com/testing/scripted-run/)

```eval_rst
.. note::

   Please note that the ``run-tests.sh`` script that runs the tests needs to be at the root of the unzipped package. This means that your zipping method should not create an extra folder when compressing everything together.

```

### BitBar project example

``` note::

    This example is not yet updated to work with version 2.0.0. We will updated this ASAP. 

```

You can download our example BitBar project [here](https://github.com/alttester-test-examples/Python-Bitbar-AltTrashCat).
It contains a pre-built ***ipa*** and ***apk*** file, so you can try out running tests on both Android and iOS

**Steps to run the tests on BitBar:**
1. From the cloned repository, run the *`create-bitbar-package.sh <ios|android>`* script, choosing your desired os as a parameter. This will create a **.zip** file, containing all the files required to execute the tests;

2. On BitBar, create a new project, and Select a target OS type (Android in our example) and a framework (Appium Server Side);
![Step1](../_static/img/alttester-with-cloud/bitbar-step-1.png)

3. Upload the application file (**.apk** or **.ipa**) and the **.zip** file. Please make sure to highlight both before clicking on *"Use selected"*.
![Step2.1](../_static/img/alttester-with-cloud/bitbar-step-2-1.png)


    By default, the selected action for the zip file should be *“Use to run the test”* and for the app file *“Install on the device”*. If not, use the dropdown lists to select them.
    ![Step2.2](../_static/img/alttester-with-cloud/bitbar-step-2-2.png)

4. If you are using a free account, leave the *“Use existing device group”* option checked, together with *“Trial Android devices”* selected. If you have a subscription, please see the BitBar Cloud documentation [here](https://docs.bitbar.com/testing/user-manuals/device-groups) for more info about creating your own device groups;

5. You can now create and run your automated tests.


Going back to the projects tab will allow you to monitor the progress of your tests and also show an overall status once the tests are done. Selecting an individual device will show you specific results for that device, as well as providing video recording of your test run.


## GitHub

GitHub Actions is a very powerful tool for creating a great process CI/CD. You can use public machines offered by GitHub or self-hosted runners in order to run tests automatically. We are using GitHub Actions to build and test our applications. You can see our workflows for AltTester Unity SDK [here](https://github.com/alttester/AltTester-Unity-SDK/tree/development/.github/workflows).

**Some useful links to create your workflows:**
  - [GitHub Action documentation](https://docs.github.com/en/actions)
  - [Unity Builder](https://github.com/marketplace/actions/unity-builder)
  - [Example project for running tests on a public machine](https://github.com/alttester/Example-Running-Tests-On-Github-Public-Runner)
  


