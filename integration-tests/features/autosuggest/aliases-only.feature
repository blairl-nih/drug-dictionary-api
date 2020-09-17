Feature: Autosuggest, restricted to drug aliases.

    Background:
        * url apiHost

    Scenario Outline: Given the search text and type, validate the query result.

        Given path 'Autosuggest'
        And params { searchText: <search>, matchType: <match>, size: 3,  includeResourceTypes: [DrugAlias]}
        When method get
        Then status 200
        And match response == read( expected )

        Examples:
            | search      | match     | expected                          |
            # Begins
            | bio         | begins    | aliases-only-begins-bio.json      |
            # Contains
            | bio         | contains  | aliases-only-contains-bio.json    |
            # Contains - with embedded spaces
            #| test case needed
            # Contains - match after dash
            #| test case needed
            # Path separator in term name
            #| test case needed
