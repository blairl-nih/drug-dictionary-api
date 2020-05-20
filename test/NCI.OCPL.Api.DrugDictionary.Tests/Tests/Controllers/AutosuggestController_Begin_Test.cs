using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging.Testing;
using Moq;
using Newtonsoft.Json;
using Xunit;

using NCI.OCPL.Api.Common;
using NCI.OCPL.Api.Common.Testing;
using NCI.OCPL.Api.DrugDictionary.Controllers;
using NCI.OCPL.Api.DrugDictionary.Tests.AutosuggestControllerTestData;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public partial class AutosuggestControllerTest
    {
        public static IEnumerable<object[]> TestData => new[]
        {
            new object[] { new AutosuggestScenario_Contains() },
            new object[] { new AutosuggestScenario_Begins_Contains_Only() },
            new object[] { new AutosuggestScenario_Begins_Begins_Only() },
            new object[] { new AutosuggestScenario_Begins() },
            new object[] { new AutosuggestScenario_Begins_Begin_Suffices() },
            new object[] { new AutosuggestScenario_Begins_ExcessData() }
        };

        [Theory, MemberData(nameof(TestData))]
        public async void Autosuggest(BaseAutosuggestControllerScenario data)
        {
            // Sanity check for test data.  The expected result array should never
            // be larger than the value of the Size property. (Smaller is fine.)
            Assert.InRange(data.ExpectedData.Length, 0, data.Size);

            // Set up the mock query service
            Mock<IAutosuggestQueryService> querySvc = new Mock<IAutosuggestQueryService>();
            AutosuggestController controller = new AutosuggestController(new NullLogger<AutosuggestController>(), querySvc.Object);

            querySvc.Setup(
                autoSuggestQSvc => autoSuggestQSvc.GetSuggestions(
                    It.IsAny<string>(),
                    It.Is<MatchType>(match => match == MatchType.Begins),
                    It.IsAny<int>(),
                    It.IsAny<DrugResourceType[]>(),
                    It.IsAny<TermNameType[]>(),
                    It.IsAny<TermNameType[]>()
                )
            )
            .Returns(Task.FromResult(data.BeginsData));

            querySvc.Setup(
                autoSuggestQSvc => autoSuggestQSvc.GetSuggestions(
                    It.IsAny<string>(),
                    It.Is<MatchType>(match => match == MatchType.Contains),
                    It.IsAny<int>(),
                    It.IsAny<DrugResourceType[]>(),
                    It.IsAny<TermNameType[]>(),
                    It.IsAny<TermNameType[]>()
                )
            )
            .Returns(Task.FromResult(data.ContainsData));

            // The search text doesn't matter.
            Suggestion[] actual = await controller.GetSuggestions("chicken", data.MatchType, data.Size);

            Assert.Equal(data.ExpectedData, actual, new ArrayComparer<Suggestion, SuggestionComparer>());
        }
    }
}