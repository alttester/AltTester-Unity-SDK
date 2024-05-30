/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

package com.alttester.altTesterExceptions;

public class ReversePortForwardingException extends AltException {

    /**
     *
     */
    private static final long serialVersionUID = -7629828251460910071L;

    public ReversePortForwardingException() {
    }

    public ReversePortForwardingException(String message) {
        super(message);
    }

    public ReversePortForwardingException(Throwable exception) {
        super(exception);
    }

    public ReversePortForwardingException(String message, Throwable exception) {
        super(message, exception);
    }
}
