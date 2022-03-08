============
Known Issues
============

This section lists the known bugs and issues with the AltUnity Tester. If
available, we list a workaround to help troubleshoot the issue.

To report a bug that isn’t listed here, see our :doc:`contributing` section
to learn how to best report the issue.


.NET Driver
===========

Calling ``GetPNGScreenshot`` throws ``StackOverflow`` error
-----------------------------------------------------------

**Problem**: For high resolutions calling ``GetPNGScreenshot`` might throw a
``StackOverflow`` error.

**Workaround**: The issue only happens with .NET 6 as a workaround you can use
.NET 5, or if you can't downgrade to .NET 5, try to run your tests with a lower
resolution until this issue is fixed.

**Affects**: AltUnityTester v1.7.0 with .NET 6
