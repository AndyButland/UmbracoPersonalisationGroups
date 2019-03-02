namespace Zone.UmbracoPersonalisationGroups.Common.Helpers
{
    using System;
 
    public static class Mandate
    {
        public static void ParameterNotNull(object paramValue, string paramName)
        {
            if (paramValue == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void ParameterNotNullOrEmpty(string paramValue, string paramName)
        {
            if (string.IsNullOrEmpty(paramValue))
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
