namespace Zone.UmbracoPersonalisationGroups.Providers.DateTime
{
    using System;

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetCurrentDateTime() => DateTime.Now;
    }
}
