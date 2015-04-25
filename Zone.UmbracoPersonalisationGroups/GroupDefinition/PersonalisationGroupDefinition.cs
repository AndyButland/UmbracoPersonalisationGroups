namespace Zone.UmbracoPersonalisationGroups
{
    using System.Collections.Generic;

    public enum PersonalisationGroupDefinitionMatch
    {
        All,
        Any
    }

    /// <summary>
    /// The definition of a personalisation group
    /// </summary>
    public class PersonalisationGroupDefinition
    {
        public PersonalisationGroupDefinitionMatch Match { get; set; }

        public IEnumerable<PersonalisationGroupDefinitionDetail> Details { get; set; }
    }
}
