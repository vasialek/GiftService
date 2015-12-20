using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Models.Logs
{
    public class LogBdo
    {
        public int Id { get; set; }

        public string Thread { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }

        public DateTime CreatedAtServer { get; set; }

        public string Level { get; set; }
    }
}
