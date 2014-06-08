Feature: Public Servers
	In order to play SA-MP
	As a SA-MP player
	I want to be told what servers I can play on

Scenario Outline: Load up the client
	Given there are <Number> public servers
	When I start the client
	Then I should see <Number> servers

	Examples:
	| Number |
	| 0      |
	| 10     |

@ignore
Scenario: No servers
	Given there are 0 public servers
	And the reason is because I cannot connect to the master list
	When I start the client
	Then I should see a message stating why