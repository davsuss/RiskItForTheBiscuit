using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRiskItForTheB
{
    class ClientGame
    {
        public Map currentMap;

        public color myColor;

        public int unplacedArmies;


        public ClientGame()
        {
            myColor = 0;
            unplacedArmies = 64;

            SynchronousSocketClient.StartClient();

            
        }
        ~ClientGame()
        {
            SynchronousSocketClient.closeSocket();
        }

        public void haveTurn()
        {

                SynchronousSocketClient.sendMessage();

                SynchronousSocketClient.waitAndGetResponse();

        }
        /*
        public static int Main(String[] args)
        {
            ClientGame name = new ClientGame();

            name.haveTurn();

            Console.Read();

            return 0;
        }
         * */
    }
}
