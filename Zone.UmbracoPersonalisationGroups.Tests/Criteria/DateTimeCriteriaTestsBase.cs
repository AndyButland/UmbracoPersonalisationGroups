namespace Zone.UmbracoPersonsalisationGroups.Tests.Criteria
{
    using System;
    using Moq;
    using Zone.UmbracoPersonalisationGroups.Criteria;

    public abstract class DateTimeCriteriaTestsBase
    {
        protected static Mock<IDateTimeProvider> MockDateTimeProvider(DateTime? dateTime = null)
        {
            dateTime = dateTime ?? new DateTime(2016, 1, 1, 10, 0, 0); // a Friday
            var mock = new Mock<IDateTimeProvider>();

            mock.Setup(x => x.GetCurrentDateTime()).Returns(dateTime.Value);

            return mock;
        }
    }
}
