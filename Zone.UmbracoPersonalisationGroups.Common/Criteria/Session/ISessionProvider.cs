namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.Session
{
    public interface ISessionProvider
    {
        bool KeyExists(string key);

        string GetValue(string key);
    }
}
