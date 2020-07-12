using NorthWind.Providers;
using NorthWind.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NorthWind.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller {
        private ProductProvider provider;

        public ProductController() {
            this.provider = new ProductProvider();
        }

        [HttpGet]
        public ActionResult CategoryIndex(string searchByName = "") {
            try {
                List<CategoryIndexVM> viewModel = provider.GetIndexCategoryVM(searchByName);
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult CategoryIndex(FormCollection formCollection) {
            try {
                string categoryName = formCollection["category-name"];
                return RedirectToAction("CategoryIndex", new { searchByName = categoryName });
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [Authorize(Roles = "admin,manager")]
        [HttpGet]
        public ActionResult SupplierIndex(string searchByCompany = "", string searchByContact = "", string searchByTitle = "") {
            try {
                var viewModel = provider.GetIndexSupplierVM(searchByCompany, searchByContact, searchByTitle);
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [Authorize(Roles = "admin,manager")]
        [HttpPost]
        public ActionResult SupplierIndex(FormCollection formCollection) {
            try {
                string company = formCollection["company"];
                string contact = formCollection["contact"];
                string title = formCollection["title"];
                return RedirectToAction("SupplierIndex", new { searchByCompany = company, searchByContact = contact, searchByTitle = title });
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpGet]
        public ActionResult ProductByCategory(int categoryID, string searchByName = "", int searchBySupplierID = 0) {
            try {
                ProductByCategoryVM viewModel = provider.GetProductByCategory(categoryID, searchByName, searchBySupplierID);
                ViewBag.SupplierDropDown = provider.GetSupplierDropDown();
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult ProductByCategory(FormCollection formCollection) {
            try {
                int hiddenCategoryID = int.Parse(formCollection["category-id"]);
                string productName = formCollection["product-name"];
                int supplierID = int.Parse(formCollection["supplier-id"]);
                return RedirectToAction("ProductByCategory", 
                    new { categoryID = hiddenCategoryID, searchByName = productName, searchBySupplierID = supplierID });
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [Authorize(Roles = "admin,manager")]
        [HttpGet]
        public ActionResult ProductBySupplier(int supplierID) {
            try {
                var viewModel = provider.GetProductBySupplier(supplierID);
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpGet]
        public ActionResult AddNewCategory() {
            try {
                CreateEditCategoryVM viewModel = new CreateEditCategoryVM();
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewCategory(CreateEditCategoryVM viewModel) {
            try {
                if (ModelState.IsValid) {
                    provider.CreateCategory(viewModel);
                    return RedirectToAction("CategoryIndex");
                }
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpGet]
        public ActionResult EditCurrentCategory(int categoryID) {
            try {
                var viewModel = provider.GetCategoryCurrentData(categoryID);
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult EditCurrentCategory(CreateEditCategoryVM viewModel) {
            try {
                if (ModelState.IsValid) {
                    provider.UpdateCategory(viewModel);
                    return RedirectToAction("CategoryIndex");
                }
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [Authorize(Roles = "admin,manager")]
        [HttpGet]
        public ActionResult AddNewSupplier() {
            try {
                var viewModel = new CreateEditSupplierVM();
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [Authorize(Roles = "admin,manager")]
        [HttpPost]
        public ActionResult AddNewSupplier(CreateEditSupplierVM viewModel) {
            try {
                if (ModelState.IsValid) {
                    provider.CreateSupplier(viewModel);
                    return RedirectToAction("SupplierIndex");
                }
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [Authorize(Roles = "admin,manager")]
        [HttpGet]
        public ActionResult EditCurrentSupplier(int supplierID) {
            try {
                var viewModel = provider.GetSupplierCurrentData(supplierID);
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [Authorize(Roles = "admin,manager")]
        [HttpPost]
        public ActionResult EditCurrentSupplier(CreateEditSupplierVM viewModel) {
            try {
                if (ModelState.IsValid) {
                    provider.UpdateSupplier(viewModel);
                    return RedirectToAction("SupplierIndex");
                }
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpGet]
        public ActionResult DeleteCategory(int categoryID) {
            try {
                bool deleteConfirmation = provider.RemoveCategory(categoryID);
                if (deleteConfirmation) {
                    return RedirectToAction("CategoryIndex");
                }
                return RedirectToAction("FailDeleteCategory");
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult DeleteSupplier(int supplierID) {
            try {
                provider.SoftRemoveSupplier(supplierID);
                return RedirectToAction("SupplierIndex");
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpGet]
        public ActionResult FailDeleteCategory() {
            return View();
        }

        [HttpGet]
        public ActionResult CreateProduct() {
            try {
                var viewModel = provider.GetCreateProduct();
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult CreateProduct(CreateEditProductVM viewModel) {
            try {
                if (ModelState.IsValid) {
                    provider.CreateProduct(viewModel);
                    return RedirectToAction("ProductByCategory", new { categoryID = viewModel.CategoryID });
                }
                viewModel.CategoryDropDown = provider.GetCategoryDropDown();
                viewModel.SupplierDropDown = provider.GetSupplierDropDown();
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpGet]
        public ActionResult EditProduct(int productID) {
            try {
                var viewModel = provider.GetEditProduct(productID);
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult EditProduct(CreateEditProductVM viewModel) {
            try {
                if (ModelState.IsValid) {
                    provider.UpdateProduct(viewModel);
                    return RedirectToAction("ProductByCategory", new { categoryID = viewModel.CategoryID });
                }
                viewModel.CategoryDropDown = provider.GetCategoryDropDown();
                viewModel.SupplierDropDown = provider.GetSupplierDropDown();
                return View(viewModel);
            } catch (Exception exception) {
                Console.WriteLine($"Tanggal Error: {DateTime.Now}, error message: {exception.Message}");
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public string FormatMoney(decimal? money) {
            if (money == null) {
                return "Belum Ada Harga";
            }
            string usDollar = money.Value.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
            return usDollar;
        }
    }
}