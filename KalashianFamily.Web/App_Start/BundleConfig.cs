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

            bundles.Add(new StyleBundle("~/Content/EJ").Include(
                        "~/Content/EJ/web/ej.widgets.core.min.css",
                        //"~/Content/EJ/default-theme/ej.web.all.css",
                        //"~/Content/EJ/default-theme/ej.widgets.all.css",
                        "~/Content/EJ/web/default-theme/ej.theme.min.css"));

            bundles.Add(new ScriptBundle("~/Scripts/EJ").Include(
                        "~/Scripts/jquery.easing.1.3.js",
                        "~/Scripts/jsrender.min.js",
                        "~/Scripts/EJ/web/ej.web.all.min.js"));
        }
    }
}