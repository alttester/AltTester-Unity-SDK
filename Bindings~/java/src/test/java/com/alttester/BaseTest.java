﻿/*
    Copyright(C) 2023  Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

package com.alttester;

import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.BeforeEach;

public class BaseTest {
    static AltDriver altDriver;

    @BeforeAll
    public static void setUp() throws Exception {
        altDriver = TestsHelper.getAltDriver();
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
