using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.ViewModels {
    public class CreateEditProductVM {
        public int ID { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        [Required]
        public int SupplierID { get; set; }
        public List<DropDownVM> SupplierDropDown { get; set; }

        [Required]
        public int CategoryID { get; set; }
        public List<DropDownVM> CategoryDropDown { get; set; }

        [StringLength(20)]
        public string Quantity { get; set; }
        public Decimal? Price { get; set; }
        public short? InStock { get; set; }
        public short? OnOrder { get; set; }
        public short? ReOrder { get; set; }
        public bool Discontinued { get; set; }
    }
}
