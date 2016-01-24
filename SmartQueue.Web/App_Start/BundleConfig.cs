using System.Web.Optimization;

namespace SmartQueue.Web
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/backstretch").Include(
                                    "~/Scripts/jquery.backstretch.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/gritter").Include(
                    "~/Scripts/gritter/js/jquery.gritter.js").Include(
                    "~/Scripts/gritter-conf.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                    "~/Scripts/jquery.dcjqaccordion.2.7.js").Include(
                    "~/Scripts/jquery.scrollTo.min.js").Include(
                    "~/Scripts/jquery.nicescroll.js").Include(
                    "~/Scripts/jquery.sparkline.js").Include(
                    "~/Scripts/common-scripts.js",
                    "~/Scripts/sweetalert.min.js").Include(
                    "~/Scripts/jquery.attachFooter.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство сборки на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/form").Include(
                    "~/Scripts/form-component.js",
                    "~/Scripts/bootstrap-switch.js",
                    "~/Scripts/jquery.tagsinput.js"/*,
                    "~/Scripts/bootstrap-datepicker/js/bootstrap-datepicker.js",
                    "~/Scripts/bootstrap-daterangepicker/daterangepicker.js",
                    "~/Scripts/bootstrap-inputmask/bootstrap-inputmask.min.js"*/
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/font-awesome/css/font-awesome.css",
                      "~/Scripts/gritter/css/jquery.gritter.css",
                      "~/Content/style.css",
                      "~/Content/style-responsive.css",
                      "~/Content/sweetalert.css"));
        }
    }
}
