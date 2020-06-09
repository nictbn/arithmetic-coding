using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitReaderWriter
{
    public class BitReader
    {
        byte readBuffer;
        long readBitsCounter;
        FileStream fileStream;
        // BinaryReader reader;

        public BitReader(string filePath)
        {
            fileStream = new FileStream(filePath, FileMode.Open);
            readBitsCounter = 0;
        }

        public void closeFile()
        {
            fileStream.Dispose();
        }

        public int readBit()
        {
            if(readBitsCounter % 8 == 0)
            {
                readBuffer = (byte)fileStream.ReadByte();
            }
            int position = 7 - (int)(readBitsCounter % 8);
            int mask = 1 << position;
            int bit = readBuffer & mask;
            bit = bit >> position;
            readBitsCounter++;
            return bit;
        }

        public int readNBits(int numberOfBits)
        {
            int position = numberOfBits - 1;
            int result = 0;
            for(int i = position; i >= 0; i--)
            {
                int bit = readBit();
                bit = bit << i;
                result = result | bit;
            }
            return result;
        }
    }
}
