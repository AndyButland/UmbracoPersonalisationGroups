namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.NumberOfVisits
{
    using System;
    using System.Web;
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.Cookie;

    public static class UserActivityTracker
    {
        public static void TrackSession(object sender, EventArgs e)
        {
            var config = PersonalisationGroupsConfig.Value;
            var cookieProvider = new HttpContextCookieProvider();

            // Check if session cookie present
            var sessionCookie = cookieProvider.GetCookie(config.CookieKeyForTrackingIfSessionAlreadyTracked);
            if (sessionCookie == null)
            {
                // If not, create or update the number of visits cookie
                var trackingCookie = cookieProvider.GetCookie(config.CookieKeyForTrackingNumberOfVisits);
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
                cookieProvider.SetCookie(trackingCookie);

                // Set the session cookie so we don't keep updating on each request
                sessionCookie = new HttpCookie(config.CookieKeyForTrackingIfSessionAlreadyTracked)
                    {
                        Value = "1",
                        HttpOnly = true,
                    };
                cookieProvider.SetCookie(sessionCookie);
            }
        }
    }
}