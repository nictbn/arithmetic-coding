using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AritmeticV2
{
    class Program
    {
        Coder coder;
        static void Main(string[] args)
        {
            Coder coder = new Coder();
            coder.Encode("test.txt", 256);
        }
    }
}
