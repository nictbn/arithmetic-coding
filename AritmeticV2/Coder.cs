using BitReaderWriter;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AritmeticV2
{
    class Coder
    {
        const uint TOP_VALUE = 0xFFFFFFFF;
        const uint FIRST_QTR = TOP_VALUE / 4 + 1;
        const uint HALF = 2 * FIRST_QTR;
        const uint THIRD_QTR = 3 * FIRST_QTR;
        BitWriter writer;
        FileStream reader;
        Model model;
        uint low;
        uint high;
        int bits_to_follow;

        //extra stuff

        public void Encode(string filePath, int numberOfCharactersForModel)
        {
            model = new Model(numberOfCharactersForModel);
            model.start_model();
            start_outputing_bits(filePath);
            start_encoding();
            for (; ; )
            {
                int ch;
                int symbol;
                ch = reader.ReadByte();
                if (ch < 0)
                {
                    break;
                }
                symbol = model.char_to_index[ch];
                encode_symbol(symbol, model.cumulative_frequencies);
                update_model(symbol);
            }
            encode_symbol(model.eof_symbol, model.cumulative_frequencies);
            done_encoding();
            done_outputing_bits();

        }

        private void done_outputing_bits()
        {
            writer.writeNBits(0, 7);
            writer.closeFile();
            reader.Dispose();
        }

        private void done_encoding()
        {
            bits_to_follow += 1;
            if (low < FIRST_QTR)
            {
                bit_plus_follow(0);
            }
            else
            {
                bit_plus_follow(1);
            }
        }

        private void update_model(int symbol)
        {
        }

        private void encode_symbol(int symbol, int[] cumulative_frequencies)
        {
            ulong range;
            range = (ulong)(high - low) - 1;
            high = (uint)(low + (range * (ulong)cumulative_frequencies[symbol - 1]) / (ulong)cumulative_frequencies[0] - 1);
            low = (uint)(low + (range * (ulong)cumulative_frequencies[symbol]) / (ulong)cumulative_frequencies[0]);
            for (; ; )
            {
                if (high < HALF)
                {
                    bit_plus_follow(0);
                }
                else if (low >= HALF)
                {
                    bit_plus_follow(1);
                    low -= HALF;
                    high -= HALF;
                }
                else if (low >= FIRST_QTR && high < THIRD_QTR)
                {        /* later if in middle half. */
                    bits_to_follow += 1;
                    low -= FIRST_QTR;
                    high -= FIRST_QTR;
                }
                else break;
                low = 2 * low;
                high = 2 * high + 1;
            }
        }

        private void bit_plus_follow(int v)
        {
            writer.writeBit(v);
            while (bits_to_follow > 0)
            {
                writer.writeBit(~v);
                bits_to_follow -= 1;
            }
        }

        private void start_outputing_bits(string filePath)
        {
            reader = new FileStream(filePath, FileMode.Open);
            writer = new BitWriter(filePath + ".arith");
        }

        private void start_encoding()
        {
            low = 0;
            high = TOP_VALUE;
            bits_to_follow = 0;
        }
    }


}
