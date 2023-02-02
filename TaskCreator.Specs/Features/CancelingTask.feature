﻿Feature: Отмена выполнения задачи.
Взрослый может отменить выполнение задачи. Ребенок не может отменить выполнение задачи.
Задача помечается не выполненной по её id.

	Scenario: Взрослый может отменить выполнение задачи
		Given семья из
		  | Email             | Phone        | Name | Adult |
		  | father@family.com | +79180000001 | Папа | да    |
		  | son@family.com    | +79180000002 | Сын  | нет   |
		And список задач из
		  | TaskID | Text           | IsCompleted | DateDeadline |
		  | 000001 | Я текст задачи | false       | 30.03.2023   |
		  | 000002 | Вторая задача  | true        | 31.03.2023   |
		  | 000003 | Третья задача  | false       | 22.03.2023   |
    	And в систему вошел Папа
		When пользователь отменяет выполнение задачи под номером 2
		And пользователь получает список задач
		Then количество элементов в списке задач 3
		And в списке задач есть невыполненная задача под номером 1
		And в списке задач есть невыполненная задача под номером 2
		And в списке задач есть невыполненная задача под номером 3
		
	Scenario: Ребенок не может отменить выполнение задачи
		Given семья из
		  | Email             | Phone        | Name | Adult |
		  | father@family.com | +79180000001 | Папа | да    |
		  | son@family.com    | +79180000002 | Сын  | нет   |
		And список задач из
		  | TaskID | Text           | IsCompleted | DateDeadline |
		  | 000001 | Я текст задачи | false       | 30.03.2023   |
		  | 000002 | Вторая задача  | true        | 31.03.2023   |
		  | 000003 | Третья задача  | false       | 22.03.2023   |
		And в систему вошел Сын
		When пользователь отменяет выполнение задачи под номером 2
		And пользователь получает список задач
		Then количество элементов в списке задач 3
		And в списке задач есть невыполненная задача под номером 1
		And в списке задач есть выполненная задача под номером 2
		And в списке задач есть невыполненная задача под номером 3
	
	Scenario: Взрослый не может отменить выполнение задачи, если она еще не выполнена
		Given семья из
		  | Email             | Phone        | Name | Adult |
		  | father@family.com | +79180000001 | Папа | да    |
		  | son@family.com    | +79180000002 | Сын  | нет   |
		And список задач из
		  | TaskID | Text           | IsCompleted | DateDeadline |
		  | 000001 | Я текст задачи | false       | 30.03.2023   |
		  | 000002 | Вторая задача  | true        | 31.03.2023   |
		  | 000003 | Третья задача  | false       | 22.03.2023   |
    	And в систему вошел Папа
		When пользователь отменяет выполнение задачи под номером 1
		And пользователь получает список задач
		Then количество элементов в списке задач 3
		And в списке задач есть невыполненная задача под номером 1
		And в списке задач есть выполненная задача под номером 2
		And в списке задач есть невыполненная задача под номером 3