/*
    Copyright(C) 2026 Altom Consulting
*/

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltBaseFindObjects : AltCommandReturningAltElement
    {
        public AltBaseFindObjects(IDriverCommunication commHandler) : base(commHandler)
        {
        }
        protected string SetPath(By by, string value)
        {
            string path = "";
            switch (by)
            {
                case By.TAG:
                    path = "//*[@tag=" + value + "]";
                    break;
                case By.LAYER:
                    path = "//*[@layer=" + value + "]";
                    break;
                case By.NAME:
                    path = "//" + value;
                    break;
                case By.COMPONENT:
                    path = "//*[@component=" + value + "]";
                    break;
                case By.PATH:
                    path = value;
                    break;
                case By.ID:
                    path = "//*[@id=" + value + "]";
                    break;
                case By.TEXT:
                    path = "//*[@text=" + value + "]";
                    break;
            }
            return path;
        }

        protected string SetPathFromObject(AltObject obj, By by, string value)
        {
            string path = "";
            switch (by)
            {
                case By.TAG:
                    path = $"//*[@id={obj.id}]//*[@tag={value}]";
                    break;
                case By.LAYER:
                    path = $"//*[@id={obj.id}]//*[@layer={value}]";
                    break;
                case By.NAME:
                    path = $"//*[@id={obj.id}]//{value}";
                    break;
                case By.COMPONENT:
                    path = $"//*[@id={obj.id}]//*[@component={value}]";
                    break;
                case By.PATH:
                    path = $"//*[@id={obj.id}]{value}";
                    break;
                case By.ID:
                    path = $"//*[@id={obj.id}]//*[@id={value}]";
                    break;
                case By.TEXT:
                    path = $"//*[@id={obj.id}]//*[@text={value}]";
                    break;
            }
            return path;
        }
        protected string SetPathContains(By by, string value)
        {
            string path = "";
            switch (by)
            {
                case By.TAG:
                    path = "//*[contains(@tag," + value + ")]";
                    break;
                case By.LAYER:
                    path = "//*[contains(@layer," + value + ")]";
                    break;
                case By.NAME:
                    path = "//*[contains(@name," + value + ")]";
                    break;
                case By.COMPONENT:
                    path = "//*[contains(@component," + value + ")]";
                    break;
                case By.PATH:
                    path = value;
                    break;
                case By.ID:
                    path = "//*[contains(@id," + value + ")]";
                    break;
                case By.TEXT:
                    path = "//*[contains(@text," + value + ")]";
                    break;
            }
            return path;
        }
    }
}
