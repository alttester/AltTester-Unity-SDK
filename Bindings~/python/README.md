# AltTester® Python Bindings

This package contains an library for adding Python language binding to the AltTester® framework.

AltTester® Unity SDK is an UI driven test automation tool that helps you find objects in your application and interacts with them using tests written in C#, Python, Java or Robot Framework.

You can run your tests on real devices (mobile, PCs, etc.) or inside the Unity Editor.

Read the documentation on https://alttester.com/docs/sdk/latest/

## Get Started

Check out the [Get Started](https://alttester.com/docs/sdk/latest/pages/get-started.html) guide from the documentation.

### Running Tests

Run the following command to install the dev dependencies:

```
$ pip install -r requirements-dev.txt
```

#### Unit Tests

```
$ pytest tests/unit/
```

#### Integration Tests

```
$ pytest tests/integration/
```
