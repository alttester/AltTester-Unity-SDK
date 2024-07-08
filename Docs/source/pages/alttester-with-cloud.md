# Running tests using device cloud services

In some cases you might want to run your tests on dozens or even hundreds of real devices, to test the compatibility of your app on many different device models and OS versions. There are multiple device farms that will enable you to do so, without having to own the devices yourself.

Some of these cloud services allow running Appium automated tests by giving you access to an Appium server running in the cloud that has access to all their mobile devices. These services will work with AltTester® Unity SDK only if they also offer some solution for local testing / tunneling that supports web sockets, so that the device in the cloud can access the AltTester® Desktop app running locally on your machine. So far, we know that **BrowserStack** (with BrowerStack Local) and **SauceLabs** work with AltTester®.

```eval_rst

.. note::
    BrowserStack and SauceLabs don’t support server-side testing, meaning that the test folder can’t be uploaded onto the platform in order to run the tests. Client-side testing generally focuses on testing the application or website directly on the user’s end. For testing carried out on cloud services, this means that the test suite is stored locally, on a computer and connected to a device in the cloud.
```

However, some of these cloud services give you access to a virtual machine or a Docker container that has a cloud device attached, where you upload your tests, configure your environment and run your tests. 

**AWS Device Farm** and **BitBar** both offer this type of "server-side" running, so they both support running AltTester® tests. If you know of any other device cloud providers that might support this, please let us know and we will try them out.

## BrowserStack

If you want to run AltTester® Unity SDK tests client-side on a cloud service you may try using **BrowserStack App Automate**.

BrowserStack doesn't support server-side testing, meaning that the user can't upload the test suite and the build on the cloud and then run them using a script.

An option for running tests that are not stored locally is to integrate with CI/CD tools, like **GitHub Actions**. 

### BrowserStack App Automate C# project example

BrowserStack App Automate is a cloud-based testing platform that allows developers and testers to perform automated testing of mobile applications on a wide range of real devices. 

In this automation process, BrowserStack uses a set of Appium capabilities to customize and configure the testing environment to match various scenarios.

You can download our example project from [here](https://github.com/alttester/EXAMPLES-CSharp-BrowserStack-AltTrashCat). Also, for more details check [this article](https://alttester.com/running-alttester-based-c-tests-on-browserstack-app-automate/) from our Blog.

```eval_rst

.. note::
    This example was created for running tests on a single device.

```

Because our tests are written in C# using the NUnit framework, we used the [Appium with NUnit section](https://www.browserstack.com/docs/app-automate/appium/getting-started/c-sharp/nunit) from the BrowserStack App Automate documentation to guide us through client-side testing.

An important aspect of running tests on BrowserStack is that there’s a [local testing connection](https://www.browserstack.com/docs/app-automate/appium/getting-started/c-sharp/nunit/local-testing#3-configure-and-run-your-local-test) needed. **Local Testing**, a BrowserStack option, allows us to conduct automated test execution for mobile apps that access resources hosted in development or testing environments.

```eval_rst
.. image:: ../_static/img/alttester-with-cloud/browserstack-local-diagram.png
```

#### **Prerequisites**
- Test suite - we used [EXAMPLES-TrashCat-Tests](https://github.com/alttester/EXAMPLES-TrashCat-Tests)
- AltTester® Desktop app installed for running AltTester® Server
- [BrowserStack account](https://www.browserstack.com/users/sign_in?utm_source=google&utm_medium=cpc&utm_platform=paidads&utm_content=610276476173&utm_campaign=Search-Brand-Tier2-EMEA-CL&utm_campaigncode=BrowserStack.com+1011804&utm_term=p+browserstack%20com) - there is a free plan available that offers 100 min to run tests
- .NET `v5.0+` and NUnit `v3.0.0+`

#### **Steps for running tests on Android and iOS**

**1. Upload a test build of your app in BrowserStack and get the generated URL**

There are [two options](https://www.browserstack.com/docs/app-automate/appium/getting-started/c-sharp/nunit/integrate-your-tests#app-uploads-and-management) for passing the build:

a. you can upload the file (.apk or .ipa) from your local file system, as shown below - use the UI button available on the [Dashboard](https://app-automate.browserstack.com/dashboard/v2/quick-start/get-started)
```eval_rst
.. image:: ../_static/img/alttester-with-cloud/browserstack-upload-build.png
```

- a unique app ID, formatted as **bs://{app_id}** will be generated if the upload was successful - take note of this value because you will use it to specify the app capability for the application under test
- the app is stored in your account and it doesn’t change if you upload the same build

b. use the BrowserStack REST API endpoint

**2. Get BrowserStack credentials**

Once you are logged into your BrowserStack account, you can find the credentials in the AccessKey section in the [BrowserStack Dashboard](https://app-automate.browserstack.com/dashboard/v2/quick-start/get-started).

```eval_rst
.. image:: ../_static/img/alttester-with-cloud/browserstack-credentials.png
```

**3. Set the BrowserStack credentials and app ID as environment variables**

To set these values as environment variables on Windows you can create a **batch file** on your local machine and run it every time you load your IDE. This will keep the sensitive information out of the repository. 

Here is an example:

```
set BROWSERSTACK_USERNAME "yourUsername"
set BROWSERSTACK_ACCESS_KEY "yourAccessKey"
set BROWSERSTACK_APP_ID_SDK_201="yourAppId"
```

**4. Install dependencies**

In your code project, you need to install a Selenium WebDriver extension for Appium and C# Bindings for BrowserStack Local. Example:

```
dotnet add package Appium.WebDriver --version 4.4.0
dotnet add package BrowserStackLocal --version 2.3.0
```

**5. Create and configure new file**

In your repository, create a new file that will hold the required settings for the integration. This file will hold all of the **Appium** and **BrowserstackLocal** settings that ensure the connection between the local environment and the cloud device. For future reference it will be called [**BaseTest**](https://github.com/alttester/EXAMPLES-CSharp-BrowserStack-AltTrashCat/blob/main/tests/BaseTest.cs). Every test file in this project inherits this C# class. 

In this file add code that will:
- access the environment variables set in the previous steps using the `GetEnvironmentVariable` method like so:
    ```c#
    String BROWSERSTACK_USERNAME = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
    String BROWSERSTACK_ACCESS_KEY = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
    String BROWSERSTACK_APP_ID_SDK_201 = Environment.GetEnvironmentVariable("BROWSERSTACK_APP_ID_SDK_201");
    ```
- configure Appium capabilities and BrowserStack options
    - at this step you pass information to BrowserStack that configures the test environment as well as organizes the test runs - BrowserStack offers a [capabilities builder](https://www.browserstack.com/app-automate/capabilities?tag=w3c), which helps you determine what settings you need and how to format them correctly
    
    Example:

    ```eval_rst
    .. tabs::

        .. tab:: Android

            .. code-block:: C#

                AppiumOptions capabilities = new AppiumOptions();
                Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();

                browserstackOptions.Add("projectName", "TrashCat");
                browserstackOptions.Add("buildName", "TrashCat201Android");
                browserstackOptions.Add("sessionName", "tests - " + DateTime.Now.ToString("MMMM dd - HH:mm"));
                browserstackOptions.Add("local", "true");
                browserstackOptions.Add("userName", BROWSERSTACK_USERNAME);
                browserstackOptions.Add("accessKey", BROWSERSTACK_ACCESS_KEY);
                capabilities.AddAdditionalCapability("bstack:options", browserstackOptions);
                capabilities.AddAdditionalCapability("platformName", "android");
                capabilities.AddAdditionalCapability("platformVersion", "11.0");
                capabilities.AddAdditionalCapability("appium:deviceName", "Samsung Galaxy S21");
                capabilities.AddAdditionalCapability("appium:app", BROWSERSTACK_APP_ID_SDK_201);

        .. tab:: iOS

            .. code-block:: C#

                AppiumOptions capabilities = new AppiumOptions();
                Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();

                browserstackOptions.Add("projectName", "TrashCat");
                browserstackOptions.Add("buildName", "TrashCat201iOS");
                browserstackOptions.Add("sessionName", "tests - " + DateTime.Now.ToString("MMMM dd - HH:mm"));
                browserstackOptions.Add("local", "true");
                browserstackOptions.Add("userName", BROWSERSTACK_USERNAME);
                browserstackOptions.Add("accessKey", BROWSERSTACK_ACCESS_KEY);
                capabilities.AddAdditionalCapability("bstack:options", browserstackOptions);
                capabilities.AddAdditionalCapability("platformName", "ios");
                capabilities.AddAdditionalCapability("platformVersion", "16");
                capabilities.AddAdditionalCapability("appium:deviceName", "iPhone 14");
                capabilities.AddAdditionalCapability("appium:app", BROWSERSTACK_APP_ID_SDK_201);
    ```

- configure the local testing connection
    - using the BrowserStackLocal package, you need to start the local testing connection. You can do this in the code like this:

    ```c#
    browserStackLocal = new Local();
    List<KeyValuePair<string, string>> bsLocalArgs = new List<KeyValuePair<string, string>>() {
                            new KeyValuePair<string, string>("key", BROWSERSTACK_ACCESS_KEY)
    };
    browserStackLocal.start(bsLocalArgs);
    ```
    - this local connection can also be started manually. You need to download the [local binary from BrowserStack docs](https://www.browserstack.com/docs/local-testing/releases-and-downloads) and run the `.exe` file from its path

- start Appium Driver
    - in order to test remotely on BrowserStack, you need to use an instance of Appium Driver. For that, use the remote BrowserStack URL and your access credentials, which are stored inside the AppiumOptions variable named `capabilities`:

    ```eval_rst
    .. tabs::

        .. tab:: Android

            .. code-block:: C#

                appiumDriver = new AndroidDriver<AndroidElement>(new Uri("https://hub-cloud.browserstack.com/wd/hub/"), capabilities);

        .. tab:: iOS

            .. code-block:: C#

                appiumDriver = new IOSDriver<IOSElement>(new Uri("https://hub-cloud.browserstack.com/wd/hub/"), capabilities);
    ```

- initialize AltDriver
    ```c#
    altDriver = new AltDriver();
    ```
- [iOS] Handle permission pop-up
    - while running your tests on iOS you might get a pop-up that asks for permission to connect to devices on the local network
    - to accept this notification and give permission, use the following lines:
    ```c#
    IWebElement ll = appiumDriver.FindElement(OpenQA.Selenium.By.Id("Allow"));
    ll.Click();
    ```
- add method to keep Appium alive
    - in this context, Appium is only used to install the application and access it on the BrowserStack test device - after that, AltTester® Unity SDK picks up the connection and carries out the tests
    - you should add an action that keeps Appium alive in the `TearDown` method of the framework to ensure that Appium is used after every test. Here is an example:

    ```eval_rst
    .. tabs::

        .. tab:: Android

            .. code-block:: C#

                appiumDriver.GetDisplayDensity();

        .. tab:: iOS

            .. code-block:: C#

                appiumDriver.GetClipboardText();
    ```

- OneTimeTearDown: Quit the Appium driver and stop the local tunnel
    - at the end of your tests, add these methods in order to quit the driver and stop the BrowserStack local connection:

    ```c#
    [OneTimeTearDown]
    public void DisposeAppium()
    {
        Console.WriteLine("Ending");
        appiumDriver.Quit();
    altDriver.Stop();
        if (browserStackLocal != null)
        {
                browserStackLocal.stop();
        }
    }
    ```

- Additional step: Increase the idle timeout to 300s
    - if you have tests that take more than 90 seconds to complete, you can also set the maximum timeout using BrowserStack options:
    ```c#
    browserstackOptions.Add("idleTimeout", "300");
    ```

**6. Have AltTester® Server running on local machine**

One of the [architectural changes from v2.0.0](https://alttester.com/alttester-desktop-2-0-0-alttester-unity-sdk-2-0-0-recorder-support-for-webgl-and-architectural-changes/) is that the AltTester® Server module is incorporated in AltTester® Desktop. In order to be able to execute tests, you need to have the **AltTester® Desktop running** so that the AltDriver from the tests can connect to the local server. You can download the free version from [our website](https://alttester.com/alttester/#pricing).

**7. Run the tests**

Make sure AltTester® Desktop is running, the environment variables are set and then trigger the test execution from the terminal with `dotnet test`.

### BrowserStack with GitHub Actions C# project example

A benefit that comes with the GitHub integration is that you don't need to have the AltTester® Desktop app running on your computer in order to run the tests. In this case **AltTester® Server** is running on another machine, making it possible for any team member to trigger the workflow that runs the tests on BrowserStack.

You can download our example project from [here](https://github.com/alttester/EXAMPLES-CSharp-Cloud-Services-AltTrashCat). Also, for more details check [this article](https://alttester.com/how-to-run-alttester-based-c-tests-on-browserstack-using-github-actions/) from our Blog.

```eval_rst

.. note::
    This example was created for running tests on a single device.

```

For this integration, the best solution is to create and use a **self-hosted runner** because it allows you to install and run the AltTester® Desktop app that keeps AltTester® Server active at all times. Starting with the [AltTester® Unity SDK 2.0.0](https://alttester.com/alttester-desktop-2-0-0-alttester-unity-sdk-2-0-0-recorder-support-for-webgl-and-architectural-changes/) update it is required to have AltTester® Server running in order to run tests.

#### **Local testing connection using BrowserStackLocal**

An important aspect of running tests on BrowserStack is that there’s a [local testing connection](https://www.browserstack.com/docs/app-automate/appium/getting-started/c-sharp/nunit/local-testing#3-configure-and-run-your-local-test) needed. Local Testing, a BrowserStack option, allows us to conduct automated test execution for mobile apps that access resources hosted in development or testing environments.
 
In this case we need a connection between the self-hosted runner that executes the tests and the BrowserStack device.

```eval_rst
.. image:: ../_static/img/alttester-with-cloud/browserstack-local-github-diagram.png
```

#### **Prerequisites**

- Test suite - we used [EXAMPLES-TrashCat-Tests](https://github.com/alttester/EXAMPLES-TrashCat-Tests)
- GitHub account and repository with the test suite
- [BrowserStack account](https://www.browserstack.com/users/sign_in?utm_source=google&utm_medium=cpc&utm_platform=paidads&utm_content=610276476173&utm_campaign=Search-Brand-Tier2-EMEA-CL&utm_campaigncode=BrowserStack.com+1011804&utm_term=p+browserstack%20com) - there is a free plan available that offers 100 min to run tests
- .NET v5.0+ and NUnit v3.0.0+
- A machine available to be defined as a self-hosted GitHub runner

#### **Setup steps**

For creating this testing context a Windows machine was defined as runner, but it is possible to use other systems as well. Please follow the [GitHub documentation](https://docs.github.com/en/actions/hosting-your-own-runners/managing-self-hosted-runners/adding-self-hosted-runners) for more details.

**1. Create a self-hosted runner and have AltTester® Server running**

- the `Listening for jobs` line confirms that it is up and running
- now [download](https://alttester.com/alttester/#pricing), install and open the AltTester® Desktop app (which contains AltTester® Server)

**2. Upload a test build of your app in BrowserStack and get the generated URL**

Once you are logged into your BrowserStack account, upload your build instrumented with AltTester® Unity SDK using the UI button available on Dashboard.
```eval_rst
.. image:: ../_static/img/alttester-with-cloud/browserstack-upload-build.png
```

**3. Set credentials and app id as GitHub Secrets**

In order to be authenticated, you need to define your username, access key and app ID (optional) as environment variables. To do that, you should set them as [GitHub Secrets](https://docs.github.com/en/actions/security-guides/encrypted-secrets?tool=webui#creating-encrypted-secrets-for-a-repository) first, due to the sensitivity of the information. Secrets can be defined in the Settings section of your repository.

Here is an example:

```
set BROWSERSTACK_USERNAME "yourUsername"
set BROWSERSTACK_ACCESS_KEY "yourAccessKey"
set BROWSERSTACK_APP_ID_SDK_201="yourAppId"
```

**4. Create .yml file and set environment variables**

Create a `.yml` file in your repository, under the `.github/workflows` folders. You can find it in the example repository as `BrowserStack.yml`.

To set the GitHub Secrets values as environment variables for your tests, use the following lines in the `.yml` workflow file:
```
env:
  BROWSERSTACK_USERNAME: ${{secrets.BROWSERSTACK_USERNAME}}
  BROWSERSTACK_ACCESS_KEY: ${{secrets.BROWSERSTACK_ACCESS_KEY}}
  BROWSERSTACK_APP_ID_SDK_201: ${{secrets.BROWSERSTACK_APP_ID_SDK_201}}
```

**5. Create a new file for BrowserStack and Appium configuration**

In your repository, create a new file that will hold the required settings for the integration. This file will hold all of the **Appium** and **BrowserStackLocal** settings that ensure the connection between the local environment and the cloud device. For future reference it will be called [BaseTest](https://github.com/alttester/EXAMPLES-CSharp-BrowserStack-AltTrashCat/blob/main/tests/BaseTest.cs). Every test file in this project inherits this C# class.

In this file add code that will:
- access the environment variables for BrowserStack credentials and app ID
    ```c#
    String BROWSERSTACK_USERNAME=Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
    String BROWSERSTACK_ACCESS_KEY = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
    String BROWSERSTACK_APP_ID_SDK_201 =
    Environment.GetEnvironmentVariable("BROWSERSTACK_APP_ID_SDK_201");
    ```
- configure **Appium capabilities** (device details) and **BrowserStack options** (e.g. authentication, session details, local tunnel settings)

    Example:

    ```eval_rst
    .. tabs::

        .. tab:: Android

            .. code-block:: C#

                AppiumOptions capabilities = new AppiumOptions();
                Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();

                browserstackOptions.Add("projectName", "TrashCat");
                browserstackOptions.Add("buildName", "TrashCat201Android");
                browserstackOptions.Add("sessionName", "tests - " + DateTime.Now.ToString("MMMM dd - HH:mm"));
                browserstackOptions.Add("local", "true");
                browserstackOptions.Add("userName", BROWSERSTACK_USERNAME);
                browserstackOptions.Add("accessKey", BROWSERSTACK_ACCESS_KEY);
                capabilities.AddAdditionalCapability("bstack:options", browserstackOptions);
                capabilities.AddAdditionalCapability("platformName", "android");
                capabilities.AddAdditionalCapability("platformVersion", "11.0");
                capabilities.AddAdditionalCapability("appium:deviceName", "Samsung Galaxy S21");
                capabilities.AddAdditionalCapability("appium:app", BROWSERSTACK_APP_ID_SDK_201);

        .. tab:: iOS

            .. code-block:: C#

                AppiumOptions capabilities = new AppiumOptions();
                Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();

                browserstackOptions.Add("projectName", "TrashCat");
                browserstackOptions.Add("buildName", "TrashCat201iOS");
                browserstackOptions.Add("sessionName", "tests - " + DateTime.Now.ToString("MMMM dd - HH:mm"));
                browserstackOptions.Add("local", "true");
                browserstackOptions.Add("userName", BROWSERSTACK_USERNAME);
                browserstackOptions.Add("accessKey", BROWSERSTACK_ACCESS_KEY);
                capabilities.AddAdditionalCapability("bstack:options", browserstackOptions);
                capabilities.AddAdditionalCapability("platformName", "ios");
                capabilities.AddAdditionalCapability("platformVersion", "16");
                capabilities.AddAdditionalCapability("appium:deviceName", "iPhone 14");
                capabilities.AddAdditionalCapability("appium:app", BROWSERSTACK_APP_ID_SDK_201);

    ```

- configure the local testing connection
    - using the BrowserStackLocal package, you need to start the local testing connection. You can do this in the code like this:

    ```c#
    browserStackLocal = new Local();
    List<KeyValuePair<string, string>> bsLocalArgs = new List<KeyValuePair<string, string>>() {
                            new KeyValuePair<string, string>("key", BROWSERSTACK_ACCESS_KEY)
    };
    browserStackLocal.start(bsLocalArgs);
    ```
    - this local connection can also be started manually. You need to download the [local binary from BrowserStack docs](https://www.browserstack.com/docs/local-testing/releases-and-downloads) and run the `.exe` file from its path

- start Appium Driver
    - in order to test remotely on BrowserStack, you need to use an instance of Appium Driver. For that, use the remote BrowserStack URL and your access credentials, which are stored inside the AppiumOptions variable named `capabilities`:
    ```eval_rst
    .. tabs::

        .. tab:: Android

            .. code-block:: C#

                appiumDriver = new AndroidDriver<AndroidElement>(new Uri("https://hub-cloud.browserstack.com/wd/hub/"), capabilities);

        .. tab:: iOS

            .. code-block:: C#

                appiumDriver = new IOSDriver<IOSElement>(new Uri("https://hub-cloud.browserstack.com/wd/hub/"), capabilities);

    ```

- initialize AltDriver
    ```c#
    altDriver = new AltDriver();
    ```

- [iOS] Handle permission pop-up
    - while running your tests on iOS you might get a pop-up that asks for permission to connect to devices on the local network
    - to accept this notification and give permission, use the following lines:
    ```c#
    IWebElement ll = appiumDriver.FindElement(OpenQA.Selenium.By.Id("Allow"));
    ll.Click();
    ```

- add method to keep Appium alive
    - in this context, Appium is only used to install the application and access it on the BrowserStack test device - after that, AltTester® SDK picks up the connection and carries out the tests
    - you should add an action that keeps Appium alive in the `TearDown` method of the framework to ensure that Appium is used after every test. Here is an example:

    ```eval_rst
    .. tabs::

        .. tab:: Android

            .. code-block:: C#

                appiumDriver.GetDisplayDensity();

        .. tab:: iOS

            .. code-block:: C#

                appiumDriver.GetClipboardText();
    ```

- OneTimeTearDown: Quit the Appium driver and stop the local tunnel
    - at the end of your tests, add these methods in order to quit the driver and stop the BrowserStack local connection:

    ```c#
    [OneTimeTearDown]
    public void DisposeAppium()
    {
        Console.WriteLine("Ending");
        appiumDriver.Quit();
    altDriver.Stop();
        if (browserStackLocal != null)
        {
                browserStackLocal.stop();
        }
    }
    ```

- Additional step: Increase the idle timeout to 300s
    - if you have tests that take more than 90 seconds to complete, you can also set the maximum timeout using BrowserStack options:
    ```c# 
    browserstackOptions.Add("idleTimeout", "300");
    ```

**6. Define the workflow and run it**

Inside the workflow file (`BrowserStack.yml`), define a job that will configure the machine environment and run the tests. To do that, give a name to your job, assign the runner using labels and define the steps needed to run your tests.

You’ll see four steps defined for this job in the example repository:
- *Checkout the repository* - so that the workflow can access it
- *Setup dotnet* - in order to use a specific version of dotnet in the workflow
- *Restore dependencies* - to ensure that all dependencies required by the project are compatible with each other and there are no conflicts between them
- *Run tests*
```
jobs:
  csharp-tests-job:
    name: 'CSharp tests on BrowserStack'
    runs-on: [self-hosted, services]
    steps:
      - name: 'Checkout the repository'
        uses: actions/checkout@v2
        
      - name: Setup .NET
        uses: actions/setup-dotnet@v1
        with:
          dotnet-version: '7.0'
          
      - name: Restore dependencies
        run: dotnet restore

      - name: 'Running tests on BrowserStack'
        run:
          dotnet test
```
To run the workflow, set when you’d like it to be triggered [at the beginning of the file](https://docs.github.com/en/actions/using-workflows/triggering-a-workflow). In this example the workflow is set to run on every push on the *browserstack-example* branch. 
```
name: BrowserStack TrashCat
on:
  push:
    branches:
      - browserstack-example
```
After the job is picked up by the runner (depending on its availability), and the flow reaches the step *“Running tests on BrowserStack”*, you’ll see in your BrowserStack Dashboard that a new session has started. This is how a successfully running workflow looks like:
```eval_rst
.. image:: ../_static/img/alttester-with-cloud/browserstack-github-actions-workflow.png
```

## SauceLabs

This is another cloud-based platform you may want to use if you want to run AltTester® Unity SDK tests client-side. 

SauceLabs provides **automated testing solutions** for web and mobile applications. It allows developers and testing teams to perform testing across a wide range of **browsers, operating systems and devices** without the need for setting up and maintaining physical hardware or virtual machines.

You can create a free trial SauceLabs account where you get 2000 credits per week (which is enough to run some test suites multiple times on their free devices and emulators).

One of the [architectural changes from v2.0.0](https://alttester.com/alttester-desktop-2-0-0-alttester-unity-sdk-2-0-0-recorder-support-for-webgl-and-architectural-changes/) is that the AltTester® Server module is incorporated in AltTester® Desktop. In order to be able to execute tests, we need to have an **AltTester® Desktop app running and publicly reachable**/accessible so that the AltDriver that is instantiated in the tests and **the instrumented app can connect to the AltTester® Server**.

### SauceLabs C# project example

You can download our example project from [here](https://github.com/alttester/EXAMPLES-CSharp-Cloud-Services-AltTrashCat/tree/saucelabs_example). Also, for more details check [this article](https://alttester.com/sauce-labs-integration-execute-alttester-based-c-tests/) from our Blog.

<!-- To update here when there are updates -->
At the moment of creating this section of the documentation for the case of having the AltTester® Desktop app running on the same machine where the tests are running the instrumented app was not able to connect to localhost successfully, due to the fact that the 
[Sauce Connect Tunnel Proxy](https://docs.saucelabs.com/secure-connections/sauce-connect/setup-configuration/basic-setup/) implementation was not yet compatible with the WebSocket used in AltTester® Server. But more recently, SauceLabs confirmed that WebSocket communication is now working for tunnel connections between tests and cloud devices. For this reason, we are now working on running the tests using **Sauce Connect Tunnel Proxy** and we will provide setup instructions shortly.

In this example, the AltTester® Desktop app is running on a **public virtual machine**, which **can be accessed by the instrumented app** installed on a device in the cloud.

#### **Prerequisites**

**1. Create a virtual machine and install AltTester® Desktop** 

It can either be a **Windows** virtual machine running AltTester® Desktop in **GUI mode** or a **Linux** machine running in **batchmode** (note that the batchmode requires an [AltTester® license](https://alttester.com/alttester/#pricing) key)

For this purpose, an [Azure virtual machine](https://azure.microsoft.com/en-us/pricing/free-services/) was used, configured with specific inbound and outbound rules to facilitate the build's connection. See the documentation for more detailed instructions on [how to create a Windows VM in the Azure portal](https://learn.microsoft.com/en-us/azure/virtual-machines/windows/quick-create-portal#create-virtual-machine).

- virtual machine **network settings**
    - the virtual machine used for this example is running on **Windows Server 2019 Datacenter, x64 architecture**, Gen2 - it is configured with inbound and outbound rules
    - besides the default port rules created, in order to make AltTester® Server visible by external devices, it was needed to create an Inbound port rule for *Protocol*: **TCP**, *Port*: **13000** and *Source*: **Any** (destination)

    ```eval_rst
    .. image:: ../_static/img/alttester-with-cloud/sauce-labs-virtual-machine-settings1.png
    ```
    <br>

    ```eval_rst
    .. image:: ../_static/img/alttester-with-cloud/sauce-labs-virtual-machine-settings2.png
    ```
    - another necessary setting: [turn Microsoft Defender Firewall off](https://support.microsoft.com/en-us/windows/turn-microsoft-defender-firewall-on-or-off-ec0844f7-aebd-0583-67fe-601ecf5d774f) on the virtual machine
- [download AltTester® Desktop](https://alttester.com/alttester/#pricing) on your virtual machine, install it, launch it and leave it running and listening on port `13000`
    ```eval_rst
    .. image:: ../_static/img/alttester-with-cloud/sauce-labs-alttester-desktop.png
    ```

**2. Have a set of C# tests that use AltTester® Unity SDK v2.0.\***

- check the [example repository](https://github.com/alttester/EXAMPLES-CSharp-Cloud-Services-AltTrashCat/tree/saucelabs_example)

**3. Prepare the build instrumented with AltTester® Unity SDK v2.0.\***

Our example is based on [TrashCat endless runner game](https://assetstore.unity.com/packages/templates/tutorials/endless-runner-sample-game-87901) that we have instrumented in Unity. 
- instrument the TrashCat application using AltTester® Unity SDK `v2.0.*`- for additional information you can follow [this tutorial](https://alttester.com/walkthrough-tutorial-upgrading-trashcat-to-2-0-x/#Instrument%20TrashCat%20with%20AltTester%20Unity%20SDK%20v.2.0.x)
- it should be created with the host IP of the VM in which AltDesktop app is running

```eval_rst

.. note::
    In order to enable automatic connection between the build and the virtual machine, it's essential for the build to have the predefined IP address of the previous virtual machine. This implies that the build will need to be re-instrumented in Unity, with its IP address set to that of the machine.
```
#### **Steps for running tests on Android and iOS**

**1. Upload the instrumented build on SauceLabs**

Before running any tests, you are required to [upload the build to the designated page on the SauceLabs platform](https://app.eu-central-1.saucelabs.com/app-management)
- in **App Management** choose the file you want to upload
- once uploaded, SauceLabs automatically handles the installation and uninstallation of the application, based on the specifications in the code, thereby eliminating the need for manual intervention or writing additional code for installation (just the application name needs to be specified)

**2. Install dependencies**

- install Appium WebDriver
    ```
    dotnet add package Appium.WebDriver --version 4.4.0
    ```

**3. Set your SauceLabs credentials as environment variables**

- once you are logged into your SauceLabs account, you can find the credentials by pressing the key from the main menu
- to set these values as environment variables on Windows you can create a **batch file** on your local machine and run it every time you load your IDE - this will keep the sensitive information out of the repository
    
    Example:
    ```
    set SAUCE_USERNAME "yourUsername"
    set SAUCE_ACCESS_KEY "yourAccessKey"
    set HOST_ALT_SERVER "your VM's IP"
    ```
**4. Create and configure new file**
    
SauceLabs offers a convenient configuration system that allows capabilities to be written and seamlessly integrated into the code- this can be achieved, for instance, by utilizing a base class (in this case, it was used [BaseTest](https://github.com/alttester/EXAMPLES-CSharp-Cloud-Services-AltTrashCat/blob/saucelabs_example/tests/BaseTest.cs)) which all test classes inherit

In this file add code that will:
- access the environment variables set in the previous steps using the GetEnvironmentVariable method like so:
    ```c#
    String SAUCE_USERNAME = Environment.GetEnvironmentVariable("SAUCE_USERNAME");
    String SAUCE_ACCESS_KEY = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY");
    ```
- configure Appium capabilities and Sauce Labs options
    ```eval_rst
    .. tabs::

        .. tab:: Android

            .. code-block:: C#

                AppiumOptions options = new AppiumOptions();
                options.AddAdditionalCapability("platformName", "Android");
                options.AddAdditionalCapability("appium:app","storage:filename=<buildName.apk>");

                options.AddAdditionalCapability("appium:deviceName", "Samsung Galaxy S10 WQHD GoogleAPI Emulator");

                options.AddAdditionalCapability("appium:platformVersion", "11.0");
                options.AddAdditionalCapability("appium:deviceOrientation", "portrait");
                options.AddAdditionalCapability("appium:automationName", "UiAutomator2");

                var sauceOptions = new Dictionary<string, object>();
                sauceOptions.Add("appiumVersion", "2.0.0");
                sauceOptions.Add("username", SAUCE_USERNAME);
                sauceOptions.Add("accessKey", SAUCE_ACCESS_KEY); 
                sauceOptions.Add("build", "<name of the build / any name you want for your test>");
                sauceOptions.Add("name", "Test " + DateTime.Now.ToString("dd.MM - HH:mm"));
                options.AddAdditionalCapability("sauce:options", sauceOptions);


        .. tab:: iOS

            .. code-block:: C#

                AppiumOptions options = new AppiumOptions();
                
                options.AddAdditionalCapability("platformName", "iOS");
                options.AddAdditionalCapability("appium:app", "storage:filename=<builName.ipa>");
                        
                options.AddAdditionalCapability("appium:deviceName", "iPhone XR");
                    

                options.AddAdditionalCapability("appium:platformVersion", "16");
                    
                options.AddAdditionalCapability("appium:deviceOrientation", "portrait");

                options.AddAdditionalCapability("appium:automationName", "XCUITest");

                var sauceOptions = new Dictionary<string, object>();
                sauceOptions.Add("appiumVersion", "2.0.0");
                sauceOptions.Add("username", SAUCE_USERNAME);
                sauceOptions.Add("accessKey", SAUCE_ACCESS_KEY); 
                sauceOptions.Add("build", "<name of the build / any name you want for your test>");
                sauceOptions.Add("name", "Test " + DateTime.Now.ToString("dd.MM - HH:mm"));
                options.AddAdditionalCapability("sauce:options", sauceOptions);

    ```

- start Appium Driver

    - in order to test remotely on SauceLabs, it is necessary to employ an instance of the **Appium Driver**. To do so, use the remote **SauceLabs URL** along with your access credentials, which can be found within the *AppiumOptions* variable named `options`:

    ```eval_rst
    .. tabs::

        .. tab:: Android

            .. code-block:: C#

                appiumDriver = new AndroidDriver<AndroidElement>(new Uri("https://ondemand.eu-central-1.saucelabs.com:443/wd/hub"), options);

        .. tab:: iOS

            .. code-block:: C#

                appiumDriver = new IOSDriver<IOSElement>(new Uri("https://ondemand.eu-central-1.saucelabs.com:443/wd/hub"), options);

    ```

- initialize AltDriver with custom IP for the Host parameter

    - Since the AltTester® Server is running and listening on the Windows VM previously created, the test classes need to know how to connect to it. In order to enable this connection, the Host parameter can be used when AltDriver is instantiated in the `BaseTest.cs` file.

    - To overcome slow build launches causing test failures due to insufficient waiting, instantiate the `altDriver` with a `connectTimeout` of `3000`.
    ```c#
    String HOST_ALT_SERVER = Environment.GetEnvironmentVariable("HOST_ALT_SERVER");
    altDriver = new AltDriver(HOST_ALT_SERVER, connectTimeout: 3000);
    ```

- [iOS] Handle permission pop-up
    - while running your tests on iOS you might get a pop-up that asks for permission to connect to devices on the local network
    - to accept this notification and give permission, use the following lines:
    ```c#
    IWebElement ll = appiumDriver.FindElement(OpenQA.Selenium.By.Id("Allow"));
    ll.Click();
    ```

- add method to keep Appium alive
    - in this context, Appium is only used to install the application and access it on the BrowserStack test device - after that, AltTester® SDK picks up the connection and carries out the tests
    - you should add an action that keeps Appium alive in the `TearDown` method of the framework to ensure that Appium is used after every test. Here is an example:

    ```eval_rst
    .. tabs::

        .. tab:: Android

            .. code-block:: C#

                appiumDriver.GetDisplayDensity();

        .. tab:: iOS

            .. code-block:: C#

                appiumDriver.GetClipboardText();
    ```

**5. Run the tests**

Before running the tests, make sure that:
- AltTester® Desktop **is running** on the virtual machine
- SauceLabs credentials and VM's IP are **set as environment variables**

Now trigger the test execution from the terminal with `dotnet test`.

You can find your tests results in the designated section **Tests Results**
```eval_rst
.. image:: ../_static/img/alttester-with-cloud/sauce-labs-tests-results.png
```

## AWS Device Farm

Amazon offers another great alternative to cloud mobile testing, in the form of [**AWS Device Farm**](https://docs.aws.amazon.com/devicefarm/index.html). You can register for free and get a 1000 device minutes trial period (a credit card will be required for registration).

Because the application is instrumented with AltUnityTester SDK with a version ≥ v2.0.0, AltTester® Server is no longer integrated into the instrumented application. In order to connect to AltTester® Server, AltTester® Desktop needs to be running and accessible from the devices running inside the AWS Device Farm. 

You can connect to AltTester® Desktop in two ways:

**A. A remote connection**

- AltTester® Desktop is opened in a remote location (e.g. a virtual machine in the cloud) from where it can be accessed through IP/URL.
- For the purpose of this example AltTester® Desktop was installed on an AWS instance. You can connect with the application through IP/URL. In this case, during instrumentation, you need to specify the IP/URL as the AltServerHost. 
- This works for both Android and iOS.

**B. A local connection**

- AltTester® Desktop is installed on the AWS Device Farm VM. Therefore a localhost connection is established, so there is no need for setting the host during instrumentation. A license for running the application in batch mode is needed as well, which is stored separately in `license.txt` (Be aware to not make this file public).
- The local connection works only for Android. Unfortunately, [IProxy does not have a way of setting up reverse port forwarding](https://alttester.com/docs/sdk/latest/pages/advanced-usage.html#in-case-of-ios).

You will need two files in order to run your tests:
* **.apk** file, with a build of your app containing the AltDriver;
* A **.zip** file containing your tests, the batchmode Linux AltTester® Desktop build (in case of local connection), and other configuration files depending on the language you're running the tests on.

### AWS Device Farm Python project example

You can download our example project from [here](https://github.com/alttester-test-examples/Python-AWS-AltTrashCat).

#### **Preparation steps**

**1. Prepare the application**
- instrument the TrashCat application using AltTester® Unity SDK `v2.1.2`- For additional information you can follow [this tutorial](https://alttester.com/walkthrough-tutorial-upgrading-trashcat-to-2-0-x/#Instrument%20TrashCat%20with%20AltTester%20Unity%20SDK%20v.2.0.x).
- Based on your option to connect to AltTester® Desktop you need to set AltTester® Server Host of the instrumented app to:
    - localhost (`127.0.0.1`) - for local connection 
    - IP/URL provided by the AWS Instance where AltTester® Desktop is running - for remote connection

```eval_rst

.. note::
    If you want to use the AltTester® Desktop Community edition (FREE plan) you need to create a Windows AWS Instance. You may only test on 1 device at a time for this plan.
```

```eval_rst

.. note::
    If you are using the AltTester® Desktop PRO edition for Linux batchmode you should take into account that when running tests on multiple devices for each device 1 license seat is required, as in the AWS Device Farm the tests run concurrently on devices and for each device, one instance of the AltTester® Desktop is launched. In conclusion, you may only test on as many devices at a time as the available number of seats for your license key. 

```

**2. Prepare test code and dependencies**
- **for local connection** - from the cloned repository, create a **.zip** file containing:
    - a `license.txt` file which will store your AltTester® Desktop PRO license, needed to run batch mode commands
    - the `requirements.txt` file
    - the `tests` folder (which contains also the `pages` folder)

- **for remote connection** - from the cloned repository, create a **.zip** file containing:
    - the `requirements.txt` file
    - the `tests` folder (which contains also the `pages` folder) - don`t forget to add the IP/URL of the remote VM when defining AltDriver in ``base_test.py``
 
Make sure to place all the files in the root directory.

**3. Prepare the configuration file**
- for a custom [test environment](https://docs.aws.amazon.com/devicefarm/latest/developerguide/custom-test-environments.html) you can edit the default configuration file by adding the needed commands. The commands are written as YAML so there are some validations for them
- using the commands from the configuration file you can access the contents of the uploaded **.zip** folder, install applications, and ultimately run your tests

Keep in mind that the setup is different for Android and iOS. 

- **for local connection**
    - here are the commands needed in the Configuration file to run [AltTester® Server in batchmode](https://alttester.com/docs/desktop/latest/pages/advanced-usage.html#running-altserver-in-terminal). You can choose to include these commands in your test code or add it in the Test Configuration file like in our example.

        ```
        - export LICENSE_KEY=$(cat license.txt)
        - wget https://alttester.com/app/uploads/AltTester/desktop/AltTesterDesktopLinuxBatchmode.zip
        - unzip AltTesterDesktopLinuxBatchmode.zip
        - cd AltTesterDesktopLinux
        - chmod +x AltTester Desktop.x86_64
        - ./AltTesterDesktop.x86_64 -batchmode -port 13000 -license $LICENSE_KEY -nographics -termsAndConditionsAccepted &
        ```
        The `&` symbol is used to make the application run in the background. Failure to add the symbol will cause the commands following it to not be triggered. 
    
        ```eval_rst

        .. note::
            We recommend using ``wget`` in order to install the `batchmode Linux build for AltTester® Desktop <https://alttester.com/app/uploads/AltTester/desktop/AltTesterDesktopLinuxBatchmode.zip>`_ and not put it in the archive because that increases the running time for the entire flow.
        ```

    - You also need to add reverse port forwarding when running on Android after Appium is started: 
        ```
        - adb reverse -- remove-all
        - adb reverse tcp:13000 tcp:13000
        ```
    - kill the AltTester® Desktop process 
        ```
        - cd AltTesterDesktopLinux
        - kill -2 `ps -ef | awk '/AltTesterDesktop.x86_64/{print $2}'`
        ```
- **for remote connection** - a way to connect to AltTester® Server, within the AltTester® Desktop application is by installing AltTester® Desktop on an [Amazon EC2 Instance](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/Instances.html). The details of creating an EC2 Instance are out of scope, however, these are the main things to take into account for a successful connection: 
    - create a [Windows instance](https://docs.aws.amazon.com/AWSEC2/latest/WindowsGuide/EC2Win_Infrastructure.html) 
    ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/aws-windows-instance.png
    ```
    - add [Inbound rule to Security Group](https://docs.aws.amazon.com/AWSEC2/latest/WindowsGuide/working-with-security-groups.html#changing-security-group) to make port `13000` accessible - the custom TCP on port `13000` is needed to have the connection to AltTester® Desktop default `13000` port  
    ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/aws-inbound-rule-to-security-group.png
    ``` 
    - [Connect to the Instance](https://docs.aws.amazon.com/AWSEC2/latest/WindowsGuide/connecting_to_windows_instance.html) through Remote Access Connection  
    ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/aws-connect-to-instance.png
    ```  
    - [Download AltTester® Desktop for Windows](https://alttester.com/app/uploads/AltTester/desktop/AltTesterDesktop__v2.1.2.exe) and install it on the Instance  
    - [Associate an Elastic IP](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/elastic-ip-addresses-eip.html#using-instance-addressing-eips-associating), so that the IP remains constant after each opening of the instance
    ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/aws-associate-elastic-ip.png
    ```
```eval_rst

.. note::
    Please make sure to deactivate any Firewalls on the Windows VM, as it might block the connection.
```

#### **Steps for running tests on Android using the remote connection**

The instructions and resources will be for running tests on Android, for an application instrumented with AltTester® Unity SDK v2.1.2 with a specific host (the elastic IP address provided when creating a remote connection), so that we can connect to it from an AWS Instance (remote connection).

```eval_rst

.. important::
    The remote connection needs to be opened and AltTester® Desktop has to be running when starting a new automated run.
```
**1. Create a new project on AWS Device Farm**
- Access [AWS Device Farm](https://us-west-2.console.aws.amazon.com/devicefarm/home?region=us-east-1#/mobile/projects) of your account and create a new project like instructed [here](https://docs.aws.amazon.com/devicefarm/latest/developerguide/getting-started.html), making sure you are in `Mobile Device Testing` > `Projects`

**2. Create a new run**
- on the created project select `Create a new run`
- add the instrumented application build making sure you are on the `Mobile App` tab
- at the configuration step, choose *"Appium Python"* for the purpose of this example, then upload the **.zip** file
- the example project contains a **.yml** configuration file. Use it at the next step, by selecting *"Run your test in a custom environment"*. This will define how your test environment is set up and how the tests run
- the next step will allow you to select on which devices the tests will be executed - You can create your own device pool, or use the recommended top devices
- for this example, no changes need to be done to the Device state configuration
- at the last step, you can set the execution timeout for your devices, then start the tests

Once the status is available the project screen will show the overall status of the tests execution progress and results. Selecting individual runs and devices will give detailed logs about the tests, together with a video recording of the run itself.

```eval_rst

.. note::
    The purpose of the example project is to offer a guide on how to run tests on AWS Device Farm, tests that are not adapted to pass on any type of Android device. In order for them to pass feel free to bring adjustments based on your needs.
```

### AWS Device Farm C# project example

AWS Device Farm does not offer an out-of-the-box solution for running tests written in C#, since it doesn't have an option for it, like in the case of Java or Python, for which there are Test Framework options available. However, using the Appium-ruby configuration from the test-type selection allows the upload of the tests written in C# as a zipped folder. 

Check [this article](https://alttester.com/running-c-tests-with-alttester-on-aws-device-farm/) from our Blog for more details on how to create the setup for running C# tests on the AWS Device Farm. The example project can be found [here](https://github.com/alttester/EXAMPLES-CSharp-AWS-AltTrashCat)

#### **Preparation steps**

**1. Prepare the application**
- get info in the same section from [the Python project example](#aws-device-farm-python-project-example)

**2. Prepare test code and dependencies**
- use the accepted .NET version (currently the .NET `6.0` version is required on the AWS Virtual Machine)
    ```
    <TargetFramework>net6.0</TargetFramework>
    ```
- install necessary packages - run the following commands in your project`s terminal:
    ```c#
    dotnet add package Appium.WebDriver --version 4.4.0 
    dotnet add package Selenium.WebDriver --version 3.141.0
    ```
- add Namespaces specific to Appium
    ```c#
    using OpenQA.Selenium.Appium;
    using OpenQA.Selenium.Appium.Android;
    using OpenQA.Selenium.Appium.iOS;
    using System.Net;
    ```
- create BaseTest Class
    - Appium driver declaration
        ```eval_rst
            .. tabs::

                .. tab:: Android

                    .. code-block:: C#

                        AndroidDriver<AndroidElement> appiumDriver;
                    
                .. tab:: iOS

                    .. code-block:: C#

                        IOSDriver<IOSElement> appiumDriver;
        ```
    - define Appium Capabilities
        ```eval_rst
            .. tabs::

                .. tab:: Android

                    .. code-block:: C#

                        capabilities.AddAdditionalCapability("device", "Android");
                        capabilities.AddAdditionalCapability("platformName", "Android");
                        capabilities.AddAdditionalCapability("appActivity", "com.unity3d.player.UnityPlayerActivity");
                    
                .. tab:: iOS

                    .. code-block:: C#

                        capabilities.AddAdditionalCapability("device", "iOS");
                        capabilities.AddAdditionalCapability("platformName", "iOS");
                        capabilities.AddAdditionalCapability("appPackage", "fi.altom.trashcat");
                        capabilities.AddAdditionalCapability("autoAcceptAlerts" ,true);
        ```
    - driver initialization and wait
        ```eval_rst
            .. tabs::

                .. tab:: Android

                    .. code-block:: C#

                        var appiumUri = new Uri("http://localhost:4723/wd/hub");

                    .. code-block:: C#

                        appiumDriver = new AndroidDriver<AndroidElement>(appiumUri, capabilities, TimeSpan.FromSeconds(300));
                    
                .. tab:: iOS

                    .. code-block:: C#
                      
                        var appiumUri = new Uri("http://localhost:4723/wd/hub");

                    .. code-block:: C#

                        appiumDriver = new IOSDriver<IOSElement>(appiumUri, capabilities, TimeSpan.FromSeconds(300));
        ```
    - cleanup - the `DisposeAppium()` teardown method is called after the tests are complete. It quits the Appium driver
- if using the **remote connection** in order to connect to AltTester® Desktop, don`t forget to add the IP/URL of the remote VM when defining AltDriver: 
    ```
    altDriver = new AltDriver(port:13000, host: "insert_ip_here");
    ```
- prepare the `.zip` folder containing tests and necessities
    - **for local connection** - from the cloned repository, create a **.zip** file containing:
        - a `license.txt` file which will store your AltTester® Desktop PRO license, needed to run batch mode commands
        - the `tests` and `pages` folders
        - other dependencies (`.csproj`, etc.)

    - **for remote connection** - from the cloned repository, create a **.zip** file containing:
        - the `tests` and `pages` folders - don`t forget to add the IP/URL of the remote VM when defining AltDriver in ``base_test.py``
        - other dependencies (`.csproj`, etc.)
    
    - make sure to place all the files in the root directory

**3. Prepare the configuration file**
- for a custom [test environment](https://docs.aws.amazon.com/devicefarm/latest/developerguide/custom-test-environments.html) you can edit the default configuration file by adding the needed commands. The commands are written as YAML so there are some validations for them
- using the commands from the configuration file you can access the contents of the uploaded **.zip** folder, install applications, and ultimately run your tests

Keep in mind that the setup is different for Android and iOS.

- **regardless of the connection type (local or remote)**
    - .NET 6.0 installation
        ```
        - curl -O -L https://dot.net/v1/dotnet-install.sh
        - chmod +x ./dotnet-install.sh
        - bash ./dotnet-install.sh --channel 6.0
        - export PATH=$PATH:$HOME/.dotnet
        - dotnet -- version
        ```
    - install necessary packages for running the C# tests
        ```
        - dotnet add package NUnit --version 3.13.3
        - dotnet add package AltTester-Driver -- version 2.0.1 
        - dotnet add package Selenium.WebDriver -- version 3.141.0
        - dotnet add package NUnit3TestAdapter --version 4.4.2
        ```
    - for running tests, just add the `dotnet` command
        ```
        dotnet test
        ```
- **for local connection**
    - here are the commands needed in the Configuration file to run [AltTester® Server in batchmode](https://alttester.com/docs/desktop/latest/pages/advanced-usage.html#running-altserver-in-terminal). You can choose to include these commands in your test code or add it in the Test Configuration file like in our example.

        ```
        - export LICENSE_KEY=$(cat license.txt)
        - wget https://alttester.com/app/uploads/AltTester/desktop/AltTesterDesktopLinuxBatchmode.zip
        - unzip AltTesterDesktopLinuxBatchmode.zip
        - cd AltTesterDesktopLinux
        - chmod +x AltTesterDesktop.x86_64
        - ./AltTesterDesktop.x86_64 -batchmode -port 13000 -license $LICENSE_KEY -nographics -termsAndConditionsAccepted &
        ```
        The `&` symbol is used to make the application run in the background. Failure to add the symbol will cause the commands following it to not be triggered. 
    
        ```eval_rst

        .. note::
            We recommend using ``wget`` in order to install the `batchmode Linux build for AltTester® Desktop <https://alttester.com/app/uploads/AltTester/desktop/AltTesterDesktopLinuxBatchmode.zip>`_ and not put it in the archive because that increases the running time for the entire flow.
        ```

    - You also need to add port reverse forwarding when running on Android after Appium is started: 
        ```
        - adb reverse -- remove-all
        - adb reverse tcp:13000 tcp:13000
        ```
    - kill the AltTester® Desktop process: 
        ```
        - cd AltTesterDesktopLinux
        - kill -2 `ps -ef | awk '/AltTesterDesktop.x86_64/{print $2}'`
        ```

- **for remote connection** - a way to connect to AltTester® Server, within the AltTester® Desktop application is by installing AltTester® Desktop on an [Amazon EC2 Instance](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/Instances.html). The details of creating an EC2 Instance are out of scope, however, these are the main things to take into account for a successful connection: 
    - create a [Windows instance](https://docs.aws.amazon.com/AWSEC2/latest/WindowsGuide/EC2Win_Infrastructure.html) 
    ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/aws-windows-instance.png
    ```
    - add [Inbound rule to Security Group](https://docs.aws.amazon.com/AWSEC2/latest/WindowsGuide/working-with-security-groups.html#changing-security-group) to make port `13000` accessible - the custom TCP on port `13000` is needed to have the connection to AltTester® Desktop default `13000` port  
    ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/aws-inbound-rule-to-security-group.png
    ``` 
    - [Connect to the Instance](https://docs.aws.amazon.com/AWSEC2/latest/WindowsGuide/connecting_to_windows_instance.html) through Remote Access Connection  
    ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/aws-connect-to-instance.png
    ```  
    - [Download AltTester® Desktop for Windows](https://alttester.com/app/uploads/AltTester/desktop/AltTesterDesktop__v2.1.2.exe) and install it on the Instance  
    - [Associate an Elastic IP](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/elastic-ip-addresses-eip.html#using-instance-addressing-eips-associating), so that the IP remains constant after each opening of the instance
    ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/aws-associate-elastic-ip.png
    ```
    ```eval_rst

    .. note::
        Please make sure to deactivate any Firewalls on the Windows VM, as it might block the connection.
    ```

#### **Steps for running tests on Android using the remote connection**

Please note that the process of running the tests is similar for iOS or Android. The differences are described above. 

The instructions and resources will be for running tests on Android, for an application instrumented with AltTester® Unity SDK v2.0.1 with a specific host (the elastic IP address provided when creating a remote connection), so that we can connect to it from an AWS Instance (remote connection)

```eval_rst

.. important::
    The remote connection needs to be opened and AltTester® Desktop has to be running when starting a new automated run.
```

**1. Create a new project on AWS Device Farm**
- Access [AWS Device Farm](https://us-west-2.console.aws.amazon.com/devicefarm/home?region=us-east-1#/mobile/projects) of your account and create a new project like instructed [here](https://docs.aws.amazon.com/devicefarm/latest/developerguide/getting-started.html), making sure you are in `Mobile Device Testing` > `Projects`

**2. Create a new run**
- on the created project select `Create a new run`
- add the instrumented application build making sure you are on the `Mobile App` tab
- at the Test Framework configuration step, choose *"Appium Ruby"* for the purpose of this example, then upload the **.zip** file

```eval_rst

.. note::
    Because C# is not a supported framework, there is no option for that. The reason why Appium Ruby is the selected Test Framework is that other setups have stronger validation for the contents of the .zip folder and would mark the uploaded folder as invalid, making it impossible to move forward in the process.
```
- at the next step, select *"Run your test in a custom environment"* and edit the default YAML in order to add the necessary commands, described in the preparation step and then save the file - this will define how your test environment is set up and how the tests run
- the next step will allow you to select on which devices the tests will be executed - You can create your own device pool, or use the recommended top devices (Device Farm offers the option to run tests concurrently on a pool of devices of your choice)
- for this example, no changes need to be done to the Device state configuration
- at the last step, you can set the execution timeout for your devices, then start the tests
    - once the status is available the project screen will show the overall status of the tests execution progress and results - selecting individual runs and devices will give detailed logs about the tests, together with a video recording of the run itself

```eval_rst

.. note::
    The purpose of the example project is to offer a guide on how to run tests on AWS Device Farm, tests that are not adapted to pass on any type of Android device. In order for them to pass feel free to adjustments based on your needs.
```

## BitBar

BitBar is another popular platform that provides access to hundreds of real iOS and Android devices, offering possibilities for testing web, native or hybrid applications. It supports both client-side and server-side test execution.

You can create a free account at <https://cloud.bitbar.com> and try out the test examples detailed below for yourself.

### Brief description of the working setups

In this dashboard you can have an overview of the setup combinations we tried and which were successful:

```eval_rst
.. image:: ../_static/img/alttester-with-cloud/bitbar-serverside-connectivity-dashboard.png
```
*because IProxy does not offer the possibility to do a reverse proxy (similar to how it is possible on adb reverse proxy) the instrumented game build cannot connect to AltTester® Server on localhost.

As in the case of running [Client-Side Appium testing](https://alttester.com/integrate-appium-and-run-your-test-suite-in-bitbar-client-side/), we need to deal with the connectivity between: the test script (where we instantiate [AltDriver](https://alttester.com/docs/sdk/2.1.2/pages/commands.html#altdriver)), AltTester® Desktop and the instrumented application installed on a device in the cloud.

In a local environment, setting up is relatively simple since all three components are co-located and can interact with each other on the localhost at port 13000. For communication with a USB-connected device on **Android**, [reverse port forwarding](https://alttester.com/docs/sdk/2.1.2/pages/commands.html#altreverseportforwarding) is employed to establish connectivity.

For **iOS** devices, things are not so straightforward because IProxy does not offer the possibility to do a reverse proxy (similar to how it is possible on adb reverse proxy) the instrumented game build does not connect to AltTester® Server - please [consult a workaround from the documentation for further details](https://alttester.com/docs/sdk/2.1.2/pages/advanced-usage.html#in-case-of-ios). 

Because we used BitBar’s free plan, we got a machine and we do not have control over what IP it has on each test session. If you are considering doing the same, we strongly recommend having **AltTester® Desktop** installed and launched on a machine which is in your control.

When starting a server-side running test session with **Android devices**, BitBar offers an Ubuntu machine - in this case the script which will set up Appium and execute the tests, also needs to install AltTester® Desktop and activate the license. 

```eval_rst

.. note::
    In order to start the **AltTester® Desktop in batchmode**, it is required you have an **AltTester® Pro license**.
```

For the testing session with iOS devices, BitBar offers a macOS machine. As we detailed above, the connectivity between the instrumented game and AltTester® Server can not be made, so please setup a machine of your choice and install AltTester® Desktop for that OS, as you can find packages for [macOS](https://alttester.com/app/uploads/AltTester/desktop/AltTesterDesktopPackageMac__v2.1.2.zip), [Windows](https://alttester.com/app/uploads/AltTester/desktop/AltTesterDesktopPackageWindows__v2.1.2.zip) and [batchmode Linux build](https://alttester.com/app/uploads/AltTester/desktop/AltTesterDesktopLinuxBatchmode.zip).

### BitBar C# project example running server-side

In BitBar terms, the [server-side](https://support.smartbear.com/bitbar/docs/en/mobile-app-tests/automated-testing/appium-support/running-cloud-side-appium-tests.html) execution means that **we upload** to the platform **everything** we need for the tests to run.

Using a `run-tests.sh` we can install all that is needed, run tests and prepare the test report. For running **C#** tests, part of the setup and installation means: installing `.NET`.

For more details check [this article](https://alttester.com/run-c-tests-with-appium-and-alttester-in-bitbar-server-side/) from our Blog.

#### Prerequisites for running AltTester® Server

You can connect to AltTester® Desktop in two ways in order to run the tests server-side:

**A. A remote connection**

- AltTester® Desktop is opened in a remote location in a **Windows Azure VM** (accessible by both tests and game build through IP)
- the conditions for the connection to work:
    - the IP of the VM needs to be specified in `BaseTest.cs` when **altDriver** is instantiated
    - the game build needs to be instrumented with the same host IP

    **Create a VM for running AltTester® Desktop**

    We used [Azure](https://azure.microsoft.com/en-us/products/virtual-machines/) to create a virtual machine running **Windows x64 architecture** and we set up an AltTester® Desktop instance. Please consult the documentation for more detailed instructions on [how to create a Windows VM in Azure portal](https://learn.microsoft.com/en-us/azure/virtual-machines/windows/quick-create-portal#create-virtual-machine).

    - virtual machine  **network settings** required in order to have this machine publicly reachable by the devices from BitBar:
        - define an **Inbound port rule for protocol TCP on port 13000: Allow connection from Any source**
        ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/bitbar-clientside-remote-connection-network-settings.png
        ```
        - another necessary setting: [Turn off Firewall on the VM](https://support.microsoft.com/en-us/windows/turn-microsoft-defender-firewall-on-or-off-ec0844f7-aebd-0583-67fe-601ecf5d774f)

    - connect using [Remote Desktop Connection](https://support.microsoft.com/en-us/windows/how-to-use-remote-desktop-5fe128d5-8fb1-7a23-3b8a-41e636865e8c) on the machine, [download AltTester® Desktop](https://alttester.com/alttester/#pricing), install it, launch it and leave it running and listening on port `13000`

    ```eval_rst

        .. note::
            You can have AltTester® Server waiting for connections either by starting it manually via GUI or you can use a cmd to start in batchmode (the ease of running in batch mode comes with the requirement to have an **AltTester® Pro license**).
    ```
    - since in our example we chose the batchmode option, we have to set up the path of the AltTester® Desktop app executable in the system **PATH environment variable**
    - then from *Azure portal Operations* > *Run Command* option we choose: *RunPowerShellScript*
        ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/bitbar-clientside-remote-connection-azur-portal-operations.png
        ```
        
        - run command:
        ```
        AltTesterDesktop.exe -batchmode -port 13000 -license <your_license_key> -nographics -logfile LOGFILE.txt
        ```

    We have now a VM where AltTester® Server is listening for connections. Further on we will use the IP of this machine to have the communication between the main actors.

**B. A local connection**

- AltTester® Desktop is installed on the **Bitbar Ubuntu VM**
- the conditions for the connection to work:
    - the script which is executed on Bitbar VM needs to contain the installation and launching of AltTester® Desktop build

    **Download, install and launch AltTester® Desktop Linux build in batch mode**

    In the `run-tests.sh` script (see **3. Prepare the `.zip` archive with tests and `run-tests.sh`** from the [Preparation Steps](#preparation-steps) section) you need to use the following commands:
    ```
    wget https://alttester.com/app/uploads/AltTester/desktop/AltTesterDesktopLinuxBatchmode.zip
    unzip AltTesterDesktopLinuxBatchmode.zip
    cd AltTesterDesktopLinux
    chmod +x ./AltTesterDesktop.x86_64
    ./AltTesterDesktop.x86_64 -batchmode -port 13000 -license $LICENSE_KEY -nographics -termsAndConditionsAccepted &
    ```

    Kill the AltTester® Desktop process:
    ```
    kill -2 `ps -ef | awk '/AltTesterDesktop.x86_64/{print $2}'`
    ```

#### **Preparation steps**

**1. Prepare the application**

You will first need to create an **.apk** (for Android) / **.ipa** (for iOS) file, with a build of your app containing the AltDriver.
[Here](https://alttester.com/walkthrough-tutorial-upgrading-trashcat-to-2-0-x/#Instrument%20TrashCat%20with%20AltTester%20Unity%20SDK%20v.2.0.x) is a helpful resource about the process of instrumenting the TrashCat application using AltTester® Unity SDK `v2.0.*`.

If you’re unsure how to generate an **.ipa** file please watch the first half of [this video](https://www.youtube.com/embed/rCwWhEeivjY?start=0&end=199) for iOS.
After you finish setting up the build, you need to use the **Archive** option to generate the standalone **.ipa**. The required steps for the archive option are described [here](https://docs.saucelabs.com/mobile-apps/automated-testing/ipa-files/#creating-ipa-files-for-appium-testing). Keep in mind that you need to select **Development** at step 6.

Based on your option to connect to AltTester® Desktop you need to set the AltTester® Server Host of the instrumented app to:
- localhost (`127.0.0.1`) - for local connection 
- IP/URL provided by the Bitbar Ubuntu VM where AltTester® Desktop is running - for remote connection

**2. Prepare the test code and dependencies**

- install the necessary libraries (we prefer dotnet CLI)
    - we need the Selenium Webdriver extension for Appium to establish a connection between our test script and the target mobile application
    - the other package, JunitXml.TestLogger is required to have test results generated and parsed nicely in BitBar’s UI.
    ```
    dotnet add package Appium.WebDriver --version 4.3.1
    dotnet add package JunitXml.TestLogger --version 3.0.134
    ```
    - after installing the packages, you can see them in `.csproj` (check the [example repository](https://github.com/alttester/EXAMPLES-CSharp-BitBar-AltTrashCat/blob/server-side-android-localhost/TestAlttrashCSharp.csproj))

- create a `BaseTest.cs` file with [**OneTimeSetUp**](https://docs.nunit.org/articles/nunit/writing-tests/attributes/onetimesetup.html) and [**OneTimeTeardown**](https://docs.nunit.org/articles/nunit/writing-tests/attributes/onetimeteardown.html) methods
    - In **OneTimeSetUp** method you need to define instructions to:
        - Start Appium driver with desired capabilities
        - Initialize [AltDriver](https://alttester.com/docs/sdk/latest/pages/commands.html#altdriver)
        Make sure that all test classes will inherit this `BaseTest.cs` in order to have this setup executed.

    - In **OneTimeTearDown** method you define instructions to:
        - Stop AltDriver
        - Quit Appium driver

    - import the Appium namespace:
    ```c#
    using OpenQA.Selenium.Appium;
    ```
    - depending on the device's OS you will use similar commands for declaring, adding capabilities and initializing the Appium driver:
        - for Android capabilities please consult the `README.md` from [Appium UiAutomator2 Driver](https://github.com/appium/appium-uiautomator2-driver)
        - for iOS capabilities please consult this list from [Appium XCUITest Driver documentation](https://appium.github.io/appium-xcuitest-driver/4.16/capabilities/)

    ```eval_rst
    .. note::
        ``DesiredCapabilities()`` is a deprecated class, so please see our version using ``AppiumOptions()``   
    ```

    ```eval_rst
    .. tabs::

        .. tab:: Android

            .. code-block:: C#

                using OpenQA.Selenium.Appium.Android;

            .. code-block:: C#

                public AndroidDriver<AndroidElement> appiumDriver;

            .. code-block:: C#

                string appPath = System.Environment.CurrentDirectory + "/../../../application.apk";
                capabilities.AddAdditionalCapability("appium:app", appPath);
                capabilities.AddAdditionalCapability("appium:deviceName", "Android Phone");
                capabilities.AddAdditionalCapability("platformName", "Android");
                capabilities.AddAdditionalCapability("automationName", "UIAutomator2");
                capabilities.AddAdditionalCapability("newCommandTimeout", 2000);

            .. code-block:: C#

                appiumDriver = new AndroidDriver<AndroidElement>(new Uri("http://localhost:4723/wd/hub"), capabilities, TimeSpan.FromSeconds(36000));

        .. tab:: iOS

            .. code-block:: C#

                using OpenQA.Selenium.Appium.iOS;

            .. code-block:: C#

                public IOSDriver<IOSElement> appiumDriver;

            .. code-block:: C#

                capabilities.AddAdditionalCapability("appium:deviceName", "Apple iPhone SE 2020 A2296 13.4.1");
                capabilities.AddAdditionalCapability("platformName", "iOS");
                capabilities.AddAdditionalCapability("appium:automationName", "XCUITest");
                capabilities.AddAdditionalCapability("appium:bundleId", "fi.altom.trashcat");
                capabilities.AddAdditionalCapability("platformVersion", "13.4");
                capabilities.AddAdditionalCapability("autoAcceptAlerts","true");
                capabilities.AddAdditionalCapability("newCommandTimeout", 2000);                

            .. code-block:: C#

                appiumDriver = new IOSDriver<IOSElement>(new Uri("http://localhost:4723/wd/hub"), capabilities, TimeSpan.FromSeconds(36000));
    ```

    - initialize AltDriver:
        - **for remote connection**: AltDriver needs to connect to another VM where is AltTester® Server
        ```c#
        altDriver = new AltDriver(host: "INSERT_VM_IP");
        ```    
        - **for local connection**: AltDriver and AltTester® Server are on the same BitBar machine
        ```c#
        altDriver = new AltDriver();
        ```  

    Please consult our example of BaseTest class:
    - for [Android, with altDriver connecting to localhost](https://github.com/alttester/EXAMPLES-CSharp-BitBar-AltTrashCat/blob/server-side-android-localhost/tests/BaseTest.cs)
    - for [iOS, with altDriver connecting to external VM](https://github.com/alttester/EXAMPLES-CSharp-BitBar-AltTrashCat/blob/server-side-android-localhost/tests/BaseTest.cs)

**3. Prepare the `.zip` archive with tests and `run-tests.sh`**

In this `.zip` you need to add all tests and the `run-test.sh` script to launch test execution **at the root level of the package**.
- For more details about the content of this file please see the BitBar documentation [here](https://support.smartbear.com/bitbar/docs/en/mobile-app-tests/automated-testing/appium-support/running-cloud-side-appium-tests.html).

- when running tests on iOS devices, the `run-tests.sh` needs to be adapted as well - be aware of the different *end of file* for OSX machines.

- **for local connection**
    - here is what the archived package contains to be able to execute tests server-side when AltTester® Server is running on the machine offered by BitBar:
    ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/bitbar-serverside-remote-connection-zip-archive.png
    ```

- **for remote connection**
    - here is what the archived package contains to be able to execute tests server-side when AltTester® Server is running on a separate machine, not on the one offered by BitBar:
    ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/bitbar-serverside-local-connection-zip-archive.png
    ```


```eval_rst
.. note::
    We recommend using ``wget`` in order to install the `batchmode Linux build for AltTester® Desktop <https://alttester.com/app/uploads/AltTester®/desktop/AltTesterDesktopLinuxBatchmode.zip>`_ and not put it in the archive because that increases the running time for the entire flow.

    An important note for this setup is that both running in batchmode and using the Linux build require `AltTester® Pro License <https://alttester.com/alttester/#pricing>`_.
```

Please see our shell script examples from the repository:
- for [**local connection**](https://github.com/alttester/EXAMPLES-CSharp-BitBar-AltTrashCat/blob/server-side-android-localhost/run-tests.sh)
- for [**remote connection**](https://github.com/alttester/EXAMPLES-CSharp-BitBar-AltTrashCat/blob/server-side-ios-VM-IP/run-tests.sh)

```eval_rst

.. note::
    When running server-side on an Android device, Bitbar offers an Ubuntu machine. Further on you can find a ``run-tests.sh`` script prepared for that. It contains the instructions for downloading, installing `AltTester® Desktop Linux batch mode <https://alttester.com/app/uploads/AltTester/desktop/AltTesterDesktopLinuxBatchmode.zip>`_ and activating and deactivating the license.
```

#### **Steps for running the tests**

**1. Upload `.zip` archive**
Upload the archive you created earlier (see **3. Prepare the `.zip` archive with tests and `run-tests.sh`** from the [Preparation Steps](#preparation-steps) section)

**2. Upload `.apk`/ `.ipa` file in BitBar's Files Library**
Upload the app you instrumented earlier (see **1. Prepare the application** from the [Preparation Steps](#preparation-steps) section)

**3. Create test run session on BitBar cloud**

There are a few important observations here as well. Please consult the [BitBar steps summary](https://support.smartbear.com/bitbar/docs/en/mobile-app-tests/automated-testing/appium-support/running-cloud-side-appium-tests.html#UUID-64e75ca6-080d-3c13-5cee-3f673df86b94_id_upload-and-execute) and [the devices and device groups available](https://support.smartbear.com/bitbar/docs/en/mobile-app-tests/organizing-your-projects-and-devices/managing-devices-and-device-groups.html).

If you are using [free trial version](https://smartbear.com/product/bitbar/free-trial/) (14 days) you will get:
- [Trial Android devices](https://cloud.bitbar.com/#testing/devices?group=14) with 4 devices
- [Trial iOS devices](https://cloud.bitbar.com/#testing/devices?group=4127) with 2 devices

An automated test session starts **simultaneously** on all the devices from the group selected.

```eval_rst
.. image:: ../_static/img/alttester-with-cloud/bitbar-serverside-test-run.png
```

### BitBar C# project example running client-side

Running the tests from your machine offers better control over the environment. The only thing we need to set up is the connectivity between the **devices from the cloud**, **the AltDriver** (from test scripts) and **the AltTester® Server module** from the AltTester® Desktop app.

For more details check [this article](https://alttester.com/integrate-appium-and-run-your-test-suite-in-bitbar-client-side/) from our Blog.

In this dashboard you can have an overview of the setup combinations we tried and which were successful:
```eval_rst
.. image:: ../_static/img/alttester-with-cloud/bitbar-clientside-connectivity-dashboard.png
```
*we used **SmartBear SecureTunnel** for this case

```eval_rst

.. important::
    Currently **running client-side tests with AltTester® Server on the same machine is failing** even if SmartBear SecureTunnel is connected. We assume this is happening due to WebSocket implementation and incompatibility with AltTester® Server.
```

#### Prerequisites for running AltTester® Server (remote connection)

- AltTester® Desktop is opened in a remote location in a **Windows Azure VM** (accessible by both tests and game build through IP)
- the conditions for the connection to work:
    - the IP of the VM needs to be specified in `BaseTest.cs` when **altDriver** is instantiated
    - the game build needs to be instrumented with the same host IP

**Create a VM for running AltTester® Desktop**

We used [Azure](https://azure.microsoft.com/en-us/products/virtual-machines/) to create a virtual machine running **Windows x64 architecture** and we set up an AltTester® Desktop instance. Please consult the documentation for more detailed instructions on [how to create a Windows VM in Azure portal](https://learn.microsoft.com/en-us/azure/virtual-machines/windows/quick-create-portal#create-virtual-machine).

- virtual machine  **network settings** required in order to have this machine publicly reachable by the devices from BitBar:
    - define an **Inbound port rule for protocol TCP on port 13000: Allow connection from Any source**
    ```eval_rst
    .. image:: ../_static/img/alttester-with-cloud/bitbar-clientside-remote-connection-network-settings.png
    ```
    - another necessary setting: [Turn off Firewall on the VM](https://support.microsoft.com/en-us/windows/turn-microsoft-defender-firewall-on-or-off-ec0844f7-aebd-0583-67fe-601ecf5d774f)

- connect using [Remote Desktop Connection](https://support.microsoft.com/en-us/windows/how-to-use-remote-desktop-5fe128d5-8fb1-7a23-3b8a-41e636865e8c) on the machine, [download AltTester® Desktop](https://alttester.com/alttester/#pricing), install it, launch it and leave it running and listening on port `13000`

```eval_rst

    .. note::
        You can have AltTester® Server waiting for connections either by starting it manually via GUI or you can use a cmd to start in batchmode (the ease of running in batch mode comes with the requirement to have an **AltTester® Pro license**).
```
- since in our example we chose the batchmode option, we have to set up the path of the AltTester® Desktop app executable in the system **PATH environment variable**
- then from *Azure portal Operations* > *Run Command* option we choose: *RunPowerShellScript*
    ```eval_rst
    .. image:: ../_static/img/alttester-with-cloud/bitbar-clientside-remote-connection-azur-portal-operations.png
    ```
    
    - run command:
    ```
    AltTesterDesktop.exe -batchmode -port 13000 -license <your_license_key> -nographics -logfile LOGFILE.txt
     ```

We have now a VM where AltTester® Server is listening for connections. Further on we will use the IP of this machine to have the communication between the main actors.

#### **Preparation steps**

**1. Prepare the application**

You will first need to create an **.apk** (for Android) / **.ipa** (for iOS) file, with a build of your app containing the AltDriver.
[Here](https://alttester.com/walkthrough-tutorial-upgrading-trashcat-to-2-0-x/#Instrument%20TrashCat%20with%20AltTester%20Unity%20SDK%20v.2.0.x) is a helpful resource about the process of instrumenting the TrashCat application using AltTester® Unity SDK `v2.1.2`.

If you’re unsure how to generate an **.ipa** file please watch the first half of [this video](https://www.youtube.com/embed/rCwWhEeivjY?start=0&end=199) for iOS.
After you finish setting up the build, you need to use the **Archive** option to generate the standalone **.ipa**. The required steps for the archive option are described [here](https://docs.saucelabs.com/mobile-apps/automated-testing/ipa-files/#creating-ipa-files-for-appium-testing). Keep in mind that you need to select **Development** at step 6.

**2. Prepare the test code and dependencies**

- install the necessary libraries (we prefer dotnet CLI)
    - we need the Selenium Webdriver extension for Appium to establish a connection between our test script and the target mobile application
    - in case you have not done it so far, add the AltTester-Driver package as well
    ```c#
    dotnet add package Appium.WebDriver --version 4.3.1
    dotnet add package AltTester-Driver --version 2.1.2
    ```
    - after installing the packages, you can see them in `.csproj` (check the [example repository](https://github.com/alttester/EXAMPLES-CSharp-BitBar-AltTrashCat/blob/client-side-ios/TestAlttrashCSharp.csproj))

- setup environment variables with BitBar secrets and your VM’s IP
    - set the environment variables: `HOST_ALT_SERVER`, `BITBAR_APIKEY`, `BITBAR_APP_ID_SDK_202`, `BITBAR_APP_ID_SDK_202_IPA`
    - on Windows we set them up by running a `.bat` file with the [set command](https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/set_1):
    ```
    set VARIABLE_NAME=value
    ```
    - on macOS you can use ther `export` command as follows:
    ```
    export VARIABLE_NAME=value
    ```
- create a `BaseTest.cs` file with [**OneTimeSetUp**](https://docs.nunit.org/articles/nunit/writing-tests/attributes/onetimesetup.html) and [**OneTimeTeardown**](https://docs.nunit.org/articles/nunit/writing-tests/attributes/onetimeteardown.html) methods
    - before the commands from actual tests we need to:
        - start Appium driver with desired capabilities
        - initialize [AltDriver](https://alttester.com/docs/sdk/latest/pages/commands.html#altdriver)
    - import the Appium namespace:
    ```c#
    using OpenQA.Selenium.Appium;
    ```
    - load the previously set environment variables:
    ```c#
    String HOST_ALT_SERVER = Environment.GetEnvironmentVariable("HOST_ALT_SERVER");
    String BITBAR_APIKEY = Environment.GetEnvironmentVariable("BITBAR_APIKEY");
    String BITBAR_APP_ID_SDK_202 = Environment.GetEnvironmentVariable("BITBAR_APP_ID_SDK_202");
    String BITBAR_APP_ID_SDK_202_IPA = Environment.GetEnvironmentVariable("BITBAR_APP_ID_SDK_202_IPA");
    ```
    - depending on the device's OS you will use similar commands for declaring, adding capabilities and initializing the Appium driver:
        - for Android capabilities please consult the `README.md` from [Appium UiAutomator2 Driver](https://github.com/appium/appium-uiautomator2-driver)
        - for iOS capabilities please consult this list from [Appium XCUITest Driver documentation](https://appium.github.io/appium-xcuitest-driver/4.16/capabilities/)
    - BitBar offers a [‘capabilities creator’](https://cloud.bitbar.com/#public/capabilities-creator) to help with these
    
    ```eval_rst
    .. note::
        ``DesiredCapabilities()`` is a deprecated class, so please see our version using ``AppiumOptions()``   
    ```

    ```eval_rst
    .. tabs::

        .. tab:: Android

            .. code-block:: C#

                using OpenQA.Selenium.Appium.Android;

            .. code-block:: C#

                public AndroidDriver<AndroidElement> appiumDriver;

            .. code-block:: C#

                capabilities.AddAdditionalCapability("platformName", "Android");
                capabilities.AddAdditionalCapability("appium:deviceName", "Android");                
                capabilities.AddAdditionalCapability("automationName", "UIAutomator2");
                capabilities.AddAdditionalCapability("newCommandTimeout", 2000);

            .. code-block:: C#

                capabilities.AddAdditionalCapability("bitbar_apiKey", BITBAR_APIKEY);
                capabilities.AddAdditionalCapability("bitbar_project", "client-side: AltTester® Server on custom host; Android");
                capabilities.AddAdditionalCapability("bitbar_testrun", "Start Page Tests on Samsung");
                capabilities.AddAdditionalCapability("bitbar_app", BITBAR_APP_ID_SDK_202);

            .. code-block:: C#

                appiumDriver = new AndroidDriver<AndroidElement>(new Uri("http://localhost:4723/wd/hub"), capabilities, TimeSpan.FromSeconds(36000));

        .. tab:: iOS

            .. code-block:: C#

                using OpenQA.Selenium.Appium.iOS;

            .. code-block:: C#

                public IOSDriver<IOSElement> appiumDriver;

            .. code-block:: C#

                capabilities.AddAdditionalCapability("platformName", "iOS");
                capabilities.AddAdditionalCapability("appium:deviceName", "Apple iPhone SE 2020 A2296 13.4.1");
                capabilities.AddAdditionalCapability("appium:automationName", "XCUITest");
                capabilities.AddAdditionalCapability("appium:bundleId", "fi.altom.trashcat");

            .. code-block:: C#

                capabilities.AddAdditionalCapability("bitbar_apiKey", BITBAR_APIKEY);
                capabilities.AddAdditionalCapability("bitbar_project", "client-side: AltTester® Server on custom host; iOS");
                capabilities.AddAdditionalCapability("bitbar_testrun", "Start Page Tests on Apple iPhone SE 2020 A2296 13.4.1");
                capabilities.AddAdditionalCapability("bitbar_app", BITBAR_APP_ID_SDK_202_IPA);

            .. code-block:: C#

                appiumDriver = new IOSDriver<IOSElement>(new Uri(""http://localhost:4723/wd/hub""), capabilities)

    ```
    ```eval_rst
    .. note::
        It is important to consult the `list of devices available on BitBar <https://cloud.bitbar.com/#public/devices>`_, to know what you can set for bitbar_device capability.
    ```
    
    ```eval_rst
    .. note::
        Make sure you review all these capabilities before trying to execute, as you might encounter issues otherwise. For example, providing **appium:bundleId** is important so that the application is installed by Appium on the selected iOS device.
    ```
    - initialize AltDriver:
    ```c#
    altDriver = new AltDriver(host: HOST_ALT_SERVER);
    ```
Please see our `BaseTest.cs` examples from the repository:
- for triggering [running tests on an Android device](https://github.com/alttester/EXAMPLES-CSharp-BitBar-AltTrashCat/blob/client-side-android/tests/BaseTest.cs)
- for triggering [running tests on an iOS devics](https://github.com/alttester/EXAMPLES-CSharp-BitBar-AltTrashCat/blob/client-side-ios/tests/BaseTest.cs)

#### **Steps for running the tests**

**1. Upload `.apk`/ `.ipa` file in BitBar's Files Library**

Once you have instrumented your app build with host: <IP_of_VM_where_AltServer_is_running> upload it in BitBar’s files library. You will need to copy the **ID of the application** to use it later on in tests as an environment variable.

Since we are running our tests from a local environment we need a way to authenticate to BitBar. Make sure you save your API_KEY as an environment variable locally. You can get this from your account [as described in the documentation](https://support.smartbear.com/bitbar/docs/en/use-rest-apis-with-bitbar/authentication.html).

**2. Execute test run to trigger new session on BitBar cloud**

From your machine trigger execution for tests. We prefer using the cmd terminal for this:
```c#
dotnet test --filter <test_class_name>
```
- a new automation test session should be visible running under the *bitbar_project* defined in script
- once the test execution is finished you can consult the logs (`appium.log`, `console.log`, `device.log`) and screen record of the execution

```eval_rst

.. note::
    Don’t forget that the AltTester® Desktop app also has logs you can see live (in GUI mode) or consult the log file generated afterward
```

```eval_rst
.. image:: ../_static/img/alttester-with-cloud/bitbar-clientside-test-run.png
```

### BitBar Python project example running server-side

In BitBar terms, the [server-side](https://support.smartbear.com/bitbar/docs/en/mobile-app-tests/automated-testing/appium-support/running-cloud-side-appium-tests.html) execution means that **we upload** to the platform **everything** we need for the tests to run.

Using a `run-tests.sh` we can install all that is needed, run tests and prepare the test report.

You can download our example BitBar project FROM [here](https://github.com/alttester-test-examples/Python-Bitbar-AltTrashCat).

#### Prerequisites for running AltTester® Server

You can connect to AltTester® Desktop in two ways in order to run the tests server-side:

**A. A remote connection**

- AltTester® Desktop is opened in a remote location in a **Windows Azure VM** (accessible by both tests and game build through IP)
- the conditions for the connection to work:
    - the IP of the VM needs to be specified in `base_test.py` when **AltDriver** is instantiated
    - the game build needs to be instrumented with the same host IP

    **Create a VM for running AltTester® Desktop**

    We used [Azure](https://azure.microsoft.com/en-us/products/virtual-machines/) to create a virtual machine running **Windows x64 architecture** and we set up an AltTester® Desktop instance. Please consult the documentation for more detailed instructions on [how to create a Windows VM in Azure portal](https://learn.microsoft.com/en-us/azure/virtual-machines/windows/quick-create-portal#create-virtual-machine).

    - virtual machine  **network settings** required in order to have this machine publicly reachable by the devices from BitBar:
        - define an **Inbound port rule for protocol TCP on port 13000: Allow connection from Any source**
        ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/bitbar-clientside-remote-connection-network-settings.png
        ```
        - another necessary setting: [Turn off Firewall on the VM](https://support.microsoft.com/en-us/windows/turn-microsoft-defender-firewall-on-or-off-ec0844f7-aebd-0583-67fe-601ecf5d774f)

    - connect using [Remote Desktop Connection](https://support.microsoft.com/en-us/windows/how-to-use-remote-desktop-5fe128d5-8fb1-7a23-3b8a-41e636865e8c) on the machine, [download AltTester® Desktop](https://alttester.com/alttester/#pricing), install it, launch it and leave it running and listening on port `13000`

    ```eval_rst

        .. note::
            You can have AltTester® Server waiting for connections either by starting it manually via GUI or you can use a cmd to start in batchmode (the ease of running in batch mode comes with the requirement to have an **AltTester® Pro license**).
    ```
    - since in our example we chose the batchmode option, we have to set up the path of the AltTester® Desktop app executable in the system **PATH environment variable**
    - then from *Azure portal Operations* > *Run Command* option we choose: *RunPowerShellScript*
        ```eval_rst
        .. image:: ../_static/img/alttester-with-cloud/bitbar-clientside-remote-connection-azur-portal-operations.png
        ```
        
        - run command:
        ```
        AltTesterDesktop.exe -batchmode -port 13000 -license <your_license_key> -nographics -logfile LOGFILE.txt
        ```

    We have now a VM where AltTester® Server is listening for connections. Further on we will use the IP of this machine to have the communication between the main actors.

**B. A local connection**

- AltTester® Desktop is installed on the **Bitbar Ubuntu VM**
- the conditions for the connection to work:
    - the script which is executed on Bitbar VM needs to contain the installation and launching of AltTester® Desktop build

    **Download, install and launch AltTester® Desktop Linux build in batch mode**

    In the `run-tests.sh` script (see **3. Prepare the `.zip` archive with tests and `run-tests.sh`** from the [Preparation Steps](#preparation-steps) section) you need to use the following commands:
    ```
    wget https://alttester.com/app/uploads/AltTester/desktop/AltTesterDesktopLinuxBatchmode.zip
    unzip AltTesterDesktopLinuxBatchmode.zip
    cd AltTesterDesktopLinux
    chmod +x ./AltTesterDesktop.x86_64
    ./AltTesterDesktop.x86_64 -batchmode -port 13000 -license $LICENSE_KEY -nographics -termsAndConditionsAccepted &
    ```

    Kill the AltTester® Desktop process:
    ```
    kill -2 `ps -ef | awk '/AltTesterDesktop.x86_64/{print $2}'`
    ```

#### **Preparation steps**

**1. Prepare the application**

You will first need to create an **.apk** (for Android) / **.ipa** (for iOS) file, with a build of your app containing the AltDriver.
[Here](https://alttester.com/walkthrough-tutorial-upgrading-trashcat-to-2-0-x/#Instrument%20TrashCat%20with%20AltTester%20Unity%20SDK%20v.2.0.x) is a helpful resource about the process of instrumenting the TrashCat application using AltTester® Unity SDK `v2.1.2`.

If you’re unsure how to generate an **.ipa** file please watch the first half of [this video](https://www.youtube.com/embed/rCwWhEeivjY?start=0&end=199) for iOS.
After you finish setting up the build, you need to use the **Archive** option to generate the standalone **.ipa**. The required steps for the archive option are described [here](https://docs.saucelabs.com/mobile-apps/automated-testing/ipa-files/#creating-ipa-files-for-appium-testing). Keep in mind that you need to select **Development** at step 6.

Based on your option to connect to AltTester® Desktop you need to set the AltTester® Server Host of the instrumented app to:
- localhost (`127.0.0.1`) - for local connection 
- IP/URL provided by the Bitbar Ubuntu VM where AltTester® Desktop is running - for remote connection

**2. Prepare the test code and dependencies**

- create a `base_test.py` file with **setUpClass** and **tearDownClass** methods
    - before the commands from actual tests we need to:
        - start Appium driver with desired capabilities
        - initialize [AltDriver](https://alttester.com/docs/sdk/latest/pages/commands.html#altdriver)
    - import the Appium namespaces:
    ```python
    from appium import webdriver
    from appium.options.common import AppiumOptions
    ```
    - depending on the device's OS you will use similar commands for declaring, adding capabilities and initializing the Appium driver:
        - for Android capabilities please consult the `README.md` from [Appium UiAutomator2 Driver](https://github.com/appium/appium-uiautomator2-driver)
        - for iOS capabilities please consult this list from [Appium XCUITest Driver documentation](https://appium.github.io/appium-xcuitest-driver/4.16/capabilities/)

    ```eval_rst
    .. note::
        ``DesiredCapabilities()`` is a deprecated class, so please see our version using ``AppiumOptions()``   
    ```

    ```eval_rst
    .. tabs::

        .. tab:: Android

            .. code-block:: python

                from appium import webdriver
                from appium.options.common import AppiumOptions
                
            .. code-block:: python

                options = AppiumOptions()
                options.platform_name = 'Android'
                options.automation_name = "UiAutomator2"
                options.set_capability("app", os.path.abspath("application.apk"))

            .. code-block:: python

                cls.driver = webdriver.Remote('http://localhost:4723/wd/hub', options=options)            

        .. tab:: iOS

            .. code-block:: python

                from appium import webdriver
                from appium.options.common import AppiumOptions
                
            .. code-block:: python

                <!-- To recheck here OS capabilities -->
                options = XCUITestOptions()
                options.set_capability("platformName", "iOS")
                options.set_capability("appium:automationName", "XCUITest")
                options.set_capability("appium:deviceName", "Apple iPhone SE 2020 A2296 13.4.1")                
                options.set_capability("appium:bundleId", "fi.altom.trashcat")
                options.set_capability("platformVersion", "13.4")
                options.set_capability("autoAcceptAlerts", "true")
                options.set_capability("newCommandTimeout", 2000)

            .. code-block:: python

                cls.driver = webdriver.Remote('http://localhost:4723/wd/hub', options=options)

    ```

    ```eval_rst
    .. note::
        It is important to consult the `list of devices available on BitBar <https://cloud.bitbar.com/#public/devices>`_, to know what you can set for bitbar_device capability.
    ```
    
    ```eval_rst
    .. note::
        Make sure you review all these capabilities before trying to execute, as you might encounter issues otherwise. For example, providing **appium:bundleId** is important so that the application is installed by Appium on the selected iOS device.
    ```

    - initialize AltDriver:
        - **for remote connection**: AltDriver needs to connect to another VM where is AltTester® Server
        ```python
        cls.alt_driver = AltDriver(host="INSERT_VM_IP")
        ```    
        - **for local connection**: AltDriver and AltTester® Server are on same BitBar machine
        ```python
        cls.alt_driver = AltDriver()
        ```  

**3. Prepare the `.zip` archive with tests and `run-tests.sh`**

In this `.zip` you need to add all tests and the `run-test.sh` script to launch test execution **at the root level of the package**.
- For more details about the content of this file please see the BitBar documentation [here](https://support.smartbear.com/bitbar/docs/en/mobile-app-tests/automated-testing/appium-support/running-cloud-side-appium-tests.html).

- when running tests on iOS devices, the `run-tests.sh` needs to be adapted as well - be aware of the different *end of file* for OSX machines.

- **for remote connection**
    - here is what the archived package contains to be able to execute tests server-side when AltTester® Server is running on the machine offered by BitBar:
        - the `requirements.txt` file
        - the `tests` folder (which contains also the `pages` folder) - don`t forget to add the IP/URL of the remote VM when defining AltDriver in ``base_test.py``

        Our example repository already contains a [script](https://github.com/alttester/EXAMPLES-Python-Bitbar-AltTrashCat/blob/server-side-ios-VM-IP/create-bitbar-package.sh) to create the required package for iOS. Just run the `create-bitbar-package.sh` script and it will create a ``.zip`` file, containing all the files required to execute the tests.

- **for local connection**
    - here is what the archived package contains to be able to execute tests server-side when AltTester® Server is running on a separate machine, not on the one offered by BitBar:
        - a `license.txt` file which will store your AltTester® Desktop PRO license, needed to run batch mode commands 
        - the `requirements.txt` file
        - the `tests` folder (which contains also the `pages` folder)
    
        Our example repository already contains a [script](https://github.com/alttester/EXAMPLES-Python-Bitbar-AltTrashCat/blob/server-side-android-localhost/create-bitbar-package.sh) to create the required package for Android. Just run the `create-bitbar-package.sh` script and it will create a ``.zip`` file, containing all the files required to execute the tests.

    ```eval_rst
    .. note::
        We recommend using ``wget`` in order to install the `batchmode Linux build for AltTester® Desktop <https://alttester.com/app/uploads/AltTester/desktop/AltTesterDesktopLinuxBatchmode.zip>`_ and not put it in the archive because that increases the running time for the entire flow.
        
        An important note for this setup is that both running in batchmode and using the Linux build require `AltTester® Pro License <https://alttester.com/alttester/#pricing>`_.
    ```

Please see our shell script examples from the repository:
- for [**remote connection**](https://github.com/alttester/EXAMPLES-Python-Bitbar-AltTrashCat/blob/server-side-ios-VM-IP/run-tests.sh)
- for [**local connection**](https://github.com/alttester/EXAMPLES-Python-Bitbar-AltTrashCat/blob/server-side-android-localhost/run-tests.sh)

```eval_rst

    .. note::
        When running server-side on an Android device, Bitbar offers an Ubuntu machine. Further on you can find a ``run-tests.sh`` script prepared for that. It contains the instructions for downloading, installing `AltTester® Desktop Linux batch mode <https://alttester.com/app/uploads/AltTester/desktop/AltTesterDesktopLinuxBatchmode.zip>`_ and activating and deactivating the license.
```

#### **Steps for running the tests**

**1. Upload `.zip` archive**
Upload the archive you created earlier (see **3. Prepare the `.zip` archive with tests and `run-tests.sh`** from the [Preparation Steps](#preparation-steps) section)

**2. Upload `.apk`/ `.ipa` file in BitBar's Files Library**
Upload the app you instrumented earlier (see **1. Prepare the application** from the [Preparation Steps](#preparation-steps) section)

**3. Create test run session on BitBar cloud**

There are a few important observations here as well. Please consult the [BitBar steps summary](https://support.smartbear.com/bitbar/docs/en/mobile-app-tests/automated-testing/appium-support/running-cloud-side-appium-tests.html#UUID-64e75ca6-080d-3c13-5cee-3f673df86b94_id_upload-and-execute) and [the devices and device groups available](https://support.smartbear.com/bitbar/docs/en/mobile-app-tests/organizing-your-projects-and-devices/managing-devices-and-device-groups.html).

If you are using [free trial version](https://smartbear.com/product/bitbar/free-trial/) (14 days) you will get:
- [Trial Android devices](https://cloud.bitbar.com/#testing/devices?group=14) with 4 devices
- [Trial iOS devices](https://cloud.bitbar.com/#testing/devices?group=4127) with 2 devices

An automated test session starts **simultaneously** on all the devices from the group selected.

```eval_rst
.. image:: ../_static/img/alttester-with-cloud/bitbar-serverside-test-run.png
```

### BitBar Python project example running client-side

Running the tests from your machine offers better control over the environment. The only thing we need to set up is the connectivity between the **devices from the cloud**, **the AltDriver** (from test scripts) and **the AltTester® Server module** from the AltTester® Desktop app.

In this dashboard you can have an overview of the setup combinations we tried and which were successful:
```eval_rst
.. image:: ../_static/img/alttester-with-cloud/bitbar-clientside-connectivity-dashboard.png
```
*we used **SmartBear SecureTunnel** for this case

```eval_rst

.. important::
    Currently **running client-side tests with AltTester® Server on the same machine is failing** even if SmartBear SecureTunnel is connected. We assume this is happening due to WebSocket implementation and incompatibility with AltTester® Server.
```

#### Prerequisites for running AltTester® Server (remote connection)

- AltTester® Desktop is opened in a remote location in a **Windows Azure VM** (accessible by both tests and game build through IP)
- the conditions for the connection to work:
    - the IP of the VM needs to be specified in `base_test.py` when **altDriver** is instantiated
    - the game build needs to be instrumented with the same host IP

**Create a VM for running AltTester® Desktop**

We used [Azure](https://azure.microsoft.com/en-us/products/virtual-machines/) to create a virtual machine running **Windows x64 architecture** and we set up an AltTester® Desktop instance. Please consult the documentation for more detailed instructions on [how to create a Windows VM in Azure portal](https://learn.microsoft.com/en-us/azure/virtual-machines/windows/quick-create-portal#create-virtual-machine).

- virtual machine  **network settings** required in order to have this machine publicly reachable by the devices from BitBar:
    - define an **Inbound port rule for protocol TCP on port 13000: Allow connection from Any source**
    ```eval_rst
    .. image:: ../_static/img/alttester-with-cloud/bitbar-clientside-remote-connection-network-settings.png
    ```
    - another necessary setting: [Turn off Firewall on the VM](https://support.microsoft.com/en-us/windows/turn-microsoft-defender-firewall-on-or-off-ec0844f7-aebd-0583-67fe-601ecf5d774f)

- connect using [Remote Desktop Connection](https://support.microsoft.com/en-us/windows/how-to-use-remote-desktop-5fe128d5-8fb1-7a23-3b8a-41e636865e8c) on the machine, [download AltTester® Desktop](https://alttester.com/alttester/#pricing), install it, launch it and leave it running and listening on port `13000`

```eval_rst

    .. note::
        You can have AltTester® Server waiting for connections either by starting it manually via GUI or you can use a cmd to start in batchmode (the ease of running in batch mode comes with the requirement to have an **AltTester® Pro license**).
```
- since in our example we chose the batchmode option, we have to set up the path of the AltTester® Desktop app executable in the system **PATH environment variable**
- then from *Azure portal Operations* > *Run Command* option we choose: *RunPowerShellScript*
    ```eval_rst
    .. image:: ../_static/img/alttester-with-cloud/bitbar-clientside-remote-connection-azur-portal-operations.png
    ```
    
    - run command:
    ```
    AltTesterDesktop.exe -batchmode -port 13000 -license <your_license_key> -nographics -logfile LOGFILE.txt
     ```

We have now a VM where AltTester® Server is listening for connections. Further on we will use the IP of this machine to have the communication between the main actors.

#### **Preparation steps**

**1. Prepare the application**

You will first need to create an **.apk** (for Android) / **.ipa** (for iOS) file, with a build of your app containing the AltDriver.
[Here](https://alttester.com/walkthrough-tutorial-upgrading-trashcat-to-2-0-x/#Instrument%20TrashCat%20with%20AltTester%20Unity%20SDK%20v.2.0.x) is a helpful resource about the process of instrumenting the TrashCat application using AltTester® Unity SDK `v2.1.2`.

If you’re unsure how to generate an **.ipa** file please watch the first half of [this video](https://www.youtube.com/embed/rCwWhEeivjY?start=0&end=199) for iOS.
After you finish setting up the build, you need to use the **Archive** option to generate the standalone **.ipa**. The required steps for the archive option are described [here](https://docs.saucelabs.com/mobile-apps/automated-testing/ipa-files/#creating-ipa-files-for-appium-testing). Keep in mind that you need to select **Development** at step 6.

**2. Prepare the test code and dependencies**

- setup environment variables with BitBar secrets and your VM’s IP
    - set the environment variables: `HOST_ALT_SERVER`, `BITBAR_APIKEY`, `BITBAR_APP_ID_SDK_202`, `BITBAR_APP_ID_SDK_202_IPA`
    - on Windows we set them up by running a `.bat` file with the [set command](https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/set_1):
    ```
    set VARIABLE_NAME=value
    ```
    - on macOS you can use ther `export` command as follows:
    ```
    export VARIABLE_NAME=value
    ```
- create a `base_test.py` file with **setUpClass** and **tearDownClass** methods
    - before the commands from actual tests we need to:
        - start Appium driver with desired capabilities
        - initialize [AltDriver](https://alttester.com/docs/sdk/latest/pages/commands.html#altdriver)
    - import the Appium namespaces:
    ```python
    from appium import webdriver
    from appium.options.common import AppiumOptions
    ```
    - load the previously set environment variables:
    ```python
    HOST_ALT_SERVER = os.getenv("HOST_ALT_SERVER")
    BITBAR_APIKEY = os.getenv("BITBAR_APIKEY")
    BITBAR_APP_ID_SDK_202 = os.getenv("BITBAR_APP_ID_SDK_202")
    BITBAR_APP_ID_SDK_202_IPA = os.getenv("BITBAR_APP_ID_SDK_202_IPA")
    ```
    - depending on the device's OS you will use similar commands for declaring, adding capabilities and initializing the Appium driver:
        - for Android capabilities please consult the `README.md` from [Appium UiAutomator2 Driver](https://github.com/appium/appium-uiautomator2-driver)
        - for iOS capabilities please consult this list from [Appium XCUITest Driver documentation](https://appium.github.io/appium-xcuitest-driver/4.16/capabilities/)
    - BitBar offers a [‘capabilities creator’](https://cloud.bitbar.com/#public/capabilities-creator) to help with these
    
    ```eval_rst
    .. note::
        ``DesiredCapabilities()`` is a deprecated class, so please see our version using ``AppiumOptions()``   
    ```

    ```eval_rst
    .. tabs::

        .. tab:: Android

            .. code-block:: python

                from appium import webdriver
                from appium.options.common import AppiumOptions
                
            .. code-block:: python

                options = AppiumOptions()
                options.platform_name = 'Android'
                options.automation_name = "UiAutomator2"
                options.set_capability("app", os.path.abspath("application.apk"))


            .. code-block:: python

                options.set_capability("bitbar_apikey", BITBAR_APIKEY)
                options.set_capability("bitbar_app", BITBAR_APP_ID_SDK_202)
                options.set_capability("bitbar_project", "client-side: AltTester® Server on custom host; Android")
                options.set_capability("bitbar_testrun", "Start Page Tests on Samsung")
                options.set_capability("bitbar_device", "Samsung Galaxy A52 -US")                

            .. code-block:: python

                cls.driver = webdriver.Remote('http://localhost:4723/wd/hub', options=options)

        .. tab:: iOS

            .. code-block:: python

                from appium import webdriver
                from appium.options.common import AppiumOptions
                
            .. code-block:: python

                options = XCUITestOptions()
                options.platform_name = 'iOS'
                options.automation_name = "XCUITest"
                options.set_capability("deviceName", "Apple iPhone SE 2020 A2296 13.4.1")
                options.set_capability("appium:bundleId", "fi.altom.trashcat")

            .. code-block:: python

                options.set_capability("bitbar_apiKey", BITBAR_APIKEY);
                options.set_capability("bitbar_app", BITBAR_APP_ID_SDK_202_IPA);
                options.set_capability("bitbar_project", "client-side: AltTester® Server on custom host; iOS");
                options.set_capability("bitbar_testrun", "Start Page Tests on Apple iPhone SE 2020 A2296 13.4.1");
                

            .. code-block:: python

                cls.driver = webdriver.Remote('http://localhost:4723/wd/hub', options=options)

    ```

    ```eval_rst
    .. note::
        It is important to consult the `list of devices available on BitBar <https://cloud.bitbar.com/#public/devices>`_, to know what you can set for bitbar_device capability.
    ```
    
    ```eval_rst
    .. note::
        Make sure you review all these capabilities before trying to execute, as you might encounter issues otherwise. For example, providing **appium:bundleId** is important so that the application is installed by Appium on the selected iOS device.
    ```
    - initialize AltDriver:
        ```python
        cls.alt_driver = AltDriver(host=HOST_ALT_SERVER)
        ```

Please see our `base_test.py` examples from the repository:
- for triggering [running tests on an Android device](https://github.com/alttester/EXAMPLES-Python-Bitbar-AltTrashCat/blob/client-side-android/tests/base_test.py)
- for triggering [running tests on an iOS devics](https://github.com/alttester/EXAMPLES-Python-Bitbar-AltTrashCat/blob/client-side-ios/tests/base_test.py)

#### **Steps for running the tests**

**1. Upload `.apk`/ `.ipa` file in BitBar's Files Library**

Once you have instrumented your app build with host: <IP_of_VM_where_AltServer_is_running> upload it in BitBar’s files library. You will need to copy the **ID of the application** to use it later on in tests as an environment variable.

Since we are running our tests from a local environment we need a way to authenticate to BitBar. Make sure you save your API_KEY as an environment variable locally. You can get this from your account [as described in the documentation](https://support.smartbear.com/bitbar/docs/en/use-rest-apis-with-bitbar/authentication.html).

**2. Execute test run to trigger new session on BitBar cloud**

From your machine trigger execution for tests. 
- a new automation test session should be visible running under the *bitbar_project* defined in script
- once the test execution is finished you can consult the logs (`appium.log`, `console.log`, `device.log`) and screen record of the execution

```eval_rst

.. note::
    Don’t forget that the AltTester® Desktop app also has logs you can see live (in GUI mode) or consult the log file generated afterward
```

```eval_rst
.. image:: ../_static/img/alttester-with-cloud/bitbar-clientside-test-run.png
```

## GitHub

GitHub Actions is a very powerful tool for creating a great process CI/CD. You can use public machines offered by GitHub or self-hosted runners in order to run tests automatically. We are using GitHub Actions to build and test our applications. You can see our workflows for AltTester® Unity SDK [here](https://github.com/alttester/AltTester-Unity-SDK/tree/development/.github/workflows).

**Some useful links to create your workflows:**
  - [GitHub Action documentation](https://docs.github.com/en/actions)
  - [Unity Builder](https://github.com/marketplace/actions/unity-builder)
  - [Example project for running tests on a public machine](https://github.com/alttester/Example-Running-Tests-On-Github-Public-Runner)
  


