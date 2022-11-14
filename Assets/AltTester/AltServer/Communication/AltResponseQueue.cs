using System.Collections.Generic;
using Altom.AltTester.Logging;

namespace Altom.AltTester.Communication
{
    public delegate void SendResponse();

    public class AltResponseQueue
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        private readonly Queue<SendResponse> responseQueue = new Queue<SendResponse>();
        private readonly object queueLock = new object();

        public void Cycle()
        {
            // logger.Debug("Cycle - Count: " + responseQueue.Count);

            if (responseQueue.Count == 0) return;

            lock (queueLock)
            {
                if (responseQueue.Count > 0)
                {
                    responseQueue.Dequeue()();
                }
            }
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

        public void Clear() {
            lock (queueLock)
            {
                responseQueue.Clear();
            }
        }
    }
}
