namespace Zone.UmbracoPersonalisationGroups.Criteria.Session
{
    public interface ISessionProvider
    {
        bool KeyExists(string key);

        string GetValue(string key);
    }
}
