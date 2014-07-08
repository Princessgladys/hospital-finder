using System.Web;
using System.Web.Optimization;

namespace HospitalF
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            // Bundle for Home layout
            bundles.Add(new StyleBundle("~/Content/HomeLayout").Include(
                        "~/Content/css/bootstrap.m.css",
                        "~/Content/css/font-awesome.m.css",
                        "~/Content/css/smartadmin-production.css",
                        "~/Content/css/smartadmin-skins.css",
                        "~/Content/css/custom-home.css",
                        "~/Content/css/custom-font.css",
                        "~/Content/css/custom-search-result.css"));

            // Bundle for Admin and Hospital User layout
            bundles.Add(new StyleBundle("~/Content/AdminLayout").Include(
                        "~/Content/css/bootstrap.m.css",
                        "~/Content/css/font-awesome.m.css",
                        "~/Content/css/smartadmin-production.m.css",
                        "~/Content/css/smartadmin-skins.m.css",
                        "~/Content/css/custom-font.css"));

            // Bundle for Error page
            bundles.Add(new StyleBundle("~/Content/Error").Include(
                        "~/Content/css/error.css"));

        }
    }
}