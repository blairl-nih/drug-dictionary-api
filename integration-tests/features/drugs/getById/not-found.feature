Feature: Verify that non-existant IDs result in the proper response.

    Background:
        * url apiHost

    Scenario Outline: Get a specific definition, validate the query results.

        Given path 'Drugs', id
        When method get
        Then status 404
        And match response == read( expected )

        Examples:
            | id         | expected                  |
            | 100        | not-found-100.json        |
            | 32767      | not-found-32767.json      |
            | 8000000000 | not-found-8000000000.json |
