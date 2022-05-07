Feature: deelnemen sessie
Als leerling wil ik deelnemen aan een sessie zodat ik gebruik kan maken van de foto app.

    Background:
	
		Given StudentProfileQuestions:
          | Id | Question            | IsRequired |
          | 1  | what is your gender | false    |
          | 2  | how old are you     | true     |
		  
        Given Setup:
          | Id | NeededStudentProfileQuestions | 
          | 1  | 1,2                           |
		  
		Given Teacher:
			| Id | SetUp |
			| 1  |   1   |

        Given Tasks:
          | Id | Title       |
          | 1  | mijn buurt |
          | 2  | onze buurt |

        Given Groups:
          | Id     | Tasks   | Teacher |
          | 1A1B1C |         |    1    |
          | 2D2E2F | 1     |    1    |
          | 3G3H3I | 1,2  |    1    |
		  
		  Given StudentProfileAnswers:
          | Id | Question | Answer |
          | 1  | 1        | male   |
          | 2  | 2        | 64     |
          | 3  | 1        |  |
          | 4  | 2        |      |
          | 5  | 1        |  |
          | 6  | 2        |     |
		  
        Given Students:
          | Id | ProfileAnswers | Group  |
          | 1  | 1,2            | 2D2E2F |
          | 2  | 3,4            | 2D2E2F |
          | 3  | 5,6            | 2D2E2F |

        

    Scenario: A new student tries to join a group
        When A user inputs '1A1B1C' in the group code field
        Then student '4' is created
        And student '4' is linked to group '1A1B1C'
		And StudentProfileAnswer '7' is created and linked to Student '4' and studentProfileQuestion '1'
		And StudentProfileAnswer '8' is created and linked to Student '4' and studentProfileQuestion '2'
		
	Scenario: A new student tries to join an unexisting group
        When A user inputs 'AAA999' in the group code field 
        Then There is no student '4' 

    Scenario: A student answers all questions
        When Student '2' gives studentProfileAnswer '3' value 'female' to StudentProfileQuestion '1' 
        And Student '2' gives studentProfileAnswer '4' value '15' to StudentProfileQuestion '2' 
        Then Value of studentProfileAnswer '3' is 'female' 
        And Value of studentProfileAnswer '4' is '15' 
        And Delivery '1' is created 
        And Delivery '1' is linked to group '2D2E2F' 

    Scenario: A student answers all mandatory questions
        When Student '3' gives studentProfileAnswer '5' value 'female' to StudentProfileQuestion '1' 
        And Student '3' gives studentProfileAnswer '6' no value to StudentProfileQuestion '2'
        Then Value of studentProfileAnswer '5' is 'female'
        And Value of studentProfileAnswer '6' is null
        And Delivery '1' is created
        And Delivery '1' is linked to group '2D2E2F' 

    Scenario: A student answers no questions
        When student '2' leaves all question fields blank and presses next
        Then no deliveries are created