using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Optimization;

namespace Prototype
{
    /// <summary>
    /// Bundle設定クラス。
    /// </summary>
    internal class BundleConfig
    {
        /// <summary>
        /// Bundleを登録します。
        /// </summary>
        /// <param name="bundles">Bundleコレクション</param>
        /// <remarks>バンドルの詳細については、http://go.microsoft.com/fwlink/?LinkId=301862 を参照してください</remarks>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/bootstrap-datepicker3.css",
                      "~/Content/bootstrap-multiselect.css",
                      "~/Content/jquery.simplecolorpicker.css",
                      "~/Content/jquery.simplecolorpicker-regularfont.css",
                      "~/Content/jquery.simplecolorpicker-glyphicons.css",
                      "~/Content/jquery.simplecolorpicker-fontawesome.css",
                      "~/Content/non-responsive.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.simplecolorpicker.js",
                        "~/Scripts/jquery.blockUI.js"));

            bundles.Add(new ScriptBundle("~/bundles/microsoftajax").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/MicrosoftAjax.js",
                        "~/Scripts/MicrosoftMvcAjax.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/non-responsive.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/bootstrap-datepicker.ja.min.js",
                      "~/Scripts/bootstrap-multiselect.js",
                      "~/Scripts/bootstrap-simplefileupload.js"));

            bundles.Add(new ScriptBundle("~/bundles/prototype").Include(
                      "~/Scripts/prototype-common.js",
                      "~/Scripts/prototype-message-data.js",
                      "~/Scripts/prototype-message.js",
                      "~/Scripts/prototype-validation.js",
                      "~/Scripts/prototype-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/startup").Include(
                      "~/Scripts/startup.js"));

            // 画面固有スクリプトの登録
            RegisterBundlesViewScripts(bundles);
        }

        /// <summary>
        /// バンドルに画面固有スクリプトを登録します。
        /// </summary>
        /// <param name="bundles">バンドルの格納先</param>
        private static void RegisterBundlesViewScripts(BundleCollection bundles)
        {
            var files = Directory.GetDirectories(HostingEnvironment.MapPath("~/Scripts"))
                .SelectMany(x => Directory.GetFiles(x))
                .Select(x => new FileInfo(x));
            foreach (var file in files)
            {
                var dirName = file.Directory.Name;
                var baseName = Path.GetFileNameWithoutExtension(file.Name);
                bundles.Add(new ScriptBundle($"~/bundles/{dirName}/{baseName}").Include(
                    $"~/Scripts/{dirName}/{file.Name}"));
            }
        }
    }
}