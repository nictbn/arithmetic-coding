using BitReaderWriter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AritmeticV2
{
    class Decoder
    {
        const uint TOP_VALUE = 0xFFFFFFFF;
        const uint FIRST_QTR = 1u << 30;
        const uint HALF = 1u << 31;
        const uint THIRD_QTR = HALF | FIRST_QTR;
        BitReader reader;
        FileStream writer;
        Model model;
        uint low;
        uint high;
        uint value;
        public void Decode(string filePath, int numberOfCharactersForModel)
        {
            model = new Model(numberOfCharactersForModel);
            model.start_model();
            start_inputing_bits(filePath);
            start_decoding();
            for (; ; )
            {
                int ch;
                int symbol;
                symbol = decode_symbol(model.cumulative_frequencies);
                if (symbol == model.eof_symbol)
                {
                    break;
                }
                ch = model.index_to_char[symbol];
                writer.WriteByte((byte)ch);
                model.update_model(symbol);
            }
            reader.closeFile();
            writer.Close();
        }

        private int decode_symbol(int[] cumulative_frequencies)
        {
            ulong range;
            int cum;
            int symbol;

            range = (ulong)(high - low) + 1;
            cum = (int)((ulong)((value - low + 1) * cumulative_frequencies[0] - 1) / range);

            for (symbol = 1; cumulative_frequencies[symbol] > cum; symbol++) ;

            high = (uint)(low + (range * (ulong)cumulative_frequencies[symbol - 1]) / (ulong)cumulative_frequencies[0] - 1);
            low = (uint)(low + (range * (ulong)cumulative_frequencies[symbol]) / (ulong)cumulative_frequencies[0]);

            for (; ; )
            {
                if (high < HALF)
                {
                }
                else if (low >= HALF)
                {
                    value -= HALF;
                    low -= HALF;
                    high -= HALF;
                }
                else if (low >= FIRST_QTR && high < THIRD_QTR)
                {
                    value -= FIRST_QTR;
                    low -= FIRST_QTR;
                    high -= FIRST_QTR;
                }
                else break;
                low = 2 * low;
                high = 2 * high + 1;
                value = 2 * value + (uint)reader.readBit();
            }
            return symbol;
        }

        private void start_decoding()
        {
            value = 0;
            value = (uint)reader.readNBits(32);
            low = 0;
            high = TOP_VALUE;
        }

        private void start_inputing_bits(string filePath)
        {
            reader = new BitReader(filePath);
            string pathWithoutArithExtension = filePath.Substring(0, filePath.LastIndexOf("."));
            string extension = Path.GetExtension(pathWithoutArithExtension);
            writer = new FileStream(filePath + extension, FileMode.Create);
        }
    }
}
