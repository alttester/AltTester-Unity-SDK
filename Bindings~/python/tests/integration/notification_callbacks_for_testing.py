from altunityrunner.commands.Notifications.base_notification_callbacks import BaseNotificationCallbacks
from loguru import logger


class TestNotificationCallback(BaseNotificationCallbacks):
    last_scene_loaded = ""
    last_scene_unloaded = ""
    application_paused = False

    def scene_loaded_callback(self, load_scene_notification_result):
        self.last_scene_loaded = load_scene_notification_result.scene_name

    def scene_unloaded_callback(self, scene_name):
        self.last_scene_unloaded = scene_name

    def application_paused_callback(self, application_paused):
        self.application_paused = application_paused
