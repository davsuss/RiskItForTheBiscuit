using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using  System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
public class AddressDetails
{
    public int HouseNo { get; set; }
    public string StreetName { get; set; }
    public string City { get; set; }
    private string PoAddress { get; set; }
}  


namespace ServerRiskItForTheB
{
    static class MainServer
    {

        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        /// <summary>
        /// This function starts a while(1) loop which waits for connections, the connections are then sent to the GAME class 
        /// to be handled by an individual game
        /// </summary>
        public static void startLisenting()
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);


            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Server Initalized! Here are the Specs");
            Console.WriteLine(ipAddress.ToString());
            Console.WriteLine(localEndPoint.Port.ToString());

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        /// <summary>
        /// This callback is called when a socket connects to the server, currently it just reads in the data form a single socket
        /// </summary>
        /// <param name="ar"></param>
        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0
                                 , new AsyncCallback(ReadCallback), state);
        }
        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                //state.sb.Append(Encoding.ASCII.GetString(
                //    state.buffer, 0, bytesRead));
                XmlSerializer formattor = new XmlSerializer(typeof(AddressDetails));
                AddressDetails objFileInfo = new AddressDetails();
                byte [] y = new byte[bytesRead];

                Array.Copy(state.buffer, y, bytesRead);
                MemoryStream ms = new MemoryStream(y);
                try
                {
                    objFileInfo = (AddressDetails)formattor.Deserialize(ms);
                }
                catch (Exception x)
                {
                    Console.WriteLine("Serialzation Failed");
                }
                    Console.WriteLine(objFileInfo.City);
                
                // Check for end-of-file tag. If it is not there, read 
                // more data.
               // content = state.sb.ToString();
                // All the data has been read from the 
                // client. Display it on the console.
              //  Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
              //      content.Length, content);
                // Echo the data back to the client.
                //Send(handler, content);
                // Not all data received. Get more.
               // state.sb.Clear();
                
            }
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        }
    

}
