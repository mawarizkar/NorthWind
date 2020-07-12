using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.ViewModels {
    public class ProductGridBySupplierVM {
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
