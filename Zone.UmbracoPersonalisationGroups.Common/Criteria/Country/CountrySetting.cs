namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.Country
{
    using System.Collections.Generic;

    public class CountrySetting
    {
        public GeoLocationSettingMatch Match { get; set; }

        public List<string> Codes { get; set; }
    }
}
