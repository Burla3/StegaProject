using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HammingSteganography;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ISteganographier steg = new Steganographier(new HammingMatrix(1));

            int multiplier = 10000000;

            while (true)
            {
                BitArray cover = new BitArray(multiplier);
                BitArray message = new BitArray(multiplier);
                message.SetAll(true);

                Stopwatch sw = Stopwatch.StartNew();

                steg.EncodeBinaryMessage(cover, message);

                sw.Stop();

                Console.WriteLine($"Multi: {multiplier, 10} \t Elasped: {sw.Elapsed}\n");
                break;
                Console.ReadKey();
                multiplier *= 10;
            }
        }
    }
}
