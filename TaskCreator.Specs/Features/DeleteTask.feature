﻿Feature: Удаление задачи.
Взрослый может удалить задачу. Ребенок не может удалить задачу.
Задача удаляется по её id.

    Scenario: Взрослый может удалить задачу
        Given семья из
          | Email             | Phone        | Name | Adult |
          | father@family.com | +79180000001 | Папа | да    |
          | son@family.com    | +79180000002 | Сын  | нет   |
        And список задач из
          | TaskID | Text           | IsCompleted | DateDeadline |
          | 000001 | Я текст задачи | false       | 30.03.2023   |
          | 000002 | Вторая задача  | true        | 31.03.2023   |
        And в систему вошел Папа
        When пользователь удаляет задачу под номером 1
        And пользователь получает список задач
        Then количество элементов в списке задач 1
        And в списке задач есть задача с текстом "Вторая задача" и под номером 2
        
    Scenario: Ребенок не может удалить задачу
        Given семья из
          | Email             | Phone        | Name | Adult |
          | father@family.com | +79180000001 | Папа | да    |
          | son@family.com    | +79180000002 | Сын  | нет   |
        And список задач из
          | TaskID | Text           | IsCompleted | DateDeadline |
          | 000001 | Я текст задачи | false       | 30.03.2023   |
          | 000002 | Вторая задача  | true        | 31.03.2023   |
        And в систему вошел Сын
        When пользователь удаляет задачу под номером 1
        And пользователь получает список задач
        Then количество элементов в списке задач 2
        And в списке задач есть задача с текстом "Я текст задачи" и под номером 1
        And в списке задач есть задача с текстом "Вторая задача" и под номером 2
        
    Scenario: Взрослый не может удалить задачу с номером, которого не существует в списке
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
        When удаляет задачу под номером 4
        And пользователь получает список задач
        Then количество элементов в списке задач 3
        And в списке задач есть задача с текстом "Я текст задачи" и под номером 1
        And в списке задач есть задача с текстом "Вторая задача" и под номером 2
        And в списке задач есть задача с текстом "Третья задача" и под номером 3
        