"""
    Copyright(C) 2024 Altom Consulting
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

import subprocess

from ppadb.client import Client as AdbClient


class AltReversePortForwarding:
    _client = AdbClient(host="127.0.0.1", port=5037)

    @classmethod
    def _get_device(cls, device_id=""):
        if device_id == "":
            devices = cls._client.devices()
            if len(devices) == 0:
                raise Exception("No device found")
            return devices[0]
        else:
            return cls._client.device(device_id)

    @staticmethod
    def reverse_port_forwarding_android(device_port=13000, local_port=13000, device_Id=""):
        command = ['adb']

        if device_Id:
            command.extend(['-s', device_Id])

        command.extend(['reverse', f'tcp:{device_port}', f'tcp:{local_port}'])

        subprocess.Popen(command).wait()

    @staticmethod
    def remove_reverse_port_forwarding_android(device_port=13000, device_Id=""):
        command = ['adb']

        if device_Id:
            command.extend(['-s', device_Id])

        command.extend(['reverse', '--remove', f'tcp:{device_port}'])

        subprocess.Popen(command).wait()

    @staticmethod
    def remove_all_reverse_port_forwardings_android():
        subprocess.Popen(['adb', 'reverse', '--remove-all']).wait()
