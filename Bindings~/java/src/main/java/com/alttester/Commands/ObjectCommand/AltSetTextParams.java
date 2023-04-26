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
