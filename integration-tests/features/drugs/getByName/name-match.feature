Feature: Get definitions by specifying the pretty URL name.

    Background:
        * url apiHost

    Scenario Outline: Get a definition with an exact name match and validate the query results.

        Given path 'Drugs', prettyName
        When method get
        Then status 200
        And match response == read( expected )

        Examples:
            | prettyName    | expected                      |
            | nivolumab     | name-match-nivolumab.json     |
            | pembrolizumab | name-match-pembrolizumab.json |
            | bevacizumab   | name-match-bevacizumab.json   |
            | atezolizumab  | name-match-atezolizumab.json  |
            | durvalumab    | name-match-durvalumab.json    |
            | trastuzumab   | name-match-trastuzumab.json   |
            | avelumab      | name-match-avelumab.json      |
            | ipilimumab    | name-match-ipilimumab.json    |
