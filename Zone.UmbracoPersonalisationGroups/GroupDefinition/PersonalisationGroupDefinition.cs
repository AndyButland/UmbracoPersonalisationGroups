namespace Zone.UmbracoPersonalisationGroups
{
    using System.Collections.Generic;

    public enum PersonalisationGroupDefinitionMatch
    {
        All,
        Any
    }

    public enum PersonalisationGroupDefinitionDuration
    {
        Page,
        Session,
        Visitor
    }

    /// <summary>
    /// The definition of a personalisation group
    /// </summary>
    public class PersonalisationGroupDefinition
    {
        public PersonalisationGroupDefinitionMatch Match { get; set; }

        public PersonalisationGroupDefinitionDuration Duration { get; set; }

        public IEnumerable<PersonalisationGroupDefinitionDetail> Details { get; set; }
    }
}
