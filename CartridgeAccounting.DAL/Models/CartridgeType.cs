using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartridgeAccounting.DAL.Models
{
    public class CartridgeType:IDomainObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
