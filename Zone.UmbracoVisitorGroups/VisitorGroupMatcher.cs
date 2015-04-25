namespace Zone.UmbracoVisitorGroups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Zone.UmbracoVisitorGroups.VisitorGroupCriteria;
    
    /// <summary>
    /// Static class providing available details and matchin logic for visitor groups
    /// </summary>
    public static class VisitorGroupMatcher
    {
        /// <summary>
        /// Application lifetime variable storing the available visitor group criteria
        /// </summary>
        private static readonly Dictionary<string, IVisitorGroupCriteria> _availableCriteria = new Dictionary<string, IVisitorGroupCriteria>();

        /// <summary>
        /// Constructor of the static class called once to retrieve and store the available visitor group criteria 
        /// </summary>
        static VisitorGroupMatcher()
        {
            BuildAvailableCriteria();
        }

        /// <summary>
        /// Gets the stored available visitor group criteria
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IVisitorGroupCriteria> GetAvailableCriteria()
        {
            return _availableCriteria.Values;
        }

        /// <summary>
        /// Checks if a given detail record of a visitor group definition matches the current site visitor
        /// </summary>
        /// <param name="definitionDetail">Visitor group definition detail record</param>
        /// <returns>True of the current site visitor matches the definition</returns>
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

        /// <summary>
        /// Helper to scan the loaded assemblies and retrive the available visitor group criteria (that implement the
        /// <see cref="IVisitorGroupCriteria"/> interface
        /// </summary>
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
