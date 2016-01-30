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
                        "~/Scripts/jquery.dynamiclist.js",
                        "~/Scripts/flipclock.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/KalashianFamily.js"));

            bundles.Add(new ScriptBundle("~/Assets/jQueryVal").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/Content/CSS").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/bootstrap-datepicker3.css",
                        "~/Content/flipclock.css",
                        "~/Content/site.css"));
        }
    }
}