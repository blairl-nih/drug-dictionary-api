Feature: Autosuggest, including only PreferredName and USBrandName name types in order to
    approximate the legacy WCMS dictionary.

    Background:
        * url apiHost

    Scenario Outline: Given the search text and type, validate the query result.

        Given path 'Autosuggest', search
        And params { matchType: <match>, size: 5, includeNameTypes: ['USBrandName', 'PreferredName'] }
        When method get
        Then status 200
        And match response == read( expected )

        Examples:
            | search             | match     | expected                          |
            # Begins
            | bio                | begins    | legacy-begins-bio.json            |
            # Contains
            | juice              | contains  | legacy-contains-juice.json        |
            # Contains - with embedded spaces
            | tomato juice       | contains  | legacy-contains-tomato-juice.json |
            # Contains - match after dash
            | containing         | contains  | legacy-contains-containing.json   |
            # Path separator in term name
            | TLR5/TLR5          | contains  | legacy-contains-tlr5.json         |
