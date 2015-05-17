using System.Web;
using System.Web.Optimization;

namespace MicheleCaggiano
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

      // Use the development version of Modernizr to develop with and learn from. Then, when you're
      // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
      bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                  "~/Scripts/modernizr-*"));

      bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

      bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
                "~/Scripts/tinymce/tinymce.js",
                "~/Scripts/tinymce/langs/it.js",
                "~/Scripts/chosen.jquery.min.js"));


      // CSS
      bundles.Add(new StyleBundle("~/Content/css").Include(
        "~/Content/bootstrap/bootstrap.min.css",
        "~/Content/bootswatch.min.css",
        "~/Content/font-awesome.css",
        "~/Content/chosen.min.css",
        "~/Content/site.min.css"));

      // Set EnableOptimizations to false for debugging. For more information,
      // visit http://go.microsoft.com/fwlink/?LinkId=301862
      BundleTable.EnableOptimizations = false;
    }
  }
}
