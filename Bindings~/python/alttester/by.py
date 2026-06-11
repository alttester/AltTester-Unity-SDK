"""
    Copyright(C) 2026 Altom Consulting
"""

from enum import Enum, unique

"""The By implementation."""


@unique
class By(Enum):
    """Set of supported locator strategies."""

    NAME = 1
    TAG = 2
    LAYER = 3
    COMPONENT = 4
    ID = 5
    PATH = 6
    TEXT = 7

    def __str__(self):
        return self.name
