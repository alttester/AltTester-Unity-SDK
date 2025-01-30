/*
    Copyright(C) 2025 Altom Consulting

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

public class AltGetComponentPropertyParams extends AltObjectParams {
    public static class Builder {
        private String componentName;
        private String propertyName;
        private String assembly = "";
        private int maxDepth = 2;

        public Builder(String componentName, String propertyName, String assembly) {
            this.componentName = componentName;
            this.propertyName = propertyName;
            this.assembly = assembly;
        }

        public Builder withMaxDepth(int maxDepth) {
            this.maxDepth = maxDepth;
            return this;
        }

        public AltGetComponentPropertyParams build() {
            AltGetComponentPropertyParams altGetComponentPropertyParameters = new AltGetComponentPropertyParams();
            altGetComponentPropertyParameters.component = this.componentName;
            altGetComponentPropertyParameters.property = this.propertyName;
            altGetComponentPropertyParameters.assembly = this.assembly;
            altGetComponentPropertyParameters.maxDepth = this.maxDepth;
            return altGetComponentPropertyParameters;
        }
    }

    private AltGetComponentPropertyParams() {
    }

    public String getAssembly() {
        return assembly;
    }

    public void setAssembly(String assembly) {
        this.assembly = assembly;
    }

    public String getPropertyName() {
        return property;
    }

    public void setPropertyName(String propertyName) {
        this.property = propertyName;
    }

    public String getComponentName() {
        return component;
    }

    public void setComponentName(String componentName) {
        this.component = componentName;
    }

    public int getMaxDepth() {
        return maxDepth;
    }

    public void setMaxDepth(int maxDepth) {
        this.maxDepth = maxDepth;
    }

    private String component;
    private String property;
    private String assembly = "";
    private int maxDepth;
}
