package com.alttester;

import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.BeforeEach;

public class BaseTest {
    static AltDriver altDriver;

    @BeforeAll
    public static void setUp() throws Exception {
        altDriver = TestsHelper.GetAltDriver();
    }

    @AfterAll
    public static void tearDown() throws Exception {
        if (altDriver != null) {
            altDriver.stop();
        }
        Thread.sleep(1000);
    }

    @BeforeEach
    public void resetInput() throws Exception {
        altDriver.resetInput();
    }

}
