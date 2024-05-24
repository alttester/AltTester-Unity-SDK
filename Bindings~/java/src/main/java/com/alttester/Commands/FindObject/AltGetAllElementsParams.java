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
import com.alttester.AltDriver.By;

public class AltGetAllElementsParams extends AltMessage {

    public static class Builder {
        private By cameraBy = By.NAME;
        private String cameraValue = "";
        private boolean enabled = true;

        public Builder() {
        }

        public AltGetAllElementsParams.Builder isEnabled(boolean enabled) {
            this.enabled = enabled;
            return this;
        }

        public AltGetAllElementsParams.Builder withCamera(By cameraBy, String cameraValue) {
            this.cameraValue = cameraValue;
            this.cameraBy = cameraBy;
            return this;
        }

        public AltGetAllElementsParams build() {
            AltGetAllElementsParams altGetAllElementsParameters = new AltGetAllElementsParams();
            altGetAllElementsParameters.cameraBy = this.cameraBy;
            altGetAllElementsParameters.cameraValue = this.cameraValue;
            altGetAllElementsParameters.enabled = this.enabled;
            return altGetAllElementsParameters;
        }
    }

    private AltGetAllElementsParams() {
    }

    public String getCameraValue() {
        return cameraValue;
    }

    public void setCameraValue(String cameraValue) {
        this.cameraValue = cameraValue;
    }

    private By cameraBy;
    private boolean enabled;
    private String cameraValue;

    public By getCameraBy() {
        return cameraBy;
    }

    public void setCameraBy(By cameraBy) {
        this.cameraBy = cameraBy;
    }

    public boolean isEnabled() {
        return enabled;
    }

    public void setEnabled(boolean enabled) {
        this.enabled = enabled;
    }
}
