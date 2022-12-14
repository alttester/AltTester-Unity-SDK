# AltTester Unity SDK Docs

## Install

```
$ pip install -r requirements.txt
```

## Build

To build the docs:

```
$ make clean
$ make html
```

To open the docs in your default browser:

```
$ open build/html/index.html
```

You can also start a web server:

```
$ python -m http.server -d build/html/
```

Visit http://localhost:8000/.