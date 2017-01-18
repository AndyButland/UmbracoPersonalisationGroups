namespace Zone.UmbracoPersonalisationGroups.Controllers
{
    using System;
    using System.Collections.Generic;
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
        public JsonResult GetCountries(bool withRegionsOnly = false)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = GetResourceName("countries");
            using (var stream = assembly.GetManifestResourceStream(resourceName))
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
                            name = CleanName(x.Split(',')[1])
                        });

                    if (withRegionsOnly)
                    {
                        var countryCodesWithRegions = GetCountryCodesWithRegions(assembly);
                        countries = countries
                            .Where(x =>countryCodesWithRegions.Contains(x.code));
                    }

                    countries = countries.OrderBy(x => x.name);

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
            var resourceName = GetResourceName("regions");

            using (var stream = assembly.GetManifestResourceStream(resourceName))
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
                            name = CleanName(x.Split(',')[2])
                        })
                        .OrderBy(x => x.name);
                    return Json(regions, JsonRequestBehavior.AllowGet);
                }
            }
        }

        private IEnumerable<string> GetCountryCodesWithRegions(Assembly assembly)
        {
            var resourceName = GetResourceName("regions");
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return new string[0];
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd()
                        .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Split(',')[0])
                        .Distinct()
                        .ToArray();
                }
            }
        }

        private string GetResourceName(string area)
        {
            return $"Zone.UmbracoPersonalisationGroups.Data.{area}.txt";
        }

        private string CleanName(string name)
        {
            return name.Replace("\"", string.Empty).Trim();
        }
    }
}
