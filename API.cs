using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

static class HelperFunction
{

    public static byte[] SeralizeFunction(object o, Type t)
    {
        MemoryStream fs = new MemoryStream();
        XmlSerializer formatter = new XmlSerializer(t);
        formatter.Serialize(fs, o);
        byte[] buffer = fs.ToArray();
        return buffer;
    }
    public static object deseralize(byte[] data, int bytesRead, Type t)
    {
        XmlSerializer formattor = new XmlSerializer(t);
        byte[] y = new byte[bytesRead];
        Array.Copy(data, y, bytesRead);
        MemoryStream ms = new MemoryStream(y);
        return formattor.Deserialize(ms);
    }
}

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
    public static string instructConvert(InstructionType x)
    {
        switch (x)
        {
            case (InstructionType.ATTACK):
                {
                    return "ATTACK";
                }
            case (InstructionType.REINFORCE):
                {
                    return "REINFORCE";
                }
            case (InstructionType.ENDTURN):
                {
                    return "ENDTURN";
                }
            case (InstructionType.GETMAP):
                {
                    return "GETMAP";
                }
            case (InstructionType.GETSTATUS):
                {
                    return "GETSTATUS";
                }
            default:
                {
                    return "UNKNOWN";
                }
        }
    }

    public byte[] serialize()
    {
        return HelperFunction.SeralizeFunction(this, typeof(Instruction));
    }

    public static Instruction deseralize(int bytesread, byte[] data)
    {
        return (Instruction)HelperFunction.deseralize(data, bytesread, typeof(Instruction));
    }

}

public enum ResponseType
{
    UNKNOWNINSTRUCTION,
    OK,
    NOTENOUGHARMY,
    TOOMANYARMY,
    DONTOWN,
    SELECTLOCATION,
}

public class Response
{

    public ResponseType response { get; set; }

    public byte[] serialize()
    {
        return HelperFunction.SeralizeFunction(this, typeof(Response));
    }

    public static Response deseralize(int bytesread, byte[] data)
    {
        return (Response)HelperFunction.deseralize(data, bytesread, typeof(Response));
    }
    public static string ResponseConvert(ResponseType x)
    {
        switch (x)
        {
            case (ResponseType.OK):
                {
                    return "OK";
                }
            case (ResponseType.DONTOWN):
                {
                    return "DONTOWN";
                }
            case (ResponseType.NOTENOUGHARMY):
                {
                    return "NOTENOUGHARMY";
                }
            case (ResponseType.SELECTLOCATION):
                {
                    return "SELECTLOCATION";
                }
            case (ResponseType.TOOMANYARMY):
                {
                    return "TOOMANYARMY";
                }
            case (ResponseType.UNKNOWNINSTRUCTION):
                {
                    return "UNKNOWNINSTRUCTION";
                }
            default:
                {
                    return "UNKNOWN";
                }
        }
    }

}

