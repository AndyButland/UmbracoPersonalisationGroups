namespace Zone.UmbracoPersonalisationGroups.Common.Providers.GeoLocation
{
    using System.Collections.Generic;

    public class Region
    {
        public string City { get; set; }

        public string[] Subdivisions { get; set; }

        public Country Country { get; set; }

        public string[] GetAllNames()
        {
            var names = new List<string> { City };
            if (Subdivisions != null)
            {
                names.AddRange(Subdivisions);
            }

            return names.ToArray();
        }
    }
}
