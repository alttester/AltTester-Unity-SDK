from enum import Enum, unique


@unique
class PlayerPrefKeyType(Enum):
    Int = 1
    String = 2
    Float = 3

    def __str__(self):
        return str(self.value)

    @classmethod
    def values(cls):
        return [pref_type.value for pref_type in cls]
