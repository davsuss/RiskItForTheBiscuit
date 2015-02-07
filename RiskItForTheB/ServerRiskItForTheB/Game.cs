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
        public void handleRecieveCallBack(IAsyncResult ar)
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
                XmlSerializer formattor = new XmlSerializer(typeof(Instruction));
                Instruction instruct = new Instruction();
                byte[] y = new byte[bytesRead];
                Array.Copy(state.buffer, y, bytesRead);
                MemoryStream ms = new MemoryStream(y);
                try
                {
                    instruct = (Instruction)formattor.Deserialize(ms);
                }
                catch (Exception x)
                {
                    Console.WriteLine("Serialzation Failed");
                }
                handleInstruction(instruct);
            }
            
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(handleRecieveCallBack), state);

        }

        public void handleInstruction(Instruction x)
        {
            Console.WriteLine(Instruction.instructConvert(x.type));
        }



    }
}
