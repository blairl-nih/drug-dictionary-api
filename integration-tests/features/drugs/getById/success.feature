Feature: Get definitions by specifying the ID.

    Background:
        * url apiHost

    Scenario Outline: Get a specific definition, validate the query results.

        Given path 'Drugs', id
        When method get
        Then status 200
        And match response == read( expected )

        Examples:
            | id     | expected                   |
            | 539733 | success-nivolumab.json     |
            | 695789 | success-pembrolizumab.json |
            | 43234  | success-bevacizumab.json   |
            | 702758 | success-atezolizumab.json  |
            | 740856 | success-durvalumab.json    |
            | 42265  | success-trastuzumab.json   |
            | 745752 | success-avelumab.json      |
            | 38447  | success-ipilimumab.json    |
