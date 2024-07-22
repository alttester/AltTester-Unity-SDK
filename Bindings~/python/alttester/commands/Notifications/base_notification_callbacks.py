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
