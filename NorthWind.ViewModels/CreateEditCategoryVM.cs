using NorthWind.ViewModels.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.ViewModels {
    public class CreateEditCategoryVM {
        public int ID { get; set; }

        /// <summary>
        /// Nama Kategori
        /// </summary>
        [Required(ErrorMessage = "Nama dari Kategori harus di input")]
        [StringLength(15, ErrorMessage = "Nama Kategori tidak boleh lebih dari 15 karakter.")]
        [UniqueCategoryName]
        public string Name { get; set; }

        /// <summary>
        /// Deskripsi Kategori
        /// </summary>
        [StringLength(100, ErrorMessage = "Deskripsi tidak boleh lebih dari 100 karakter")]
        public string Description { get; set; }
    }
}
