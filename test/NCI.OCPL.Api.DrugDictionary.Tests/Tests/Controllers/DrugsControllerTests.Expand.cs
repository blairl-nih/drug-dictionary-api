using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging.Testing;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

using NCI.OCPL.Api.Common;
using NCI.OCPL.Api.DrugDictionary;
using NCI.OCPL.Api.DrugDictionary.Controllers;
using NCI.OCPL.Api.Common.Testing;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{

    /// <summary>
    /// Tests for the DrugsController's Expand method.
    /// </summary>
    public partial class DrugsControllerTests
    {
        /// <summary>
        /// Verify that Expand behaves in the expected manner when only required parameters are passed in.
        /// </summary>
        [Fact]
        public async void Expand_RequiredParametersOnly()
        {
            // Create a mock query that always returns the same result.
            Mock<IDrugsQueryService> querySvc = new Mock<IDrugsQueryService>();
            querySvc.Setup(
                svc => svc.Expand(
                    It.IsAny<char>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<DrugResourceType[]>(),
                    It.IsAny<TermNameType[]>(),
                    It.IsAny<TermNameType[]>()
                )
            )
            .Returns(Task.FromResult(new DrugTermResults()));

            // Call the controller, we don't care about the actual return value.
            DrugsController controller = new DrugsController(NullLogger<DrugsController>.Instance, querySvc.Object);
            await controller.Expand('s');

            // Verify that the query layer is called:
            //  a) with the expected updated values for size, from, and requestedFields.
            //  b) exactly once.
            querySvc.Verify(
                svc => svc.Expand('s', DEFAULT_SEARCH_SIZE, DEFAULT_SEARCH_FROM,
                    DEFAULT_DRUG_RESOURCE_TYPE_LIST,
                    DEFAULT_INCLUDED_TERM_TYPE_LIST,
                    DEFAULT_EXCLUDED_TERM_TYPE_LIST ),
                Times.Once,
                "ITermsQueryService::Expand() should be called once, using default values."
            );
        }

        /// <Summary>
        /// Verify that Expand behaves in the expected manner when size is an invalid value.
        /// </Summary>
        [Fact]
        public async void Expand_InvalidSize()
        {
            // Create a mock query that always returns the same result.
            Mock<IDrugsQueryService> querySvc = new Mock<IDrugsQueryService>();
            querySvc.Setup(
                svc => svc.Expand(
                    It.IsAny<char>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<DrugResourceType[]>(),
                    It.IsAny<TermNameType[]>(),
                    It.IsAny<TermNameType[]>()
                )
            )
            .Returns(Task.FromResult(new DrugTermResults()));

            // Call the controller, we don't care about the actual return value.
            DrugsController controller = new DrugsController(NullLogger<DrugsController>.Instance, querySvc.Object);
            await controller.Expand('s', -1, DEFAULT_SEARCH_FROM,
                DEFAULT_DRUG_RESOURCE_TYPE_LIST,
                DEFAULT_INCLUDED_TERM_TYPE_LIST,
                DEFAULT_EXCLUDED_TERM_TYPE_LIST
            );

            // Verify that the query layer is called:
            //  a) with the expected updated values for size.
            //  b) exactly once.
            querySvc.Verify(
                svc => svc.Expand('s', DEFAULT_SEARCH_SIZE, DEFAULT_SEARCH_FROM,
                    DEFAULT_DRUG_RESOURCE_TYPE_LIST, DEFAULT_INCLUDED_TERM_TYPE_LIST, DEFAULT_EXCLUDED_TERM_TYPE_LIST),
                Times.Once,
                "ITermsQueryService::Expand() should be called once, with the updated value for size"
            );
        }

        /// <summary>
        /// Verify that Expand behaves in the expected manner when from is an invalid value.
        /// </summary>
        [Fact]
        public async void Expand_InvalidFrom()
        {
            // Create a mock query that always returns the same result.
            Mock<IDrugsQueryService> querySvc = new Mock<IDrugsQueryService>();
            querySvc.Setup(
                svc => svc.Expand(
                    It.IsAny<char>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<DrugResourceType[]>(),
                    It.IsAny<TermNameType[]>(),
                    It.IsAny<TermNameType[]>()
                )
            )
            .Returns(Task.FromResult(new DrugTermResults()));

            // Call the controller, we don't care about the actual return value.
            DrugsController controller = new DrugsController(NullLogger<DrugsController>.Instance, querySvc.Object);
            await controller.Expand('s', 10, -1);

            // Verify that the query layer is called:
            //  a) with the expected updated values for from and size.
            //  b) exactly once.
            querySvc.Verify(
                svc => svc.Expand('s', 10, DEFAULT_SEARCH_FROM,
                    DEFAULT_DRUG_RESOURCE_TYPE_LIST, DEFAULT_INCLUDED_TERM_TYPE_LIST, DEFAULT_EXCLUDED_TERM_TYPE_LIST),
                Times.Once,
                "ITermsQueryService::Expandl() should be called once, with the updated value for from"
            );
        }

        /// <summary>
        /// Verify that Expand returns a DrugTermResults identical to the one it recieves from the service level.
        /// (This test will need to change if Expand ever gains any logic of its own.)
        /// </summary>
        [Fact]
        public async void ExpandTerms()
        {
            Mock<IDrugsQueryService> termsQueryService = new Mock<IDrugsQueryService>();
            DrugsController controller = new DrugsController(NullLogger<DrugsController>.Instance, termsQueryService.Object);
            
            DrugTermResults drugTermResults = new DrugTermResults()
            {
                Results = new DrugTerm[] {
                    new DrugTerm()
                    {
                        TermId = 475765,
                        Name = "siltuximab",
                        FirstLetter = 's',
                        //Type = DrugResourceType.DrugTerm,
                        TermNameType = TermNameType.PreferredName,
                        PrettyUrlName = "siltuximab",
                        Aliases = new TermAlias[]
                        {
                            new TermAlias()
                            {
                                Type = TermNameType.CodeName,
                                Name = "CNTO 328"
                            },
                            new TermAlias()
                            {
                                Type = TermNameType.LexicalVariant,
                                Name = "anti-IL-6 chimeric monoclonal antibody"
                            },
                            new TermAlias()
                            {
                                Type=TermNameType.CASRegistryName,
                                Name = "541502-14-1"
                            },
                            new TermAlias()
                            {
                                Type = TermNameType.USBrandName,
                                Name = "Sylvant"
                            }
                        },
                        Definition = new Definition()
                        {
                            Text = "A chimeric, human-murine, monoclonal antibody targeting the pro-inflammatory cytokine interleukin 6 (IL-6), with antitumor and anti-inflammatory activities. Upon intravenous administration of siltuximab, this agent targets and binds to IL-6. This inhibits the binding of IL-6 to the IL-6 receptor (IL-6R), which results in the blockade of the IL-6/IL-6R-mediated signal transduction pathway. This inhibits cancer cell growth in tumors overexpressing IL-6. Check for active clinical trials using this agent. (NCI Thesaurus)",
                            Html = "A chimeric, human-murine, monoclonal antibody targeting the pro-inflammatory cytokine interleukin 6 (IL-6), with antitumor and anti-inflammatory activities. Upon intravenous administration of siltuximab, this agent targets and binds to IL-6. This inhibits the binding of IL-6 to the IL-6 receptor (IL-6R), which results in the blockade of the IL-6/IL-6R-mediated signal transduction pathway. This inhibits cancer cell growth in tumors overexpressing IL-6. Check for <a ref=\"https://www.cancer.gov/about-cancer/treatment/clinical-trials/intervention/C61084\">active clinical trials</a> using this agent. (<a ref=\"https://ncit.nci.nih.gov/ncitbrowser/ConceptReport.jsp?dictionary=NCI%20Thesaurus&code=C61084\">NCI Thesaurus</a>)"
                        },
                        DrugInfoSummaryLink = new DrugInfoSummaryLink()
                        {
                            Text = "Siltuximab",
                            URI = new Uri("https://www.cancer.gov/about-cancer/treatment/drugs/siltuximab")
                        },
                        NCIConceptId = "C61084",
                        NCIConceptName = "Siltuximab"
                    },
                    new DrugTerm()
                    {
                        TermId = 674543,
                        Name = "silver nitrate",
                        FirstLetter = 's',
                        Type = DrugResourceType.DrugTerm,
                        TermNameType = TermNameType.PreferredName,
                        PrettyUrlName = "silver-nitrate"
                        ,Aliases = new TermAlias[]
                        {
                           new TermAlias()
                           {
                               Type = TermNameType.CASRegistryName,
                               Name = "7761-88-8"
                           }
                        },
                        Definition = new Definition()
                        {
                           Text = "An inorganic chemical with antiseptic activity. Silver nitrate can potentially be used as a cauterizing or sclerosing agent. Check for active clinical trials using this agent. (NCI Thesaurus)",
                           Html = "An inorganic chemical with antiseptic activity. Silver nitrate can potentially be used as a cauterizing or sclerosing agent. Check for <a ref=\"https://www.cancer.gov/about-cancer/treatment/clinical-trials/intervention/C77057\">active clinical trials</a> using this agent. (<a ref=\"https://ncit.nci.nih.gov/ncitbrowser/ConceptReport.jsp?dictionary=NCI%20Thesaurus&code=C77057\">NCI Thesaurus</a>)"
                        },
                        NCIConceptId = "C77057",
                        NCIConceptName = "Silver Nitrate"
                    },
                },
                Meta = new ResultsMetadata()
                {
                    TotalResults = 854,
                    From = 10
                },
                Links = null
            };

            termsQueryService.Setup(
                termQSvc => termQSvc.Expand(
                    It.IsAny<char>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<DrugResourceType[]>(),
                    It.IsAny<TermNameType[]>(),
                    It.IsAny<TermNameType[]>()
                )
            )
            .Returns(Task.FromResult(drugTermResults));

            DrugTermResults actualReslts = await controller.Expand('s', 5, 10, DEFAULT_DRUG_RESOURCE_TYPE_LIST,
                DEFAULT_INCLUDED_TERM_TYPE_LIST, DEFAULT_EXCLUDED_TERM_TYPE_LIST);

            // Verify that the service layer is called:
            //  a) with the expected values.
            //  b) exactly once.
            termsQueryService.Verify(
                svc => svc.Expand('s', 5, 10, DEFAULT_DRUG_RESOURCE_TYPE_LIST,
                    DEFAULT_INCLUDED_TERM_TYPE_LIST, DEFAULT_EXCLUDED_TERM_TYPE_LIST),
                Times.Once
            );

            // What we're really doing is verifying that Expand() returns the same
            // object it received from the service. If Expand() ever implements logic
            // to do its own processing, this test will need to change.
            Assert.Equal(drugTermResults, actualReslts, new DrugTermResultsComparer());
        }

    }
}