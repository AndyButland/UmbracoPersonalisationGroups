namespace Zone.UmbracoPersonalisationGroups.Common.ExtensionMethods
{
    using System.Collections.Generic;
    using System.Linq;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;

    public static class EnumerableExtensions
    {
        public static bool ContainsAll<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other)
        {
            Mandate.ParameterNotNull(source, nameof(source));
            Mandate.ParameterNotNull(other, nameof(other));
            return !other.Except(source).Any();
        }

        public static bool ContainsAny<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other)
        {
            Mandate.ParameterNotNull(source, nameof(source));
            Mandate.ParameterNotNull(other, nameof(other));
            return other.Any(source.Contains);
        }
    }
}
