using NorthWind.APIModel;
using NorthWind.DataAccess;
using NorthWind.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Providers {
    public class ProductProvider {
        protected readonly NorthWindEntities northwindContext;

        public ProductProvider() {
            this.northwindContext = new NorthWindEntities();
        }

        private IQueryable<Categories> GetAllCategories() {
            //LinQ
            IQueryable<Categories> allCategories = northwindContext.Categories;
            return allCategories;
        }

        private IQueryable<Products> GetAllProducts() {
            var allProducts = northwindContext.Products;
            return allProducts;
        }

        private IQueryable<Suppliers> GetAllSuppliers(){
            return northwindContext.Suppliers.Where(sup => sup.DeleteDate == null);
        }

        public List<CategoryIndexVM> GetIndexCategoryVM(string searchByName) {
            var filteredCategories = GetAllCategories().Where(cat => cat.CategoryName.Contains(searchByName));
            IQueryable<CategoryIndexVM> query = from cat in filteredCategories
                                                select new CategoryIndexVM {
                                                    CategoryID = cat.CategoryID,
                                                    CategoryName = cat.CategoryName,
                                                    CategoryDescription = cat.Description
                                                };
            List<CategoryIndexVM> hasil = query.ToList();
            return hasil;
        }
    
        /// <summary>
        /// Method untuk mendapatkan data-data Categories
        /// </summary>
        /// <returns>Data semua categories dalam List</returns>
        public List<CategoryAM> GetCategoriesForAPI() {
            var query = from cat in GetAllCategories()
                        select new CategoryAM {
                            Name = cat.CategoryName,
                            Description = cat.Description
                        };
            return query.ToList();
        }

        /// <summary>
        /// Function untuk mendapatkan 1 category data, yang dipilih berdasarkan PK category.
        /// </summary>
        /// <param name="categoryID">Primary Key dari Category</param>
        /// <returns>Category API Model</returns>
        public CategoryAM GetSingleCategoryAM(int categoryID) {
            var categoryEntity = GetSingleCategory(categoryID);
            var apiModel = new CategoryAM {
                Name  = categoryEntity.CategoryName,
                Description = categoryEntity.Description
            };
            return apiModel;
        }

        /// <summary>
        /// Fungsi untuk mendapatkan product-product berdasarkan kategori dan nama produknya.
        /// </summary>
        /// <param name="categoryID">PK dari category header</param>
        /// <param name="productName">komposisi nama produk</param>
        /// <returns></returns>
        public List<ProductAM> GetSelectedProductAM(int categoryID, string productName) {
            var productsFilteredByCategory = GetAllProducts()
                .Where(prod => prod.CategoryID == categoryID && prod.ProductName.Contains(productName));
            var query = from prod in productsFilteredByCategory
                        select new ProductAM {
                            Name = prod.ProductName,
                            Supplier = prod.Suppliers.CompanyName,
                            Price = prod.UnitPrice
                        };
            return query.ToList();
        }

        public List<SupplierIndexVM> GetIndexSupplierVM(string searchByCompany, string searchByContact, string searchByTitle){
            var query = from sup in GetAllSuppliers()
                        where sup.CompanyName.Contains(searchByCompany) && 
                            sup.ContactName.Contains(searchByContact) &&
                            sup.ContactTitle.Contains(searchByTitle)
                        select new SupplierIndexVM
                        {
                            ID = sup.SupplierID,
                            Company = sup.CompanyName,
                            Contact = sup.ContactName,
                            Title = sup.ContactTitle
                        };
            return query.ToList();
        }

        private Categories GetSingleCategory(int categoryID) {
            Categories category = GetAllCategories().SingleOrDefault(cat => cat.CategoryID == categoryID);
            return category;
        }

        private Suppliers GetSingleSupplier(int supplierID){
            return GetAllSuppliers().SingleOrDefault(sup => sup.SupplierID == supplierID);
        }

        private List<ProductGridVM> GetProductGrids(int categoryID, string searchByName, int searchBySupplierID) {
            var productsFilteredByCategory = GetAllProducts()
                .Where(prod => prod.CategoryID == categoryID && prod.ProductName.Contains(searchByName));
            if (searchBySupplierID > 0) {
                productsFilteredByCategory = productsFilteredByCategory.Where(prod => prod.SupplierID == searchBySupplierID);
            }
            var query = from prod in productsFilteredByCategory
                        select new ProductGridVM{
                            ID = prod.ProductID,
                            Name = prod.ProductName,
                            SupplierCompany = prod.Suppliers.CompanyName,
                            UnitPrice = prod.UnitPrice
                        };
            return query.ToList();     
        }

        private List<ProductGridBySupplierVM> GetProductGridsBySupplier(int supplierID) {
            var query = from prod in GetAllProducts()
                        where prod.SupplierID == supplierID
                        select new ProductGridBySupplierVM{
                            Name = prod.ProductName,
                            CategoryName = prod.Categories.CategoryName,
                            UnitPrice = prod.UnitPrice
                        };
            return query.ToList();
        }

        public ProductByCategoryVM GetProductByCategory(int categoryID, string searchByName, int searchBySupplierID) {
            var category = GetSingleCategory(categoryID);
            var productGrid = GetProductGrids(categoryID, searchByName, searchBySupplierID);
            var viewModel = new ProductByCategoryVM {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName,
                CategoryDescription = category.Description,
                ProductGrid = productGrid
            };
            return viewModel;
        }

        public ProductBySupplierVM GetProductBySupplier(int supplierID) {
            var supplier = GetSingleSupplier(supplierID);
            var productGrid = GetProductGridsBySupplier(supplierID);
            var viewModel = new ProductBySupplierVM
            {
                SupplierCompany = supplier.CompanyName,
                SupplierContact = supplier.ContactName,
                SupplierTitle = supplier.ContactTitle,
                ProductGrid = productGrid
            };
            return viewModel;
        }

        public void CreateCategory(CreateEditCategoryVM viewModel) {
            Categories categoryEntity = new Categories{
                CategoryName = viewModel.Name,
                Description = viewModel.Description
            };
            northwindContext.Categories.Add(categoryEntity);
            northwindContext.SaveChanges();
        }

        public void CreateSupplier(CreateEditSupplierVM viewModel){
            var supplierEntity = new Suppliers {
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
            northwindContext.Suppliers.Add(supplierEntity);
            northwindContext.SaveChanges();
        }

        //untuk mendapatkan data yang sekarang
        public CreateEditCategoryVM GetCategoryCurrentData(int categoryID) {
            var category = GetSingleCategory(categoryID);
            var viewModel = new CreateEditCategoryVM {
                ID = category.CategoryID,
                Name = category.CategoryName,
                Description = category.Description
            };
            return viewModel;
        }

        public CreateEditSupplierVM GetSupplierCurrentData(int supplierID) {
            var supplier = GetSingleSupplier(supplierID);
            var viewModel = new CreateEditSupplierVM {
                ID = supplier.SupplierID,
                Company = supplier.CompanyName,
                Contact = supplier.ContactName,
                Title = supplier.ContactTitle,
                Address = supplier.Address,
                City = supplier.City,
                Region = supplier.Region,
                PostalCode = supplier.PostalCode,
                Country = supplier.Country,
                Phone = supplier.Phone,
                Fax = supplier.Fax
            };
            return viewModel;
        }

        //untuk save data baru
        public void UpdateCategory(CreateEditCategoryVM viewModel) {
            var category = GetSingleCategory(viewModel.ID);
            category.CategoryName = viewModel.Name;
            category.Description = viewModel.Description;
            northwindContext.SaveChanges();
        }

        public void UpdateSupplier(CreateEditSupplierVM viewModel) {
            var supplier = GetSingleSupplier(viewModel.ID);
            supplier.CompanyName = viewModel.Company;
            supplier.ContactName = viewModel.Contact;
            supplier.ContactTitle = viewModel.Title;
            supplier.Address = viewModel.Address;
            supplier.City = viewModel.City;
            supplier.Region = viewModel.Region;
            supplier.PostalCode = viewModel.PostalCode;
            supplier.Country = viewModel.Country;
            supplier.Phone = viewModel.Phone;
            supplier.Fax = viewModel.Fax;
            northwindContext.SaveChanges();
        }

        public bool CategoryDependency(int categoryID) {
            var existingProduct = GetAllProducts().FirstOrDefault(prod => prod.CategoryID == categoryID);
            if (existingProduct != null) {
                return true;
            }
            return false;
        }

        public bool RemoveCategory(int categoryID) {
            if (!CategoryDependency(categoryID)) {
                var category = GetSingleCategory(categoryID);
                northwindContext.Categories.Remove(category);
                northwindContext.SaveChanges();
                return true;
            }
            return false;
        }

        public void SoftRemoveSupplier(int supplierID) {
            var supplier = GetSingleSupplier(supplierID);
            supplier.DeleteDate = DateTime.Now;
            northwindContext.SaveChanges();
        }

        public List<DropDownVM> GetSupplierDropDown() {
            var query = from sup in GetAllSuppliers()
                        select new DropDownVM {
                            Value = sup.SupplierID,
                            Text = sup.CompanyName
                        };
            return query.ToList();
        }

        public List<DropDownVM> GetCategoryDropDown() {
            var query = from cat in GetAllCategories()
                        select new DropDownVM {
                            Value = cat.CategoryID,
                            Text = cat.CategoryName
                        };
            return query.ToList();
        }

        public CreateEditProductVM GetCreateProduct() {
            var viewModel = new CreateEditProductVM();
            viewModel.SupplierDropDown = GetSupplierDropDown();
            viewModel.CategoryDropDown = GetCategoryDropDown();
            return viewModel;
        }

        public void CreateProduct(CreateEditProductVM viewModel) {
            var productEntity = new Products {
                ProductName = viewModel.Name,
                SupplierID = viewModel.SupplierID,
                CategoryID = viewModel.CategoryID,
                QuantityPerUnit = viewModel.Quantity,
                UnitPrice = viewModel.Price,
                UnitsInStock = viewModel.InStock,
                UnitsOnOrder = viewModel.OnOrder,
                ReorderLevel = viewModel.ReOrder,
                Discontinued = viewModel.Discontinued
            };
            northwindContext.Products.Add(productEntity);
            northwindContext.SaveChanges();
        }

        public Products GetSingleProduct(int productID) {
            return GetAllProducts().SingleOrDefault(prod => prod.ProductID == productID);
        }

        public CreateEditProductVM GetEditProduct(int productID) {
            var productEntity = GetSingleProduct(productID);
            var viewModel = new CreateEditProductVM {
                ID = productEntity.ProductID,
                Name = productEntity.ProductName,
                SupplierID = productEntity.SupplierID.Value,
                SupplierDropDown = GetSupplierDropDown(),
                CategoryID = productEntity.CategoryID.Value,
                CategoryDropDown = GetCategoryDropDown(),
                Quantity = productEntity.QuantityPerUnit,
                Price = productEntity.UnitPrice,
                InStock = productEntity.UnitsInStock,
                OnOrder = productEntity.UnitsOnOrder,
                ReOrder = productEntity.ReorderLevel,
                Discontinued = productEntity.Discontinued
            };
            return viewModel;
        }

        public void UpdateProduct(CreateEditProductVM viewModel) {
            var productEntity = GetSingleProduct(viewModel.ID);
            productEntity.ProductName = viewModel.Name;
            productEntity.SupplierID = viewModel.SupplierID;
            productEntity.CategoryID = viewModel.CategoryID;
            productEntity.QuantityPerUnit = viewModel.Quantity;
            productEntity.UnitPrice = viewModel.Price;
            productEntity.UnitsInStock = viewModel.InStock;
            productEntity.UnitsOnOrder = viewModel.OnOrder;
            productEntity.ReorderLevel = viewModel.ReOrder;
            productEntity.Discontinued = viewModel.Discontinued;
            northwindContext.SaveChanges();
        }

    }
}
