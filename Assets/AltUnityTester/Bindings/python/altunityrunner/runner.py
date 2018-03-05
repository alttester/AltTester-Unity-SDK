#! /usr/bin/env python
import subprocess
import json
import time
import socket
BUFFER_SIZE = 1024

class AltElement(object):
    def __init__(self, alt_unity_driver, appium_driver, json_data):
        self.alt_unity_driver = alt_unity_driver
        self.appium_driver = appium_driver
        data = json.loads(json_data)
        self.name = str(data['name'])
        self.id = str(data['id'])
        self.x = str(data['x'])
        self.y = str(data['y'])
        self.mobileY = str(data['mobileY'])
        self.text = str(data['text'])
        self.type = str(data['type'])
        self.enabled = str(data['enabled'])

    def toJSON(self):
        return '{"name":"' + self.name + '", \
                 "text":"' + self.text + '", \
                 "id":"' + self.id + '", \
                 "x":"' + self.x + '", \
                 "y":"' + self.y + '", \
                 "mobileY":"' + self.mobileY + '", \
                 "type":"' + self.type + '", \
                 "enabled":"' + self.enabled + '"}'
        

    def get_component_property(self, component_name, property_name):
        alt_object = self.toJSON()
        property_info = '{"component":"' + component_name + '", "property":"' + property_name + '"}'
        self.alt_unity_driver.socket.send('getObjectComponentProperty;' + alt_object + ';'+ property_info + ';&')
        data = self.alt_unity_driver.recvall(self.alt_unity_driver.socket)
        return data

    def set_component_property(self, component_name, property_name, value):
        alt_object = self.toJSON()
        property_info = '{"component":"' + component_name + '", "property":"' + property_name + '"}'
        self.alt_unity_driver.socket.send('setObjectComponentProperty;' + alt_object + ';'+ property_info + ';' + value + ';&')
        data = self.alt_unity_driver.recvall(self.alt_unity_driver.socket)
        return data

    def call_component_method(self, component_name, method_name, parameters):
        alt_object = self.toJSON()
        action_info = '{"component":"' + component_name + '", "method":"' + method_name + '", "parameters":"' + parameters + '"}'
        self.alt_unity_driver.socket.send('callComponentMethodForObject;' + alt_object + ';'+ action_info + ';&')
        data = self.alt_unity_driver.recvall(self.alt_unity_driver.socket)
        return data

    def get_text(self):
        alt_object = self.toJSON()
        self.alt_unity_driver.socket.send('getText;' + alt_object + ';&')
        data = self.alt_unity_driver.recvall(self.alt_unity_driver.socket)
        return data

    def tap(self, durationInSeconds=0.5):
        self.appium_driver.tap([[float(self.x), float(self.mobileY)]], durationInSeconds * 1000)
    
    def dragTo(self, end_x, end_y, durationIndSeconds=0.5):
        self.appium_driver.swipe(self.x, self.mobileY, end_x, end_y, durationIndSeconds)

    def dragToElement(self, other_element, durationIndSeconds=0.5):
        self.appium_driver.swipe(self.x, self.mobileY, other_element.x, other_element.mobileY, durationIndSeconds)

class AltrunUnityDriver():

    def __init__(self, appium_driver, TCP_IP='127.0.0.1', TCP_PORT=13001, timeout=60):
        self.appium_driver = appium_driver
        
        while (timeout > 0):
            try:
                try:
                    subprocess.call(['adb', 'forward','tcp:' + str(TCP_PORT), 'tcp:13000'])
                except:
                    print 'AltUnityServer - could not start adb forwarding'
                try:
                    subprocess.call(['iproxy', str(TCP_PORT),'13000'])
                except:
                    print 'AltUnityServer - could not start iproxy forwarding'
                self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                self.socket.connect((TCP_IP, TCP_PORT))
                self.socket.send('startConnection;&')
                self.recvall(self.socket)
                break
            except Exception as e:
                print e
                print 'AltUnityServer not running on port ' + str(TCP_PORT) + ', retrying (timing out in ' + str(timeout) + ' secs)...'
                timeout -= 5
                time.sleep(5)

        if (timeout <= 0):
            raise Exception('AltUnityServer not running on port ' + str(TCP_PORT) + ', did you run ``adb forward tcp:' + str(TCP_PORT) + ' tcp:13000`` or ``iproxy ' + str(TCP_PORT) + ' 13000``?')

        
    def stop(self):
        self.socket.send('closeConnection;&')
        self.socket.close()

    def recvall(self, socket):
        data = ''
        counter = 0
        while True:
            part = socket.recv(BUFFER_SIZE)
            data += part
            if '::altend' in part:
                break
        try:
            data = data.split('altstart::')[1].split('::altend')[0]
        except:
            print 'Data received from socket doesn not have correct start and end control strings'
            return ''
        return data

    def get_all_elements(self):
        alt_elements = []
        self.socket.send('findAllObjects;&')
        data = self.recvall(self.socket)
        if (data != ''):
            try:
                elements = json.loads(data)
                for i in range(0, len(elements)):
                    alt_elements.append(AltElement(self, self.appium_driver, json.dumps(elements[i])))
            except Exception as e:
                print e
                print 'Received message could not be parsed: ' + data
        return alt_elements

    def find_element(self, name):  
        self.socket.send('findObjectByName;' + name + ';&')
        data = self.recvall(self.socket)
        if (data != '' and 'error:notFound' not in data):
            alt_el = AltElement(self, self.appium_driver, data)
            if alt_el.name == name and alt_el.enabled:
                print 'Element ' + name + ' found at x:' + str(alt_el.x) + ' y:' + str(alt_el.y) + ' mobileY:' + str(alt_el.mobileY)
                return alt_el
        return None

    def find_element_where_name_contains(self, name):  
        self.socket.send('findObjectWhereNameContains;' + name + ';&')
        data = self.recvall(self.socket)
        if (data != '' and 'error:notFound' not in data):
            alt_el = AltElement(self, self.appium_driver, data)
            if alt_el.name == name and alt_el.enabled:
                print 'Element ' + name + ' found at x:' + str(alt_el.x) + ' y:' + str(alt_el.y) + ' mobileY:' + str(alt_el.mobileY)
                return alt_el
        return None

    def find_elements(self, name):
        alt_elements = []
        self.socket.send('findObjectsByName;' + name + ';&')
        data = self.recvall(self.socket)
        if (data != ''):
            try:
                elements = json.loads(data)
                for i in range(0, len(elements)):
                    alt_elements.append(AltElement(self, self.appium_driver, json.dumps(elements[i])))
            except Exception as e:
                print e
                print 'Received message could not be parsed: ' + data
        return alt_elements

    def find_elements_where_name_contains(self, name):
        alt_elements = []
        self.socket.send('findObjectsWhereNameContains;' + name + ';&')
        data = self.recvall(self.socket)
        if (data != ''):
            try:
                elements = json.loads(data)
                for i in range(0, len(elements)):
                    alt_elements.append(AltElement(self, self.appium_driver, json.dumps(elements[i])))
            except Exception as e:
                print e
                print 'Received message could not be parsed: ' + data
        return alt_elements

    def get_current_scene(self):
        self.socket.send('getCurrentScene;&')
        data = self.recvall(self.socket)

        if (data != ''):
            alt_el = AltElement(self, self.appium_driver, data)
            print 'Current scene is ' + alt_el.name
            return alt_el.name
        return None

    def wait_for_current_scene_to_be(self, scene_name, timeout=30, interval=1):
        t = 0
        current_scene = ''
        while (t <= timeout):
            print 'Waiting for scene to be ' + scene_name + '...'
            current_scene = self.get_current_scene()
            if current_scene != scene_name:
                time.sleep(interval)
                t += interval
            else:
                break
        assert current_scene == scene_name, 'Scene ' + scene_name + ' not loaded after ' + str(timeout) + ' seconds'
        return current_scene

    def get_element_text(self, name):
        return self.find_element(name).text

    def wait_for_element(self, name, timeout=20, interval=0.5):
        t = 0
        alt_element = None
        while (t <= timeout):
            print 'Waiting for element ' + name + '...'
            alt_element = self.find_element(name)
            if alt_element == None:
                time.sleep(interval)
                t += interval
            else:
                break
        assert alt_element is not None, 'Element ' + name + ' not found after ' + str(timeout) + ' seconds'
        return alt_element


    def wait_for_element_where_name_contains(self, name, timeout=20, interval=0.5):
        t = 0
        alt_element = None
        while (t <= timeout):
            print 'Waiting for element where name contains ' + name + '...'
            alt_element = self.find_element_where_name_contains(name)
            if alt_element == None:
                time.sleep(interval)
                t += interval
            else:
                break
        assert alt_element is not None, 'Element where name contains ' + name + ' not found after ' + str(timeout) + ' seconds'
        return alt_element
    
    def wait_for_element_to_not_be_present(self, name, timeout=20, interval=0.5):
        t = 0
        while (t <= timeout):
            print 'Waiting for element ' + name + ' to not be present...'
            if self.find_element(name) != None:
                time.sleep(interval)
                t += interval
            else:
                break
        assert self.find_element(name) is None, 'Element ' + name + ' still found after ' + str(timeout) + ' seconds'

    def wait_for_element_with_text(self, name, text, timeout=20, interval=0.5):
        t = 0
        alt_element = None
        while (t <= timeout):
            print 'Waiting for element ' + name + ' to have text ' + text
            alt_element = self.wait_for_element(name)
            if alt_element.text != text:
                time.sleep(interval)
                t += interval
            else:
                break
        assert alt_element.text == text, 'Element ' + name + ' should have text `' + text + '` but has `' + alt_element.text + '` after ' + str(timeout) + ' seconds'
        return alt_element

    def find_element_by_component(self, component_name):
        self.socket.send('findObjectByComponent;' + component_name + ';&')
        data = self.recvall(self.socket)
        if (data != '' and 'error:notFound' not in data):
            alt_el = AltElement(self, self.appium_driver, data)
            if alt_el.name == component_name and alt_el.enabled:
                print 'Element with component ' + component_name + ' found at x:' + str(alt_el.x) + ' y:' + str(alt_el.y) + ' mobileY:' + str(alt_el.mobileY)
                return alt_el
        return None

    def find_elements_by_component(self, component_name):
        alt_elements = []
        self.socket.send('findObjectsByComponent;' + component_name + ';&')
        data = self.recvall(self.socket)
        if (data != ''):
            try:
                elements = json.loads(data)
                for i in range(0, len(elements)):
                    alt_elements.append(AltElement(self, self.appium_driver, json.dumps(elements[i])))
            except Exception as e:
                print e
                print 'Received message could not be parsed: ' + data
        return alt_elements