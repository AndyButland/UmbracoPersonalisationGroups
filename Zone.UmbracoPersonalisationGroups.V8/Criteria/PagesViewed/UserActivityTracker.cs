namespace Zone.UmbracoPersonalisationGroups.V8.Criteria.PagesViewed
{
    using System;
    using Umbraco.Web.Composing;

    public static class UserActivityTracker
    {
        public static void TrackPageView(object sender, EventArgs e)
        {
            var umbracoContext = Current.UmbracoContext;
            var isFrontEndRequest = umbracoContext?.IsFrontEndUmbracoRequest ?? false;
            if (!isFrontEndRequest)
            {
                return;
            }

            var pageId = umbracoContext?.PublishedRequest?.PublishedContent?.Id;
            if (pageId == null)
            {
                return;
            }

            Common.Criteria.PagesViewed.UserActivityTracker.TrackPageView(pageId.Value);
        }
    }
}