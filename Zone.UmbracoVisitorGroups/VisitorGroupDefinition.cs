namespace Zone.UmbracoVisitorGroups
{
    using System.Collections.Generic;

    public enum VisitorGroupDefinitionMatch
    {
        All,
        Any
    }

    public class VisitorGroupDefinition
    {
        public VisitorGroupDefinitionMatch Match { get; set; }

        public IEnumerable<VisitorGroupDefinitionDetail> Details { get; set; }
    }
}
