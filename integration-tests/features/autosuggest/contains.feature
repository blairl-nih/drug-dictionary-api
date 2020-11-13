Feature: Autosuggest contains matches, with no filtering of alias types. This is a good place for
    tests based on bugs from production.

    Background:
        * url apiHost

    Scenario Outline: Given the search text, validate contains query for DrugTerms.
        Search term is `<search>`.

        Given path 'Autosuggest'
        And params { searchText: <search>, matchType: contains, size: 20, includeResourceTypes: ['DrugTerm'] }
        When method get
        Then status 200
        And match response == read( expected )

        Examples:
            | search             | expected                                        |
            | FT-1101            | contains-term-exact-match-with-punctuation.json |
            | ster               | contains-term-match-on-word-boundary.json       |

    Scenario Outline: Given the search text, validate contains query for DrugAliases.
        Search term is `<search>`.

        Given path 'Autosuggest'
        And params { searchText: <search>, matchType: contains, size: 20, includeResourceTypes: ['DrugAlias'] }
        When method get
        Then status 200
        And match response == read( expected )

        Examples:
            | search             | expected                                         |
            | t-cel              | contains-alias-exact-match-with-punctuation.json   |
            | mab                | contains-alias-match-on-word-boundary.json       |


    Scenario Outline: Given the search text, validate contains query for all resource types.
        Search term is `<search>`.

        Given path 'Autosuggest'
        And params { searchText: <search>, matchType: contains, size: 20, includeResourceTypes: ['DrugTerm','DrugAlias'] }
        When method get
        Then status 200
        And match response == read( expected )

        Examples:
            | search             | expected                                               |
            | interleukin-2      | contains-all-records-exact-match-with-punctuation.json |
            | anti               | contains-all-records-match-on-word-boundary.json       |
            | doxorubicin        | contains-all-records-match-on-word-boundary2.json      |


    Scenario Outline: Given the search text, validate contains query for default resource types.
        Search term is `<search>`.

        Given path 'Autosuggest'
        And params { searchText: <search>, matchType: contains, size: 20 }
        When method get
        Then status 200
        And match response == read( expected )

        Examples:
            | search             | expected                                               |
            # Default should be the same results as all, so let's use the same results files.
            | interleukin-2      | contains-all-records-exact-match-with-punctuation.json |
            | anti               | contains-all-records-match-on-word-boundary.json       |
            | doxorubicin        | contains-all-records-match-on-word-boundary2.json      |
