package com.alttester.Commands.AltCommands;

import com.alttester.AltMessage;
import com.alttester.Logging.AltLogLevel;
import com.alttester.Logging.AltLogger;

public class AltSetServerLoggingParams extends AltMessage {

    public static class Builder {
        private AltLogger logger;
        private AltLogLevel logLevel;

        public Builder(AltLogger logger, AltLogLevel logLevel) {
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

    private AltLogger logger;
    private AltLogLevel logLevel;

    public AltLogger getLogger() {
        return logger;
    }

    public void setLogger(AltLogger logger) {
        this.logger = logger;
    }

    public AltLogLevel getLogLevel() {
        return logLevel;
    }

    public void setLogLevel(AltLogLevel logLevel) {
        this.logLevel = logLevel;
    }
}
