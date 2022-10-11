from loguru import logger


class BaseNotificationCallbacks():
    def scene_loaded_callback(self, load_scene_notification_result):
        logger.debug("Scene {0} was loaded {1}".format(str(load_scene_notification_result.scene_name),
                                                       str(load_scene_notification_result.loadSceneMode)))

    def scene_unloaded_callback(self, scene_name):
        logger.debug("Scene {0} was unloaded".format(scene_name))

    def log_callback(self, log_notification_result):
        logger.debug("Log of type {0} with message {1} was received".format(str(log_notification_result.type),
                                                                            str(log_notification_result.message)))

    def application_paused_callback(self, application_paused):
        logger.debug("Application paused: {0}".format(application_paused))
