namespace Zone.UmbracoVisitorGroups
{
    using System.Collections.Generic;

    public enum VisitorGroupDefinitionMatch
    {
        All,
        Any
    }

    /// <summary>
    /// The definition of a visitor group
    /// </summary>
    public class VisitorGroupDefinition
    {
        public VisitorGroupDefinitionMatch Match { get; set; }

        public IEnumerable<VisitorGroupDefinitionDetail> Details { get; set; }
    }
}
