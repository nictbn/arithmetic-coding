using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AritmeticV2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the path to the file: ");
            string path = Console.ReadLine();
            if (!File.Exists(path))
            {
                Console.WriteLine("The file does not exist!");
                Console.Read();
                return;
            }
            Console.WriteLine("Encoding Start");
            Coder coder = new Coder();
            coder.Encode(path, 256);
            Console.WriteLine("Decoding Start");
            Decoder decoder = new Decoder();
            decoder.Decode(path + ".arith", 256);
            Console.WriteLine("Finished processing");
        }
    }
}
