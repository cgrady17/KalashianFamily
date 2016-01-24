using System.Web.Optimization;

namespace KalashianFamily.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Assets/Scripts").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.form.js",
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/Assets/jQueryVal").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/Assets/CSS").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/site.css"));
        }
    }
}