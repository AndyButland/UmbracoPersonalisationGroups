namespace Zone.UmbracoPersonalisationGroups.Criteria.PagesViewed
{
    using System;
    using Umbraco.Web;

    public static class UserActivityTracker
    {
        public static void TrackPageView(object sender, EventArgs e)
        {
            var umbracoContext = UmbracoContext.Current;
            var isFrontEndRequest = umbracoContext?.IsFrontEndUmbracoRequest ?? false;
            if (!isFrontEndRequest)
            {
                return;
            }

            var pageId = umbracoContext?.PageId;
            if (pageId == null)
            {
                return;
            }

            Common.Criteria.PagesViewed.UserActivityTracker.TrackPageView(pageId.Value);
        }
    }
}