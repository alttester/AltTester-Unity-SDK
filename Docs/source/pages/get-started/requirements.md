# Requirements

Before you start, check these. Most setup problems are one of them.

```eval_rst
.. list-table::
   :widths: 30 70
   :header-rows: 1

   * - What
     - Needs to be
   * - Unity
     - 2021.3 LTS or newer. Unity 2020.3 LTS is no longer supported — it is past Unity's own
       end-of-support and had an IL2CPP limitation.
   * - AltTester® Desktop
     - Installed on the machine that will run the server, with a licence covering Unity. See the
       :doc:`compatibility matrix </pages/intro/compatibility>`.
   * - A test project
     - Any language with an AltDriver binding: C#, Python, Java or Robot Framework. You install
       AltDriver into it when you :doc:`write your first test </pages/get-started/first-test>`.
   * - Network
     - If the app runs on a device rather than your machine, both must reach each other, or you
       need reverse port forwarding. See
       :doc:`Reverse port forwarding </pages/guides/reverse-port-forwarding>`.
```

```eval_rst
.. important::
    All scenes must be included in the build, even ones loaded from an AssetBundle. Scenes left
    out cannot be inspected, and objects in them will not be found.
```

Not sure how the pieces fit together? [How it works](../intro/how-it-works.md) is one page.
