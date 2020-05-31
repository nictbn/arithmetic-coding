using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitReaderWriter
{
    public class BitWriter
    {
        byte writeBuffer;
        long writtenBitsCounter;
        FileStream fileStream;
        BinaryWriter writer;

        public BitWriter(string filePath)
        {
            fileStream = new FileStream(filePath, FileMode.Create);
            writer = new BinaryWriter(fileStream);
            writtenBitsCounter = 0;
        }

        public void closeFile()
        {
            writer.Dispose();
            fileStream.Dispose();
        }

        public void writeBit(int bit)
        {
            int lsb = bit & 1; // get the least significant bit
            lsb = lsb << 7 - (int)(writtenBitsCounter % 8);
            writeBuffer = (byte)(writeBuffer | lsb);
            writtenBitsCounter++;
            if (writtenBitsCounter % 8 == 0)
            {
                writer.Write(writeBuffer);
                writeBuffer = 0;
            }
        }

        public void writeNBits(int payload, int numberOfBits)
        {
            int position = numberOfBits - 1;
            for(int i = position; i >= 0; i--)
            {
                int mask = 1 << i;
                int bit = payload & mask;
                bit = bit >> i;
                writeBit(bit);
            }
        }

    }
}
