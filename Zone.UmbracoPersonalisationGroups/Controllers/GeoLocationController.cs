namespace Zone.UmbracoPersonalisationGroups.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;

    /// <summary>
    /// Controller making available country & region details to HTTP requests
    /// </summary>
    public class GeoLocationController : BaseJsonResultController
    {
        /// <summary>
        /// Gets a JSON list of the available countries
        /// </summary>
        /// <returns>JSON response of available criteria</returns>
        public JsonResult GetCountries()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string ResourceName = "Zone.UmbracoPersonalisationGroups.Data.countries.txt";

            using (var stream = assembly.GetManifestResourceStream(ResourceName))
            {
                if (stream == null)
                {
                    return null;
                }

                using (var reader = new StreamReader(stream))
                {
                    var countries = reader.ReadToEnd()
                        .Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => new
                        {
                            code = x.Split(',')[0],
                            name = x.Split(',')[1]
                        })
                        .OrderBy(x => x.name);
                    return Json(countries, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// Gets a JSON list of the available regions for a given country
        /// </summary>
        /// <param name="countryCode">Country code</param>
        /// <returns>JSON response of available criteria</returns>
        public JsonResult GetRegions(string countryCode)
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string ResourceName = "Zone.UmbracoPersonalisationGroups.Data.regions.txt";

            using (var stream = assembly.GetManifestResourceStream(ResourceName))
            {
                if (stream == null)
                {
                    return null;
                }

                using (var reader = new StreamReader(stream))
                {
                    var regions = reader.ReadToEnd()
                        .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(x => x.Split(',')[0] == countryCode.ToUpperInvariant())
                        .Select(x => new
                        {
                            code = x.Split(',')[1],
                            name = x.Split(',')[2]
                        })
                        .OrderBy(x => x.name);
                    return Json(regions, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}
