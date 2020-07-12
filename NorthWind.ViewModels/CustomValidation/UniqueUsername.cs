using NorthWind.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.ViewModels.CustomValidation {
    public sealed class UniqueUsername : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value != null) {
                string username = value.ToString();
                var northwindContext = new NorthWindEntities();
                var allAccounts = northwindContext.Account;
                var sameUsername = allAccounts.FirstOrDefault(acc => acc.Username == username);
                if (sameUsername != null) {
                    return new ValidationResult("Username sudah ada, coba pakai yang lain");
                }
            }
            return ValidationResult.Success;
        }
    }
}
