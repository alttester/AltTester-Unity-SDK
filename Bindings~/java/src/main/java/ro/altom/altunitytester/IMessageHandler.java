package ro.altom.altunitytester;

public interface IMessageHandler {

    public <T> AltMessageResponse<T> receive(AltMessage altMessage, Class<T> type);

    public void send(AltMessage altMessage);

    public void onMessage(String message);
}