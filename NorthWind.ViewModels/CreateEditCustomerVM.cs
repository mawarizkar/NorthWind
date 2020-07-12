using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.ViewModels {
    public class CreateEditCustomerVM {

        [Required (ErrorMessage = "ID Dibutuhkan untuk mensubmit")]
        [StringLength(5)]
        public string ID { get; set; }

        [Required (ErrorMessage = "Nama Perusahaan wajib di input")]
        [StringLength(40)]
        public string Company { get; set; }

        [StringLength(30)]
        public string Contact { get; set; }

        [StringLength(30)]
        public string Title { get; set; }

        [StringLength(60)]
        public string Address { get; set; }

        [StringLength(15)]
        public string City { get; set; }

        [StringLength(15)]
        public string Region { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(15)]
        public string Country { get; set; }

        [StringLength(24)]
        public string Phone { get; set; }

        [StringLength(24)]
        public string Fax { get; set; }
    }
}
