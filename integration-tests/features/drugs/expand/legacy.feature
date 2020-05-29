Feature: Expand, with includedNameTypes set to 'PreferredName', 'USBrandName'
    (equivalent to legacy WCMS functionality)

    Background:
        * url apiHost

    Scenario Outline: Given a letter of the alphabet, validate the query results.

        Given path 'Drugs', 'expand', letter
        And params {from: <from>, size: 6, includeNameTypes: ['PreferredName', 'USBrandName']}
        When method get
        Then status 200
        And match response == read( expected )

        Examples:
            | letter | from | expected       |
            | a      | 0    | legacy-a.json  |
            | s      | 20   | legacy-s.json  |
            | #      | 5    | legacy-hash.json  |
