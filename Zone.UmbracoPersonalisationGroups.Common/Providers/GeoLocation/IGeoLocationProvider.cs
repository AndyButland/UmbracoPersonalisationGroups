namespace Zone.UmbracoPersonalisationGroups.Common.Providers.GeoLocation
{
    public interface IGeoLocationProvider
    {
        Continent GetContinentFromIp(string ip);

        Country GetCountryFromIp(string ip);

        Region GetRegionFromIp(string ip);
    }
}
