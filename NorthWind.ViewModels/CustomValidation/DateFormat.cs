using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.ViewModels.CustomValidation {

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DateFormat : ValidationAttribute {
        public string Message { get; set; }

        public DateFormat(string message) {
            this.Message = Message;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value != null) {
                string inputDate = value.ToString().Trim();
                DateTime output;
                bool validate = DateTime.TryParse(inputDate, out output);
                if (!validate) {
                    return new ValidationResult(this.Message);
                }
            }
            return ValidationResult.Success;
        }
    }
}
