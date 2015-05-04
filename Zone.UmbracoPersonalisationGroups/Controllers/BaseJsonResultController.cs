namespace Zone.UmbracoPersonalisationGroups.Controllers
{
    using System.Web.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Base class for controller making available details to HTTP requests
    /// </summary>
    public abstract class BaseJsonResultController : Controller
    {
        protected ContentResult CamelCasedJsonResult(object result)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var json = JsonConvert.SerializeObject(result, Formatting.Indented, jsonSerializerSettings);

            return Content(json, "application/json");
        }
    }
}
