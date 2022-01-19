package ro.altom.altunitytester.Commands.AltUnityCommands;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.Logging.AltUnityLogLevel;
import ro.altom.altunitytester.Logging.AltUnityLogger;

public class AltSetServerLoggingParams extends AltMessage {

    public static class Builder {
        private AltUnityLogger logger;
        private AltUnityLogLevel logLevel;

        public Builder(AltUnityLogger logger, AltUnityLogLevel logLevel) {
            this.logger = logger;
            this.logLevel = logLevel;
        }

        public AltSetServerLoggingParams build() {
            AltSetServerLoggingParams setServerLoggingParameters = new AltSetServerLoggingParams();
            setServerLoggingParameters.logger = this.logger;
            setServerLoggingParameters.logLevel = this.logLevel;
            return setServerLoggingParameters;
        }
    }

    private AltSetServerLoggingParams() {
        this.setCommandName("setServerLogging");
    }

    private AltUnityLogger logger;
    private AltUnityLogLevel logLevel;

    public AltUnityLogger getLogger() {
        return logger;
    }

    public void setLogger(AltUnityLogger logger) {
        this.logger = logger;
    }

    public AltUnityLogLevel getLogLevel() {
        return logLevel;
    }

    public void setLogLevel(AltUnityLogLevel logLevel) {
        this.logLevel = logLevel;
    }
}
