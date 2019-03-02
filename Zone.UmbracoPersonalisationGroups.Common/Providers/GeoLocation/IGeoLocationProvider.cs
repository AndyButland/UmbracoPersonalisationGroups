namespace Zone.UmbracoPersonalisationGroups.Common.Providers.GeoLocation
{
    public interface IGeoLocationProvider
    {
        Country GetCountryFromIp(string ip);

        Region GetRegionFromIp(string ip);
    }
}
