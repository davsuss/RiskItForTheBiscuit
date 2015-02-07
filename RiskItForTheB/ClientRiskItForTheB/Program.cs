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
                    while (true)
                    {
                        Console.Write("Enter in a int: ");
                        int x = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine();
                        Instruction instruct = new Instruction();
                        switch(x)
                        {
                            case(1):
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