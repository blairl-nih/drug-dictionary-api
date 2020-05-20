using NCI.OCPL.Api.DrugDictionary;

namespace NCI.OCPL.Api.DrugDictionary.Tests.AutosuggestControllerTestData
{
    /// <summary>
    /// Test data for a Begins match where only the Begins query produces results.
    /// </summary>
    public class AutosuggestScenario_Begins_Begins_Only : BaseAutosuggestControllerScenario
    {
        public override MatchType MatchType => MatchType.Begins;

        public override int Size => 5;

        public override Suggestion[] BeginsData => new Suggestion[]{
            new Suggestion()
            {
                TermId = 1,
                TermName = "Begins Term 1"
            },
            new Suggestion()
            {
                TermId = 2,
                TermName = "Begins Term 2"
            },new Suggestion()
            {
                TermId = 3,
                TermName = "Begins Term 3"
            },
            new Suggestion()
            {
                TermId = 4,
                TermName = "Begins Term 4"
            },
            new Suggestion()
            {
                TermId = 5,
                TermName = "Begins Term 5"
            }
        };

        public override Suggestion[] ContainsData => new Suggestion[0];

        public override Suggestion[] ExpectedData => new Suggestion[]
        {
            new Suggestion()
            {
                TermId = 1,
                TermName = "Begins Term 1"
            },
            new Suggestion()
            {
                TermId = 2,
                TermName = "Begins Term 2"
            },new Suggestion()
            {
                TermId = 3,
                TermName = "Begins Term 3"
            },
            new Suggestion()
            {
                TermId = 4,
                TermName = "Begins Term 4"
            },
            new Suggestion()
            {
                TermId = 5,
                TermName = "Begins Term 5"
            }
        };

    }
}