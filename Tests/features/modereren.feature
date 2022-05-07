Feature: leerkracht foto's modereren
  Als leerkracht Wil ik een plaats hebben waar ik foto’s kan modereren Om de beste foto’s te kunnen onderscheiden van de slechte/ongepaste


  Background:
	
	 
    Given SideQuestions:
      | Id | Question                       | Options               |
      | 1  | waarom nam je deze foto        |                       |
      | 2  | waarom vind je deze mooi       |                       |
      | 3  | is dit subjectief of objectief | subjectief,objectief  |
	  
    Given PhotoQuestions:
      | Id | Question                   | SideQuestions |
      | 1  | neem een foto uit je buurt |     1         |
      | 2  | wat vind je mooi           |     2,3       |
      | 3  | wat vind je lelijk         |               |
    
	
	 
	Given SetTasks:
      | Id | Title        | Questions |
      | 1  | mijn buurt   |    1      |
      | 2  | wat is kunst |    2,3    |
	
	 Given Setup:
      | Id | Tasks | 
      | 1  | 2     |  
	 
	 Given Teacher:
      | UserId | Name           | setup |
      | 1  	   | karel de grote | 1     | 
	 
    Given Groups:
      | Groupcode |Teacher | Tasks |
      | 1A1B1C    |1       |   1,2 |
      | 2D2E2F    |1       |    1  |
      | 3G3H3I    |1       |    2  |
	  
    Given Students:
      | Id | Group |
      | 1  |1A1B1C |
      | 2  |1A1B1C |
      | 3  |2D2E2F |
      | 4  |2D2E2F |
	  
	   Given Photos:
      | Id | Image            |
      | 1  | mijnbuurt.jpg    |
      | 2  | ookmijnbuurt.jpg |
      | 3  | kappa.jpg        |
      | 4  | pogchamp.jpg     |
      | 5  | pikachu.jpg      |
      | 6  | froakie.jpg      |
      | 7  | kingkong.jpg     |
      | 8  | spiderman.jpg    |
      | 9  | charmander.jpg   |
      | 10 | jigglypuff.jpg   |
      | 11 | raichu.jpg       |
      | 12 | kaonashi.jpg     |
      | 13 | Yubaba.jpg       |
      | 14 | totoro.jpg       |
	  
	  Given SideAnswers:
      | Id | GivenAnswer                                                              | SideQuestion |
      | 1  | hjfj                                                                     | 1            |
      | 2  | omdat dit het eerste is dat ik tegen kwam en te lui ben verder te zoeken | 2            |
      | 3  | omdat ik het zeg                                                         | 2            |
      | 4  | omdat ik het zeg4                                                        | 1            |
      | 5  | omdat ik het zeg5                                                        | 2            |
      | 6  | omdat ik het zeg6                                                        | 1            |
      | 7  | omdat ik het zeg                                                         | 2            |
      | 8  | omdat ik het zeg4                                                        | 1            |
      | 9  | omdat ik het zeg5                                                        | 2            |
      | 10 | omdat ik het zeg6                                                        | 2            |
      | 11 | objectief                                                                | 3            |
      | 12 | omdat ik het zeg4                                                        | 2            |
      | 13 | omdat ik het zeg5                                                        | 2            |
      | 14 | subjectief                                                               | 2            |
	  
	  Given Answers:
      | Id | PhotoQuestion | SideAnswers | AssignedPhoto |
      | 1  | 1             |      1      |       2       |
      | 2  | 1             |      4      |       1       |
      | 3  | 2             |      2,3    |      11       |
      | 4  | 3             |             |       7       |
      | 5  | 3             |             |       9       |
      | 6  | 3             |             |      12       |
      | 7  | 2             |      9,10   |               |
      | 8  | 2             |       5,7   |               |
      | 9  | 1             |       6     |        8      |
      | 10 | 1             |       8     |               |
      | 11 | 3             |             |        2      |
      | 12 | 2             |     11,12   |        3      |
      | 13 | 2             |     13,14   |        4      |
	  
    Given SetTaskDeliveries:
      | Id | SentPhotos  | Answers  | SetTask | Group  |
      | 1  | 2           |     1    |     1   | 1A1B1C |
      | 2  | 2,3,4       | 11,12,13 |     2   | 2D2E2F |
      | 3  | 1,8,10      |   2,9,10 |     1   | 3G3H3I |
      | 4  | 7,9         |   4,5    |     2   | 3G3H3I |
      | 5  |             |          |     1   | 3G3H3I |
      | 6  | 11,12       |   3,6    |     2   | 3G3H3I |
   



  Scenario: one submitted foto is detected with face
    When  group '2D2E2F' deliveres
    Then  Value flagged in answer '11' changes to false
    And  Value flagged in answer '12' changes to false
    And  Value flagged in answer '13' changes to true

  Scenario: multiple submitted fotos detected with face
    When  group '3G3H3I' deliveres
    Then  Value flagged in answer '2' changes to false
    And  Value flagged in answer '9' changes to true
    And  Value flagged in answer '10' changes to false
    And  Value flagged in answer '4' changes to false
    And  Value flagged in answer '5' changes to false
    And  Value flagged in answer '6' changes to true
  

  Scenario: no faces detected
	 When  group '1A1B1C' deliveres
    Then  Value flagged in answer '1' changes to false
