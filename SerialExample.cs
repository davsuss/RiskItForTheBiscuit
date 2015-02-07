using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;



namespace WindowsFormsApplication1
{
    [Serializable()]
    class SerClass
    {
        public int number;
        public SerClass()
        {
            number = 0;
        }

        public void setNumber(int num)
        {
            number = num;
        }

        public int getNumber()
        {
            return number;
        }
    }

    static class SerialExample
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            SerClass a = new SerClass();
            a.setNumber(5);
            Console.WriteLine("" + a.getNumber());

            IFormatter formatter = new BinaryFormatter();
            //write to WindowsFormsApplication1/bin/Debug
            Stream stream = new FileStream("MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, a);
            stream.Close();

            //read from WindowsFormsApplication1/bin/Debug
            Stream stream2 = new FileStream("MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            SerClass obj = (SerClass)formatter.Deserialize(stream2);
            stream2.Close();

            // Here's the proof. The variable has been read from file
            Console.WriteLine("number: " + obj.getNumber());
        }
    }

}
