namespace Zone.UmbracoPersonalisationGroups.Criteria.Country
{
    using System.Collections.Generic;

    public enum CountrySettingMatch
    {
        IsLocatedIn,
        IsNotLocatedIn,
    }

    public class CountrySetting
    {
        public CountrySettingMatch Match { get; set; }

        public List<string> Codes { get; set; }
    }
}
