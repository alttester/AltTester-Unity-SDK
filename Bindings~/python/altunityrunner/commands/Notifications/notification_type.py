from enum import IntEnum


class NotificationType(IntEnum):
    LOADSCENE = 0
    UNLOADSCENE = 1
    LOG = 2,
    APPLICATION_PAUSED = 3
