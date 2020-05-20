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

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public partial class AutosuggestControllerTest
    {
        static readonly DrugResourceType[] DEFAULT_DRUG_RESOURCE_TYPE_LIST = { DrugResourceType.DrugTerm, DrugResourceType.DrugAlias };
        static readonly TermNameType[] DEFAULT_INCLUDED_TERM_TYPE_LIST = { TermNameType.CodeName, TermNameType.ObsoleteName, TermNameType.Abbreviation,
                            TermNameType.INDCode, TermNameType.NSCNumber, TermNameType.ForeignBrandName, TermNameType.Synonym,
                            TermNameType.CASRegistryName, TermNameType.Subtype, TermNameType.Spanish, TermNameType.LexicalVariant,
                            TermNameType.ChemicalStructureName, TermNameType.CommonUsage, TermNameType.Acronym, TermNameType.USBrandName,
                            TermNameType.Broader, TermNameType.RelatedString, TermNameType.PreferredName };
        static readonly TermNameType[] DEFAULT_EXCLUDED_TERM_TYPE_LIST = {};

        /// <summary>
        /// Verify that explicit values passed to the controller are passed in turn to the query service.
        /// </summary>
        [Fact]
        public async void Verify_Explicit_Values_Passed_to_Service()
        {
            const string queryText = "chicken";
            const MatchType matchType = MatchType.Contains;
            const int resultSize = 100;
            DrugResourceType[] expectedResourceTypes = DEFAULT_DRUG_RESOURCE_TYPE_LIST;
            TermNameType[] expectedIncludedTermTypes = DEFAULT_INCLUDED_TERM_TYPE_LIST;
            TermNameType[] expectedExclusionList = DEFAULT_EXCLUDED_TERM_TYPE_LIST;

            Mock<IAutosuggestQueryService> querySvc = new Mock<IAutosuggestQueryService>();
            AutosuggestController controller = new AutosuggestController(NullLogger<AutosuggestController>.Instance, querySvc.Object);

            // Set up the mock query service to return an empty array.
            querySvc.Setup(
                autoSuggestQSvc => autoSuggestQSvc.GetSuggestions(
                    It.IsAny<string>(),
                    It.IsAny<MatchType>(),
                    It.IsAny<int>(),
                    It.IsAny<DrugResourceType[]>(),
                    It.IsAny<TermNameType[]>(),
                    It.IsAny<TermNameType[]>()
                )
            )
            .Returns(Task.FromResult(new Suggestion[] { }));

            Suggestion[] result = await controller.GetSuggestions(queryText, matchType, resultSize);

            // Verify that the service layer is called:
            //  a) with the expected values.
            //  b) exactly once.
            querySvc.Verify(
                svc => svc.GetSuggestions(queryText, matchType, resultSize, expectedResourceTypes, expectedIncludedTermTypes, expectedExclusionList),
                Times.Once
            );

            Assert.Empty(result);
        }

        /// <summary>
        /// Verify the correct defaults are passed to the query service when no valies are specified for beginsWith and size.
        /// </summary>
        [Fact]
        public async void Verify_Default_Values_Passed_to_Service()
        {
            const string queryText = "chicken";

            const MatchType expectedMatchType = MatchType.Begins;
            const int expecedSizeRequest = 20;
            DrugResourceType[] expectedResourceTypes = DEFAULT_DRUG_RESOURCE_TYPE_LIST;
            TermNameType[] expectedIncludedTermTypes = DEFAULT_INCLUDED_TERM_TYPE_LIST;
            TermNameType[] expectedExclusionList = DEFAULT_EXCLUDED_TERM_TYPE_LIST;

            Mock<IAutosuggestQueryService> querySvc = new Mock<IAutosuggestQueryService>();
            AutosuggestController controller = new AutosuggestController(NullLogger<AutosuggestController>.Instance, querySvc.Object);

            // Set up the mock query service to return an empty array.
            querySvc.Setup(
                autoSuggestQSvc => autoSuggestQSvc.GetSuggestions(
                    It.IsAny<string>(),
                    It.IsAny<MatchType>(),
                    It.IsAny<int>(),
                    It.IsAny<DrugResourceType[]>(),
                    It.IsAny<TermNameType[]>(),
                    It.IsAny<TermNameType[]>()
                )
            )
            .Returns(Task.FromResult(new Suggestion[] { }));

            Suggestion[] result = await controller.GetSuggestions(queryText);

            // Verify that the service layer is called:
            //  a) with the expected values.
            //  b) exactly once.
            querySvc.Verify(
                svc => svc.GetSuggestions(queryText, expectedMatchType, expecedSizeRequest, expectedResourceTypes, expectedIncludedTermTypes, expectedExclusionList),
                Times.Once
            );

            Assert.Empty(result);
        }

        /// <summary>
        /// Verify that negative values for size and from are properly handled before the service is invoked.
        /// </summary>
        [Fact]
        public async void Verify_Negative_Value_Handling()
        {
            const string queryText = "chicken";
            const MatchType matchType = MatchType.Contains;
            const int invalidSize = -200;

            const int expecedSizeRequest = 20;
            DrugResourceType[] expectedResourceTypes = DEFAULT_DRUG_RESOURCE_TYPE_LIST;
            TermNameType[] expectedIncludedTermTypes = DEFAULT_INCLUDED_TERM_TYPE_LIST;
            TermNameType[] expectedExclusionList = DEFAULT_EXCLUDED_TERM_TYPE_LIST;

            Mock<IAutosuggestQueryService> querySvc = new Mock<IAutosuggestQueryService>();
            AutosuggestController controller = new AutosuggestController(NullLogger<AutosuggestController>.Instance, querySvc.Object);

            // Set up the mock query service to return an empty array.
            querySvc.Setup(
                autoSuggestQSvc => autoSuggestQSvc.GetSuggestions(
                    It.IsAny<string>(),
                    It.IsAny<MatchType>(),
                    It.IsAny<int>(),
                    It.IsAny<DrugResourceType[]>(),
                    It.IsAny<TermNameType[]>(),
                    It.IsAny<TermNameType[]>()
                )
            )
            .Returns(Task.FromResult(new Suggestion[] { }));

            Suggestion[] result = await controller.GetSuggestions(queryText, matchType, invalidSize);

            // Verify that the service layer is called:
            //  a) with the expected values.
            //  b) exactly once.
            querySvc.Verify(
                svc => svc.GetSuggestions(queryText, matchType, expecedSizeRequest, expectedResourceTypes, expectedIncludedTermTypes, expectedExclusionList),
                Times.Once
            );

            Assert.Empty(result);
        }

        /// <summary>
        /// Verify that passing zero for the  size argument is properly handled before the service is invoked.
        /// </summary>
        [Fact]
        public async void Verify_Zero_Size_Handling()
        {
            const string queryText = "chicken";
            const MatchType matchType = MatchType.Begins;
            const int invalidSize = 0;

            const int expecedSizeRequest = 20;
            DrugResourceType[] expectedResourceTypes = DEFAULT_DRUG_RESOURCE_TYPE_LIST;
            TermNameType[] expectedIncludedTermTypes = DEFAULT_INCLUDED_TERM_TYPE_LIST;
            TermNameType[] expectedExclusionList = DEFAULT_EXCLUDED_TERM_TYPE_LIST;

            Mock<IAutosuggestQueryService> querySvc = new Mock<IAutosuggestQueryService>();
            AutosuggestController controller = new AutosuggestController(NullLogger<AutosuggestController>.Instance, querySvc.Object);

            // Set up the mock query service to return an empty array.
            querySvc.Setup(
                autoSuggestQSvc => autoSuggestQSvc.GetSuggestions(
                    It.IsAny<string>(),
                    It.IsAny<MatchType>(),
                    It.IsAny<int>(),
                    It.IsAny<DrugResourceType[]>(),
                    It.IsAny<TermNameType[]>(),
                    It.IsAny<TermNameType[]>()
                )
            )
            .Returns(Task.FromResult(new Suggestion[] { }));

            Suggestion[] result = await controller.GetSuggestions(queryText, matchType, invalidSize);

            // Verify that the service layer is called:
            //  a) with the expected values.
            //  b) exactly once.
            querySvc.Verify(
                svc => svc.GetSuggestions(queryText, matchType, expecedSizeRequest, expectedResourceTypes, expectedIncludedTermTypes, expectedExclusionList),
                Times.Once
            );

            Assert.Empty(result);
        }


    }
}