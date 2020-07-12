using NorthWind.DataAccess;
using NorthWind.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Providers {
    public class CustomerProvider {
        protected readonly NorthWindEntities northwindContext;
        public CustomerProvider() {
            this.northwindContext = new NorthWindEntities();
        }

        private IQueryable<Customers> GetAllCustomers() {
            return northwindContext.Customers;
        }

        private Customers GetSingleCustomer(string customerID) {
            return GetAllCustomers().SingleOrDefault(cus => cus.CustomerID == customerID);
        }

        public List<CustomerIndexVM> GetCustomerIndexVM() {
            var query = from cus in GetAllCustomers()
                        select new CustomerIndexVM {
                            ID = cus.CustomerID,
                            Company = cus.CompanyName,
                            Contact = cus.ContactName,
                            Title = cus.ContactTitle
                        };
            return query.ToList();
        }

        public void CreateCustomer(CreateEditCustomerVM viewModel) {
            var customerEntity = new Customers {
                CustomerID = viewModel.ID,
                CompanyName = viewModel.Company,
                ContactName = viewModel.Contact,
                ContactTitle = viewModel.Title,
                Address = viewModel.Address,
                City = viewModel.City,
                Region = viewModel.Region,
                PostalCode = viewModel.PostalCode,
                Country = viewModel.Country,
                Phone = viewModel.Phone,
                Fax = viewModel.Fax
            };
            northwindContext.Customers.Add(customerEntity);
            northwindContext.SaveChanges();
        }

        public CreateEditCustomerVM GetEditCustomer(string customerID) {
            var customerEntity = GetSingleCustomer(customerID);
            var viewModel = new CreateEditCustomerVM {
                ID = customerEntity.CustomerID,
                Company = customerEntity.CompanyName,
                Contact = customerEntity.ContactName,
                Title = customerEntity.ContactTitle,
                Address = customerEntity.Address,
                City = customerEntity.City,
                Region = customerEntity.Region,
                PostalCode = customerEntity.PostalCode,
                Country = customerEntity.Country,
                Phone = customerEntity.Phone,
                Fax = customerEntity.Fax
            };
            return viewModel;
        }

        public void UpdateCustomer(CreateEditCustomerVM viewModel) {
            var customerEntity = GetSingleCustomer(viewModel.ID);
            customerEntity.CompanyName = viewModel.Company;
            customerEntity.ContactName = viewModel.Contact;
            customerEntity.ContactTitle = viewModel.Title;
            customerEntity.Address = viewModel.Address;
            customerEntity.City = viewModel.City;
            customerEntity.Region = viewModel.Region;
            customerEntity.PostalCode = viewModel.PostalCode;
            customerEntity.Country = viewModel.Country;
            customerEntity.Phone = viewModel.Phone;
            customerEntity.Fax = viewModel.Fax;
            northwindContext.SaveChanges();
        }

        private IQueryable<Orders> GetAllOrders() {
            return this.northwindContext.Orders;
        }

        private bool CheckCustomerDependency(string customerID) {
            var existingOrder = GetAllOrders().FirstOrDefault(ord => ord.CustomerID == customerID);
            var demographics = GetSingleCustomer(customerID).CustomerDemographics;
            if (existingOrder != null || demographics.Count > 0) {
                return true;
            }
            return false;
        }

        public bool DeleteCustomer(string customerID) {
            if (!CheckCustomerDependency(customerID)) {
                var customerEntity = GetSingleCustomer(customerID);
                northwindContext.Customers.Remove(customerEntity);
                northwindContext.SaveChanges();
                return true;
            }
            return false;
        }
        
    }
}
