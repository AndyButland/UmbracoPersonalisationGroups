namespace Zone.UmbracoPersonalisationGroups.Criteria
{
    using System;

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetCurrentDateTime() => DateTime.Now;
    }
}
