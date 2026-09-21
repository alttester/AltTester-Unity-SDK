# Running tests concurrently

## Execute tests concurrently

In the `AltDriver` constructor you have the option to specify multiple tags. The available tags are: app name, platform, platform version, device instance id and app id. The app id can be used to uniquely identify an app. In case you specify no tags, the tests will be run on a randomly chosen app.

Keep in mind that, the tags given in the constructor will choose one random free app satisfying the requirements. Only one test can run on one app simultaneously. If you want to run the same tests on multiple apps concurrently, you have to start the `dotnet test` command multiple times, once for each app/device that you want your tests to be executed on. Depending on your setup, you might want to replace the `dotnet test` command with `pytest` or any other command that you usually use to start your tests.

```eval_rst
.. note::

    In order to ensure that the `dotnet test` command is executed multiple times concurrently within the same terminal add an `&` at the end of the command to run it in the background.
```

```eval_rst
.. note::

    Make sure that your **product names are different** in case you started multiple instrumented apps on the **same device**, otherwise your tests might fail because they are using the same resources (ex. values saved to `PlayerPref`).
```

Ex1. Let's say we want to run a set of tests on all apps started on Windows 11 (the exact platform version is displayed in the green popup and in AltTester® Desktop). For that, use the following code snippet:

```eval_rst
.. tabs::
    .. code-tab:: c#

            altDriver = new AltDriver (host: "127.0.0.1", port: 13000, platformVersion: "Windows 11  (10.0.22621) 64bit");

    .. code-tab:: java

            altDriver = new AltDriver ("127.0.0.1", 13000, false, 60, "unknown", "unknown", "Windows 11  (10.0.22621) 64bit", "unknown", "unknown");

    .. code-tab:: py

            alt_driver = AltDriver(host="127.0.0.1", port=13000, platform_version="Windows 11  (10.0.22621) 64bit")

    .. code-tab:: robot

            Initialize Altdriver    host=127.0.0.1    port=13000    platform_version=Windows 11 ${SPACE}(10.0.22621) 64bit 
```

Ex2. Let's say we want to run the same set of tests on Windows and Android platforms. If you run your tests with `pytest`, use the following code snippets:

In your test file:
```eval_rst
    .. code-block:: py

        def test(platform):
            alt_driver = AltDriver(host="127.0.0.1", port=13000, platform=platform)
```

In your conftest.py file:
```eval_rst
    .. code-block:: py

        def pytest_addoption(parser):
            parser.addoption("--platform", action="store", default="default name")


        def pytest_generate_tests(metafunc):
            option_value = metafunc.config.option.platform
            if 'platform' in metafunc.fixturenames and option_value is not None:
                metafunc.parametrize("platform", [option_value])
```

Then you can run from the command line with a command line argument:

```eval_rst
    .. code-block:: bash

        pytest --platform "WindowsPlayer" &
        pytest --platform "Android"
```

Another way of doing this is with environment variables:

In your test file:
```eval_rst
    .. code-block:: py

        def test():
            alt_driver = AltDriver(host="127.0.0.1", port=13000, platform=get_platform())
```

In your conftest.py file:
```eval_rst
    .. code-block:: py

        def get_platform():
            return os.environ.get("PLATFORM", "")
```

Then you can set the environment variables and run from the command line the `pytest` command:

```eval_rst
    .. code-block:: bash

        export PLATFORM="WindowsPlayer"
        pytest &
        export PLATFORM="Android"
        pytest
```

```eval_rst
.. important::

    Although this version of AltTester® Unity SDK is backwards compatible, in case you have older versions of instrumented apps, you won't be able to run your tests concurrently.
```
