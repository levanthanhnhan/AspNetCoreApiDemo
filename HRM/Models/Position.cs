using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public class Position
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateId { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdateId { get; set; }
    }
}
