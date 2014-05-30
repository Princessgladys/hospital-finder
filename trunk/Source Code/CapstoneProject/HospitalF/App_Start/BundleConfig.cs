using System.Web;
using System.Web.Optimization;

namespace HospitalF
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js").Include(
                        "~/Scripts/jquery-{version}.js"));           

            bundles.Add(new StyleBundle("~/css").Include(
                        "~/Content/css/bootstrap.min.css",
                        "~/Content/css/font-awesome.min.css",
                        "~/Content/css/smartadmin-production.css",
                        "~/Content/css/smartadmin-skins.css"));
        }
    }
}