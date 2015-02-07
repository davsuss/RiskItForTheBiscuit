using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using  System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Net;
using System.Net.Sockets;
namespace ServerRiskItForTheB
{
    public class Game
    {
        List<Socket> clientList;
        List<Socket> stateList;
        List<Socket> turnList;
        static int maxClients = 4;
        public void handleSocket(Socket s)
        {
            StateObject state = new StateObject();
            state.workSocket = s;
            s.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0
                                 , new AsyncCallback(handleRecieveCallBack), state);
        }

        private static void Send(Socket handler, Response data)
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
        
        public void handleRecieveCallBack(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            Instruction instruct = new Instruction();
            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {

                try
                {
                    instruct = Instruction.deseralize(bytesRead,state.buffer);
                }
                catch (Exception x)
                {
                    Console.WriteLine("Serialzation Failed");
                }
                handleInstruction(instruct);
            }
            Response re = new Response();
            re.response = ResponseType.UNKNOWNINSTRUCTION;
            Send(handler, re);
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(handleRecieveCallBack), state);

        }

        public void handleInstruction(Instruction x)
        {
            Console.WriteLine(Instruction.instructConvert(x.type));
        }



    }
}
