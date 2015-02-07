using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Threading;

namespace ClientRiskItForTheB
{


    public class SynchronousSocketClient
    {

        public static IPAddress ipAddress;
        public static IPEndPoint remoteEP;
        public static ManualResetEvent allDone = new ManualResetEvent(false);

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
                try
                {
                    for (int i = 0; i < 4; i++)
                    {
                        serverIP[i] = Convert.ToByte(ipList[i]);
                    }
                }
                catch (Exception ex)
                {
                    //default to stephens computer if you dont enter everything correct
                    serverIP[0] = 172;
                    serverIP[1] = 31;
                    serverIP[2] = 218;
                    serverIP[3] = 244;
                }
                /*
                 * NATES IP
                serverIP[0] = 172;
                serverIP[1] = 31;
                serverIP[2] = 222;
                serverIP[3] = 176;
                 * */
                ipAddress = new IPAddress(serverIP);
                remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.
                sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

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


            MemoryStream fs = new MemoryStream();
            XmlSerializer formatter = new XmlSerializer(typeof(Instruction));
            formatter.Serialize(fs, instruct);
            byte[] buffer = fs.ToArray();
            Console.WriteLine(buffer.ToString());
            
            sender.Send(buffer);
        }



        public static void closeSocket()
        {
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }

        public static void waitAndGetResponse()
        {
            byte[] bytes = new Byte[1024];
            while (true)
            {
                int bytesRcvd = sender.Receive(bytes);
                Console.WriteLine(bytesRcvd);
                Console.Write(String.Format("Response received: {0}",
                Encoding.ASCII.GetString(bytes, 0, bytes.Length),
                "Connected to server"));
                Array.Clear(bytes, 0, bytes.Length);
            }

        }

        public static Socket sender;
    }
}