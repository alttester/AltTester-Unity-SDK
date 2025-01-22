
# For more info on the setup, check out our docs https://alttester.com/docs/sdk/latest/pages/get-started.html#write-and-execute-first-test-for-your-app  
import unittest
from alttester import By, AltKeyCode, AltDriver

class MyTests(unittest.TestCase):

  PlanePath="/UIWithWorldSpace/Plane"
  Path="/UIWithWorldSpace/1234"
  CapsulePath="/Capsule"
  TextMeshInputFieldPath="/Canvas/TextMeshInputField"
  @classmethod
  def setUp(self):
    self.alt_driver = AltDriver(host="127.0.0.1", port=13002, app_name="__default__")

  # You might want to load the scene here
  #   self.alt_driver.load_scene("Scene 1 AltDriverTestScene", True)

  @classmethod
  def tearDown(self):
    self.alt_driver.stop()

  def test(self):
    Plane = self.alt_driver.wait_for_object(By.PATH,self.PlanePath)
    altObject = self.alt_driver.wait_for_object(By.PATH,self.Path)
    Capsule = self.alt_driver.wait_for_object(By.PATH,self.CapsulePath)
    Capsule.click()
    Plane1 = self.alt_driver.wait_for_object(By.PATH,self.PlanePath)
    self.alt_driver.hold_button(Plane1.get_screen_position(),1.078217)
    TextMeshInputField = self.alt_driver.wait_for_object(By.PATH,self.TextMeshInputFieldPath)
    Plane2 = self.alt_driver.wait_for_object(By.PATH,self.PlanePath)
    self.alt_driver.swipe(TextMeshInputField.get_screen_position(),Plane2.get_screen_position(),0.5589905)
    Plane3 = self.alt_driver.wait_for_object(By.PATH,PlanePath)
    Plane3.tap()
