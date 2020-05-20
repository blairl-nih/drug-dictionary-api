using System;
using NCI.OCPL.Api.DrugDictionary;

namespace NCI.OCPL.Api.DrugDictionary.Tests.AutosuggestControllerTestData
{
    /// <summary>
    /// Test data for a Contains match.
    /// </summary>
    public class AutosuggestScenario_Contains : BaseAutosuggestControllerScenario
    {
        public override MatchType MatchType => MatchType.Contains;

        public override int Size => 5;

        // This should never actually be used, but the property has to exist.
        public override Suggestion[] BeginsData => null;

        public override Suggestion[] ContainsData => new Suggestion[]{
            new Suggestion()
            {
                TermId = 1,
                TermName = "Contains Term 1"
            },
            new Suggestion()
            {
                TermId = 2,
                TermName = "Contains Term 2"
            },new Suggestion()
            {
                TermId = 3,
                TermName = "Contains Term 3"
            },
            new Suggestion()
            {
                TermId = 4,
                TermName = "Contains Term 4"
            },
            new Suggestion()
            {
                TermId = 5,
                TermName = "Contains Term 5"
            }
        };

        public override Suggestion[] ExpectedData => new Suggestion[]
        {
            new Suggestion()
            {
                TermId = 1,
                TermName = "Contains Term 1"
            },
            new Suggestion()
            {
                TermId = 2,
                TermName = "Contains Term 2"
            },new Suggestion()
            {
                TermId = 3,
                TermName = "Contains Term 3"
            },
            new Suggestion()
            {
                TermId = 4,
                TermName = "Contains Term 4"
            },
            new Suggestion()
            {
                TermId = 5,
                TermName = "Contains Term 5"
            }
        };

    }
}