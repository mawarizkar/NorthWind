using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.APIModel {
    public class ProductAM {
        /// <summary>
        /// Nama product
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Nama perusahaan dari Supplier
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// Harga product
        /// </summary>
        public decimal? Price { get; set; }
    }
}
