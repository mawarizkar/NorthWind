using NorthWind.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.ViewModels.CustomValidation {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class UniqueCategoryName : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value != null) {
                string categoryName = value.ToString().Trim().ToUpper();
                var norhtwindContext = new NorthWindEntities();
                var allCategories = norhtwindContext.Categories;
                Categories sameCategoryName = allCategories.FirstOrDefault(cat => cat.CategoryName.Trim().ToUpper() == categoryName);

                //feature dari class validationAttribute dari data annotations, Object instance membawa seluruh object view model ke sini.
                var viewModel = (CreateEditCategoryVM)validationContext.ObjectInstance;

                if (sameCategoryName != null && sameCategoryName.CategoryID != viewModel.ID) {
                    return new ValidationResult("Nama Kategori sudah ada di dalam database, gunakan nama lain.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
