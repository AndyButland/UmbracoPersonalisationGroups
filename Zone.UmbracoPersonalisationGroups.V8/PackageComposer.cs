namespace Zone.UmbracoPersonalisationGroups.V8
{
    using Umbraco.Core;
    using Umbraco.Core.Composing;
    using Zone.UmbracoPersonalisationGroups.Common.Controllers;
    using Zone.UmbracoPersonalisationGroups.V8.Controllers;
    using Zone.UmbracoPersonalisationGroups.V8.Criteria.NumberOfVisits;
    using Zone.UmbracoPersonalisationGroups.V8.Criteria.PagesViewed;
    
    public class PackageComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<RoutingComponent>();

            ComposeCriteriaComponents(composition);

            RegisterControllers(composition);
        }

        private static void ComposeCriteriaComponents(Composition composition)
        {
            composition.Components().Append<NumberOfVisitsComponent>();
            composition.Components().Append<PagesViewedComponent>();
        }

        private static void RegisterControllers(IRegister composition)
        {
            composition.Register<CriteriaController>();
            composition.Register<MemberController>();
            composition.Register<GeoLocationController>();
            composition.Register<ResourceController>();
        }
    }
}