﻿Feature: Добавление задачи 
Для создания задачи текст для данной задачи не должен быть пустой строкой.

	Scenario: Пользователь может добавить первую задачу 
		Given список задач
		  | TaskID | Text           | IsCompleted | DateDeadline |
		When пользователь добавляет задачу "Новая задача"(16.01.2023)
		And пользователь получает список задач
		Then количество элементов в списке задач 1
		And в списке задач есть задача с текстом "Новая задача" и id 000001
		
	Scenario: Пользователь может добавить задачу
		Given список задач
		  | TaskID | Text           | IsCompleted | DateDeadline |
		  | 000001 | Я текст задачи | false       | 30.03.2023   |
		  | 000002 | Вторая задача  | true        | 31.03.2023   |
    	When пользователь добавляет задачу "Новая задача"(12.04.2023)
    	And пользователь получает список задач
    	Then количество элементов в списке задач 3
    	And в списке задач есть задача с текстом "Я текст задачи" и id 000001
    	And в списке задач есть задача с текстом "Вторая задача" и id 000002
    	And в списке задач есть задача с текстом "Новая задача"
    	
    Scenario: Пользователь не может добавить задачу с пустым текстом
    	Given список задач
          | TaskID | Text           | IsCompleted | DateDeadline |
          | 000001 | Я текст задачи | false       | 30.03.2023   |
        When пользователь добавляет задачу ""(11.12.2024)
        And пользователь получает список задач
        Then количество элементов в списке задач 1
        And в списке задач есть задача с текстом "Я текст задачи" и id 000001