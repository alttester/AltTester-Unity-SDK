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

package com.alttester.Commands.ObjectCommand;

import com.google.gson.Gson;

public class AltSetComponentPropertyParams extends AltObjectParams {
    public static class Builder {
        private String componentName;
        private String propertyName;
        private String assembly;
        private String value;

        public Builder(String componentName, String propertyName, String assembly, Object value) {
            this.componentName = componentName;
            this.propertyName = propertyName;
            this.assembly = assembly;
            this.value = new Gson().toJson(value);
        }

        public AltSetComponentPropertyParams build() {
            AltSetComponentPropertyParams altSetComponentPropertyParameters = new AltSetComponentPropertyParams();
            altSetComponentPropertyParameters.assembly = this.assembly;
            altSetComponentPropertyParameters.property = this.propertyName;
            altSetComponentPropertyParameters.component = this.componentName;
            altSetComponentPropertyParameters.value = this.value;

            return altSetComponentPropertyParameters;
        }
    }

    private AltSetComponentPropertyParams() {
    }

    public String getComponentName() {
        return component;
    }

    public void setComponentName(String componentName) {
        this.component = componentName;
    }

    public String getPropertyName() {
        return property;
    }

    public void setPropertyName(String propertyName) {
        this.property = propertyName;
    }

    public String getAssembly() {
        return assembly;
    }

    public void setAssembly(String assembly) {
        this.assembly = assembly;
    }

    private String component;
    private String property;
    private String assembly;

    public String getValue() {
        return value;
    }

    public void setValue(Object value) {
        this.value = new Gson().toJson(value);
    }

    private String value;
}
