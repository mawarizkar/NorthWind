using NorthWind.Providers;
using NorthWind.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NorthWind.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class EmployeeController : Controller
    {
        private EmployeeProvider provider;

        public EmployeeController() {
            this.provider = new EmployeeProvider();
        }

        [HttpGet]
        public ActionResult EmployeeIndex() {
            try {
                var viewModel = provider.GetEmployeeIndex();
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpGet]
        public ActionResult CreateEmployee() {
            try {
                var viewModel = new CreateEditEmployeeVM();
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult CreateEmployee(CreateEditEmployeeVM viewModel) {
            try {
                if (ModelState.IsValid) {
                    provider.CreateEmployee(viewModel);
                    return RedirectToAction("EmployeeIndex");
                }
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpGet]
        public ActionResult EditEmployee(int employeeID) {
            try {
                var viewModel = provider.GetEditEmployee(employeeID);
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult EditEmployee(CreateEditEmployeeVM viewModel) {
            try {
                if (ModelState.IsValid) {
                    provider.UpdateEmployee(viewModel);
                    return RedirectToAction("EmployeeIndex");
                }
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpGet]
        public ActionResult DeleteEmployee(int employeeID) {
            try {
                bool deleteConfirmation = provider.RemoveEmployee(employeeID);
                if (deleteConfirmation) {
                    return RedirectToAction("EmployeeIndex");
                }
                return RedirectToAction("FailDeleteEmployee");
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpGet]
        public ActionResult FailDeleteEmployee() {
            return View();
        }

        [HttpGet]
        public JsonResult EmployeeContact(int employeeID) {
            var viewModel = provider.GetEmployeeContact(employeeID);
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        public string DateFormatter(DateTime? date) {
            if (date != null) {
                return date.Value.ToString("dd/MM/yyyy");
            }
            return "Not Applicable";
        }
    }
}