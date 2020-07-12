using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.ViewModels {
    public class ProductBySupplierVM {
        public string SupplierCompany { get; set; }
        public string SupplierContact { get; set; }
        public string SupplierTitle { get; set; }
        public List<ProductGridBySupplierVM> ProductGrid { get; set; }
    }
}
