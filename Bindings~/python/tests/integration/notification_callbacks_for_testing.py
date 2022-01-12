from altunityrunner.commands.Notifications.base_notification_callbacks import BaseNotificationCallbacks
from altunityrunner.logging import AltUnityLogLevel


class TestNotificationCallback(BaseNotificationCallbacks):
    last_scene_loaded = ""
    last_scene_unloaded = ""
    log_message = ""
    log_type = AltUnityLogLevel.Error
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
