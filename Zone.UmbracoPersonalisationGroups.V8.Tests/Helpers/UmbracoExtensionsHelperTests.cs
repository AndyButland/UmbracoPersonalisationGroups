namespace Zone.UmbracoPersonalisationGroups.V8.Tests.Helpers
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;
    using Zone.UmbracoPersonalisationGroups.Common.GroupDefinition;
    using Zone.UmbracoPersonalisationGroups.Common.Tests.TestHelpers;
    using Zone.UmbracoPersonalisationGroups.V8.ExtensionMethods;

    [TestClass]
    public class UmbracoExtensionsHelperTests
    {
        [TestInitialize]
        public void Setup()
        {
            // Ensure that Config.Setup is OK in each test
            PersonalisationGroupsConfig.Reset();
        }

        [TestMethod]
        public void MatchGroups_WithPackageDisabled_ReturnsTrue()
        {
            // Arrange
            var pickedGroups = new List<IPublishedContent>();
            PersonalisationGroupsConfig.Setup(new PersonalisationGroupsConfig(disablePackage: true));

            // Act
            var result = UmbracoExtensionsHelper.MatchGroups(pickedGroups);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MatchGroups_WithNoGroups_ReturnsFalse()
        {
            // Arrange
            var pickedGroups = new List<IPublishedContent>();
            PersonalisationGroupsConfig.Setup(new PersonalisationGroupsConfig());

            // Act
            var result = UmbracoExtensionsHelper.MatchGroups(pickedGroups);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MatchGroups_WithNoMatchingGroupsUsingAll_ReturnsFalse()
        {
            // Arrange
            var pickedGroups = new List<IPublishedContent>()
            {
                MockPublishedContent(1000,
                    PersonalisationGroupDefinitionMatch.All,
                    PersonalisationGroupDefinitionDuration.Page,
                    new List<PersonalisationGroupDefinitionDetail>
                    {
                        Definitions.NonMatchingDayOfWeekDefinition(),
                        Definitions.MatchingDayOfWeekDefinition(),
                    }).Object,
            };
            PersonalisationGroupsConfig.Setup(new PersonalisationGroupsConfig());

            // Act
            var result = UmbracoExtensionsHelper.MatchGroups(pickedGroups);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MatchGroups_WithNoMatchingGroupsUsingAny_ReturnsFalse()
        {
            // Arrange
            var pickedGroups = new List<IPublishedContent>()
            {
                MockPublishedContent(1000,
                    PersonalisationGroupDefinitionMatch.Any,
                    PersonalisationGroupDefinitionDuration.Page,
                    new List<PersonalisationGroupDefinitionDetail>
                    {
                        Definitions.NonMatchingDayOfWeekDefinition(),
                        Definitions.NonMatchingDayOfWeekDefinition()
                    }).Object,
            };
            PersonalisationGroupsConfig.Setup(new PersonalisationGroupsConfig());

            // Act
            var result = UmbracoExtensionsHelper.MatchGroups(pickedGroups);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MatchGroups_WithMatchingGroupsUsingAll_ReturnsTrue()
        {
            // Arrange
            var pickedGroups = new List<IPublishedContent>()
            {
                MockPublishedContent(1000,
                    PersonalisationGroupDefinitionMatch.All,
                    PersonalisationGroupDefinitionDuration.Page,
                    new List<PersonalisationGroupDefinitionDetail>
                    {
                        Definitions.MatchingDayOfWeekDefinition(),
                        Definitions.MatchingTimeOfDayDefinition()
                    }).Object,
            };
            PersonalisationGroupsConfig.Setup(new PersonalisationGroupsConfig());

            // Act
            var result = UmbracoExtensionsHelper.MatchGroups(pickedGroups);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MatchGroups_WithMatchingGroupsUsingAny_ReturnsTrue()
        {
            // Arrange
            var pickedGroups = new List<IPublishedContent>()
            {
                MockPublishedContent(1000,
                    PersonalisationGroupDefinitionMatch.Any,
                    PersonalisationGroupDefinitionDuration.Page,
                    new List<PersonalisationGroupDefinitionDetail>
                    {
                        Definitions.MatchingDayOfWeekDefinition(),
                        Definitions.NonMatchingDayOfWeekDefinition()
                    }).Object,
            };
            PersonalisationGroupsConfig.Setup(new PersonalisationGroupsConfig());

            // Act
            var result = UmbracoExtensionsHelper.MatchGroups(pickedGroups);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MatchGroups_WithMatchingMultipleGroups_ReturnsTrue()
        {
            // Arrange
            var pickedGroups = new List<IPublishedContent>()
            {
                MockPublishedContent(1000,
                    PersonalisationGroupDefinitionMatch.All,
                    PersonalisationGroupDefinitionDuration.Page,
                    new List<PersonalisationGroupDefinitionDetail>
                    {
                        Definitions.MatchingDayOfWeekDefinition(),
                        Definitions.MatchingTimeOfDayDefinition(),
                        Definitions.NonMatchingDayOfWeekDefinition(),
                    }).Object,
                MockPublishedContent(1001,
                    PersonalisationGroupDefinitionMatch.Any,
                    PersonalisationGroupDefinitionDuration.Page,
                    new List<PersonalisationGroupDefinitionDetail>
                    {
                        Definitions.NonMatchingDayOfWeekDefinition(),
                        Definitions.MatchingDayOfWeekDefinition(),
                    }).Object,
            };
            PersonalisationGroupsConfig.Setup(new PersonalisationGroupsConfig());

            // Act
            var result = UmbracoExtensionsHelper.MatchGroups(pickedGroups);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MatchGroupsByName_WithPackageDisabledUsingAll_ReturnsTrue()
        {
            // Arrange
            var groups = new string[] { "Group 1000", "Group X" };
            var pickedGroups = new List<IPublishedContent>();
            PersonalisationGroupsConfig.Setup(new PersonalisationGroupsConfig(disablePackage: true));

            // Act
            var result = UmbracoExtensionsHelper.MatchGroupsByName(groups, pickedGroups, PersonalisationGroupDefinitionMatch.All);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MatchGroupsByName_WithPackageDisabledUsingAny_ReturnsTrue()
        {
            // Arrange
            var groups = new string[] { "Group 1000", "Group X" };
            var pickedGroups = new List<IPublishedContent>();
            PersonalisationGroupsConfig.Setup(new PersonalisationGroupsConfig(disablePackage: true));

            // Act
            var result = UmbracoExtensionsHelper.MatchGroupsByName(groups, pickedGroups, PersonalisationGroupDefinitionMatch.Any);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MatchGroupsByName_WithNonMatchingGroupsUsingAll_ReturnsFalse()
        {
            // Arrange
            var groups = new string[] { "Group 1000", "Group X" };
            var pickedGroups = new List<IPublishedContent>()
            {
                MockPublishedContent(1000,
                    PersonalisationGroupDefinitionMatch.All,
                    PersonalisationGroupDefinitionDuration.Page,
                    new List<PersonalisationGroupDefinitionDetail>
                    {
                        Definitions.MatchingDayOfWeekDefinition(),
                    }).Object,
                MockPublishedContent(1001,
                    PersonalisationGroupDefinitionMatch.Any,
                    PersonalisationGroupDefinitionDuration.Page,
                    new List<PersonalisationGroupDefinitionDetail>
                    {
                        Definitions.MatchingTimeOfDayDefinition(),
                    }).Object,
            };
            PersonalisationGroupsConfig.Setup(new PersonalisationGroupsConfig());

            // Act
            var result = UmbracoExtensionsHelper.MatchGroupsByName(groups, pickedGroups, PersonalisationGroupDefinitionMatch.All);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MatchGroupsByName_WithNonMatchingGroupsUsingAny_ReturnsFalse()
        {
            // Arrange
            var groups = new string[] { "Group X", "Group Y" };
            var pickedGroups = new List<IPublishedContent>()
            {
                MockPublishedContent(1000,
                    PersonalisationGroupDefinitionMatch.All,
                    PersonalisationGroupDefinitionDuration.Page,
                    new List<PersonalisationGroupDefinitionDetail>
                    {
                        Definitions.MatchingDayOfWeekDefinition(),
                    }).Object,
                MockPublishedContent(1001,
                    PersonalisationGroupDefinitionMatch.Any,
                    PersonalisationGroupDefinitionDuration.Page,
                    new List<PersonalisationGroupDefinitionDetail>
                    {
                        Definitions.MatchingTimeOfDayDefinition(),
                    }).Object,
            };
            PersonalisationGroupsConfig.Setup(new PersonalisationGroupsConfig());

            // Act
            var result = UmbracoExtensionsHelper.MatchGroupsByName(groups, pickedGroups, PersonalisationGroupDefinitionMatch.Any);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MatchGroupsByName_WithNameMatchingNonMatchingGroups_ReturnsFalse()
        {
            // Arrange
            var groups = new string[] { "Group 1000", "Group Y" };
            var pickedGroups = new List<IPublishedContent>()
            {
                MockPublishedContent(1000,
                    PersonalisationGroupDefinitionMatch.All,
                    PersonalisationGroupDefinitionDuration.Page,
                    new List<PersonalisationGroupDefinitionDetail>
                    {
                        Definitions.NonMatchingDayOfWeekDefinition(),
                    }).Object,
            };
            PersonalisationGroupsConfig.Setup(new PersonalisationGroupsConfig());

            // Act
            var result = UmbracoExtensionsHelper.MatchGroupsByName(groups, pickedGroups, PersonalisationGroupDefinitionMatch.Any);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MatchGroupsByName_WithNameMatchingMatchingGroups_ReturnsTrue()
        {
            // Arrange
            var groups = new string[] { "Group 1000", "Group 1001" };
            var pickedGroups = new List<IPublishedContent>()
            {
                MockPublishedContent(1000,
                    PersonalisationGroupDefinitionMatch.All,
                    PersonalisationGroupDefinitionDuration.Page,
                    new List<PersonalisationGroupDefinitionDetail>
                    {
                        Definitions.MatchingDayOfWeekDefinition(),
                    }).Object,
                MockPublishedContent(1001,
                    PersonalisationGroupDefinitionMatch.Any,
                    PersonalisationGroupDefinitionDuration.Page,
                    new List<PersonalisationGroupDefinitionDetail>
                    {
                        Definitions.MatchingTimeOfDayDefinition(),
                    }).Object,
            };
            PersonalisationGroupsConfig.Setup(new PersonalisationGroupsConfig());

            // Act
            var result = UmbracoExtensionsHelper.MatchGroupsByName(groups, pickedGroups, PersonalisationGroupDefinitionMatch.All);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ScoreGroups_WithPackageDisabled_ReturnsDefaultScore()
        {
            // Arrange
            var pickedGroups = new List<IPublishedContent>();
            PersonalisationGroupsConfig.Setup(new PersonalisationGroupsConfig(disablePackage: true));

            // Act
            var result = UmbracoExtensionsHelper.ScoreGroups(pickedGroups);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void ScoreGroups_WithMatchingMultipleGroups_ReturnsScore()
        {
            // Arrange
            var pickedGroups = new List<IPublishedContent>()
            {
                MockPublishedContent(1000,
                    PersonalisationGroupDefinitionMatch.All,
                    PersonalisationGroupDefinitionDuration.Page,
                    new List<PersonalisationGroupDefinitionDetail>
                    {
                        Definitions.MatchingDayOfWeekDefinition(),
                        Definitions.MatchingTimeOfDayDefinition(),
                    }, 50).Object,
                MockPublishedContent(1001,
                    PersonalisationGroupDefinitionMatch.Any,
                    PersonalisationGroupDefinitionDuration.Page,
                    new List<PersonalisationGroupDefinitionDetail>
                    {
                        Definitions.NonMatchingDayOfWeekDefinition(),
                        Definitions.MatchingDayOfWeekDefinition(),
                    }, 40).Object,
            };
            PersonalisationGroupsConfig.Setup(new PersonalisationGroupsConfig());

            // Act
            var result = UmbracoExtensionsHelper.ScoreGroups(pickedGroups);

            // Assert
            Assert.AreEqual(90, result);
        }

        private static Mock<IPublishedContent> MockPublishedContent(int id,
            PersonalisationGroupDefinitionMatch match,
            PersonalisationGroupDefinitionDuration duration,
            IEnumerable<PersonalisationGroupDefinitionDetail> definitionDetails,
            int score = 0)
        {
            var definitionPropertyMock = new Mock<IPublishedProperty>();
            definitionPropertyMock.Setup(c => c.Alias).Returns("definition");
            definitionPropertyMock.Setup(c => c.GetValue(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new PersonalisationGroupDefinition
                    {
                        Match = match,
                        Duration = duration,
                        Details = definitionDetails,
                        Score = score
                    });
            definitionPropertyMock.Setup(c => c.HasValue(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            var contentMock = new Mock<IPublishedContent>();
            contentMock.Setup(c => c.Id).Returns(id);
            contentMock.Setup(c => c.Name).Returns("Group " + id);
            contentMock.Setup(c => c.GetProperty(It.Is<string>(x => x == "definition")))
                .Returns(definitionPropertyMock.Object);
           
            return contentMock;
        }
    }
}
