Feature: opdrachten maken
  Als leerling wil ik opdrachten maken zodat ik mee kan doen aan de groepsopdracht.


  Background:

Given sideQuestions:
      | Id | Question                       | Options                       | 
      | 1  | waarom nam je deze foto        |                               | 
      | 2  | waarom vind je deze mooi       |                               | 
      | 3  | waarom vind je deze lelijk     |                               | 
      | 4  | is dit subjectief of objectief | objectief,subjectief          | 

    Given photoQuestions:
      | Id | Question                   | SideQuestions  |
      | 1  | neem een foto uit je buurt | 1              |
      | 2  | wat vind je mooi           | 2              |
      | 3  | wat vind je lelijk         | 3,4            |
    
    Given setTasks:
      | Id | Title         | Questions |
      | 1  | mijn buurt    |   1       |
      | 2  | wat is kunst  |   2,3     |

    Given Setup:
      | Id | Tasks |
      | 1  | 1,2   |

    Given teachers:
      | UserId | Name           | SetUp | 
      | 1  | karel de grote     | 1     | 

    Given groups:
      | Id     | Teacher | Tasks |
      | 1A1B1C | 1       | 1     |
      | 2D2E2F | 1       | 1     |
      | 3G3H3I | 1       | 1,2   |

    Given students:
      | Id | Group  | 
      | 1  | 2D2E2F | 
      | 2  | 2D2E2F |
      | 3  | 2D2E2F |

    Given photos:
      | Id | Image            | 
      | 1  | mijnbuurt.jpg    | 
      | 2  | ookmijnbuurt.jpg | 
      | 3  | kappa.jpg        | 
      | 4  | pogchamp.jpg     | 
      | 5  | antwerpen.jpg    | 
      | 6  | halle.jpg        | 
      | 7  | kingkong.jpg     | 
      | 8  | morgana.jpg      | 

    Given sideAnswers:
      | Id | GivenAnswer                                                                 | SideQuestion  | 
      | 1  |                                                                             | 1             | 
      | 2  | omdat dit het eerste is dat ik tegen kwam en te lui ben om verder te zoeken | 2             |
      | 3  | omdat ik het zeg                                                            | 3             | 
      | 4  | 2                                                                           | 4             | 

    Given answers:
      | Id | PhotoQuestion | SideAnswers | AssignedPhoto |
      | 1  | 1             |  1          | 1             |
      | 2  | 2             |             | 1             |
      | 3  | 3             |  2          | 2             |
      | 4  | 4             |  3,4        | 3             |

    Given taskDeliveries:
      | id | SentPhotos | Answers    | SetTask | Group  |
      | 1  |            |            | 1       | 2D2E2F |
      | 2  |            |            | 2       | 2D2E2F | 
      | 3  | 1,5        | 1          | 1       | 2D2E2F |
      | 4  | 3,4,7,8    | 3,4        | 2       | 2D2E2F |
      | 5  | 2,6        | 2          | 1       | 2D2E2F |
      | 6  |            |            | 2       | 2D2E2F |
    

  Scenario: A student sends in a picture for a task with a single question
    When  Student adds the picture "antwerpen.jpg" as an answer to the task "mijn buurt"
    Then  Answer "5" is initiated and linked to taskDelivery "1" and question "1"
    And   Answer "5" is linked to question "1"
    And   Photo "9" is initiated and linked to taskDelivery "1" and answer "5"
    And   Photo "9" is given the value "antwerpen.jpg"
    And   sideAnswer "5" is created and linked to sideQuestion "1" and answer "5"

  Scenario: A student sends in a picture for a task with multiple questions from the task screen
    When  Student adds the picture "monalisa.jpg" as a picture related to the task "wat is kunst"
    Then  photo "9", is created and is linked to taskDelivery "2"
    And   Photo "9" is given the value "monalisa.jpg"

  Scenario: A student assigns a picture to a specific question
    When  Student selects "kingkong.jpg" from the uploaded list of photos as an answer to the question "wat vind je mooi"
    Then  Answer "5" is initiated and linked to taskDelivery "4" and question "2"
    And   Photo "7" is linked to answer "6"
    And   SideAnswer "5" is initiated and linked to answer "5" and sideQuestion "2"

  Scenario: A student sends in a picture for a task with multiple questions from the question screen
    When  Student adds the picture "jhoncena.jpg" answering the question "wat vind je lelijk" in the "wat is kunst" task
    Then  Answer "5" is initiated and linked to taskDelivery "2" and question "3"
    And   Photo "9" is initiated and linked to answer "7" and taskDelivery "2"
    And   Photo "9" is given the value "jhoncena.jpg"
    And   SideAnswer "5" is initiated and linked to answer "5" and sideQuestion "3"
    And   SideAnswer "5" is initiated and linked to answer "5" and sideQuestion '4'

  Scenario: A student answers a side question
    When  Student answers sideQuestion "waarom nam je deze foto" with "waarom niet"
    Then  SideAnswer is given the value "waarom niet"

  Scenario: A student tries to send in a completed task
    When  Student presses the complete button for task "wat is kunst"
    Then  taskDelivery "3" is send to the database

  Scenario: A student tries to send in a task with uncompleted photoQuestions
    When  Student presses the complete button for task "wat is kunst"
    Then  An exception is shown "de volgende fotovragen zijn niet beantwoord: wat vind je mooi, wat vind je lelijk"

  Scenario: A student tries to send in a task with uncompleted photoQuestions2
    When  Student presses the complete button for task "mijn buurt"
    Then  An exception is shown "je hebt 1 of meerdere side answers niet beantwoord in: neem een foto uit je buurt"
