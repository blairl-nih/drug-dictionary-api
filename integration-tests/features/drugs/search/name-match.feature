Feature: Search for variations on a drug name.

    Background:
        * url apiHost

    Scenario Outline: Search for a drug name, varying the Match type and a preferred name versus an alias.

        Given path 'Drugs', 'Search'
        And params { query: <name>, matchType: <match>, size: 5 }
        When method get
        Then status 200
        And match response == read( expected )

        Examples:
            | name                | match    | expected                                     |
            | palbociclib         | begins   | name-match-begin-exact-preferred.json        |
            | paclitaxel          | contains | name-match-contain-exact-preferred.json      |
            | mycobutin           | begins   | name-match-begin-exact-alias.json            |
            | comtan              | contains | name-match-contain-exact-alias.json          |
            # partial names
            | vaccinia            | contains | name-match-contain-partial-preferred.json    |
            # special cases
            | neu intracellular   | contains | name-match-contains-spaces-preferred.json    |
            | 70-kD               | contains | name-match-contains-dash-preferred.json      |
            | 4-thiazolidinedione | contains | name-match-contains-dash-alias.json          |