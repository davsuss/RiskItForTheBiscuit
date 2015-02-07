using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMONAPI
{
    enum color
    {
        Green,
        Black,
        Blue,
        Red,
        Yellow
    }

    struct position
    {
        string user;
        int color;
        int ArmyStrength;
    }
    class Map
    {
        static int mapSize = 64;

        position[] map;

        position getPosition(int x)
        {
            return map[x];
        }
    }

    enum InstructionType
    {
        ATTACK,
        REINFORCE,
        GETMAP,
        GETSTATUS,
        ENDTURN,
    }
    class Instruction
    {

        int[] payload;
        InstructionType type;

        int getpayload(int location)
        {
            return payload[location];
        }
        InstructionType getType()
        {
            return type;
        }
    }


    enum ResponseType
    {
        UNKNOWNINSTRUCTION,
        OK,
        NOTENOUGHARMY,
        TOOMANYARMY,
        DONTOWN,
        SELECTLOCATION,
    }


}
