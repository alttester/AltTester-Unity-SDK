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

using System.Collections.Generic;

namespace AltTester.AltTesterUnitySDK.Driver
{
    public delegate void SendResponse();

    public class AltResponseQueue
    {
        private readonly Queue<SendResponse> responseQueue = new Queue<SendResponse>();
        private readonly object queueLock = new object();

        public void Cycle()
        {
            if (responseQueue.Count == 0)
            {
                return;
            }

            lock (queueLock)
            {
                if (responseQueue.Count > 0)
                {
                    responseQueue.Dequeue()();
                }
            }
        }
        public int GetCount()
        {
            return responseQueue.Count;
        }

        public void ScheduleResponse(SendResponse newResponse)
        {
            lock (queueLock)
            {
                if (responseQueue.Count < 100)
                {
                    responseQueue.Enqueue(newResponse);
                }
            }
        }

        public void Clear()
        {
            lock (queueLock)
            {
                responseQueue.Clear();
            }
        }
    }
}
