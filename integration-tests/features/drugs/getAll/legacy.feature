Feature: Get All definitions, with includedNameTypes set to 'PreferredName', 'USBrandName'
    (equivalent to legacy WCMS functionality)

    Background:
        * url apiHost

    Scenario Outline: Get a group of definitions at a given offset, validate the query results.

        Given path 'Drugs'
        And params {from: <from>, size: 5, includeNameTypes: ['PreferredName', 'USBrandName']}
        When method get
        Then status 200
        And match response == read( expected )

        Examples:
            | from | expected                 |
            | 0    | legacy-offset-0.json     |
            | 2000 | legacy-offset-2000.json  |
