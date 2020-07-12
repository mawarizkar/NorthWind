using System.Web;
using System.Web.Optimization;

namespace NorthWind.Web {
    public class BundleConfig {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/javascript").Include(
                        "~/Scripts/jquery-3.5.0.js",
                        "~/Scripts/my-script.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/normalize.css",
                      "~/Content/font-awesome.css",
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/login").Include(
                      "~/Content/normalize.css",
                      "~/Content/font-awesome.css",
                      "~/Content/login.css"));
        }
    }
}
