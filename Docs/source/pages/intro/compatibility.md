# Compatibility

The three pieces — the SDK in your build, AltTester® Desktop, and AltDriver in your tests — are
released independently, so the combination matters.

```eval_rst
.. important::
    Upgrading one without the others is the most common way a working setup stops working.
```

The authoritative matrix lives with AltTester® Desktop, because it is the piece that has to work
with every SDK and driver version:

```eval_rst
:altTesterDesktopDocumentation:`Compatibility matrix <pages/intro/compatibility.html>`
```

## Finding your versions

```eval_rst
.. list-table::
   :widths: 34 66
   :header-rows: 1

   * - Piece
     - Where to look
   * - AltTester® Desktop
     - **Versions** in the application header.
   * - AltTester® SDK
     - The **SDK version** tag on the app's row in the Connected Apps list.
   * - AltDriver
     - The package version in your test project.
```
