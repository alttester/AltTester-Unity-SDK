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

import pytest
from alttester.altby import AltBy
from .utils import Scenes
from alttester.altdriver_unity import AltDriverUnity


@pytest.mark.usefixtures("alt_driver_unity")
class TestFindObjectsWithUnityDriver:
    alt_driver: AltDriverUnity

    @pytest.fixture(autouse=True)
    def setup(self):
        self.alt_driver.reset_input()
        self.alt_driver.load_scene(Scenes.Scene01)

    def test_find_object_with_unity_driver(self):
        capsule = self.alt_driver.find_object(AltBy.name("CapsuleInfo"))
        self.alt_driver.find_object(AltBy.path("//Capsule"), AltBy.name(""))
        self.alt_driver.find_object(AltBy.path("//Capsule"), AltBy.name(""), True)
        self.alt_driver.find_object(AltBy.tag("plane"))
        self.alt_driver.find_object(AltBy.layer("Water"))
        capsule_text = capsule.get_text()
        self.alt_driver.find_object(AltBy.text(capsule_text))
        self.alt_driver.find_object(
            AltBy.name("CapsuleInfo"), AltBy.name(""), enabled=True
        )
        self.alt_driver.find_object(
            AltBy.name("CapsuleInfo"), AltBy.name(""), enabled=False
        )

    def test_find_objects_with_unity_driver(self):
        objects = self.alt_driver.find_objects(AltBy.name("Capsule"))
        assert len(objects) > 0
        objects = self.alt_driver.find_objects(
            AltBy.name("Capsule"), AltBy.name(""), enabled=True
        )
        assert len(objects) > 0
        objects = self.alt_driver.find_objects(
            AltBy.name("Capsule"), AltBy.name(""), enabled=False
        )
        assert len(objects) > 0

    def test_find_object_which_contains_with_unity_driver(self):
        self.alt_driver.find_object_which_contains(AltBy.name("Capsule"))
        self.alt_driver.find_object_which_contains(
            AltBy.name("Capsule"), AltBy.name(""), enabled=True
        )
        self.alt_driver.find_object_which_contains(
            AltBy.name("Capsule"), AltBy.name(""), enabled=False
        )

    def test_find_objects_which_contain_with_unity_driver(self):
        objects = self.alt_driver.find_objects_which_contain(AltBy.name("Capsule"))
        assert len(objects) > 0
        objects = self.alt_driver.find_objects_which_contain(
            AltBy.name("Capsule"), AltBy.name(""), enabled=True
        )
        assert len(objects) > 0
        objects = self.alt_driver.find_objects_which_contain(
            AltBy.name("Capsule"), AltBy.name(""), enabled=False
        )
        assert len(objects) > 0

    def test_wait_for_object_with_unity_driver(self):
        self.alt_driver.wait_for_object(AltBy.name("Capsule"))
        self.alt_driver.wait_for_object(
            AltBy.name("Capsule"), AltBy.name(""), enabled=True
        )

    def test_wait_for_object_which_contains_with_unity_driver(self):
        self.alt_driver.wait_for_object_which_contains(AltBy.name("Capsule"))
        self.alt_driver.wait_for_object_which_contains(
            AltBy.name("Capsule"), AltBy.name(""), enabled=True
        )

    def test_wait_for_object_to_not_be_present_with_unity_driver(self):
        self.alt_driver.wait_for_object_to_not_be_present(
            AltBy.name("NonExistentObject"), timeout=1
        )
        self.alt_driver.wait_for_object_to_not_be_present(
            AltBy.name("NonExistentObject"), AltBy.name(""), timeout=1, enabled=True
        )
        self.alt_driver.wait_for_object_to_not_be_present(
            AltBy.name("NonExistentObject"), AltBy.name(""), timeout=1, enabled=False
        )