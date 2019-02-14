# Running AltUnityTester in AWS Device Farm

This folder contains a few example tests that can be run in [AWS Device Farm](https://aws.amazon.com/device-farm/)

Full instructions and documentation on how to setup your tests for running in the AWS cloud can be found here: https://docs.aws.amazon.com/devicefarm/latest/developerguide/test-types-android-appium-python.html

## Creating the .zip file to upload to AWS Device Farm

The initial folder structure looks like this:
```
─ aws-device-farm-example
  ├─ tests/
  ├─ requirements.txt
  └─ README.md (this file)
```
You need to use [Wheelhouse](https://pypi.org/project/Wheelhouse/) to create a local cache of the needed Python packages. You can do that by running the following command in ```aws-device-farm-example``` folder:

```
cd aws-device-farm-example
pip wheel --wheel-dir wheelhouse -r requirements.txt
```

If you don't have Wheelhouse installed, you need to install it using:

```
pip install wheel
```

Your ```aws-device-farm-example``` folder should now look like this:
```
─ aws-device-farm-example
  ├─ tests/
  ├─ requirements.txt
  ├─ README.md (this file)
  └─ wheelhouse/
```
Then you need to create the ```.zip``` file containing the tests and the required packages, that you will use later on to upload to AWS Device Farm:

```
zip -r test_bundle.zip tests/ wheelhouse/ requirements.txt
```
```
─ aws-device-farm-example
  ├─ tests/
  ├─ requirements.txt
  ├─ test_bundle.zip
  ├─ README.md (this file)
  └─ wheelhouse/
```
The ```test_bundle.zip``` is the file you will be uploading to AWS Device Farm. 

## Uploading the tests

When you upload your files to AWS Device Farm, you just need to:

1. Upload your ```.apk``` (or ```.ipa```, not tested yet) - use the [sampleGame.apk](https://gitlab.com/altom/altunitytester/blob/20-running-on-aws-device-farm/sampleGame.apk) with the example tests
2. Choose ```Appium Python``` for the Test Framework setup and upload the ```test_bundle.zip``` from previous steps
3. Select some devices and choose next (or whatever setting you might need for the following steps)

## The Sample Tests

The tests that are included in the example folder will simply check the current scene and click on the Capsule and on the UIButton of the sample app. 

There are only 2 special things that I am aware of that you need to be aware of in your tests to get the working with AWS Device Farm:

1. The tests need to be runnable with py.test, so check that the following command shows the tests you want to run:
 ``` py.test --collect-only tests/```

2. You don't need to specify which app to run the Appium tests with when creating the Appium driver - the Appium server is already started with the app capability pointing to the application that you have uploaded at step 1 in the previous section


## Anything else?
If there's anything else you think should be mentioned for this setup, please open an issue and let us know!
