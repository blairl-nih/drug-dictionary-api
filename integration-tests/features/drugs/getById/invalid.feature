Feature: Verify that invalid IDs result in the proper response.

    Background:
        * url apiHost

    Scenario Outline: Get a specific definition, validate the query results.

        Given path 'Drugs', id
        When method get
        Then status 400
        And match response == read( expected )

        Examples:
            | id         | expected                   |
            | -7400      | invalid-negative-7400.json |
            | 0          | invalid-0.json             |
