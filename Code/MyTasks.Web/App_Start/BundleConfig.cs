using System.Web;
using System.Web.Optimization;

namespace MyTasks.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                  "~/Scripts/jquery-ui-1.12.1.min.js"));

            //TODO: Add version of jquery ui what is comming from Nice Admin template.

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/lodash").Include(
                      "~/Scripts/lodash.core.min.js",
                      "~/Scripts/lodash.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/summernote").Include(
             "~/Scripts/summernote.js"
             ));

            bundles.Add(new ScriptBundle("~/bundles/niceadmin").Include(
          "~/Scripts/adminTemplate/jquery.scrollTo.min.js",
          //"~/Scripts/adminTemplate/jquery.nicescroll.js",
          "~/Scripts/adminTemplate/scripts.js"
          ));



            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/font-awesome.css"));


            bundles.Add(new StyleBundle("~/Content/niceadmin").Include(
                                        "~/Content/adminTemplate/line-icons.css",
                                        "~/Content/adminTemplate/elegant-icons-style.css",
                                        "~/Content/adminTemplate/style.css",
                                        "~/Content/adminTemplate/style-responsive.css",
                                        "~/Content/adminTemplate/validations.css",
                                        "~/Content/themes/base/all.css"
            ));


            bundles.Add(new StyleBundle("~/Content/summernote").Include(
                "~/Content/summernote.css"
                ));

        
        

           


        }
    }
}
