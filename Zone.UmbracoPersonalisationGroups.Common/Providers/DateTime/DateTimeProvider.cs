namespace Zone.UmbracoPersonalisationGroups.Common.Providers.DateTime
{
    using System;

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetCurrentDateTime() => DateTime.Now;
    }
}
