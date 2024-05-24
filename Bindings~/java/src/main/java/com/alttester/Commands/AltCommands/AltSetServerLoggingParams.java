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
