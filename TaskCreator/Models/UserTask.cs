using System;

namespace TaskCreator.Models;

/// <summary>
/// Задача пользователя
/// </summary>
/// <param name="Id">Идентификатор задачи</param>
/// <param name="TasksListId">Идентификатор списка задач</param>
/// <param name="Text">Текст задачи</param>
/// <param name="IsCompleted">Выполнена ли задача</param>
/// <param name="DateDeadline">Дата дедлайна задачи</param>
public record UserTask (
    Guid Id,
    int TasksListId,
    string Text,
    bool IsCompleted,
    DateTime DateDeadline);