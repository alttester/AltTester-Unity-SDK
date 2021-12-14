from altunityrunner.commands.Notifications.base_notification_callbacks import BaseNotificationCallbacks
from loguru import logger


class TestNotificationCallback(BaseNotificationCallbacks):
    last_scene_loaded = ""

    def scene_loaded_callback(self, load_scene_notification_result):
        self.last_scene_loaded = load_scene_notification_result.scene_name
