namespace Zone.UmbracoPersonalisationGroups
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Zone.UmbracoPersonalisationGroups.Criteria;

    /// <summary>
    /// Static class providing available details and matching logic for personalisation groups
    /// </summary>
    public static class PersonalisationGroupMatcher
    {
        /// <summary>
        /// Application lifetime variable storing the available personalisation group criteria
        /// </summary>
        private static readonly Dictionary<string, IPersonalisationGroupCriteria> AvailableCriteria = new Dictionary<string, IPersonalisationGroupCriteria>();

        /// <summary>
        /// Initializes static members of the <see cref="PersonalisationGroupMatcher"/> class. Called once to retrieve and store the available personalisation group criteria.
        /// </summary>
        static PersonalisationGroupMatcher()
        {
            BuildAvailableCriteria();
        }

        /// <summary>
        /// Gets the stored available personalisation group criteria
        /// </summary>
        /// <returns>Enumerable of available criteria</returns>
        public static IEnumerable<IPersonalisationGroupCriteria> GetAvailableCriteria()
        {
            return AvailableCriteria.Values
                .OrderBy(x => x.Name);
        }

        /// <summary>
        /// Checks if a given detail record of a personalisation group definition matches the current site visitor
        /// </summary>
        /// <param name="definitionDetail">Personalisation group definition detail record</param>
        /// <returns>True of the current site visitor matches the definition</returns>
        public static bool IsMatch(PersonalisationGroupDefinitionDetail definitionDetail)
        {
            try
            {
                var criteria = AvailableCriteria[definitionDetail.Alias];
                return criteria.MatchesVisitor(definitionDetail.Definition);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException(string.Format("Personalisation group criteria not found with alias '{0}'",
                    definitionDetail.Alias));
            }
        }

        /// <summary>
        /// Helper to scan the loaded assemblies and retrieve the available personalisation group criteria (that implement the
        /// <see cref="IPersonalisationGroupCriteria"/> interface
        /// </summary>
        private static void BuildAvailableCriteria()
        {
            var type = typeof(IPersonalisationGroupCriteria);
            var typesImplementingInterface = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass)
                .Select(x => Activator.CreateInstance(x) as IPersonalisationGroupCriteria);

            var includeCriteria = ConfigurationManager.AppSettings[AppConstants.ConfigKeyForIncludeCriteria];
            if (!string.IsNullOrEmpty(includeCriteria))
            {
                typesImplementingInterface = typesImplementingInterface
                    .Where(x => includeCriteria.Split(',').Contains(x.Alias, StringComparer.InvariantCultureIgnoreCase));
            }

            var excludeCriteria = ConfigurationManager.AppSettings[AppConstants.ConfigKeyForExcludeCriteria];
            if (!string.IsNullOrEmpty(excludeCriteria))
            {
                typesImplementingInterface = typesImplementingInterface
                    .Where(x => !excludeCriteria.Split(',').Contains(x.Alias, StringComparer.InvariantCultureIgnoreCase));
            }

            foreach (var typeImplementingInterface in typesImplementingInterface)
            {
                AvailableCriteria.Add(typeImplementingInterface.Alias, typeImplementingInterface);
            }
        }
    }
}
