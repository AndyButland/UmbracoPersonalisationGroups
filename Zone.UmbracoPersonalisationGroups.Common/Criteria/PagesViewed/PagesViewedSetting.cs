namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.PagesViewed
{
    public enum PagesViewedSettingMatch
    {
        ViewedAny,
        ViewedAll,
        NotViewedAny,
        NotViewedAll,
    }

    public class PagesViewedSetting
    {
        public PagesViewedSettingMatch Match { get; set; }

        public int[] NodeIds { get; set; }
    }
}
