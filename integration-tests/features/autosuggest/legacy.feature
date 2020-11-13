Feature: Autosuggest, approximating the legacy WCMS drug dictionary.

    Background:
        * url apiHost

    Scenario Outline: Given the search text and type, validate the query result.
        search: '<search>', match: '<match>', expected: '<expected>'

        Given path 'Autosuggest'
        And params { searchText: <search>, matchType: <match>, size: 5 }
        When method get
        Then status 200
        And match response == read( expected )

        Examples:
            | search             | match     | expected                                 |
            # Begins
            | bio                | begins    | legacy-begins-single-word.json           |
            # Contains
            | juice              | contains  | legacy-contains-single-word.json         |
            # Contains - with embedded spaces
            | tomato juice       | contains  | legacy-contains-with-space.json          |
            # Contains - match after hyphen
            | containing         | contains  | legacy-contains-after-hyphen.json          |
            # Path separator in term name
            | SD/01              | contains  | legacy-contains-with-path-separator.json |
