# Java builders

## AltCallStaticMethodsParameters
#### Usage
This builder is used for following commands:
*  callStaticMethods
#### Methods

* Builder(By,String)
* withAssembly(String)
* withTypeOfParameters(String)
* build()

#### Example
 ```java
 AltCallStaticMethodsParameters altCallStaticMethodsParameters=new AltCallStaticMethodsParameters.Builder(typeName,methodName,parameters).withAssembly(assembly).withTypeOfParameters(typeOfParameters).build();
 ```
 
## AltFindObjectParameters
#### Usage
This builder is used for following commands:
* findObject
* findObjectWhichContains
* findObjects
* findObjectsWhichContains

#### Methods

* Builder(By,String)
* isEnabled(boolean)
* withCamera(String)
* build()

#### Example
 ```java
 AltFindObjectsParameters altFindObjectsParameters=new AltFindObjectsParameters.Builder(by,value).isEnabled(enabled).withCamera(cameraName).build();
 ```
## AltGetAllElementsParameters
#### Usage
This builder is used for following commands:
*  getAllElements
#### Methods

* Builder()
* isEnabled(boolean)
* withCamera(String)
* build()

#### Example
 ```java
 AltGetAllElementsParameters altGetAllElementsParameters=new AltGetAllElementsParameters.Builder().withCamera(cameraName).isEnabled(enabled).build();
 ```
  
 
## AltWaitForObjectsParameters
#### Usage
This builder is used for following commands:
* waitForObject
* waitForObjectToNotBePresent
* waitForObjectWhichContains

#### Methods

* Builder(AltFindObjectParameters)
* withTimeout(double)
* withInterval(double)
* build()

#### Example
 ```java
 AltFindObjectsParameters altFindObjectsParameters=new AltFindObjectsParameters.Builder(by,value).withCamera(cameraName).isEnabled(enabled).build();
 AltWaitForObjectsParameters altWaitForObjectsParameters=new AltWaitForObjectsParameters.Builder(altFindObjectsParameters).withInterval(interval).withTimeout(timeout).build();
 ```

## AltWaitForObjectWithTextParameters

#### Usage
This builder is used for following commands:
* waitForObjectWithText

#### Methods

* Builder(AltFindObjectsParameters,String)
* withTimeout(double)
* withInterval(double)
* build()

#### Example
 ```java
 AltFindObjectsParameters altFindElementsParameters=new AltFindObjectsParameters.Builder(by,value).isEnabled(enabled).withCamera(cameraName).build();
 AltWaitForObjectWithTextParameters altWaitForElementWithTextParameters=new AltWaitForObjectWithTextParameters.Builder(altFindElementsParameters,text).withInterval(interval).withTimeout(timeout).build();
 ```

## AltMoveMouseParameters

#### Usage
This builder is used for following commands:
* moveMouse
* moveMouseAndWait
#### Methods

* Builder(int,int)
* withDuration(float)
* build()

#### Example
 ```java
 AltMoveMouseParameters altMoveMouseParameters=new AltMoveMouseParameters.Builder(x, y).withDuration(duration).build();
 ```

## AltPressKeyParameters

#### Usage
This builder is used for following commands:
* pressKey
* pressKeyAndWait

#### Methods

* Builder(String)
* withDuration(float)
* withPower(float)
* build()

#### Example
 ```java
  AltPressKeyParameters altPressKeyParameters=new AltPressKeyParameters.Builder(keyName).withPower(power).withDuration(duration).build();
 ```

## AltScrollMouseParameters

#### Usage
This builder is used for following commands:
* scrollMouse
* scrollMouseAndWait

#### Methods

* Builder()
* withDuration(boolean)
* withSpeed(String)
* build()

#### Example
 ```java
 AltMoveMouseParameters altMoveMouseParameters=new AltMoveMouseParameters.Builder(x,y).withDuration(duration).build();
 ```

## AltComponentMethodParameters
#### Usage
This builder is used for following commands:
* callComponentMethod
#### Methods

* Builder(String.String,String)
* withAssembly(String)
* withTypeOfParameters(String)
* build()


#### Example
 ```java
 AltCallComponentMethodParameters altCallComponentMethodParameters=new AltCallComponentMethodParameters.Builder(componentName,methodName,parameters).withTypeOfParameters(typeOfParameters).withAssembly(assemblyName).build();

 ```

## AltGetComponentPropertyParameters
#### Usage
This builder is used for following commands:
* getComponentProperty
#### Methods

* Builder(String,String)
* withAssembly(String)
* build()

#### Example
 ```java
 AltGetComponentPropertyParameters altGetComponentPropertyParameters=new AltGetComponentPropertyParameters.Builder(componentName,propertyName).withAssembly(assemblyName).build();
 ```

## AltSetComponentPropertyParameters
#### Usage
This builder is used for following commands:
* setComponentProperty
#### Methods

* Builder(String.String,String)
* withAssembly(String)
* build()

#### Example
 ```java
 AltSetComponentPropertyParameters altSetComponentPropertyParameters=new AltSetComponentPropertyParameters.Builder(componentName,propertyName,value).withAssembly(assemblyName).build();
 ```

## AltWaitForCurrentSceneToBeParameters
#### Usage
This builder is used for following commands:
* waitForCurrentSceneToBe
#### Methods

* Builder(String)
* withTimeout(double)
* withInterval(double)
* build()

#### Example
 ```java
 AltWaitForCurrentSceneToBeParameters altWaitForCurrentSceneToBeParameters=new AltWaitForCurrentSceneToBeParameters.Builder(sceneName).withInterval(interval).withTimeout(timeout).build();
 ```
