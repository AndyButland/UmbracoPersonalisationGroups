namespace Zone.UmbracoVisitorGroups.VisitorGroupCriteria
{
    public interface IVisitorGroupCriteria
    {
        string Name { get; }

        string Alias { get; }

        string Description { get; }

        string DefinitionSyntaxDescription { get; }

        bool HasDefinitionEditorView { get; }

        bool MatchesVisitor(string definition);
    }
}
