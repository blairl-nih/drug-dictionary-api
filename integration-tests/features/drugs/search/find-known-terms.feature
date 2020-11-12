Feature: Search for terms known to exist. (This is a good place for items found in production.)

    Background:
        * url apiHost

    Scenario Outline: Given the search text and type, validate the query result for a term known to exist.
        Search for searchText: '<search>', matchType: '<match>'

        Given path 'Drugs', 'Search'
        And params { query: <search>, matchType: <match>, size: 10}
        When method get
        Then status 200
        And match response == read( expected )

        Examples:
            | search        | match     | expected                                       |
            | juice         | contains  | find-known-terms-contains-simple-word.json     |
            | tomato juice  | contains  | find-known-terms-begins-word-with-space.json   |
            | AO+           | begins    | find-known-terms-begins-plus-sign.json         |
            | (+)XK469      | contains  | find-known-terms-contains-plus-sign.json       |