//package ro.altom.unit;
//
//import org.junit.Rule;
//import org.junit.Test;
//import org.junit.rules.ExpectedException;
//import org.junit.runner.RunWith;
//import org.junit.runners.Parameterized;
//import org.junit.runners.Parameterized.Parameters;
//import ro.altom.altunitytester.AltUnityDriver;
//import ro.altom.altunitytester.altUnityTesterExceptions.AltUnityException;
//import ro.altom.altunitytester.altUnityTesterExceptions.ConnectionException;
//import ro.altom.altunitytester.altUnityTesterExceptions.InvalidParamerException;
//
//import java.util.Arrays;
//import java.util.Collection;
//
//@RunWith(Parameterized.class)
//public class ConstructionTests {
//
//    @Parameters (name = "Initialize with ip[{0}] port[{1}]")
//    public static Collection<Object[]> data() {
//        return Arrays.asList( new Object[][] {
//                {"", 30000, InvalidParamerException.class, "Provided IP address is null or empty"},
//                {null, 30000, InvalidParamerException.class, "Provided IP address is null or empty"},
//                {"some string", 30000, ConnectionException.class, "Could not create connection to"},
//                {"localhost", 17000, ConnectionException.class, "Could not create connection to"} // still fails since there is no server to connect to
//            }
//        );
//    }
//
//    @Parameterized.Parameter
//    public String ip;
//
//    @Parameterized.Parameter(1)
//    public int port;
//
//    @Parameterized.Parameter(2)
//    public Class<AltUnityException> clazz;
//
//    @Parameterized.Parameter(3)
//    public String expectedMessage;
//
//    @Rule
//    public ExpectedException thrown = ExpectedException.none();
//
//    @Test
//    public void createWithInvalidIp() {
//        thrown.expect(clazz);
//        thrown.expectMessage(expectedMessage);
//        AltUnityDriver driver = new AltUnityDriver(ip, port);
//    }
//}
