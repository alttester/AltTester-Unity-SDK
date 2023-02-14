
namespace AltTester.AltDriver.Tests
{
    public class TestsHelper
    {
        public static int GetAltDriverPort()
        {
            string port = System.Environment.GetEnvironmentVariable("PROXY_PORT");
            if (!string.IsNullOrEmpty(port))
            {
                return int.Parse(port);
            }

            return 13000;
        }

        public static string GetAltDriverHost()
        {
            string host = System.Environment.GetEnvironmentVariable("PROXY_HOST");
            if (!string.IsNullOrEmpty(host))
            {
                return host;
            }

            return "127.0.0.1";
        }

        public static AltDriver GetAltDriver()
        {
            return new AltDriver(host: GetAltDriverHost(), port: GetAltDriverPort(), enableLogging: true);
        }

    }
}
