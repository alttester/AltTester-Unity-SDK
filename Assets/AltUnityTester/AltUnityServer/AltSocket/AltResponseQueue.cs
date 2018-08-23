using System.Collections.Generic;

public delegate void SendResponse();

public class AltResponseQueue {

    private Queue<SendResponse> _responseQueue = new Queue<SendResponse>();
    private readonly object _queueLock = new object();

    public void Cycle() {
        lock (_queueLock) {
            if (_responseQueue.Count > 0)
                _responseQueue.Dequeue()();
        }
    }

    public void ScheduleResponse(SendResponse newResponse) {
        lock (_queueLock) {
            if (_responseQueue.Count < 100)
                _responseQueue.Enqueue(newResponse);
        }
    }
}
