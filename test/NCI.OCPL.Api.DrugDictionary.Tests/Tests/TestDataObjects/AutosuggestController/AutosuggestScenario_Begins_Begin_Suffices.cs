using NCI.OCPL.Api.DrugDictionary;

namespace NCI.OCPL.Api.DrugDictionary.Tests.AutosuggestControllerTestData
{
    /// <summary>
    /// Test data for a Begins match where the Begins query produces sufficient
    /// results and the contains results are ignored.
    /// </summary>
    public class AutosuggestScenario_Begins_Begin_Suffices : BaseAutosuggestControllerScenario
    {
        public override MatchType MatchType => MatchType.Begins;

        public override int Size => 3;

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
            }
        };

        public override Suggestion[] ContainsData => new Suggestion[]{
            new Suggestion()
            {
                TermId = 4,
                TermName = "Contains Term 1"
            },
            new Suggestion()
            {
                TermId = 5,
                TermName = "Contains Term 2"
            },new Suggestion()
            {
                TermId = 6,
                TermName = "Contains Term 3"
            }
        };

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
            }

        };

    }
}