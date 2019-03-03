namespace Zone.UmbracoPersonalisationGroups.V8.Criteria.MemberProfileField
{
    using Zone.UmbracoPersonalisationGroups.Common.Criteria;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.MemberProfileField;

    /// <summary>
    /// Implements a personalisation group criteria based on the presence, absence or value of a session key
    /// </summary>
    public class MemberProfileFieldPersonalisationGroupCriteria : MemberProfileFieldPersonalisationGroupCriteriaBase, IPersonalisationGroupCriteria
    {
        public MemberProfileFieldPersonalisationGroupCriteria()
            : this(new UmbracoMemberProfileFieldProvider())
        {
        }

        public MemberProfileFieldPersonalisationGroupCriteria(IMemberProfileFieldProvider memberProfileFieldProvider)
            : base(memberProfileFieldProvider)
        {
        }
    }
}
