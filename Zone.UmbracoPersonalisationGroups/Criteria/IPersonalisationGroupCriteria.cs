namespace Zone.UmbracoPersonalisationGroups.Criteria
{
    /// <summary>
    /// Specifies an interface for a personalisation group criteria
    /// </summary>
    public interface IPersonalisationGroupCriteria
    {
        /// <summary>
        /// Gets the name of the criteria
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the unique alias of the criteria
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// Gets the description of the criteria
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Checks whether the attributes of the current site visitor match the provided definition
        /// </summary>
        /// <param name="definition">Definition of the criteria to check</param>
        /// <returns>True if definition matches visitor</returns>
        bool MatchesVisitor(string definition);
    }
}
