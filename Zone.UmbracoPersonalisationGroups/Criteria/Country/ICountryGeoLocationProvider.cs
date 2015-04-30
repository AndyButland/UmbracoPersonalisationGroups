namespace Zone.UmbracoPersonalisationGroups.Criteria.Country
{
    public interface ICountryGeoLocationProvider
    {
        string GetCountryFromIp(string ip);
    }
}
