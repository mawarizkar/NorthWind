using NorthWind.DataAccess;
using NorthWind.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Providers {
    public class EmployeeProvider {
        protected readonly NorthWindEntities northwindContext;
        public EmployeeProvider() {
            this.northwindContext = new NorthWindEntities();
        }

        private IQueryable<Employees> GetAllEmployees() {
            return this.northwindContext.Employees;
        }

        private IQueryable<Orders> GetAllOrders() {
            return this.northwindContext.Orders;
        }

        private Employees GetSingleEmployee(int employeeID) {
            return GetAllEmployees().SingleOrDefault(emp => emp.EmployeeID == employeeID);
        }

        public IEnumerable<EmployeeIndexVM> GetEmployeeIndex() {
            //Limitasi dari LINQ dan EF, mereka tidak bisa menyimpan C# function dan invocationnya ke dalam query
            var query = from emp in GetAllEmployees()
                        select new EmployeeIndexVM {
                            ID = emp.EmployeeID,
                            FullName = emp.TitleOfCourtesy + " " + emp.FirstName + " " + emp.LastName, 
                            JobTitle = emp.Title,
                            HireDate = emp.HireDate,
                            BirthDate = emp.BirthDate
                        };
            var listViewmodels = query.ToList();
            return listViewmodels;
        }

        public void CreateEmployee(CreateEditEmployeeVM viewModel) {
            var employeeEntity = new Employees {
                LastName = viewModel.LastName,
                FirstName = viewModel.FirstName,
                Title = viewModel.Title,
                TitleOfCourtesy = viewModel.TitleOfCourtesy,
                BirthDate = DateTime.Parse(viewModel.BirthDate),
                HireDate = DateTime.Parse(viewModel.HireDate),
                Address = viewModel.Address,
                City = viewModel.City,
                Region = viewModel.Region,
                PostalCode = viewModel.PostalCode,
                Country = viewModel.Country,
                HomePhone = viewModel.HomePhone,
                Extension = viewModel.Extension
            };
            northwindContext.Employees.Add(employeeEntity);
            northwindContext.SaveChanges();
        }

        public string FormatDate(DateTime? date) {
            if (date != null) {
                return date.Value.ToString("yyyy-MM-dd");
            }
            return "";
        }

        public CreateEditEmployeeVM GetEditEmployee(int employeeID) {
            var employeeEntity = GetSingleEmployee(employeeID);
            var viewModel = new CreateEditEmployeeVM {
                ID = employeeEntity.EmployeeID,
                LastName = employeeEntity.LastName,
                FirstName = employeeEntity.FirstName,
                TitleOfCourtesy = employeeEntity.TitleOfCourtesy,
                Title = employeeEntity.Title,
                BirthDate = FormatDate(employeeEntity.BirthDate),
                HireDate = FormatDate(employeeEntity.HireDate),
                Address = employeeEntity.Address,
                City = employeeEntity.City,
                Region = employeeEntity.Region,
                PostalCode = employeeEntity.PostalCode,
                Country = employeeEntity.Country,
                HomePhone = employeeEntity.HomePhone,
                Extension = employeeEntity.Extension
            };
            return viewModel;
        }

        public void UpdateEmployee(CreateEditEmployeeVM viewModel) {
            var employeeEntity = GetSingleEmployee(viewModel.ID);
            employeeEntity.LastName = viewModel.LastName;
            employeeEntity.FirstName = viewModel.FirstName;
            employeeEntity.Title = viewModel.Title;
            employeeEntity.TitleOfCourtesy = viewModel.TitleOfCourtesy;
            employeeEntity.BirthDate = DateTime.Parse(viewModel.BirthDate);
            employeeEntity.HireDate = DateTime.Parse(viewModel.HireDate);
            employeeEntity.Address = viewModel.Address;
            employeeEntity.City = viewModel.City;
            employeeEntity.Region = viewModel.Region;
            employeeEntity.PostalCode = viewModel.PostalCode;
            employeeEntity.Country = viewModel.Country;
            employeeEntity.HomePhone = viewModel.HomePhone;
            employeeEntity.Extension = viewModel.Extension;
            northwindContext.SaveChanges();
        }

        public bool CheckEmployeeDependency(int employeeID) {
            var existingInferior = GetAllEmployees().FirstOrDefault(emp => emp.ReportsTo == employeeID);
            var existingOrder = GetAllOrders().FirstOrDefault(ord => ord.EmployeeID == employeeID);
            var territories = GetSingleEmployee(employeeID).Territories;
            if (existingInferior != null || existingOrder != null || territories.Count > 0) {
                return true;
            }
            return false;
        }

        public bool RemoveEmployee(int employeeID) {
            if (!CheckEmployeeDependency(employeeID)) {
                var employeeEntity = GetSingleEmployee(employeeID);
                northwindContext.Employees.Remove(employeeEntity);
                northwindContext.SaveChanges();
                return true;
            }
            return false;
        }

        public EmployeContactVM GetEmployeeContact(int employeeID) {
            var employeeEntity = GetSingleEmployee(employeeID);
            var viewModel = new EmployeContactVM {
                Address = employeeEntity.Address,
                City = employeeEntity.City,
                Region = employeeEntity.Region,
                PostalCode = employeeEntity.PostalCode,
                Country = employeeEntity.Country,
                HomePhone = employeeEntity.HomePhone,
                Extension = employeeEntity.Extension
            };
            return viewModel;
        }
    }
}
