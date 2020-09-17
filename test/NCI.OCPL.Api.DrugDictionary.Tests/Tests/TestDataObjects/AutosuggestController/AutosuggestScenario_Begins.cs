using NCI.OCPL.Api.DrugDictionary;

namespace NCI.OCPL.Api.DrugDictionary.Tests.AutosuggestControllerTestData
{
    /// <summary>
    /// Test data for a Begins match where the Begins and Contains queries
    /// are both required to produces enough results.
    /// </summary>
    public class AutosuggestScenario_Begins : BaseAutosuggestControllerScenario
    {
        public override MatchType MatchType => MatchType.Begins;

        public override int Size => 6;

        public override Suggestion[] Data => new Suggestion[]{
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

        public override Suggestion[] ExpectedResult => new Suggestion[]
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