# AltUnityTester changelog

The latest version of this file can be found at the master branch of the
AltUnityTester repository.

1.3.0
- introduce Lombok. In order to use the project with this library you need to install IDE extension from [official page](https://projectlombok.org/download)
- add unit and integration tests
- encapsulate Socket instance in [AltUnityDriver](src/main/java/ro/altom/altunitytester/AltUnityDriver.java)
- make instance of AltUnityDriver package private in [AltUnityObject](/src/main/java/ro/altom/altunitytester/AltUnityObject.java)
- change logging to use slf4j and log4j as a implementation - annotation based using Lombok
- add javadoc in the [AltUnityObject](/src/main/java/ro/altom/altunitytester/AltUnityObject.java) for  the state variables - don't access them directly as they will get encapsulated