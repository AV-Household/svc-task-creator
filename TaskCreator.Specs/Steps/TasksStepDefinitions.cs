using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using TaskCreator.Controllers;
using TaskCreator.Models;
using TechTalk.SpecFlow;

namespace TaskCreator.Specs.Steps;

[Binding]
public sealed class TasksStepDefinitions
{
    private Dictionary<string, (string Email, string Phone, bool IsAdult)> _givenFamilyMembers = new();
    private Dictionary<Guid, (string Text, bool IsCompleted, DateTime DateDeadline)> _givenTasks = new();
    private static List<UserTask>? _tasksList = new();
    private static TaskController _taskController;

    [Given(@"семья из")]
    public void GivenFamily(Table membersTable)
    {
        _taskController = new();
        _givenFamilyMembers = membersTable.Rows
            .ToDictionary(row => row["Name"].Trim(), row=> (Email: row["Email"].Trim(), Phone:row["Phone"].Trim(),  IsAdult: row["Adult"].Trim() == "да"));
        
        var members = _givenFamilyMembers
            .Select(givenMember => new FamilyMember(
                Guid.NewGuid(),
                1,
                givenMember.Key,
                givenMember.Value.Phone,
                givenMember.Value.Email,
                givenMember.Value.IsAdult
            ));
        
        _taskController.InitFamilyMembers(members.ToList());
    }

    [Given(@"список задач из")]
    public void GivenTasks(Table tasksTable)
    {
        _taskController = new();
        _givenTasks = tasksTable.Rows
            .ToDictionary(row => new Guid(row["TaskID"].Trim()),
                row => (Text: row["Text"].Trim(), IsCompleted: row["IsCompleted"].Trim() == "true",
                    DateDeadline: DateTime.ParseExact(row["DateDeadline"].Trim(), "dd.MM.yyyy", CultureInfo.CurrentCulture)));

        var tasks = _givenTasks
            .Select(givenTask => new UserTask(
                givenTask.Key,
                1,
                givenTask.Value.Text,
                givenTask.Value.IsCompleted,
                givenTask.Value.DateDeadline
            ));
       
        _taskController.InitTasksList(tasks.ToList());
    }

    [Given(@"в систему вошел (.*)")]
    public void GivenUserLogin(string userName)
    {
        _taskController = new();
        _taskController.LogInUser(userName);
    }

    [When(@"пользователь добавляет задачу ""(.*)""\((.*)\)")]
    public void WhenUserAddTask(string text, string date)
    {
        _taskController = new();
        _taskController.Post(new TaskController.AddUserTaskDto(text, 
            DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.CurrentCulture)));
    }

    [When(@"пользователь получает список задач")]
    public void WhenUserGetListOfTasks()
    {
        _taskController = new();
        var resultValue = _taskController.Get();
        _tasksList = resultValue.ToList();
    }

    [Then(@"количество элементов в списке задач (.*)")]
    public void ThenTasksInListAmountIs(int taskCount) =>
        _tasksList.Should().HaveCount(taskCount);

    [Then(@"в списке задач есть задача с текстом ""(.*)"" под номером (.*-.*-.*-.*-.*)")]
    public void ThenListOfTasksContainTaskWithTextAndId(string taskText, string idStr)
    {
        Guid id = new Guid(idStr);
        _tasksList.Should().ContainSingle(userTask => userTask.Id == id && userTask.Text == taskText);
    }

    [Then(@"в списке задач есть задача с текстом ""(.*)""")]
    public void ThenListOfTasksContainTaskWithText(string taskText) =>
        _tasksList.Should().ContainSingle(userTask => userTask.Text == taskText);

    [Then(@"в списке задач есть (.*) задача под номером (.*)")]
    public void ThenListOfTasksContainNoCompletedWithId(string compl, string idStr)
    {
        Guid id = new Guid(idStr);
        bool isCompleted = compl == "выполненная";
        _tasksList.Should().ContainSingle(userTask => userTask.IsCompleted == isCompleted && userTask.Id == id);
    }
    
    [When(@"пользователь отменяет выполнение задачи под номером (.*-.*-.*-.*-.*)")]
    public void WhenUserMarkAsNotCompletedTask(string idStr)
    {
        _taskController = new();
        Guid id = new Guid(idStr);
        _taskController.PutNotCompleteTask(id);
    }
    
    [When(@"пользователь помечает задачу под номером (.*-.*-.*-.*-.*) как выполненную")]
    public void WhenUserMarkAsCompletedTask(string idStr)
    {
        _taskController = new();
        Guid id = new Guid(idStr);
        _taskController.PutCompleteTask(id);
    }

    [When(@"пользователь удаляет задачу под номером (.*-.*-.*-.*-.*)")]
    public void WhenUserDeleteTask(string idStr)
    {
        _taskController = new();
        var id = new Guid(idStr);
        _taskController.Delete(id);
    }

    [When(@"пользователь редактирует задачу под номером (.*-.*-.*-.*-.*) c новым текстом ""(.*)""")]
    public void WhenUserEditText(string idStr, string taskText)
    {
        _taskController = new();
        var id = new Guid(idStr);
        _taskController.PutTextTask(id, taskText);
    }
}