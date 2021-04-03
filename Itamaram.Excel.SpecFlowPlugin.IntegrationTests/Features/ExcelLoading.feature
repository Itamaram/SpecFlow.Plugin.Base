Feature: SpecFlowFeature
	I want to load some excel stuffs

Scenario Outline: Load From Excel
	Given I have followed the instructions
	Then I should've loaded <A> <B> <C>

	@excel:Book1.xlsx:Sheet1
	Examples: 
	| A | B | C |