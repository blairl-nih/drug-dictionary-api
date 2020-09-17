Feature: Attempt to retrieve definition by specifying a mismatched pretty url name.

    Background:
        * url apiHost

    Scenario Outline: Attempt to retrieve a definition, using a pretty url name which doesn't
        match anything, and verify the failure result.

        Given path 'Drugs', prettyName
        When method get
        Then status 404
        And match response == read( expected )

        Examples:
            | prettyName         | expected                        |
            | chicken            | mismatch-no-match.json          |
            | autologous-ic9-gd2 | mismatch-partial.json           |
            | egfr antisense dna | mismatch-spaces.json            |
            | BENZOYLPHENYLUREA  | mismatch-uppercase.json         |
            | s-3304             | mismatch-preferred-instead.json |
