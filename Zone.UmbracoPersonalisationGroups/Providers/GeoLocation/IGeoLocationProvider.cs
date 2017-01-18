namespace Zone.UmbracoPersonalisationGroups.Providers.GeoLocation
{
    public interface IGeoLocationProvider
    {
        Country GetCountryFromIp(string ip);

        Region GetRegionFromIp(string ip);
    }
}
