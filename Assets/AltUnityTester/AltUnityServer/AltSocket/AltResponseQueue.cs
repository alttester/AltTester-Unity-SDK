
public delegate void SendResponse();

public class AltResponseQueue {

    private System.Collections.Generic.Queue<SendResponse> _responseQueue = new System.Collections.Generic.Queue<SendResponse>();
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
