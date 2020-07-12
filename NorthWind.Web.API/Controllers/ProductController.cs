using NorthWind.APIModel;
using NorthWind.Providers;
using NorthWind.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NorthWind.Web.API.Controllers
{
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {

        /*
            Bagaimana cara men-display XML Comment menjadi XML Documentation di halaman API Documentation:

            1. Project > Properties > Build (Tab) > Checked XML Documentation File > "App_Data\Documentation.xml" 
            2. Areas > Help Page > App_Start > HelpPageConfig.cs > Uncomment config.SetDocumentationProvider
                MapPath("~/App_Data/Documentation.xml")
        */
        private ProductProvider provider;

        public ProductController() {
            this.provider = new ProductProvider();
        }

        /// <summary>
        /// URL untuk mendapatkan list all categories
        /// </summary>
        /// <returns>Munculkan seluruh kategori</returns>
        [Route("AllCategories")]
        [HttpGet]
        public List<CategoryAM> AllCategories() {
            var allCategories = provider.GetCategoriesForAPI();
            return allCategories;
        }

        /// <summary>
        /// URL untuk mendapatkan 1 kategori yang diinginkan dengan menggunakan ID.
        /// </summary>
        /// <param name="categoryID">PK dari Category Data</param>
        /// <returns>object dengan tipe data CategoryAM</returns>
        [Route("SingleCategory/cat={categoryID:int}")]
        [HttpGet]
        public CategoryAM SelectedCategory(int categoryID) {
            var category = provider.GetSingleCategoryAM(categoryID);
            return category;
        }

        /// <summary>
        /// URL untuk mendapatkan products berdasarkan id category dan komposisi nama produk
        /// </summary>
        /// <param name="categoryID">PK dari header Kategori</param>
        /// <param name="productName">Nama dari produk</param>
        /// <returns>beberapa produk dari hasil filter</returns>
        [Route("SelectedProducts/cat={categoryID:int}/prod={productName}")]
        [HttpGet]
        public List<ProductAM> SelectedProducts(int categoryID, string productName) {
            var products = provider.GetSelectedProductAM(categoryID, productName);
            return products;
        }

        /// <summary>
        /// URL untuk mendapatkan products by categoryID
        /// </summary>
        /// <param name="categoryID">PK dari category</param>
        /// <returns>beberapa produk sesuai kategori ID</returns>
        [Route("SelectedProducts/cat={categoryID:int}")]
        [HttpGet]
        public List<ProductAM> SelectedProducts(int categoryID) {
            var products = provider.GetSelectedProductAM(categoryID, "");
            return products;
        }

        /// <summary>
        /// Membuat Category Baru dengan API Post Method
        /// </summary>
        /// <param name="model">object category pada http Body</param>
        /// <returns>Konfirmasi true untuk berhasil</returns>
        [Route("CreateCategory")]
        [HttpPost]
        public bool CreateCategory(CreateEditCategoryVM model) {
            try {
                provider.CreateCategory(model);
                return true;
            } catch(Exception exception) {
                return false;
            }
        }
    }
}
