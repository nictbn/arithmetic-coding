using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AritmeticV2
{
    class Model
    {
        int no_of_chars;
        int eof_symbol;
        int no_of_symbols;
        int[] char_to_index;
        int[] index_to_char;
        int[] cumulative_frequencies;
        int[] frequencies;
        
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
            for (int i = 0; i < no_of_symbols; i++)
            {
                frequencies[i] = 1;
                cumulative_frequencies[i] = no_of_symbols - i;
            }
            frequencies[0] = 0;
        }
    }
}
