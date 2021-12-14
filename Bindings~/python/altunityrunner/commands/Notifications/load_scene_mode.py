from enum import Enum, unique


@unique
class LoadSceneMode(Enum):
    Single = 0
    Additive = 1

    @classmethod
    def values(cls):
        return [scene_mode.value for scene_mode in cls]
