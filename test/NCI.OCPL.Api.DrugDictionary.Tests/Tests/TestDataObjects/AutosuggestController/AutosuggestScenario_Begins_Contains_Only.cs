using NCI.OCPL.Api.DrugDictionary;

namespace NCI.OCPL.Api.DrugDictionary.Tests.AutosuggestControllerTestData
{
    /// <summary>
    /// Test data for a Begins match where only the Contains query produces results.
    /// </summary>
    public class AutosuggestScenario_Begins_Contains_Only : BaseAutosuggestControllerScenario
    {
        public override MatchType MatchType => MatchType.Begins;

        public override int Size => 5;

        public override Suggestion[] BeginsData => new Suggestion[0];

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