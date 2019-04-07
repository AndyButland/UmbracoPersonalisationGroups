namespace Zone.UmbracoPersonalisationGroups.Common.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;

    /// <summary>
    /// Controller making available country & region details to HTTP requests
    /// </summary>
    public class GeoLocationController : BaseJsonResultController
    {
        /// <summary>
        /// Gets a JSON list of the available continents
        /// </summary>
        /// <returns>JSON response of available criteria</returns>
        public JsonResult GetContinents()
        {
            var cacheKey = $"PersonalisationGroups_GeoLocation_Continents";
            var countries = RuntimeCacheHelper.GetCacheItem(cacheKey,
                () =>
                    {
                        var assembly = GetResourceAssembly();
                        var resourceName = GetResourceName("continents");
                        using (var stream = assembly.GetManifestResourceStream(resourceName))
                        {
                            if (stream == null)
                            {
                                return null;
                            }

                            using (var reader = new StreamReader(stream))
                            {
                                var continentRecords = reader.ReadToEnd()
                                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(x => new
                                        {
                                            code = x.Split(',')[0],
                                            name = CleanName(x.Split(',')[1])
                                        });

                                continentRecords = continentRecords.OrderBy(x => x.name);

                                return continentRecords;
                            }
                        }
                    });

            return Json(countries, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets a JSON list of the available countries
        /// </summary>
        /// <returns>JSON response of available criteria</returns>
        public JsonResult GetCountries(bool withRegionsOnly = false)
        {
            var cacheKey = $"PersonalisationGroups_GeoLocation_Countries_{withRegionsOnly}";
            var countries = RuntimeCacheHelper.GetCacheItem(cacheKey, 
                () =>
                    {
                        var assembly = GetResourceAssembly();
                        var resourceName = GetResourceName("countries");
                        using (var stream = assembly.GetManifestResourceStream(resourceName))
                        {
                            if (stream == null)
                            {
                                return null;
                            }

                            using (var reader = new StreamReader(stream))
                            {
                                var countryRecords = reader.ReadToEnd()
                                    .Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(x => new
                                    {
                                        code = x.Split(',')[0],
                                        name = CleanName(x.Split(',')[1])
                                    });

                                if (withRegionsOnly)
                                {
                                    var countryCodesWithRegions = GetCountryCodesWithRegions(assembly);
                                    countryRecords = countryRecords
                                        .Where(x => countryCodesWithRegions.Contains(x.code));
                                }

                                countryRecords = countryRecords.OrderBy(x => x.name);

                                return countryRecords;
                            }
                        }
                    });

            return Json(countries, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets a JSON list of the available regions for a given country
        /// </summary>
        /// <param name="countryCode">Country code</param>
        /// <returns>JSON response of available criteria</returns>
        public JsonResult GetRegions(string countryCode)
        {
            var cacheKey = $"PersonalisationGroups_GeoLocation_Regions_{countryCode}";
            var regions = RuntimeCacheHelper.GetCacheItem(cacheKey,
                () =>
                {
                    var assembly = GetResourceAssembly();
                    var resourceName = GetResourceName("regions");

                    using (var stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        if (stream == null)
                        {
                            return null;
                        }

                        using (var reader = new StreamReader(stream))
                        {
                            var regionRecords = reader.ReadToEnd()
                                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                .Where(x => x.Split(',')[0] == countryCode.ToUpperInvariant())
                                .Select(x => new
                                {
                                    code = x.Split(',')[1],
                                    name = CleanName(x.Split(',')[2])
                                })
                                .OrderBy(x => x.name);
                            return regionRecords;
                        }
                    }
                });

            return Json(regions, JsonRequestBehavior.AllowGet);
        }

        private static Assembly GetResourceAssembly()
        {
            return Assembly.Load(AppConstants.CommonAssemblyName);
        }


        private static string GetResourceName(string area)
        {
            return $"{AppConstants.CommonAssemblyName}.Data.{area}.txt";
        }

        private static string CleanName(string name)
        {
            return name.Replace("\"", string.Empty).Trim();
        }

        private static IEnumerable<string> GetCountryCodesWithRegions(Assembly assembly)
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
    }
}
