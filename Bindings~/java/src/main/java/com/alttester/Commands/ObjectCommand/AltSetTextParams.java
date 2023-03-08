package com.alttester.Commands.ObjectCommand;

public class AltSetTextParams extends AltObjectParams {
    private String newText;
    private boolean submit = true;

    public static class Builder {
        private String newText;
        private boolean submit = true;

        public Builder(String newText) {
            this.newText = newText;
        }

        public Builder withSubmit(boolean submit) {
            this.submit = submit;
            return this;
        }

        public AltSetTextParams build() {
            AltSetTextParams altSetTextParams = new AltSetTextParams();
            altSetTextParams.newText = this.newText;
            altSetTextParams.submit = this.submit;

            return altSetTextParams;
        }
    }

    private AltSetTextParams() {
    }

    public String getNewText() {
        return newText;
    }

    public void setNewText(String newText) {
        this.newText = newText;
    }

    public boolean getSubmit() {
        return submit;
    }

    public void setSubmit(boolean submit) {
        this.submit = submit;
    }
}
