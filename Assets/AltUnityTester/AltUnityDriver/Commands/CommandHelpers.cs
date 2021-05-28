namespace Altom.AltUnityDriver.Commands
{
    public class CommandHelpers
    {
        public static string[] ParseMethodCallParameters(string parameters, string[] parameterTypes)
        {
            if (parameters == null) return new string[0];
            var parameterValues = parameters.Split(new char[] { '?' });

            if ((parameterTypes == null || parameterTypes.Length == 0) && parameterValues.Length == 1 && parameterValues[0] == string.Empty) // special case to handle no paramters
            {
                return new string[0];
            }
            return parameterValues;
        }

        public static string[] ParseParseMethodCallypeOfParameters(string typeOfParameters)
        {
            if (string.IsNullOrEmpty(typeOfParameters))
                return null;
            return typeOfParameters.Split('?');
        }
    }
}