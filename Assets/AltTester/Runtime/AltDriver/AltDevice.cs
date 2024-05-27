/*
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
*/

namespace AltTester.AltTesterUnitySDK.Driver
{
    public class AltDevice
    {
        public string DeviceId { get; set; }
        public int LocalPort { get; set; }
        public int RemotePort { get; set; }
        public bool Active { get; set; }
        public string Platform { get; set; }
        public int Pid { get; set; }

        public AltDevice(string deviceId, string platform, int localPort = 13000, int remotePort = 13000, bool active = false, int pid = 0)
        {
            DeviceId = deviceId;
            LocalPort = localPort;
            RemotePort = remotePort;
            Active = active;
            Platform = platform;
            Pid = pid;
        }
    }
}
