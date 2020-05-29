using System;
using System.Linq;
using System.Collections.Generic;

using NCI.OCPL.Api.DrugDictionary;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    /// <summary>
    /// A IEqualityComparer for DrugTerm
    /// </summary>
    public class DrugTermComparer : IEqualityComparer<DrugTerm>
    {
        public bool Equals(DrugTerm x, DrugTerm y)
        {
            // If the items are both null, or if one or the other is null, return
            // the correct response right away.
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }

            bool isEqual =
                x.TermId == y.TermId
                && x.Name == y.Name
                && x.FirstLetter == y.FirstLetter
                && x.Type == y.Type
                && x.TermNameType == y.TermNameType
                && x.PrettyUrlName == y.PrettyUrlName
                && new ArrayComparer<TermAlias, TermAliasComparer>().Equals(x.Aliases, y.Aliases)
                && new DefinitionComparer().Equals(x.Definition, y.Definition)
                && new DrugInfoSummaryLinkComparer().Equals(x.DrugInfoSummaryLink, y.DrugInfoSummaryLink)
                && x.NCIConceptId == y.NCIConceptId
                && x.NCIConceptName == y.NCIConceptName
            ;

            return isEqual;
            }

            public int GetHashCode(DrugTerm obj)
            {
            int hash = 0;
            hash ^=
                obj.TermId.GetHashCode()
                ^ obj.Name.GetHashCode()
                ^ obj.FirstLetter.GetHashCode()
                ^ obj.Type.GetHashCode()
                ^ obj.TermNameType.GetHashCode()
                ^ (obj.PrettyUrlName != null ? obj.PrettyUrlName.GetHashCode() : 0)
                ^ (obj.Aliases != null ? new ArrayComparer<TermAlias, TermAliasComparer>().GetHashCode(obj.Aliases) : 0)
                ^ (obj.Definition != null ? new DefinitionComparer().GetHashCode(obj.Definition) : 0)
                ^ (obj.DrugInfoSummaryLink != null ? new DrugInfoSummaryLinkComparer().GetHashCode(obj.DrugInfoSummaryLink) : 0)
                ^ obj.NCIConceptId.GetHashCode()
                ^ obj.NCIConceptName.GetHashCode()
            ;

            return hash;
        }

    }
}