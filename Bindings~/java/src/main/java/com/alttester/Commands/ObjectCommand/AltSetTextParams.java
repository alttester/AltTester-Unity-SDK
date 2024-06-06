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

public class AltSetTextParams extends AltObjectParams {
    private String value;
    private boolean submit = true;

    public static class Builder {
        private String value;
        private boolean submit = true;

        public Builder(String newText) {
            this.value = newText;
        }

        public Builder withSubmit(boolean submit) {
            this.submit = submit;
            return this;
        }

        public AltSetTextParams build() {
            AltSetTextParams altSetTextParams = new AltSetTextParams();
            altSetTextParams.value = this.value;
            altSetTextParams.submit = this.submit;

            return altSetTextParams;
        }
    }

    private AltSetTextParams() {
    }

    public String getNewText() {
        return value;
    }

    public void setNewText(String newText) {
        this.value = newText;
    }

    public boolean getSubmit() {
        return submit;
    }

    public void setSubmit(boolean submit) {
        this.submit = submit;
    }
}
