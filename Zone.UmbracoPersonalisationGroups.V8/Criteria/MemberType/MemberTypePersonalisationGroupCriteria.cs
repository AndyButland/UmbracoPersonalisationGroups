namespace Zone.UmbracoPersonalisationGroups.V8.Criteria.MemberType
{
    using Zone.UmbracoPersonalisationGroups.Common.Criteria;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.MemberType;

    /// <summary>
    /// Implements a personalisation group criteria based on the presence, absence or value of a session key
    /// </summary>
    public class MemberTypePersonalisationGroupCriteria : MemberTypePersonalisationGroupCriteriaBase, IPersonalisationGroupCriteria
    {
        public MemberTypePersonalisationGroupCriteria()
            : this(new UmbracoMemberTypeProvider())
        {
        }

        public MemberTypePersonalisationGroupCriteria(IMemberTypeProvider memberTypeProvider)
            : base(memberTypeProvider)
        {
        }
    }
}
