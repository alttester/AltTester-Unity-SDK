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

from alttester.by import By


class AltBy:
    """The AltBy class is used to find elements in the scene using By locators strategy and value."""

    def __init__(self, by=None, value=None):
        self.by = by
        self.value = value

    def __str__(self):
        return f"By.{self.by}({self.value})"

    def __repr__(self):
        return f"By.{self.by}({self.value})"

    def __eq__(self, other):
        return self.by == other.by and self.value == other.value

    @classmethod
    def name(cls, name):
        return cls(by=By.NAME, value=name)

    @classmethod
    def id(cls, id):
        return cls(by=By.ID, value=id)

    @classmethod
    def path(cls, path):
        return cls(by=By.PATH, value=path)

    @classmethod
    def tag(cls, tag):
        return cls(by=By.TAG, value=tag)

    @classmethod
    def layer(cls, layer):
        return cls(by=By.LAYER, value=layer)

    @classmethod
    def text(cls, text):
        return cls(by=By.TEXT, value=text)

    @classmethod
    def component(cls, component):
        return cls(by=By.COMPONENT, value=component)
