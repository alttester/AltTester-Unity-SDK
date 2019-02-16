import json
import re
import socket
import subprocess
import time

from altunityrunner.altUnityExceptions import *

BUFFER_SIZE = 1024

class PlayerPrefKeyType(object):
    Int = 1
    String = 2
    Float = 3


class AltElement(object):
    def __init__(self, alt_unity_driver, appium_driver, json_data):
        self.alt_unity_driver = alt_unity_driver
        if (appium_driver != None):
            self.appium_driver = appium_driver
        data = json.loads(json_data)
        self.name = str(data['name'])
        self.id = str(data['id'])
        self.x = str(data['x'])
        self.y = str(data['y'])
        self.z=str(data['z'])
        self.mobileY = str(data['mobileY'])
        self.type = str(data['type'])
        self.enabled = str(data['enabled'])
        self.worldX = str(data['worldX'])
        self.worldY = str(data['worldY'])
        self.worldZ = str(data['worldZ'])
        self.idCamera=str(data['idCamera'])

    def toJSON(self):
        return '{"name":"' + self.name + '", \
                 "id":"' + self.id + '", \
                 "x":"' + self.x + '", \
                 "y":"' + self.y + '", \
                 "z":"'+self.z+'",\
                 "mobileY":"' + self.mobileY + '", \
                 "type":"' + self.type + '", \
                 "enabled":"' + self.enabled + '", \
                 "worldX":"' + self.worldX + '", \
                 "worldY":"' + self.worldY + '", \
                 "worldZ":"' + self.worldZ + '",\
                 "idCamera":"'+self.idCamera+'"}'
        
    def get_component_property(self, component_name, property_name, assembly_name=''):
        alt_object = self.toJSON()
        property_info = '{"component":"' + component_name + '", "property":"' + property_name + '"'+',"assembly":"' + assembly_name + '"}'
        data = self.alt_unity_driver.send_data(self.alt_unity_driver.create_command('getObjectComponentProperty',alt_object,property_info ))
        return self.alt_unity_driver.handle_errors(data)

    def set_component_property(self, component_name, property_name, value, assembly_name=''):
        alt_object = self.toJSON()
        property_info = '{"component":"' + component_name + '", "property":"' + property_name + '"'+',"assembly":"' + assembly_name + '"}'
        data = self.alt_unity_driver.send_data(self.alt_unity_driver.create_command('setObjectComponentProperty',alt_object, property_info,value ))
        return self.alt_unity_driver.handle_errors(data)

    def call_component_method(self, component_name, method_name, parameters,assembly_name='',type_of_parameters=''):
        alt_object = self.toJSON()
        action_info = '{"component":"' + component_name + '", "method":"' + method_name + '", "parameters":"' + parameters +'"'+',"assembly":"' + assembly_name + '", "typeofparameters":"' + type_of_parameters +'"}'
        data = self.alt_unity_driver.send_data(self.alt_unity_driver.create_command('callComponentMethodForObject', alt_object,action_info  ))
        return self.alt_unity_driver.handle_errors(data)

    def get_text(self):
        alt_object = self.toJSON()
        property_info = '{"component":"UnityEngine.UI.Text", "property":"text"}'
        data = self.alt_unity_driver.send_data(self.alt_unity_driver.create_command('getObjectComponentProperty', alt_object,  property_info  ))
        return self.alt_unity_driver.handle_errors(data)

    def click_Event(self):
        alt_object = self.toJSON()
        data = self.alt_unity_driver.send_data(self.alt_unity_driver.create_command('clickObject',alt_object))
        return self.alt_unity_driver.handle_errors(data)
    def mobile_tap(self, durationInSeconds=0.5):
        self.appium_driver.tap([[float(self.x), float(self.mobileY)]], durationInSeconds * 1000)
    
    def mobile_dragTo(self, end_x, end_y, durationIndSeconds=0.5):
        self.appium_driver.swipe(self.x, self.mobileY, end_x, end_y, durationIndSeconds* 1000)

    def mobile_dragToElement(self, other_element, durationIndSeconds=0.5):
        self.appium_driver.swipe(self.x, self.mobileY, other_element.x, other_element.mobileY, durationIndSeconds* 1000)
    
    def drag(self, x, y):
        alt_object = self.toJSON()
        position_string = self.alt_unity_driver.vector_to_json_string(x, y)
        data = self.alt_unity_driver.send_data(self.alt_unity_driver.create_command('dragObject', position_string,alt_object  ))
        return self.alt_unity_driver.handle_errors(data)

    def drop(self, x, y):
        alt_object = self.toJSON()
        position_string = self.alt_unity_driver.vector_to_json_string(x, y)
        data = self.alt_unity_driver.send_data(self.alt_unity_driver.create_command('dropObject', position_string , alt_object ))
        return self.alt_unity_driver.handle_errors(data)
    
    def pointer_up(self):
        alt_object = self.toJSON()
        data = self.alt_unity_driver.send_data(self.alt_unity_driver.create_command('pointerUpFromObject', alt_object ))
        return self.alt_unity_driver.handle_errors(data)

    def pointer_down(self):
        alt_object = self.toJSON()
        data = self.alt_unity_driver.send_data(self.alt_unity_driver.create_command('pointerDownFromObject', alt_object ))
        return self.alt_unity_driver.handle_errors(data)

    def pointer_enter(self):
        alt_object = self.toJSON()
        data = self.alt_unity_driver.send_data(self.alt_unity_driver.create_command('pointerEnterObject', alt_object ))
        return self.alt_unity_driver.handle_errors(data)

    def pointer_exit(self):
        alt_object = self.toJSON()
        data = self.alt_unity_driver.send_data(self.alt_unity_driver.create_command('pointerExitObject', alt_object ))
        return self.alt_unity_driver.handle_errors(data)
    
    def tap(self):
        alt_object=self.toJSON()
        data=self.alt_unity_driver.send_data(self.alt_unity_driver.create_command('tapObject',alt_object))
        return self.alt_unity_driver.handle_errors(data)




class AltrunUnityDriver(object):

    def __init__(self, appium_driver,  platform, TCP_IP='127.0.0.1', TCP_FWD_PORT=13000, TCP_PORT=13000, timeout=60,requestSeparator=';',requestEnd='&',deviceID=""):
        self.TCP_PORT = TCP_PORT
        self.requestSeparator=requestSeparator
        self.requestEnd=requestEnd
        if (appium_driver != None):
            self.appium_driver = appium_driver
            if (platform != None):
                print('Starting tests on ' + platform)
                self.setup_port_forwarding(deviceID=deviceID,platform=platform, port=TCP_FWD_PORT)

        while (timeout > 0):
            try:
                self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                self.socket.connect((TCP_IP, TCP_FWD_PORT))
                self.get_current_scene()
                break
            except Exception as e:
                print(e)
                print('AltUnityServer not running on port ' + str(TCP_FWD_PORT) + ', retrying (timing out in ' + str(timeout) + ' secs)...')
                timeout -= 5
                time.sleep(5)

        if (timeout <= 0):
            raise Exception('AltUnityServer not running on port ' + str(TCP_FWD_PORT) + ', did you run ``adb forward tcp:' + str(TCP_FWD_PORT) + ' tcp:' + str(self.TCP_PORT) + '`` or ``iproxy ' + str(TCP_FWD_PORT) + ' ' + str(self.TCP_PORT) + '``?')

    def remove_port_forwarding(self, port):
        try:
            subprocess.Popen(['killall', 'iproxy']).wait()
            print('Removed iproxy forwarding')
        except:
            print('AltUnityServer - no iproxy process was running/present')
        try:
            subprocess.Popen(['adb', 'forward', '--remove-all']).wait()
            print('Removed adb forwarding')
        except:
            print('AltUnityServer - adb probably not installed ')

    def setup_port_forwarding(self,deviceID="", platform="android", port=13000):
        if (platform == "android"):
            try:
                if deviceID=="":
                    subprocess.Popen(['adb', 'forward', 'tcp:' + str(port), 'tcp:' + str(self.TCP_PORT)])
                else:
                    subprocess.Popen(['adb', 'forward','-s '+deviceID, 'tcp:' + str(port), 'tcp:' + str(self.TCP_PORT)])
            except:
                print('AltUnityServer - could not use port ' + str(port))
        if (platform == "ios"):
            try:
                if deviceID=="":
                    subprocess.Popen(['iproxy', str(port),str(self.TCP_PORT)])
                else:
                    subprocess.Popen(['iproxy', str(port),str(self.TCP_PORT),deviceID])
            except:
                print('AltUnityServer - could not use port ' + str(port))

    def stop(self):
        data = self.send_data(self.create_command('closeConnection'))
        print('Sent close connection command...')
        time.sleep(1)
        self.socket.close()
        print('Socket closed.')

    def recvall(self):
        data = ''
        previousPart=''
        while True:
            part = self.socket.recv(BUFFER_SIZE)
            data += str(part)
            partToSeeAltEnd=previousPart+str(part)
            if '::altend' in partToSeeAltEnd:
                break
            previousPart=str(part)
        try:
            data = data.split('altstart::')[1].split('::altend')[0]
        except:
            print('Data received from socket doesn not have correct start and end control strings')
            return ''
        print('Received data was: ' + data)
        return data

    def send_data(self, data):
        self.socket.send(data.encode('ascii'))
        if ('closeConnection' in data):
            return ''
        else:
            return self.recvall()

    def create_command(self,*arguments):
        command=''
        for argument in arguments:
            command+=str(argument)+self.requestSeparator
        command+=self.requestEnd
        return command
            

    def call_static_methods(self, type_name, method_name, parameters, type_of_parameters = '',assembly=''):
        action_info = '{"component":"' + type_name + '", "method":"' + method_name + '", "parameters":"' + parameters + '", "typeofparameters":"' + type_of_parameters +'", "assembly":"'+assembly+'"}'
        data=self.send_data(self.create_command("callComponentMethodForObject","",action_info))
        return self.handle_errors(data)
    
    def get_alt_element(self, data):
        print(data)
        if (data != '' and 'error:' not in data):
            alt_el = None
            try:
                alt_el = AltElement(self, self.appium_driver, data)
            except:
                alt_el = AltElement(self, None, data)
            print('Element ' + alt_el.name + ' found at x:' + str(alt_el.x) + ' y:' + str(alt_el.y) + ' mobileY:' + str(alt_el.mobileY))
            return alt_el
        self.handle_errors(data)
        return None
    
    def get_alt_elements(self, data):
        if (data != '' and 'error:' not in data):
            alt_elements = []
            elements = []
            try:
                elements = json.loads(data)
            except:
                raise Exception("Couldn't parse json data: " + data)
            
            alt_el = None
            for i in range(0, len(elements)):
                try:
                    alt_el = AltElement(self, self.appium_driver, json.dumps(elements[i]))
                except:
                    alt_el = AltElement(self, None, json.dumps(elements[i]))
                    
                alt_elements.append(alt_el)
                print('Element ' + alt_el.name + ' found at x:' + str(alt_el.x) + ' y:' + str(alt_el.y) + ' mobileY:' + str(alt_el.mobileY))
            return alt_elements
            
        self.handle_errors(data)
        return None

 
    def get_all_elements(self,camera_name='',enabled=True):
        if enabled==True:
            data = self.send_data(self.create_command('findAllObjects', camera_name ,'true'))
        else:
            data = self.send_data(self.create_command('findAllObjects', camera_name ,'false'))

        return self.get_alt_elements(data)

    def find_element(self, name,camera_name='',enabled=True):
        if enabled==True:
            data = self.send_data(self.create_command('findObjectByName', name , camera_name ,'true'))
        else:
            data = self.send_data(self.create_command('findObjectByName', name , camera_name ,'false'))

        return self.get_alt_element(data)

    def find_element_where_name_contains(self, name,camera_name='',enabled=True):
        if enabled==True:
            data = self.send_data(self.create_command('findObjectWhereNameContains', name , camera_name,'true' ))
        else:
            data = self.send_data(self.create_command('findObjectWhereNameContains', name , camera_name,'false' ))
        return self.get_alt_element(data)

    def find_elements(self, name,camera_name='',enabled=True):
        if enabled==True:
            data = self.send_data(self.create_command('findObjectsByName', name , camera_name ,'true'))
        else:
            data = self.send_data(self.create_command('findObjectsByName', name , camera_name ,'false'))
        return self.get_alt_elements(data)        

    def find_elements_where_name_contains(self, name,camera_name='',enabled=True):
        if enabled==True:
            data = self.send_data(self.create_command('findObjectsWhereNameContains', name , camera_name ,'true'))
        else:
            data = self.send_data(self.create_command('findObjectsWhereNameContains', name , camera_name ,'false'))
        return self.get_all_elements(data)

    def get_current_scene(self):
        data = self.send_data(self.create_command('getCurrentScene'))
        if (data != '' and 'error:' not in data):
            alt_el = self.get_alt_element(data)
            print('Current scene is ' + alt_el.name)
            return alt_el.name
        return self.handle_errors(data)

    def click_at_coordinates(self, x, y):
        data = self.send_data(self.create_command("clickScreenOnXY",x,y ))
        print('Clicked at ' + str(x) + ', ' + str(y))
        return data

    def swipe(self, x_start, y_start, x_end, y_end, duration_in_secs):
        start_position = self.vector_to_json_string(x_start, y_start)
        end_position = self.vector_to_json_string(x_end, y_end)
        print('Swipe from ' + start_position + ' to ' + end_position + ' with duration: ' + str(duration_in_secs) + ' secs')
        data = self.send_data(self.create_command('movingTouch', start_position , end_position , str(duration_in_secs) ))
        return self.handle_errors(data)


    def swipe_and_wait(self, x_start, y_start, x_end, y_end, duration_in_secs):
        data = self.swipe(x_start, y_start, x_end, y_end, duration_in_secs)
        self.handle_errors(data)
        print('Wait for swipe to finish')
        time.sleep(duration_in_secs)
        swipe_in_progress = True
        while swipe_in_progress:
            swipe_finished = self.send_data(self.create_command('swipeFinished'))
            self.handle_errors(swipe_finished)
            if swipe_finished is 'Yes':
                break
            elif swipe_finished != 'No':
                swipe_in_progress = False
        return self.handle_errors(data)

    def tilt(self, x, y, z):
        acceleration = self.vector_to_json_string(x, y, z)
        print ('Tilt with acceleration: ' + acceleration)
        data = self.send_data(self.create_command('tilt', acceleration ))
        return self.handle_errors(data)

    def set_player_pref_key(self, key_name, value, type):
        data = ''
        if type is 1:
            print('Set Int Player Pref for key: ' + key_name + ' to ' + str(value))
            data = self.send_data(self.create_command('setKeyPlayerPref', key_name , str(value) , str(PlayerPrefKeyType.Int) ))
        if type is 2:
            print('Set String Player Pref for key: ' + key_name + ' to ' + str(value))
            data = self.send_data(self.create_command('setKeyPlayerPref', key_name , str(value) , str(PlayerPrefKeyType.String) ))
        if type is 3:
            print('Set Float Player Pref for key: ' + key_name + ' to ' + str(value))
            data = self.send_data(self.create_command('setKeyPlayerPref', key_name , str(value) , str(PlayerPrefKeyType.Float) ))
        return self.handle_errors(data)

    def get_player_pref_key(self, key_name, type):
        data = ''
        if type is 1:
            print('Get Int Player Pref for key: ' + key_name)
            data = self.send_data(self.create_command('getKeyPlayerPref', key_name , str(PlayerPrefKeyType.Int) ))
        if type is 2:
            print('Get String Player Pref for key: ' + key_name)            
            data = self.send_data(self.create_command('getKeyPlayerPref', key_name , str(PlayerPrefKeyType.String) ))
        if type is 3:
            print('Get Float Player Pref for key: ' + key_name)            
            data = self.send_data(self.create_command('getKeyPlayerPref', key_name , str(PlayerPrefKeyType.Float) ))
        return self.handle_errors(data)

    
    def delete_player_pref_key(self, key_name):
        print('Delete Player Pref for key: ' + key_name)        
        data = self.send_data(self.create_command('deleteKeyPlayerPref', key_name ))
        return self.handle_errors(data)


    def delete_player_prefs(self):
        print('Delete all Player Prefs')
        data = self.send_data(self.create_command('deletePlayerPref'))
        return self.handle_errors(data)

    def load_scene(self, scene_name):
        data = self.send_data(self.create_command('loadScene', scene_name))
        if (data == 'Ok'):
            print('Scene loaded: ' + scene_name)
            return data
        return None

    def set_time_scale(self, time_scale):
        print('Set time scale to: ' + str(time_scale))
        data = self.send_data(self.create_command('setTimeScale', str(time_scale)))
        if (data == 'Ok'):
            print('Time scale set to: ' + str(time_scale))
            return data
        return None

    def get_time_scale(self):
        print('Get time scale')
        data = self.send_data(self.create_command('getTimeScale'))
        if (data != '' and 'error:' not in data):
            print('Got time scale: ' + data)
            return float(data)
        return None

    def wait_for_current_scene_to_be(self, scene_name, timeout=30, interval=1):
        t = 0
        current_scene = ''
        while (t <= timeout):
            print('Waiting for scene to be ' + scene_name + '...')
            current_scene = self.get_current_scene()
            if current_scene != scene_name:
                time.sleep(interval)
                t += interval
            else:
                break
        if t>=timeout:
            raise WaitTimeOutException('Scene ' + scene_name + ' not loaded after ' + str(timeout) + ' seconds')
        return current_scene

    def wait_for_element(self, name,camera_name='', timeout=20, interval=0.5,enabled=True):
        t = 0
        alt_element = None
        while (t <= timeout):
            try:
                alt_element = self.find_element(name,camera_name,enabled)
                break
            except Exception:
                print('Waiting for element ' + name + '...')
                time.sleep(interval)
                t += interval
        if t>=timeout:
            raise WaitTimeOutException('Element ' + name + ' not found after ' + str(timeout) + ' seconds')
        return alt_element


    def wait_for_element_where_name_contains(self, name,camera_name='', timeout=20, interval=0.5,enabled=True):
        t = 0
        alt_element = None
        while (t <= timeout):
            try:
                alt_element = self.find_element_where_name_contains(name,camera_name,enabled)
                break
            except Exception:
                print('Waiting for element where name contains ' + name + '...')
                time.sleep(interval)
                t += interval
        if t>=timeout:
            raise WaitTimeOutException('Element where name contains ' + name + ' not found after ' + str(timeout) + ' seconds')
        return alt_element
    
    def wait_for_element_to_not_be_present(self, name,camera_name='', timeout=20, interval=0.5,enabled=True):
        t = 0
        while (t <= timeout):
            try:
                print('Waiting for element ' + name + ' to not be present...')
                alt_element=self.find_element(name,camera_name,enabled)
                time.sleep(interval)
                t += interval
            except Exception:
                break
        if t>=timeout:
            raise WaitTimeOutException('Element ' + name + ' still found after ' + str(timeout) + ' seconds')

    def wait_for_element_with_text(self, name, text,camera_name='', timeout=20, interval=0.5,enabled=True):
        t = 0
        alt_element = None
        while (t <= timeout):
            try:
                alt_element = self.find_element(name,camera_name,enabled)
                if alt_element.get_text() == text:
                    break
                raise Exception('Not the wanted text')
            except Exception:
                print('Waiting for element ' + name + ' to have text ' + text)
                time.sleep(interval)
                t += interval
        if t>=timeout:
            raise WaitTimeOutException('Element ' + name + ' should have text `' + text + '` but has `' + alt_element.get_text() + '` after ' + str(timeout) + ' seconds')
        return alt_element

    def find_element_by_component(self, component_name,assembly_name='',camera_name='',enabled=True):
        if enabled==True:
            data = self.send_data(self.create_command('findObjectByComponent',assembly_name, component_name , camera_name ,'true'))
        else:
            data = self.send_data(self.create_command('findObjectByComponent',assembly_name, component_name , camera_name ,'false'))
        return self.get_alt_element(data)

    def find_elements_by_component(self, component_name,assembly_name='',camera_name='',enabled=True):
        if enabled==True:
            data = self.send_data(self.create_command('findObjectsByComponent',assembly_name, component_name , camera_name ,'true'))
        else:
            data = self.send_data(self.create_command('findObjectsByComponent',assembly_name, component_name , camera_name ,'false'))
        return self.get_alt_elements(data)

    def vector_to_json_string(self, x, y, z=None):
        if z is None:
            return '{"x":' + str(x) + ', "y":' + str(y) + '}'
        else:
            return '{"x":' + str(x) + ', "y":' + str(y) +', "z":' + str(z) + '}'



    def handle_errors(self, data):
        if ('error' in data):
            if ('error:notFound' in data):
                raise  NotFoundException(data)
            elif ('error:propertyNotFound' in data): 
                raise  PropertyNotFoundException(data)
            elif ('error:methodNotFound' in data): 
                raise  MethodNotFoundException(data)
            elif ('error:componentNotFound' in data): 
                raise  ComponentNotFoundException(data)
            elif ('error:couldNotPerformOperation' in data): 
                raise  CouldNotPerformOperationException(data)
            elif ('error:couldNotParseJsonString' in data): 
                raise  CouldNotParseJsonStringException(data)
            elif ('error:incorrectNumberOfParameters' in data): 
                raise  IncorrectNumberOfParametersException(data)
            elif ('error:failedToParseMethodArguments' in data): 
                raise  FailedToParseArgumentsException(data)
            elif ('error:objectNotFound' in data): 
                raise  ObjectWasNotFoundException(data)
            elif ('error:propertyCannotBeSet' in data): 
                raise  PropertyNotFoundException(data)
            elif ('error:nullRefferenceException' in data): 
                raise  NullRefferenceException(data)
            elif ('error:unknownError' in data): 
                raise  UnknownErrorException(data)
            elif ('error:formatException' in data): 
                raise  FormatException(data)
        else:
            return data
        

    def tap_at_coordinates(self,x,y):
        data=self.send_data(self.create_command('tapScreen',x,y))
        if 'error:notFound' in data:
            return None
        return self.get_alt_element(data)
