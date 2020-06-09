using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AritmeticV2
{
    class Model
    {
        public const uint MAX_FREQUENCY = (1u << 30) - 1;
        public int no_of_chars;
        public int eof_symbol;
        public int no_of_symbols;
        public int[] char_to_index;
        public int[] index_to_char;
        public int[] cumulative_frequencies;
        public int[] frequencies;

        public Model(int numberOfCharacters)
        {
            no_of_chars = numberOfCharacters;
            eof_symbol = no_of_chars + 1;
            no_of_symbols = no_of_chars + 1;
            char_to_index = new int[no_of_chars];
            index_to_char = new int [no_of_symbols + 1];
            cumulative_frequencies = new int[no_of_symbols + 1];
            frequencies = new int[no_of_symbols + 1];
        }

        public void start_model() 
        {
            for (int i = 0; i < no_of_chars; i++)
            {
                char_to_index[i] = i + 1;
                index_to_char[i + 1] = i;
            }
            for (int i = 0; i <= no_of_symbols; i++)
            {
                frequencies[i] = 1;
                cumulative_frequencies[i] = no_of_symbols - i;
            }
            frequencies[0] = 0;
        }

        public void update_model(int symbol)
        {
            int i;
            if (cumulative_frequencies[0] == Model.MAX_FREQUENCY)
            {
                int cum = 0;
                for (i = no_of_symbols; i >= 0; i--)
                {
                    frequencies[i] = (frequencies[i] + 1) / 2;
                    cumulative_frequencies[i] = cum;
                    cum += frequencies[i];
                }
            }
            for (i = symbol; frequencies[i] == frequencies[i - 1]; i--) ;
            if (i < symbol)
            {
                int ch_i, ch_symbol;
                ch_i = index_to_char[i];
                ch_symbol = index_to_char[symbol];
                index_to_char[i] = ch_symbol;
                index_to_char[symbol] = ch_i;
                char_to_index[ch_i] = symbol;
                char_to_index[ch_symbol] = i;
            }
            frequencies[i]++;
            while (i > 0)
            {
                i--;
                cumulative_frequencies[i]++;
            }
        }
    }
}
