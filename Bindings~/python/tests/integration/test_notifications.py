"""
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
"""

import pytest

from .utils import Scenes
from alttester import By
from alttester.commands.Notifications.notification_type import NotificationType
from alttester.commands.Notifications.base_notification_callbacks import BaseNotificationCallbacks
from alttester.logging import AltLogLevel


class MockNotificationCallbacks(BaseNotificationCallbacks):
    last_scene_loaded = ""
    last_scene_unloaded = ""
    log_message = ""
    log_type = AltLogLevel.Error
    log_stack_trace = ""
    application_paused = False

    def scene_loaded_callback(self, load_scene_notification_result):
        self.last_scene_loaded = load_scene_notification_result.scene_name

    def scene_unloaded_callback(self, scene_name):
        self.last_scene_unloaded = scene_name

    def log_callback(self, log_notification_result):
        self.log_message = log_notification_result.message
        self.log_stack_trace = log_notification_result.stack_trace
        self.log_type = log_notification_result.type

    def application_paused_callback(self, application_paused):
        self.application_paused = application_paused


class TestNotifications:

    @pytest.fixture(autouse=True)
    def setup(self, alt_driver):
        self.alt_driver = alt_driver
        self.alt_driver.reset_input()

    def test_load_scene_notification(self):
        test_notification_callbacks = MockNotificationCallbacks()
        self.alt_driver.add_notification_listener(
            NotificationType.LOADSCENE, test_notification_callbacks.scene_loaded_callback)
        self.alt_driver.load_scene(Scenes.Scene01)
        assert test_notification_callbacks.last_scene_loaded == Scenes.Scene01
        self.alt_driver.remove_notification_listener(
            NotificationType.LOADSCENE)

    def test_unload_scene_notification(self):
        test_notification_callbacks = MockNotificationCallbacks()
        self.alt_driver.add_notification_listener(
            NotificationType.UNLOADSCENE, test_notification_callbacks.scene_unloaded_callback)
        self.alt_driver.load_scene(Scenes.Scene01)
        self.alt_driver.load_scene(Scenes.Scene02, load_single=False)
        self.alt_driver.unload_scene(Scenes.Scene02)
        assert test_notification_callbacks.last_scene_unloaded == Scenes.Scene02
        self.alt_driver.remove_notification_listener(
            NotificationType.UNLOADSCENE)

    def test_log_notification(self):
        test_notification_callbacks = MockNotificationCallbacks()
        self.alt_driver.add_notification_listener(
            NotificationType.LOG, test_notification_callbacks.log_callback)
        self.alt_driver.load_scene(Scenes.Scene01)
        assert "\"commandName\":\"loadScene" in test_notification_callbacks.log_message
        assert test_notification_callbacks.log_type == AltLogLevel.Debug.value
        self.alt_driver.remove_notification_listener(NotificationType.LOG)

    def test_application_paused_notification(self):
        test_notification_callbacks = MockNotificationCallbacks()
        self.alt_driver.add_notification_listener(
            NotificationType.APPLICATION_PAUSED, test_notification_callbacks.application_paused_callback)
        self.alt_driver.load_scene(Scenes.Scene01)
        alt_object = self.alt_driver.find_object(By.NAME, "AltTesterPrefab")
        alt_object.call_component_method(
            "AltTester.AltTesterUnitySDK.Commands.AltRunner", "OnApplicationPause", "AltTester.AltTesterUnitySDK",
            parameters=[True],
            type_of_parameters=["System.Boolean"]
        )

        assert test_notification_callbacks.application_paused
        self.alt_driver.remove_notification_listener(
            NotificationType.APPLICATION_PAUSED)
