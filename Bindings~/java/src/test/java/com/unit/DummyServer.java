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
// import java.io.BufferedReader;
// import java.io.IOException;
// import java.io.InputStreamReader;
// import java.io.PrintWriter;
// import java.net.ServerSocket;
// import java.net.Socket;
// import java.util.concurrent.ExecutorService;
// import java.util.concurrent.Executors;
//
/// **
// * Dummy server that Alt client can connect to.
// */
// public class DummyServer {
// private ServerSocket serverSocket;
// private Socket socket;
// private BufferedReader in;
// private PrintWriter out;
// private ExecutorService executorService;
//
// private class ServerCore implements Runnable {
// @Override
// public void run() {
// try {
// System.out.println("Accepting connection");
// socket = serverSocket.accept();
// in = new BufferedReader(new InputStreamReader(socket.getInputStream()));
// out = new PrintWriter(socket.getOutputStream(), true);
// } catch (IOException e) {
// e.printStackTrace();
// }
// }
// }
//
// private DummyServer(int port) {
// try {
// serverSocket = new ServerSocket(port);
// } catch (IOException e) {
// throw new RuntimeException("Could not create server socket.");
// }
// }
//
// /**
// * Creates an instance of the {@link #DummyServer}
// * @param port Port server will be listening at.
// * @return Instance of an object
// */
// public static DummyServer onPort(int port) {
// return new DummyServer(port);
// }
//
// /**
// * Creates server core and runs it is a separate thread.
// */
// public void start() {
// executorService = Executors.newFixedThreadPool(1);
// executorService.execute(new ServerCore());
// }
//
// /**
// * Closes all opened IO
// */
// public void stop() {
// try {
// out.close();
// in.close();
// socket.close();
// serverSocket.close();
// executorService.shutdown();
// } catch (IOException e) {
// e.printStackTrace();
// }
// }
// }
