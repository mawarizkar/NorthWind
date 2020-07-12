using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.ViewModels {
    public class ProductGridVM {
        public int ID { get; set; }
        public string Name { get; set; }
        public string SupplierCompany { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
