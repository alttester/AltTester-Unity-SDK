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

import json
import unittest.mock as mock

import pytest

from alttester._websocket import Store, WebsocketConnection, CommandHandler, NotificationHandler
from alttester.exceptions import ConnectionError


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


class TestWebsocketConnection:

    @pytest.fixture(autouse=True)
    def setup(self):
        self.host = "127.0.0.1"
        self.port = 1300
        self.timeout = 5

        self.websocket_mock = mock.Mock()
        self.create_connection_mock = mock.Mock(
            return_value=self.websocket_mock
        )

        self.connection = WebsocketConnection(
            host=self.host,
            port=self.port,
            timeout=self.timeout,
            command_handler=CommandHandler(),
            notification_handler=NotificationHandler()
        )
        self.connection._create_connection = self.create_connection_mock
        self.connection._is_open = True

    def test_connect(self):
        self.connection.connect()
        self.create_connection_mock.assert_called_once()

        self.websocket_mock.close.assert_not_called()

    def test_on_open(self):
        self.connection._is_open = False
        self.connection._on_open(self.websocket_mock)

        assert self.connection._is_open

    def test_on_message(self):
        command = {
            "messageId": "0",
            "commandName": "TestCommand"
        }

        self.connection._command_handler.set_current_command(command)
        assert not self.connection._command_handler.has_response()

        self.connection._on_message(
            self.connection._websocket, json.dumps(command))

        assert self.connection._command_handler.has_response()
        assert self.connection._command_handler.get_response() == command

    def test_on_error(self):
        assert not self.connection._errors

        error_message = "Error message."
        self.connection._on_error(self.connection._websocket, error_message)

        assert self.connection._errors.pop() == error_message

    def test_on_close(self):
        self.connection._is_open = True
        self.connection._on_close(self.websocket_mock, None, None)

        assert not self.connection._is_open
        assert self.connection._websocket is None

    def test_recv(self):
        command = {
            "messageId": "0",
            "commandName": "TestCommand"
        }
        self.connection.connect()
        self.connection._websocket = self.websocket_mock
        self.connection._command_handler.set_current_command(command)
        self.connection._command_handler.handle_command(command)

        response = self.connection.recv()

        assert response == command

    def test_recv_with_closed_connection(self):
        self.connection._is_open = False

        with pytest.raises(ConnectionError):
            self.connection.recv()

    def test_send(self):
        self.connection.connect()
        self.connection._websocket = self.websocket_mock

        command = {
            "messageId": "0",
            "commandName": "TestCommand"
        }
        self.connection.send(command)

        assert self.connection._command_handler.get_current_command() == (
            command["messageId"], command["commandName"])

    def test_send_with_close_connection(self):
        self.connection._is_open = False

        command = {
            "messageId": "0",
            "commandName": "TestCommand"
        }

        with pytest.raises(ConnectionError):
            self.connection.send(command)

        assert self.connection._command_handler.get_current_command() != (
            command["messageId"], command["commandName"])
