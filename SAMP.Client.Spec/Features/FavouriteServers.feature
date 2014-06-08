@ignore
Feature: Favourite Servers
	In order to play my favourite SA-MP servers
	As a SA-MP player
	I want to be told what servers I have favourited

	In order to play my favourite SA-MP servers
	As a SA-MP player
	I want to be able to favourite servers

Scenario: Favourite a server
	Given there are two public servers
	When I favourite one of them
	Then it should become a favourited server

Scenario: Show a favourite
	Given I have favourited a server
	And that server is publically availalbe
	When I start the client
	Then it should be a favourited server

Scenario: Show all favourites
	Given I have favourited 4 servers
	When I open the favourites tab
	Then I should see 4 favourite servers