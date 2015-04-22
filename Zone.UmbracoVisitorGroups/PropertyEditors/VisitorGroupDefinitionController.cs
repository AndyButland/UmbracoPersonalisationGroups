namespace Zone.UmbracoVisitorGroups.PropertyEditors
{
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class VisitorGroupDefinitionController : Controller
    {
        public ContentResult AvailableCriteria()
        {
            var criteria = VisitorGroupMatcher.GetAvailableCriteria();
            var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var json = JsonConvert.SerializeObject(criteria, Formatting.Indented, jsonSerializerSettings);

            return Content(json, "application/json");
        }

        public FileStreamResult Resource(string fileName)
        {
            var resourceName =
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceNames()
                    .ToList()
                    .FirstOrDefault(f => f.EndsWith(fileName));

            var assembly = typeof(VisitorGroupDefinitionController).Assembly;

            return new FileStreamResult(assembly.GetManifestResourceStream(resourceName), GetMimeType(fileName));
        }

        public FileStreamResult Resource(string criteriaAlias, string fileName)
        {
            var criteria = VisitorGroupMatcher.GetAvailableCriteria()
                .SingleOrDefault(x => x.Alias == criteriaAlias);
            if (criteria != null)
            {
                var resourceName =
                    criteria.GetType().Assembly
                        .GetManifestResourceNames()
                        .ToList()
                        .FirstOrDefault(f => f.EndsWith(fileName));

                var assembly = criteria.GetType().Assembly;

                return new FileStreamResult(assembly.GetManifestResourceStream(resourceName), GetMimeType(fileName));
            }

            return null;
        }

        private string GetMimeType(string fileName)
        {
            if (fileName.EndsWith(".js"))
            {
                return "text/javascript";
            }

            if (fileName.EndsWith(".html"))
            {
                return "text/html";
            }

            if (fileName.EndsWith(".css"))
            {
                return "text/stylesheet";
            }

            return "text/plain";
        }
    }
}
