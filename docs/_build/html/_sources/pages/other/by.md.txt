# BY

## Description:
It is used in find objects methods to set the criteria of which the objects are searched.  
Currenty there are 6 type implemented:
  * *Tag* - search for objects that have a specific tag
  * *Layer* - search for objects that are set on a specific layer
  * *Name* - search for objects that are named in a certain way
  * *Component* - search for objects that have certain component
  * *Id* - search for objects that has assigned certain id (every object has an unique id so this criteria always will return 1 or 0 objects)
  * *Path* - search for objects that respect a certain path


### Searching object by path

The following selecting nodes, attributes and attributes are implemented:
  * *object* -	Selects all object with the name "object"
  * */* - 	Selects from the root node
  * *//* - Selects nodes in the document from the current node that match the selection no matter where they are
  * *..* - Selects the parent of the current node
  * \* - 	Matches any element node
  * *@tag* - 
  * *@layer* -
  * *@name* -
  * *@component* -
  * *@id* -
  * *contains* -
  


How a correct path should look like:  
  ```//Canvas/Panel/*[@tag="UI"]```
  
 #### Examples
 ```
//Button - Returns every object named button in the scene 
//*[@tag=UI] -Returns every object that is tagged as UI
/Canvas//Button[@component=ButtonLogic] - Return every button who are in an canvas that is a root object and has a component name ButtonLogic
//*[contains(@name,Ca)] - Returns every object in the scene that contains in the name "Ca"
```

 