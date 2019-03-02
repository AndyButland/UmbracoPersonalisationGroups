namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.NumberOfVisits
{
    using System;
    using System.Web;
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;

    public static class UserActivityTracker
    {
        public static void TrackSession(object sender, EventArgs e)
        {
            var httpContext = HttpContext.Current;
            var config = PersonalisationGroupsConfig.Value;

            // Check if session cookie present
            var sessionCookie = httpContext.Request.Cookies[config.CookieKeyForTrackingIfSessionAlreadyTracked];
            if (sessionCookie == null)
            {
                // If not, create or update the number of visits cookie
                var trackingCookie = httpContext.Request.Cookies[config.CookieKeyForTrackingNumberOfVisits];
                if (trackingCookie != null)
                {
                    trackingCookie.Value = int.TryParse(trackingCookie.Value, out int cookieValue) ? (cookieValue + 1).ToString() : "1";
                }
                else
                {
                    trackingCookie = new HttpCookie(config.CookieKeyForTrackingNumberOfVisits)
                        {
                            Value = "1",
                            HttpOnly = true,
                        };
                }

                trackingCookie.Expires = DateTime.Now.AddDays(config.NumberOfVisitsTrackingCookieExpiryInDays);
                httpContext.Response.Cookies.Add(trackingCookie);

                // Set the session cookie so we don't keep updating on each request
                sessionCookie = new HttpCookie(config.CookieKeyForTrackingIfSessionAlreadyTracked)
                    {
                        Value = "1",
                        HttpOnly = true,
                    };
                httpContext.Response.Cookies.Add(sessionCookie);
            }
        }
    }
}