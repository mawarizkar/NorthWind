using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.ViewModels {
    public class ValidationVM {
        public string Property { get; set; }
        public string FirstErrorMessage { get; set; }
        public ValidationVM(string property, string errorMessage) {
            this.Property = property;
            this.FirstErrorMessage = errorMessage;
        }
    }
}
