import socket
import time
import unittest
import re

TCP_IP = '127.0.0.1'
TCP_PORT = 13000
BUFFER_SIZE = 1024

class AltUnityDriverTests(unittest.TestCase):

    def recvall(self, socket):
        data = ''
        counter = 0
        while True:
            counter += 1
            part = socket.recv(BUFFER_SIZE)
            data += part
            if "::altend" in part:
                break
        try:
            data = data.split('altstart::')[1].split('::altend')[0]
        except:
            print 'Data received from socket doesn not have correct start and end control strings'
            return ''
        return data
    
    @classmethod
    def setUpClass(self):
        self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.s.connect((TCP_IP, TCP_PORT))
    
    @classmethod
    def tearDownClass(self):
        self.s.send('closeConnection;&')
        self.s.close()

    def test_find_object_by_name(self):
        self.s.send('findObjectByName;Capsule;&')
        data = self.recvall(self.s)
        expected_repsonse_pattern = '{"name":"Capsule","id":[0-9]+,"x":[0-9]+,"y":[0-9]+,"mobileY":[0-9]+,"text":"","type":"","enabled":true}'
        assert re.match(expected_repsonse_pattern, data), 'data was ' + data

    def test_find_object_where_name_contains(self):
        self.s.send('findObjectWhereNameContains;Info;&')
        data = self.recvall(self.s)
        expected_repsonse_pattern = '{"name":"CapsuleInfo","id":[0-9]+,"x":[0-9]+,"y":[0-9]+,"mobileY":[0-9]+,"text":".*?","type":"","enabled":true}'
        assert re.match(expected_repsonse_pattern, data), 'data was ' + data

    def test_find_objects_by_name(self):
        self.s.send('findObjectsByName;Plane;&')
        data = self.recvall(self.s)
        expected_repsonse_pattern = '[{"name":"Plane","id":[0-9]+,"x":[0-9]+,"y":[0-9]+,"mobileY":[0-9]+,"text":"","type":"","enabled":true},{"name":"Plane","id":[0-9]+,"x":[0-9]+,"y":[0-9]+,"mobileY":[0-9]+,"text":"","type":"","enabled":true}]'
        assert re.match(expected_repsonse_pattern, data), 'data was ' + data

    def test_find_objects_where_name_contains(self):
        self.s.send('findObjectsWhereNameContains;Plan;&')
        data = self.recvall(self.s)
        expected_repsonse_pattern = '[{"name":"Plane","id":[0-9]+,"x":[0-9]+,"y":[0-9]+,"mobileY":[0-9]+,"text":"","type":"","enabled":true},{"name":"Plane","id":[0-9]+,"x":[0-9]+,"y":[0-9]+,"mobileY":[0-9]+,"text":"","type":"","enabled":true}]'
        assert re.match(expected_repsonse_pattern, data), 'data was ' + data

    def test_get_current_scene(self):
        self.s.send('getCurrentScene;&')
        data = self.recvall(self.s)
        expected_repsonse = '{"name":"AltUnityDriverTestScene","id":[0-9]+,"x":0,"y":0,"mobileY":0,"text":"","type":"UnityScene","enabled":true}'
        assert re.match(expected_repsonse, data), 'data was ' + data

    def test_tap_object_by_name(self):
        self.s.send('tapObjectByName;Capsule;&')
        self.recvall(self.s)
        self.s.send('findObjectByName;CapsuleInfo;&')        
        data = self.recvall(self.s)
        assert '"Capsule was clicked to jump!"' in data, 'data was ' + data

    def test_tap_ui_object_by_name(self):
        self.s.send('tapObjectByName;UIButton;&')
        self.recvall(self.s)
        self.s.send('findObjectByName;CapsuleInfo;&')        
        data = self.recvall(self.s)
        assert '"UIButton clicked to jump capsule!"' in data, 'data was ' + data

    def test_find_all_objects(self):
        self.s.send('findAllObjects;&')
        data = self.recvall(self.s)
        assert '[{"name":' in data, 'response should contain `name` in the beginning'
        assert 'Capsule' in data, 'response should contain Capsule'
        assert 'Plane' in data, 'response should contain Plane'
        assert 'UIButton' in data, 'response should contain UIButton'
        assert 'AltUnityDriver' in data, 'response should contain AltUnityDriver'
        assert 'Main Camera' in data, 'response should contain Main Camera'
        assert 'CapsuleInfo' in data, 'response should contain CapsuleInfo'

    def test_look_for_non_existent_object(self):
        self.s.send('findObjectByName;NonExistent;&')
        data = self.recvall(self.s)
        assert 'error:notFound' in data, 'data was ' + data

    def test_look_for_non_existent_objects(self):
        self.s.send('findObjectsByName;NonExistent;&')
        data = self.recvall(self.s)
        assert '[]' in data, 'data was ' + data

    def test_look_for_object_with_name_that_contains_non_existent_text(self):
        self.s.send('findObjectWhereNameContains;NonExistent;&')
        data = self.recvall(self.s)
        assert 'error:notFound' in data, 'data was ' + data
    
    def test_look_for_objects_with_name_that_contains_non_existent_text(self):
        self.s.send('findObjectsWhereNameContains;NonExistent;&')
        data = self.recvall(self.s)
        assert '[]' in data, 'data was ' + data

    def test_find_object_by_component(self):
        self.s.send('findObjectByComponent;Capsule;&')
        data = self.recvall(self.s)
        expected_repsonse_pattern = '{"name":"Capsule","id":[0-9]+,"x":[0-9]+,"y":[0-9]+,"mobileY":[0-9]+,"text":"","type":"","enabled":true}'
        assert re.match(expected_repsonse_pattern, data), 'data was ' + data

    def test_find_objects_by_component(self):
        self.s.send('findObjectsByComponent;UnityEngine.MeshFilter;&')
        data = self.recvall(self.s)
        expected_repsonse_pattern = '[{"name":"Plane","id":[0-9]+,"x":[0-9]+,"y":[0-9]+,"mobileY":[0-9]+,"text":"","type":"","enabled":true},{"name":"Plane","id":[0-9]+,"x":[0-9]+,"y":[0-9]+,"mobileY":[0-9]+,"text":"","type":"","enabled":true},{"name":"Capsule","id":[0-9]+,"x":[0-9]+,"y":[0-9]+,"mobileY":[0-9]+,"text":"","type":"","enabled":true}]'
        assert re.match(expected_repsonse_pattern, data), 'data was ' + data

    def test_find_object_by_non_existent_component(self):
        self.s.send('findObjectByComponent;nonexistent;&')
        data = self.recvall(self.s)
        expected_repsonse = 'error:componentNotFound'
        assert expected_repsonse == data, 'data was ' + data

    def test_find_object_by_wrong_format_component(self):
        self.s.send('findObjectByComponent;Unity.Engine;&')
        data = self.recvall(self.s)
        expected_repsonse = 'error:componentNotFound'
        assert expected_repsonse == data, 'data was ' + data
    
    def test_get_object_component_property(self):
        self.s.send('findObjectByComponent;Capsule;&')
        capsule_object = self.recvall(self.s)
        property = '{"component":"Capsule", "property":"capsuleInfo"}'
        self.s.send('getObjectComponentProperty;' + capsule_object + ';'+ property + ';&')
        data = self.recvall(self.s)
        expected_repsonse = 'CapsuleInfo (UnityEngine.UI.Text)'
        assert expected_repsonse == data, 'data was ' + data

    def test_get_object_component_property_array(self):
        self.s.send('findObjectByComponent;Capsule;&')
        capsule_object = self.recvall(self.s)
        property = '{"component":"Capsule", "property":"arrayOfInts"}'
        self.s.send('getObjectComponentProperty;' + capsule_object + ';'+ property + ';&')
        data = self.recvall(self.s)
        expected_repsonse = '[1,2,3]'
        assert expected_repsonse == data, 'data was ' + data

    def test_get_object_component_property_unity_engine(self):
        self.s.send('findObjectByComponent;Capsule;&')
        capsule_object = self.recvall(self.s)
        property = '{"component":"UnityEngine.CapsuleCollider", "property":"isTrigger"}'
        self.s.send('getObjectComponentProperty;' + capsule_object + ';'+ property + ';&')
        data = self.recvall(self.s)
        expected_repsonse = 'false'
        assert expected_repsonse == data, 'data was ' + data
    
    def test_look_for_objects_with_non_existent_components(self):
        self.s.send('findObjectByComponent;Capsule;&')
        capsule_object = self.recvall(self.s)
        property_info = '{"component":"nonExistent", "property":"nonExistent"}'
        self.s.send('getObjectComponentProperty;' + capsule_object + ';'+ property_info + ';&')
        data = self.recvall(self.s)
        expected_repsonse = 'error:componentNotFound'
        assert expected_repsonse == data, 'data was ' + data

    def test_look_for_objects_with_non_existent_property(self):
        self.s.send('findObjectByComponent;Capsule;&')
        capsule_object = self.recvall(self.s)
        property_info = '{"component":"Capsule", "property":"nonExistent"}'
        self.s.send('getObjectComponentProperty;' + capsule_object + ';'+ property_info + ';&')
        data = self.recvall(self.s)
        expected_repsonse = 'error:propertyNotFound'
        assert expected_repsonse == data, 'data was ' + data

    def test_call_component_method_for_object_no_params(self):
        self.s.send('findObjectByComponent;Capsule;&')
        capsule_object = self.recvall(self.s)
        action = '{"component":"Capsule", "method":"UIButtonClicked", "parameters":""}'
        self.s.send('callComponentMethodForObject;' + capsule_object + ';'+ action + ';&')
        data = self.recvall(self.s)
        expected_repsonse = 'methodInvoked'
        assert expected_repsonse == data, 'data was ' + data

    def test_call_component_method_for_object_with_params(self):
        self.s.send('findObjectByComponent;Capsule;&')
        capsule_object = self.recvall(self.s)
        action = '{"component":"Capsule", "method":"Jump", "parameters":"new text"}'
        self.s.send('callComponentMethodForObject;' + capsule_object + ';'+ action + ';&')
        data = self.recvall(self.s)
        expected_repsonse = 'methodInvoked'
        assert expected_repsonse == data, 'data was ' + data

    def test_call_component_method_for_object_with_many_params(self):
        self.s.send('findObjectByComponent;Capsule;&')
        capsule_object = self.recvall(self.s)
        action = '{"component":"Capsule", "method":"TestMethodWithManyParameters", "parameters":"1?stringparam?0.5?[1,2,3]"}'
        self.s.send('callComponentMethodForObject;' + capsule_object + ';'+ action + ';&')
        data = self.recvall(self.s)
        expected_repsonse = 'methodInvoked'
        assert expected_repsonse == data, 'data was ' + data

    def test_call_component_method_for_object_with_incorrect_number_of_params(self):
        self.s.send('findObjectByComponent;Capsule;&')
        capsule_object = self.recvall(self.s)
        action = '{"component":"Capsule", "method":"Jump", "parameters":"new text? some other text"}'
        self.s.send('callComponentMethodForObject;' + capsule_object + ';'+ action + ';&')
        data = self.recvall(self.s)
        expected_repsonse = 'error:incorrectNumberOfParameters'
        assert expected_repsonse == data, 'data was ' + data

    def test_get_text_for_object(self):
        self.s.send('findObjectByName;CapsuleInfo;&')
        capsule_object = self.recvall(self.s)
        current_text = capsule_object.split('"text":"')[1].split('","type"')[0]
        self.s.send('getText;' + capsule_object + ';&')
        data = self.recvall(self.s)
        assert current_text == data, 'data was ' + data

    def test_get_text_for_object_with_no_text(self):
        self.s.send('findObjectByName;Capsule;&')
        capsule_object = self.recvall(self.s)
        self.s.send('getText;' + capsule_object + ';&')
        data = self.recvall(self.s)
        assert "" == data, 'data was ' + data

    def test_get_text_for_not_found_object(self):
        self.s.send('findObjectByName;NonExistent;&')
        capsule_object = self.recvall(self.s)
        self.s.send('getText;' + capsule_object + ';&')
        data = self.recvall(self.s)
        assert "error:objectNotFound" == data, 'data was ' + data

    def test_set_property_for_component(self):
        self.s.send('findObjectByComponent;Capsule;&')
        capsule_object = self.recvall(self.s)
        property = '{"component":"Capsule", "property":"stringToSetFromTests"}'
        self.s.send('setObjectComponentProperty;' + capsule_object + ';'+ property + ';newValueSetFromTest;&')
        data = self.recvall(self.s)
        expected_repsonse = 'valueSet'
        assert expected_repsonse == data, 'data was ' + data
        self.s.send('getObjectComponentProperty;' + capsule_object + ';'+ property + ';&')
        data = self.recvall(self.s)
        expected_repsonse = 'newValueSetFromTest'
        assert expected_repsonse == data, 'data was ' + data

    def test_set_object_component_property_array(self):
        self.s.send('findObjectByComponent;Capsule;&')
        capsule_object = self.recvall(self.s)
        property = '{"component":"Capsule", "property":"arrayOfInts"}'
        self.s.send('setObjectComponentProperty;' + capsule_object + ';'+ property + ';[2,3,4];&')
        data = self.recvall(self.s)
        self.s.send('getObjectComponentProperty;' + capsule_object + ';'+ property + ';&')
        data = self.recvall(self.s)
        expected_repsonse = '[2,3,4]'
        assert expected_repsonse == data, 'data was ' + data

    def test_set_property_for_non_existent_component(self):
        self.s.send('findObjectByComponent;Capsule;&')
        capsule_object = self.recvall(self.s)
        property = '{"component":"nonExistent", "property":"NonExistent"}'
        self.s.send('setObjectComponentProperty;' + capsule_object + ';'+ property + ';newValueSetFromTest;&')
        data = self.recvall(self.s)
        expected_repsonse = 'error:componentNotFound'
        assert expected_repsonse == data, 'data was ' + data

    def test_set_property_for_non_existent_property(self):
        self.s.send('findObjectByComponent;Capsule;&')
        capsule_object = self.recvall(self.s)
        property = '{"component":"Capsule", "property":"NonExistent"}'
        self.s.send('setObjectComponentProperty;' + capsule_object + ';'+ property + ';newValueSetFromTest;&')
        data = self.recvall(self.s)
        expected_repsonse = 'error:propertyNotFound'
        assert expected_repsonse == data, 'data was ' + data

    def test_set_property_incorrect_type(self):
        self.s.send('findObjectByComponent;Capsule;&')
        capsule_object = self.recvall(self.s)
        property = '{"component":"Capsule", "property":"arrayOfInts"}'
        self.s.send('setObjectComponentProperty;' + capsule_object + ';'+ property + ';2,3,4;&')
        data = self.recvall(self.s)
        expected_repsonse = 'error:propertyCannotBeSet'
        assert expected_repsonse == data, 'data was ' + data

if __name__ == '__main__':
    suite = unittest.TestLoader().loadTestsFromTestCase(AltUnityDriverTests)
    unittest.TextTestRunner(verbosity=2).run(suite)