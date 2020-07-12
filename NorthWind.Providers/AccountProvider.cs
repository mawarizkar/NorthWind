using NorthWind.DataAccess;
using NorthWind.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Providers {
    public class AccountProvider {
        protected readonly NorthWindEntities northwindContext;

        public AccountProvider() {
            this.northwindContext = new NorthWindEntities();
        }

        public bool IsAuthenticated(LoginVM viewModel) {
            var entity = northwindContext.Account.FirstOrDefault(acc => acc.Username == viewModel.Username &&
                acc.Password == viewModel.Password);
            if (entity != null) {
                return true;
            }
            return false;
        }

        public void RegisterAccount(RegisterVM viewModel) {
            var entity = new Account {
                Username = viewModel.Username,
                Password = viewModel.Password,
                Role = viewModel.Role
            };
            northwindContext.Account.Add(entity);
            northwindContext.SaveChanges();
        }

        public string GetRole(string username) {
            var role = northwindContext.Account.FirstOrDefault(acc => acc.Username == username).Role;
            return role;
        }
    }
}
