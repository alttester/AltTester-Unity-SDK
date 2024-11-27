# Running tests using device cloud services

In some cases you might want to run your tests on dozens or even hundreds of real devices, to test the compatibility of your app on many different device models and OS versions. There are multiple device farms that will enable you to do so, without having to own the devices yourself.

Some of these cloud services allow running Appium automated tests by giving you access to an Appium server running in the cloud that has access to all their mobile devices. These services will work with AltTester® Unreal SDK only if they also offer some solution for local testing / tunneling that supports web sockets, so that the device in the cloud can access the AltTester® Desktop app running locally on your machine. So far, we know that **BrowserStack** (with BrowerStack Local) works with AltTester®.

```eval_rst

.. note::
    BrowserStack doesn’t support server-side testing, meaning that the test folder can’t be uploaded onto the platform in order to run the tests. Client-side testing generally focuses on testing the application or website directly on the user’s end. For testing carried out on cloud services, this means that the test suite is stored locally, on a computer and connected to a device in the cloud.
```

## BrowserStack

If you want to run AltTester® Unreal SDK tests client-side on a cloud service you may try using **BrowserStack App Automate**.

BrowserStack doesn't support server-side testing, meaning that the user can't upload the test suite and the build on the cloud and then run them using a script.

An option for running tests that are not stored locally is to integrate with CI/CD tools, like **GitHub Actions**. 

### BrowserStack App Automate C# project example

BrowserStack App Automate is a cloud-based testing platform that allows developers and testers to perform automated testing of mobile applications on a wide range of real devices. 

In this automation process, BrowserStack uses a set of Appium capabilities to customize and configure the testing environment to match various scenarios.

You can download our example project for the AltTester® Unity SDK from [here](https://github.com/alttester/EXAMPLES-CSharp-BrowserStack-AltTrashCat). While these examples are specific to the AltTester® Unity SDK, the same approach should work for the AltTester® Unreal SDK as well. For more details, check [this article](https://alttester.com/running-alttester-based-c-tests-on-browserstack-app-automate/) from our Blog.

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
    - in this context, Appium is only used to install the application and access it on the BrowserStack test device - after that, AltTester® Unreal SDK picks up the connection and carries out the tests
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

You can download our example project for the AltTester® Unity SDK from [here](https://github.com/alttester/EXAMPLES-CSharp-Cloud-Services-AltTrashCat). While these examples are specific to the AltTester® Unity SDK, the same approach should work for the AltTester® Unreal SDK as well. For more details, check [this article](https://alttester.com/how-to-run-alttester-based-c-tests-on-browserstack-using-github-actions/) from our Blog.

```eval_rst

.. note::
    This example was created for running tests on a single device.

```

For this integration, the best solution is to create and use a **self-hosted runner** because it allows you to install and run the AltTester® Desktop app that keeps AltTester® Server active at all times.

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

Once you are logged into your BrowserStack account, upload your build instrumented with AltTester® Unreal SDK using the UI button available on Dashboard.
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