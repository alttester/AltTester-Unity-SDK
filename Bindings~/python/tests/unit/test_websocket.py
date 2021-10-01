import unittest.mock as mock

import pytest

from altunityrunner._websocket import Store


class TestStore:

    @pytest.fixture(autouse=True)
    def setup(self):
        self.store = Store()

    def test_has(self):
        assert not self.store.has("key")

        self.store.push("key", mock.sentinel.value)

        assert self.store.has("key")

    def test_push(self):
        assert not self.store.has("key")

        self.store.push("key", mock.sentinel.value)

        assert self.store.has("key")
        assert self.store.pop("key") == mock.sentinel.value
        assert not self.store.has("key")

    def test_pop(self):
        assert not self.store.has("key")
        assert self.store.pop("key") is None

        self.store.push("key", mock.sentinel.first)
        self.store.push("key", mock.sentinel.second)

        assert self.store.has("key")
        assert self.store.pop("key") == mock.sentinel.first
        assert self.store.has("key")
        assert self.store.pop("key") == mock.sentinel.second
        assert not self.store.has("key")
        assert self.store.pop("key") is None

    def test_multiple_keys(self):
        assert not self.store.has("a")
        assert self.store.pop("a") is None

        assert not self.store.has("b")
        assert self.store.pop("b") is None

        self.store.push("a", mock.sentinel.first)
        self.store.push("b", mock.sentinel.second)
        self.store.push("a", mock.sentinel.third)

        assert self.store.has("a")
        assert self.store.pop("a") == mock.sentinel.first
        assert self.store.has("b")
        assert self.store.pop("b") == mock.sentinel.second
        assert self.store.has("a")
        assert self.store.pop("a") == mock.sentinel.third

        assert not self.store.has("a")
        assert self.store.pop("a") is None
        assert not self.store.has("a")
        assert self.store.pop("a") is None