namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.Referral
{
    using System.Web;

    public class HttpContextReferrerProvider : IReferrerProvider
    {
        public string GetReferrer()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
            {
                return HttpContext.Current.Request.UrlReferrer.AbsoluteUri;
            }

            return string.Empty;
        }
    }
}
