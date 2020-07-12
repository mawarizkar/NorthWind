using NorthWind.Providers;
using NorthWind.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NorthWind.Web.Controllers
{
    [Authorize(Roles = "manager")]
    public class CustomerController : Controller
    {
        private CustomerProvider provider;

        public CustomerController() {
            this.provider = new CustomerProvider();
        }

        [HttpGet]
        public ActionResult CustomerIndex() {
            try {
                //string role = Session["Role"].ToString();//session dibaca sama action.
                //if (role == "manager") {
                    var viewModel = provider.GetCustomerIndexVM();
                    return View(viewModel);
                //} else {
                    //return RedirectToAction("Login", "Account");
                //}
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public JsonResult CreateCustomer(CreateEditCustomerVM viewModel) {
            try {
                if (ModelState.IsValid) {
                    provider.CreateCustomer(viewModel);
                    return Json(new { success = true, valid = true });
                }
                List<ValidationVM> errorList = GenerateValidationVM(ModelState);
                return Json(new { success = true, valid = false, validations = errorList });
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return Json(new { success = false });
            }
        }

        [HttpGet]
        public JsonResult EditCustomer(string customerID) {
            try {
                var viewModel = provider.GetEditCustomer(customerID);
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public JsonResult EditCustomer(CreateEditCustomerVM viewModel) {
            try {
                provider.UpdateCustomer(viewModel);
                return Json(new { success = true });
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public JsonResult DeleteCustomer(string customerID) {
            try {
                bool successDelete = provider.DeleteCustomer(customerID);
                return Json(new { success = true, removeStatus = successDelete });
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return Json(new { success = false });
            }
        }

        public List<ValidationVM> GenerateValidationVM(ModelStateDictionary dictionary) {
            List<ValidationVM> errorList = new List<ValidationVM>();
            foreach (KeyValuePair<string, ModelState> error in dictionary) {
                if (error.Value.Errors.Count < 1) {
                    continue;
                } else {
                    string firstErrorMessage = error.Value.Errors[0].ErrorMessage;
                    errorList.Add(new ValidationVM(error.Key, firstErrorMessage));
                }
            }
            return errorList;
        }
    }
}