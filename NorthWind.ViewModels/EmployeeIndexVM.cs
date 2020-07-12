using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.ViewModels {
    public class EmployeeIndexVM {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
