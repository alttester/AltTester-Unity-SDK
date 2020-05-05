//package ro.altom.unit;
//
//import java.io.BufferedReader;
//import java.io.IOException;
//import java.io.InputStreamReader;
//import java.io.PrintWriter;
//import java.net.ServerSocket;
//import java.net.Socket;
//import java.util.concurrent.ExecutorService;
//import java.util.concurrent.Executors;
//
///**
// * Dummy server that AltUnity client can connect to.
// */
//public class DummyServer {
//    private ServerSocket serverSocket;
//    private Socket socket;
//    private BufferedReader in;
//    private PrintWriter out;
//    private ExecutorService executorService;
//
//    private class ServerCore implements Runnable {
//        @Override
//        public void run() {
//            try {
//                System.out.println("Accepting connection");
//                socket = serverSocket.accept();
//                in = new BufferedReader(new InputStreamReader(socket.getInputStream()));
//                out = new PrintWriter(socket.getOutputStream(), true);
//            } catch (IOException e) {
//                e.printStackTrace();
//            }
//        }
//    }
//
//    private DummyServer(int port) {
//        try {
//            serverSocket = new ServerSocket(port);
//        } catch (IOException e) {
//            throw new RuntimeException("Could not create server socket.");
//        }
//    }
//
//    /**
//     * Creates an instance of the {@link #DummyServer}
//     * @param port Port server will be listening at.
//     * @return Instance of an object
//     */
//    public static DummyServer onPort(int port) {
//        return new DummyServer(port);
//    }
//
//    /**
//     * Creates server core and runs it is a separate thread.
//     */
//    public void start() {
//        executorService = Executors.newFixedThreadPool(1);
//        executorService.execute(new ServerCore());
//    }
//
//    /**
//     * Closes all opened IO
//     */
//    public void stop() {
//        try {
//            out.close();
//            in.close();
//            socket.close();
//            serverSocket.close();
//            executorService.shutdown();
//        } catch (IOException e) {
//            e.printStackTrace();
//        }
//    }
//}
