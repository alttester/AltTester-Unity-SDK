# AltUnityTester Python Bindings

This package contains an library for adding Python language binding to the AltUnity Tester framework.

AltUnity Tester is an open-source UI driven test automation tool that helps you find objects in your game and interacts with them using tests written in C#, Python or Java.

You can run your tests on real devices (mobile, PCs, etc.) or inside the Unity Editor.

Read the documentation on https://altom.gitlab.io/altunity/altunitytester

## Get Started

Check out the [Get Started](https://altom.gitlab.io/altunity/altunitytester/pages/get-started.html) guide from the documentation.

## Development

* Code Style: [PEP-0008](https://www.python.org/dev/peps/pep-0008/)
* Docstring style: [Google Style Docstrings](https://sphinxcontrib-napoleon.readthedocs.io/en/latest/example_google.html).

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

## Contributing

Check out the full contributing guide [contributing](https://altom.gitlab.io/altunity/altunitytester/pages/contributing.html).

## Support

Join our Google Group for questions and discussions: https://groups.google.com/a/altom.com/forum/#!forum/altunityforum

Join our Discord Server to chat with other members of the community: https://discord.gg/Ag9RSuS

## License

Distributed under the **GNU General Public License v3.0**. See [LICENSE](https://gitlab.com/altom/altunity/altunitytester/-/blob/master/LICENSE) for more information.
