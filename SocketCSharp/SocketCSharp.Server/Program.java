import java.io.PrintWriter;
import java.net.Socket;


public class Program {

    public static void main(String[] args) {
        Socket socket;
        try {
            socket = new Socket( "127.0.0.1", 100);
            PrintWriter writer = new PrintWriter(socket.getOutputStream());
			writer.print("Hello world");
			
            writer.flush();
            writer.close();
            socket.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}