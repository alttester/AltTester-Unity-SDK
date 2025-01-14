/*
 * Copyright(C) 2025 Altom Consulting
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
 */

// package com.unit;
//
// import org.junit.Rule;
// import org.junit.Test;
// import org.junit.rules.ExpectedException;
// import org.junit.runner.RunWith;
// import org.junit.runners.Parameterized;
// import org.junit.runners.Parameterized.Parameters;
// import com.alttester.AltDriver;
// import com.alttester.altTesterExceptions.AltException;
// import com.alttester.altTesterExceptions.ConnectionException;
// import com.alttester.altTesterExceptions.InvalidParameterException;
//
// import java.util.Arrays;
// import java.util.Collection;
//
// @RunWith(Parameterized.class)
// public class ConstructionTests {
//
// @Parameters (name = "Initialize with ip[{0}] port[{1}]")
// public static Collection<Object[]> data() {
// return Arrays.asList( new Object[][] {
// {"", 30000, InvalidParameterException.class, "Provided IP address is null or
// empty"},
// {null, 30000, InvalidParameterException.class, "Provided IP address is null
// or empty"},
// {"some string", 30000, ConnectionException.class, "Could not create
// connection to"},
// {"localhost", 17000, ConnectionException.class, "Could not create connection
// to"} // still fails since there is no server to connect to
// }
// );
// }
//
// @Parameterized.Parameter
// public String ip;
//
// @Parameterized.Parameter(1)
// public int port;
//
// @Parameterized.Parameter(2)
// public Class<AltException> clazz;
//
// @Parameterized.Parameter(3)
// public String expectedMessage;
//
// @Rule
// public ExpectedException thrown = ExpectedException.none();
//
// @Test
// public void createWithInvalidIp() {
// thrown.expect(clazz);
// thrown.expectMessage(expectedMessage);
// AltDriver driver = new AltDriver(ip, port);
// }
// }
