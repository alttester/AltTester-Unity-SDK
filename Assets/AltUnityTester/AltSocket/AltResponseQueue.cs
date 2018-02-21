using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SendResponse();

public class AltResponseQueue {

    private Queue<SendResponse> ResponseQueue = new Queue<SendResponse>();
    private object _queueLock = new object();

    public void Cycle() {
        lock (_queueLock) {
            if (ResponseQueue.Count > 0)
                ResponseQueue.Dequeue()();
        }
    }

    public void ScheduleResponse(SendResponse newResponse) {
        lock (_queueLock) {
            if (ResponseQueue.Count < 100)
                ResponseQueue.Enqueue(newResponse);
        }
    }
}
