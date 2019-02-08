# Unity.IO.Compression
This is a port of Microsoft's code from [here](https://github.com/Microsoft/referencesource/tree/master/System/sys/system/IO/compression).

The classes in System.IO.Compression in Unity 4.x [do not seem to work on Windows](http://answers.unity3d.com/questions/692250/gzipstream-and-deflatestream-give-entrypointnotfou.html) and perhaps several other platforms. 

Luckily, Microsoft has released much of the source code of the .NET BCL. We have ported Microsoft's code to work in Unity. This seems like the cleanest and most stable way to get the GZipStream and DeflateStream classes working in Unity. 

Find the plugin on the Unity Asset Store [here](https://www.assetstore.unity3d.com/#!/content/31902). 

Built by [Hitcents](http://hitcents.com/), contact us [here](http://hitcents.com/contact) for questions.
