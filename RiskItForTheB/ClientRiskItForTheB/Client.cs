using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
namespace ClientRiskItForTheB
{
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    public class SynchronousSocketClient
    {
        private static ManualResetEvent connectDone =
    new ManualResetEvent(false);
        public static Socket sender;
        static byte[] GetBytes(string str)
        {
            byte[] b2 = System.Text.Encoding.ASCII.GetBytes(str);
            //System.Buffer.BlockCopy(ch, 0, bytes, 0, bytes.Length);
            return b2;
        }
        public static void StartClient()
        {
            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.
                // This example uses port 11000 on the local computer.
                byte[] serverIP = new byte[4];
                Console.WriteLine("Please enter IP of server in format X.X.X.X");
                string ipLine = Console.ReadLine();
                string[] ipList = ipLine.Split('.');
                for (int i = 0; i < 4; i++)
                {
                    serverIP[i] = Convert.ToByte(ipList[i]);
                }
                /*
                 * NATES IP
                serverIP[0] = 172;
                serverIP[1] = 31;
                serverIP[2] = 222;
                serverIP[3] = 176;
                 * */
                IPAddress ipAddress = new IPAddress(serverIP);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.
                sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                  //  sender.BeginAccept(
                   //   new AsyncCallback(AcceptCallback),
                    //  sender);
                    //sender.Connect(remoteEP);

                    sender.BeginConnect(remoteEP,
                         new AsyncCallback(ConnectCallback), sender);
                    connectDone.WaitOne();


                    // Encode the data string into a byte array.
                   // byte[] msg = Encoding.ASCII.GetBytes("This is a test");

                    // Send the data through the socket.
                   // int bytesSent = sender.Send(msg);
                   
                    // Receive the response from the remote device.
        
                    // Release the socket.

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static void sendMessage()
        {
            Console.Write("Enter in a int: ");
            int x = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            Instruction instruct = new Instruction();
            switch (x)
            {
                case (1):
                    {
                        instruct.type = InstructionType.GETMAP;
                        instruct.payload[0] = 1;
                        break;
                    }
                case (2):
                    {
                        instruct.type = InstructionType.ATTACK;
                        instruct.payload[0] = 2;
                        break;
                    }
                case (3):
                    {
                        instruct.type = InstructionType.GETSTATUS;
                        instruct.payload[0] = 3;
                        break;
                    }
                case (4):
                    {
                        instruct.type = InstructionType.REINFORCE;
                        instruct.payload[0] = 4;
                        break;
                    }
            }

            Send(sender, instruct);
            
        }

        private static void Send(Socket handler, Instruction data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = data.serialize();

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        
        public static void closeSocket()
        {
           // sender.Shutdown(SocketShutdown.Both);
           // sender.Close();
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection.
            client.EndConnect(ar);

            Console.WriteLine("Socket connected to {0}",
                client.RemoteEndPoint.ToString());

            StateObject state = new StateObject();
            state.workSocket = client;
            connectDone.Set();
            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0
                                 , new AsyncCallback(handleRecieveCallBack), state);

        }

        public static void handleRecieveCallBack(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            Response re = new Response();
            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                try
                {
                    re = Response.deseralize(bytesRead, state.buffer);
                }
                catch (Exception x)
                {
                    Console.WriteLine("Serialzation Failed");
                }
                Console.WriteLine(re);
            }

            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(handleRecieveCallBack), state);

        }

        public static int Main(String[] args)
        {
            StartClient();
            while (true)
            {
                sendMessage();
            }
            while (true) { }

        }
    }
}