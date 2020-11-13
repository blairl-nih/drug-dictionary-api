Feature: Autosuggest, including only PreferredName and USBrandName name types.

    Background:
        * url apiHost

    Scenario Outline: Given the search text and type, validate the query result.
        search: '<search>', match: '<match>', expected: '<expected>'

        Given path 'Autosuggest'
        And params { searchText: <search>, matchType: <match>, size: 5, includeNameTypes: ['USBrandName', 'PreferredName'] }
        When method get
        Then status 200
        And match response == read( expected )

        Examples:
            | search             | match     | expected                              |
            # Begins
            | bio                | begins    | nametypes-begins-single-word.json     |
            # Contains
            | juice              | contains  | nametypes-contains-single-word.json   |
            # Contains - with embedded spaces
            | tomato juice       | contains  | nametypes-contains-with-space.json    |
            # Contains - match after hyphen
            | containing         | contains  | nametypes-contains-after-hyphen.json  |
