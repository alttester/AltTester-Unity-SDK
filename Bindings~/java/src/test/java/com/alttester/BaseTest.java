package com.alttester;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;

public class BaseTest {
    static AltDriver altDriver;

    @BeforeClass
    public static void setUp() throws Exception {
        altDriver = TestsHelper.GetAltDriver();
    }

    @AfterClass
    public static void tearDown() throws Exception {
        if (altDriver != null) {
            altDriver.stop();
        }
        Thread.sleep(1000);
    }

    @Before
    public void resetInput() throws Exception {
        altDriver.resetInput();
    }

}
