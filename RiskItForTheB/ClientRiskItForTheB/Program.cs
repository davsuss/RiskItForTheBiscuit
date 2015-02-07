using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
namespace ClientRiskItForTheB
{


    public class SynchronousSocketClient
    {
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
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.
                Socket sender = new Socket(AddressFamily.InterNetwork,
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
                    MemoryStream fs = new MemoryStream();
                    XmlSerializer formatter = new XmlSerializer(typeof(Instruction));
                    Instruction x = new Instruction();
                    x.type = InstructionType.GETMAP;
                    x.payload[0] = 1;
                    formatter.Serialize(fs, x);

                    byte[] buffer = fs.ToArray();
                    Console.WriteLine(buffer.ToString());

                    sender.Send(buffer);
         
                   
                    // Receive the response from the remote device.
        
                    // Release the socket.
                   sender.Shutdown(SocketShutdown.Both);
                   sender.Close();

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

        public static int Main(String[] args)
        {
            StartClient();
            Console.Read();
            return 0;
        }
    }
}