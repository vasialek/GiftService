using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Utilities
{
    public class PenetrationTests
    {
        public void GenerateAllOrderIds()
        {
            // Assume that attacker knows that order ID is letter, 2 digits, minus and 6 symbols

            char[] symbols = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            int totalGenerated = 0;
            int total = 0;
            for (char first = 'A'; first <= 'Z'; first++)
            {
                for (int digit1 = 0; digit1 < 10; digit1++)
                {
                    for (int digit2 = 0; digit2 < 10; digit2++)
                    {

                        for (int symbol1 = 0; symbol1 < symbols.Length; symbol1++)
                        {
                            for (int symbol2 = 0; symbol2 < symbols.Length; symbol2++)
                            {
                                for (int symbol3 = 0; symbol3 < symbols.Length; symbol3++)
                                {
                                    for (int symbol4 = 0; symbol4 < symbols.Length; symbol4++)
                                    {
                                        for (int symbol5 = 0; symbol5 < symbols.Length; symbol5++)
                                        {
                                            for (int symbol6 = 0; symbol6 < symbols.Length; symbol6++)
                                            {
                                                StringBuilder sb = new StringBuilder();
                                                sb.Append(first);
                                                sb.Append(digit1);
                                                sb.Append(digit2);
                                                sb.Append("-");
                                                sb.Append(symbols[symbol1]);
                                                sb.Append(symbols[symbol2]);
                                                sb.Append(symbols[symbol3]);
                                                sb.Append(symbols[symbol4]);
                                                sb.Append(symbols[symbol5]);
                                                sb.Append(symbols[symbol6]);
                                                //Console.WriteLine(sb.ToString());

                                                total++;
                                                if (total + 1 > int.MaxValue)
                                                {
                                                    total = 1;
                                                    totalGenerated++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            Console.WriteLine("Generated: {0} x {1} + {2} in {3}s", totalGenerated, int.MaxValue, total, sw.ElapsedMilliseconds / 1000);
                        }
                    }
                }

            }
            sw.Stop();


            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Generated in {0}ms", sw.ElapsedMilliseconds);

        }
    }
}
