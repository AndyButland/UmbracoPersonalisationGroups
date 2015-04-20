namespace Zone.UmbracoVisitorGroups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Zone.UmbracoVisitorGroups.VisitorGroupCriteria;

    public static class VisitorGroupMatcher
    {
        private static readonly Dictionary<string, IVisitorGroupCriteria> _availableCriteria = new Dictionary<string, IVisitorGroupCriteria>();

        static VisitorGroupMatcher()
        {
            BuildAvailableCriteria();
        }

        public static IEnumerable<string> GetAvailableCriteria()
        {
            return _availableCriteria.Keys;
        }

        public static bool IsMatch(VisitorGroupDefinitionDetail definitionDetail)
        {
            try
            {
                var criteria = _availableCriteria[definitionDetail.Alias];
                return criteria.MatchesVisitor(definitionDetail.Definition);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException(string.Format("Visitor group criteria not found with alias '{0}'",
                    definitionDetail.Alias));
            }
        }

        private static void BuildAvailableCriteria()
        {
            var type = typeof(IVisitorGroupCriteria);
            var typesImplementingInterface = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass)
                .Select(x => Activator.CreateInstance(x) as IVisitorGroupCriteria);
            foreach (var typeImplementingInterface in typesImplementingInterface)
            {
                _availableCriteria.Add(typeImplementingInterface.Alias, typeImplementingInterface);
            }
        }
    }
}
