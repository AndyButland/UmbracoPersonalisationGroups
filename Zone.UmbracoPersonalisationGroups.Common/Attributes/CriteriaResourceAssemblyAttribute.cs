namespace Zone.UmbracoPersonalisationGroups.Common.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class CriteriaResourceAssemblyAttribute : Attribute
    {
        public string AssemblyName { get; set; }
    }
}
