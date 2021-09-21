using NUnit.Framework;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityDriver.Logging;


namespace Altom.AltUnityDriver.Tests
{
    public class AltBaseCommandImpl : AltBaseCommand
    {
        public AltBaseCommandImpl(IDriverCommunication comm) : base(comm)
        {

        }
        public void Validate(string expected, string received)
        {
            base.ValidateResponse(expected, received);
        }
    }

    [Timeout(1000)]

    public class TestAltBaseCommand
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DriverLogManager.SetMinLogLevel(AltUnityLogger.Console, AltUnityLogLevel.Debug);
        }
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void TestValidateResponse()
        {
            AltBaseCommandImpl cmd = new AltBaseCommandImpl(null);

            cmd.Validate("aa", "aa");
            try
            {
                cmd.Validate("aa", "bb");
                Assert.Fail();
            }

            catch (AltUnityInvalidServerResponse ex)
            {
                Assert.AreEqual(ex.Message, string.Format("Expected to get response '{0}'; Got  '{1}'", "aa", "bb"));
            }

        }
    }
}