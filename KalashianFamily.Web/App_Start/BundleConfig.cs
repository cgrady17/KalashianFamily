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
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/KalashianFamily.js"));

            bundles.Add(new ScriptBundle("~/Assets/jQueryVal").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/Assets/CSS").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/bootstrap-datepicker3.css",
                        "~/Content/site.css"));
        }
    }
}