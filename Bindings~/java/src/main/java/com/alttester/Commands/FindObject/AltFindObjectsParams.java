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

package com.alttester.Commands.FindObject;

import com.alttester.AltMessage;
import com.alttester.AltDriver;
import com.alttester.AltDriver.By;

public class AltFindObjectsParams extends AltMessage {

    public static class Builder {
        private By by;
        private String value;
        private By cameraBy = By.NAME;
        private String cameraValue = "";
        private boolean enabled = true;

        public Builder(AltDriver.By by, String value) {
            this.by = by;
            this.value = value;
        }

        public AltFindObjectsParams.Builder isEnabled(boolean enabled) {
            this.enabled = enabled;
            return this;
        }

        public AltFindObjectsParams.Builder withCamera(By cameraBy, String cameraValue) {
            this.cameraBy = cameraBy;
            this.cameraValue = cameraValue;
            return this;
        }

        public AltFindObjectsParams build() {
            AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams();
            altFindObjectsParameters.by = this.by;
            altFindObjectsParameters.value = this.value;
            altFindObjectsParameters.cameraBy = this.cameraBy;
            altFindObjectsParameters.cameraValue = this.cameraValue;
            altFindObjectsParameters.enabled = this.enabled;
            return altFindObjectsParameters;
        }
    }

    private AltFindObjectsParams() {
    }

    public String getValue() {
        return value;
    }

    public void setValue(String value) {
        this.value = value;
    }

    public String getCameraValue() {
        return cameraValue;
    }

    public void setCameraValue(String cameraValue) {
        this.cameraValue = cameraValue;
    }

    private AltDriver.By by;
    private String path;
    private By cameraBy;
    private String cameraPath;
    private boolean enabled;
    private String value;
    private String cameraValue;

    public AltDriver.By getBy() {
        return by;
    }

    public void setBy(AltDriver.By by) {
        this.by = by;
    }

    public String getPath() {
        return path;
    }

    public void setPath(String path) {
        this.path = path;
    }

    public By getCameraBy() {
        return cameraBy;
    }

    public void setCameraBy(By cameraBy) {
        this.cameraBy = cameraBy;
    }

    public String getCameraPath() {
        return cameraPath;
    }

    public void setCameraPath(String cameraPath) {
        this.cameraPath = cameraPath;
    }

    public boolean isEnabled() {
        return enabled;
    }

    public void setEnabled(boolean enabled) {
        this.enabled = enabled;
    }
}
