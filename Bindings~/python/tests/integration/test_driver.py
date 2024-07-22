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

import os

import pytest

from .utils import Scenes
from alttester import By
from alttester.__version__ import VERSION
from alttester.commands import GetServerVersion
from alttester.logging import AltLogLevel, AltLogger
import alttester.exceptions as exceptions


class TestDriver:

    @pytest.fixture(autouse=True)
    def setup(self, alt_driver):
        self.alt_driver = alt_driver

    def test_get_version(self):
        server_version = GetServerVersion.run(self.alt_driver._connection)
        assert VERSION.startswith(server_version)

    def test_load_and_wait_for_scene(self):
        self.alt_driver.load_scene(Scenes.Scene01)
        self.alt_driver.wait_for_current_scene_to_be(Scenes.Scene01, timeout=1)

        self.alt_driver.load_scene(Scenes.Scene02)
        self.alt_driver.wait_for_current_scene_to_be(Scenes.Scene02, timeout=1)

        assert self.alt_driver.get_current_scene() == Scenes.Scene02

    def test_wait_for_current_scene_to_be_with_a_non_existing_scene(self):
        scene_name = "Scene 0"

        with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            self.alt_driver.wait_for_current_scene_to_be(
                scene_name, timeout=1, interval=0.5)

        assert str(execinfo.value) == "Scene {} not loaded after 1 seconds".format(
            scene_name)

    def test_set_and_get_time_scale(self):
        self.alt_driver.set_time_scale(0.1)

        assert self.alt_driver.get_time_scale() == 0.1

        self.alt_driver.set_time_scale(1)

    def test_screenshot(self):
        png_path = "testPython.png"
        self.alt_driver.get_png_screenshot(png_path)
        assert os.path.exists(png_path)

    def test_wait_for_object_which_contains_with_tag(self):
        alt_object = self.alt_driver.wait_for_object_which_contains(
            By.NAME, "Canva",
            By.TAG, "MainCamera"
        )
        assert alt_object.name == "Canvas"

    def test_load_additive_scenes(self):
        self.alt_driver.load_scene(Scenes.Scene01, load_single=True)

        initial_number_of_elements = self.alt_driver.get_all_elements()
        self.alt_driver.load_scene(Scenes.Scene02, load_single=False)
        final_number_of_elements = self.alt_driver.get_all_elements()

        assert len(final_number_of_elements) > len(initial_number_of_elements)

        scenes = self.alt_driver.get_all_loaded_scenes()
        assert len(scenes) == 2

    def test_load_scene_with_invalid_scene_name(self):
        with pytest.raises(exceptions.SceneNotFoundException):
            self.alt_driver.load_scene("Scene 0")

    def test_unload_scene(self):
        self.alt_driver.load_scene(Scenes.Scene01, load_single=True)
        self.alt_driver.load_scene(Scenes.Scene02, load_single=False)

        assert len(self.alt_driver.get_all_loaded_scenes()) == 2

        self.alt_driver.unload_scene(Scenes.Scene02)
        assert len(self.alt_driver.get_all_loaded_scenes()) == 1
        assert self.alt_driver.get_all_loaded_scenes()[0] == Scenes.Scene01

    def test_unload_only_scene(self):
        self.alt_driver.load_scene(Scenes.Scene01, load_single=True)

        with pytest.raises(exceptions.CouldNotPerformOperationException):
            self.alt_driver.unload_scene(Scenes.Scene01)

    @pytest.mark.WebGLUnsupported
    def test_set_server_logging(self):
        rule = self.alt_driver.call_static_method(
            "AltTester.AltTesterUnitySDK.Logging.ServerLogManager",
            "Instance.Configuration.FindRuleByName",
            "Assembly-CSharp",
            parameters=["AltServerFileRule"],
        )

        # Default logging level in AltTester® is Debug level
        assert len(rule["Levels"]) == 5

        self.alt_driver.set_server_logging(AltLogger.File, AltLogLevel.Off)
        rule = self.alt_driver.call_static_method(
            "AltTester.AltTesterUnitySDK.Logging.ServerLogManager",
            "Instance.Configuration.FindRuleByName",
            "Assembly-CSharp",
            parameters=["AltServerFileRule"],
        )

        assert len(rule["Levels"]) == 0

        # Reset logging level
        self.alt_driver.set_server_logging(AltLogger.File, AltLogLevel.Debug)

    @pytest.mark.parametrize(
        "path", ["//[1]", "CapsuleInfo[@tag=UI]",
                 "//CapsuleInfo[@tag=UI/Text", "//CapsuleInfo[0/Text"]
    )
    def test_invalid_paths(self, path):
        with pytest.raises(exceptions.InvalidPathException):
            self.alt_driver.find_object(By.PATH, path)
