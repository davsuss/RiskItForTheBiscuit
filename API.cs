using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

public enum InstructionType
{
    ATTACK,
    REINFORCE,
    GETMAP,
    GETSTATUS,
    ENDTURN,
}
public class Instruction
{
    public Instruction()
    {
        payload = new int[20];
    }
    public int[] payload;
    public InstructionType type { get; set; }
    public int getpayload(int location)
    {
        return payload[location];
    }
    public static string instructConvert(InstructionType x){
    switch(x){
        case(InstructionType.ATTACK):{
            return "ATTACK";   
        }
        case(InstructionType.REINFORCE):{
            return "REINFORCE";
        }
        case(InstructionType.ENDTURN):{
            return "ENDTURN";
        }
        case(InstructionType.GETMAP):{
            return "GETMAP";
        }
        case(InstructionType.GETSTATUS):{
            return "GETSTATUS";
        }
        default:{
            return "UNKNOWN";
        }
    }
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



