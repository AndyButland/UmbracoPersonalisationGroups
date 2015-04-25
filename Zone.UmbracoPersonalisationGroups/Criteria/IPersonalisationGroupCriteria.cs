namespace Zone.UmbracoPersonalisationGroups.Criteria
{
    /// <summary>
    /// Specifies an interface for a personalisation group criteria
    /// </summary>
    public interface IPersonalisationGroupCriteria
    {
        /// <summary>
        /// The name of the criteria
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The unique alias of the criteria
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// The description of the criteria
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The description of the syntax used for specifying the definition
        /// </summary>
        string DefinitionSyntaxDescription { get; }

        /// <summary>
        /// Flag for whether a definition editor view is provided
        /// </summary>
        bool HasDefinitionEditorView { get; }

        /// <summary>
        /// Checks whether the attributes of the current site visitor match the provided definition
        /// </summary>
        /// <param name="definition">Definition of the criteria to check</param>
        /// <returns></returns>
        bool MatchesVisitor(string definition);
    }
}
