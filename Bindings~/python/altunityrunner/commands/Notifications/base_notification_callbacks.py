from loguru import logger


class BaseNotificationCallbacks():
    def scene_loaded_callback(self, load_scene_notification_result):
        logger.debug("Scene {0} was loaded {1}".format(str(load_scene_notification_result.scene_name),
                                                       str(load_scene_notification_result.loadSceneMode)))
