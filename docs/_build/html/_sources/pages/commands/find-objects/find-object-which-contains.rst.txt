Command: FindObjectWhichContains
####################################

## Description:
****************

Find the first object in the scene that respects the given criteria. Check [By]({{ site.baseurl }}/pages/other/by) for more information about criterias.

###### Observation: Every criteria except of path works for this command

## Parameters:
===============


============ ======== ========= =========================================================================================================================================================================================================================================================================================================
Name         Type     Optional  Description 
============ ======== ========= =========================================================================================================================================================================================================================================================================================================
 by          By       false     Set what criteria to use in order to find the object
 value       string   false     The value to which object will be compared to see if they respect the criteria or not
 cameraName  string   true      the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
 enabled     boolean  true      true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not
============ ======== ========= =========================================================================================================================================================================================================================================================================================================



###### Observation: Since Java doesn't have optional paramaters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **AltGetAllElementsParameters** which we use the parameters mentioned. The java example will also show how to build such an object.

## Examples

.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestFindElementWhereNameContains()
        {
            const string name = "Cap";
            var altElement = altUnityDriver.FindObjectWhichContains(By.NAME,name);
            Assert.NotNull(altElement);
            Assert.True(altElement.name.Contains(name));
        }
    .. code-tab:: java

         @Test
        public void testFindElementWhereNameContains() throws Exception {

            String name = "Cap";
            AltUnityObject altElement = altUnityDriver.findObjectWhichContains(AltUnityDriver.By.NAME,name);
            assertNotNull(altElement);
            assertTrue(altElement.name.contains(name));
        }


    .. code-tab:: py

        def example()
            print("HelloWorld!)

.. tabs::

   .. code-tab:: c

         int main(const int argc, const char **argv) {
           return 0;
         }

   .. code-tab:: c++

         int main(const int argc, const char **argv) {
           return 0;
         }

   .. code-tab:: py

         def main():
             return

   .. code-tab:: java

         class Main {
             public static void main(String[] args) {
             }
         }

   .. code-tab:: julia

         function main()
         end

   .. code-tab:: fortran

         PROGRAM main
         END PROGRAM main
