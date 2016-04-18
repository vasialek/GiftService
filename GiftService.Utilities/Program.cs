using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Utilities
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = @"C:\_projects\GiftService\doc\import\melisanda_products.txt";
            string outputSqlFile = filename + ".sql";

            Console.WriteLine("Parsing file: {0}", filename);
            string[] lines = File.ReadAllLines(filename);

            var melisandaUtils = new MelisandaUtils();
            var products = melisandaUtils.ParseProducts(lines);

            Console.WriteLine("Products:");
            foreach (var p in products)
            {
                Console.WriteLine("  {0,-8} {1} - {2}", p.ProductCategoryId, p.Price, p.Name);
            }

            var sqls = melisandaUtils.PrepareToInsert(products);
            foreach (string s in sqls)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("Going to write to file: {0}", outputSqlFile);
            Console.WriteLine("Press any key to continue or Ctrl+C...");
            Console.ReadKey(true);
            File.WriteAllLines(outputSqlFile, sqls);
        }
    }
}
