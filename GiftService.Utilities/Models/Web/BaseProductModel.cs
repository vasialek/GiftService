using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Utilities.Models.Web
{
    internal class BaseProductModel
    {
        public int Id { get; set; }

        public int ProductCategoryId { get; set; }

        public string ProductCategoryName { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string CultureName { get; set; }

        public int Priority { get; set; }
    }
}
