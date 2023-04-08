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
    def setup(self, altdriver):
        self.altdriver = altdriver
        self.altdriver.reset_input()

    def test_load_scene_notification(self):
        test_notification_callbacks = MockNotificationCallbacks()
        self.altdriver.add_notification_listener(
            NotificationType.LOADSCENE, test_notification_callbacks.scene_loaded_callback)
        self.altdriver.load_scene(Scenes.Scene01)
        assert test_notification_callbacks.last_scene_loaded == Scenes.Scene01
        self.altdriver.remove_notification_listener(NotificationType.LOADSCENE)

    def test_unload_scene_notification(self):
        test_notification_callbacks = MockNotificationCallbacks()
        self.altdriver.add_notification_listener(
            NotificationType.UNLOADSCENE, test_notification_callbacks.scene_unloaded_callback)
        self.altdriver.load_scene(Scenes.Scene01)
        self.altdriver.load_scene(Scenes.Scene02, load_single=False)
        self.altdriver.unload_scene(Scenes.Scene02)
        assert test_notification_callbacks.last_scene_unloaded == Scenes.Scene02
        self.altdriver.remove_notification_listener(NotificationType.UNLOADSCENE)

    def test_log_notification(self):
        test_notification_callbacks = MockNotificationCallbacks()
        self.altdriver.add_notification_listener(
            NotificationType.LOG, test_notification_callbacks.log_callback)
        self.altdriver.load_scene(Scenes.Scene01)
        assert "Scene Loaded" in test_notification_callbacks.log_message
        assert test_notification_callbacks.log_type == AltLogLevel.Debug.value
        self.altdriver.remove_notification_listener(NotificationType.LOG)

    def test_application_paused_notification(self):
        test_notification_callbacks = MockNotificationCallbacks()
        self.altdriver.add_notification_listener(
            NotificationType.APPLICATION_PAUSED, test_notification_callbacks.application_paused_callback)
        self.altdriver.load_scene(Scenes.Scene01)
        alt_object = self.altdriver.find_object(By.NAME, "AltTesterPrefab")
        alt_object.call_component_method(
            "AltTester.AltTesterUnitySDK.AltRunner", "OnApplicationPause", "AltTester.AltTesterUnitySDK",
            parameters=[True],
            type_of_parameters=["System.Boolean"]
        )

        assert test_notification_callbacks.application_paused
        self.altdriver.remove_notification_listener(NotificationType.APPLICATION_PAUSED)
