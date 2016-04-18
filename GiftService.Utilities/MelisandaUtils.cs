using GiftService.Utilities.Models.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Utilities
{
    internal class MelisandaUtils
    {
        public IEnumerable<BaseProductModel> ParseProducts(string[] lines)
        {
            var list = new List<BaseProductModel>();
            ProductCategoryModel currentCategory = null;
            var categories = GetCategories();

            int maxPriority = lines.Length * 10;
            int priority = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string s = lines[i].Trim();
                if (String.IsNullOrEmpty(s) == false)
                {
                    // Should be category
                    if (s.EndsWith(":"))
                    {
                        // First product in list should be first one
                        priority = maxPriority;

                        s = s.Substring(0, s.Length - 1).Trim();
                        try
                        {
                            currentCategory = categories.First(x => x.Name.Equals(s, StringComparison.OrdinalIgnoreCase));
                        }
                        catch (InvalidOperationException ioex)
                        {
                            throw new Exception("Category not found: `" + s + "`", ioex);
                        }
                    }
                    else
                    {
                        if (currentCategory == null)
                        {
                            throw new ArgumentException("First line should be category. Must ends with semicolon (:)");
                        }

                        // Parse product
                        string[] ar = s.Split(new char[] { '\t' });
                        if (ar.Length != 2)
                        {
                            throw new ArgumentException("Line must contain NAME and PRICE: " + s);
                        }

                        // Possible prices: "12" or "20 / 30 /40"
                        string[] prices = ar[1].Split(new char[] { '/' });

                        //string name = ar[0].Trim();
                        //string[] subName = new string[0];
                        //if (prices.Length > 1)
                        //{

                        //}

                        for (int j = 0; j < prices.Length; j++)
                        {
                            var p = new BaseProductModel();
                            p.CultureName = "LT";
                            p.Name = ar[0].Trim();
                            p.ProductCategoryId = currentCategory.Id;
                            p.Price = ParsePrice(prices[j]);
                            p.Priority = priority;
                            list.Add(p);

                            priority -= 5;
                        }

                    }
                }
            }

            return list;
        }

        protected decimal ParsePrice(string s)
        {
            decimal d = 0;
            CultureInfo ciLt = new CultureInfo("lt");
            if (Decimal.TryParse(s, NumberStyles.Float, ciLt, out d))
            {
                return d;
            }
            else
            {
                Decimal.TryParse(s, out d);
            }

            return 0;
        }

        public IEnumerable<string> PrepareToInsert(IEnumerable<BaseProductModel> products)
        {
            string sqlFmt = "insert into `price`  (`id`, `status_id`, `user_id`, `priority`, `price_category_id`, `duration`, `price_minor`, `service_location_ids`) values ('%ID%', '2', null, %PRIORITY%, '%CATEGORY_ID%', null, '%PRICE_MINOR%', null);";
            string sqlI18nFmt = "insert into `price_i18n` (`name`, `text`, `text_two`, `text_three`, `id`, `culture`) values ('%NAME%', null, null, null, '%ID%', '%CULTURE%');";

            int id = 10;
            List<string> sqls = new List<string>();

            foreach (var p in products)
            {
                if (p.Id < 1)
                {
                    p.Id = id++;
                }

                string s = sqlFmt
                    .Replace("%ID%", p.Id.ToString())
                    .Replace("%PRIORITY%", p.Priority.ToString())
                    .Replace("%CATEGORY_ID%", p.ProductCategoryId.ToString())
                    .Replace("%PRICE_MINOR%", ((int)(p.Price * 100)).ToString());

                sqls.Add(s);

                s = sqlI18nFmt
                    .Replace("%ID%", p.Id.ToString())
                    //.Replace("%NAME%", p.Name)
                    .Replace("%NAME%", System.Net.WebUtility.HtmlEncode(p.Name))
                    .Replace("%CULTURE%", p.CultureName);
                sqls.Add(s);
                sqls.Add("--");
            }

            return sqls;
        }

        public static IEnumerable<ProductCategoryModel> GetCategories()
        {
            var list = new List<ProductCategoryModel>();

            list.Add(new ProductCategoryModel { Id = 1, Name = "Kirpėjų paslaugos", CultureName = "LT" });
            list.Add(new ProductCategoryModel { Id = 11, Name = "Masažai ir kitos procedūros", CultureName = "LT" });
            list.Add(new ProductCategoryModel { Id = 10, Name = "Depiliacija cukrumi", CultureName = "LT" });
            list.Add(new ProductCategoryModel { Id = 9, Name = "Depiliacija vašku", CultureName = "LT" });
            list.Add(new ProductCategoryModel { Id = 8, Name = "Makiažas", CultureName = "LT" });
            list.Add(new ProductCategoryModel { Id = 7, Name = "Kosmetologo paslaugos", CultureName = "LT" });
            list.Add(new ProductCategoryModel { Id = 6, Name = "Manikiūras / Pedikiūras", CultureName = "LT" });
            list.Add(new ProductCategoryModel { Id = 5, Name = "Plaukų dažymas", CultureName = "LT" });
            list.Add(new ProductCategoryModel { Id = 4, Name = "Sušukavimas", CultureName = "LT" });
            list.Add(new ProductCategoryModel { Id = 3, Name = "Plaukų struktūros atsatatymas, tiesinimas", CultureName = "LT" });
            list.Add(new ProductCategoryModel { Id = 2, Name = "Kompleksinės kirpėjų paslaugos", CultureName = "LT" });
            list.Add(new ProductCategoryModel { Id = 12, Name = "Vakuuminis anticeliulitinis taurelių masažas su įvyniojimu", CultureName = "LT" });

            return list;
        }
    }
}
