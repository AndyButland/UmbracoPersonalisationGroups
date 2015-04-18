namespace Zone.UmbracoVisitorGroups
{
    public interface IVisitorGroupCriteria
    {
        string Name { get; }

        string Alias { get; }

        string Description { get; }

        string DefinitionSyntaxDescription { get; }

        bool MatchesVisitor(string definition);
    }
}
