package ro.altom.altunitytester;

public class AltMessageResponse<T> {

    public String logs;
    public String messageId;
    public String commandName;
    public T data;
    public CommandError error;
}
