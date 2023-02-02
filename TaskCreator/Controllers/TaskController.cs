using Microsoft.AspNetCore.Mvc;
using TaskCreator.Models;

namespace TaskCreator.Controllers;

/// <summary>
/// Контроллер для получения данных о членах семьи 
/// </summary>
[ApiController]
[Route("tasks")]
[Consumes("application/json")]
[Produces("application/json")]
public class TaskController : ControllerBase
{
    private static List<FamilyMember>? _familyMembers = new();
    private static List<UserTask>? _tasks = new();
    private static FamilyMember? _user = 
        new(new Guid(), 0, "Alex", "0000000", "alex@mail.com", true);

    /// <summary>
    /// Инициализация списка членов семьи
    /// </summary>
    /// <param name="familyMembers">Список членов семьи</param>
    public void InitFamilyMembers(List<FamilyMember> familyMembers)
    {
        ArgumentNullException.ThrowIfNull(familyMembers, nameof(familyMembers));
        _familyMembers = familyMembers;
    }
    
    /// <summary>
    /// Инициализация списка задач
    /// </summary>
    /// <param name="tasks">Список задач</param>
    public void InitTasksList(List<UserTask> tasks)
    {
        ArgumentNullException.ThrowIfNull(tasks, nameof(tasks));
        _tasks = tasks;
    }

    /// <summary>
    /// Вход члена семьи
    /// </summary>
    /// <param name="name">Имя члена семьи</param>
    /// <returns name="true">Член семьи найден</returns>
    /// <returns name="false">Член семьи не найден</returns>
    public bool LogInUser(string name)
    {
        if (_familyMembers == null) return false;
        _user = _familyMembers.Find(x=>x.Name == name);
        return _user != null;
    }
    
    /// <summary>
    /// Возвращает информацию о задаче
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <response code="200">Информация о задаче</response>
    [HttpGet("{id:guid}")]
    public Task<ActionResult<IList<UserTask>>> Get(Guid id)
    {
        if (_tasks == null) 
            return Task.FromResult<ActionResult<IList<UserTask>>>(NotFound());
        
        var task = _tasks.Find(x => x.Id == id);

        if (task is null)
            return Task.FromResult<ActionResult<IList<UserTask>>>(NotFound());

        return Task.FromResult<ActionResult<IList<UserTask>>>(Ok(task));
    }

    /// <summary>
    /// Возвращает список задач
    /// </summary>
    /// <response code="200">Информация о задаче</response>
    [HttpGet]
    public IList<UserTask> Get()
    {
        return _tasks;
    }

    /// <summary>
    /// Добавляет новую задачу
    /// </summary>
    /// <param name="userUserTaskData">Информация о добавляемом члене семьи</param>
    /// <response code="200">Задача добавлена</response>
    [HttpPost]
    public Task<ActionResult> Post(AddUserTaskDto userUserTaskData)
    {
        if (_user == null || _tasks == null)
            return Task.FromResult<ActionResult>(NotFound());
        if (!_user.IsAdult || userUserTaskData.Text == "")
            return Task.FromResult<ActionResult>(Forbid());
    
        UserTask task = new UserTask(new Guid(), 1, 
            userUserTaskData.Text, false, userUserTaskData.DateDeadline);
        _tasks.Add(task);
    
        return Task.FromResult<ActionResult>(CreatedAtAction(nameof(Get), task));
    }

    /// <summary>
    /// Удаляет задачу из списка по её идентификатору
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <response code="200">Задача удалена</response>
    /// <response code="404">Задача не найдена</response>
    [HttpDelete("{id:guid}")]
    public Task<ActionResult> Delete(Guid id)
    {
        if (_tasks == null || _user == null) return Task.FromResult<ActionResult>(NotFound());
        if (!_user.IsAdult) return Task.FromResult<ActionResult>(Forbid());
        int res = _tasks.RemoveAll(userTask => userTask.Id == id);
        if (res == 0) return Task.FromResult<ActionResult>(NotFound());
        return Task.FromResult<ActionResult>(Ok());
    }

    /// <summary>
    /// Помечает задачу как выполненную по её идентификатору
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <response code="200">Задача помечена как выполненная</response>
    /// <response code="404">Задача не найдена</response>
    [HttpPut("markAsCompleted{id:guid}")]
    public Task<ActionResult> PutCompleteTask(Guid id)
    {
        if (_tasks == null || _user == null) return Task.FromResult<ActionResult>(NotFound());
        if (!_user.IsAdult) return Task.FromResult<ActionResult>(Forbid());
        UserTask? task = _tasks.Find(x => x.Id == id);
        if (task == null) return Task.FromResult<ActionResult>(NotFound());
        if (!task.IsCompleted)
        {
            _tasks.RemoveAll(x => x.Id == id);
            _tasks.Add(task with { IsCompleted = true });
        }
        return Task.FromResult<ActionResult>(Ok());
    }
    
    /// <summary>
    /// Помечает задачу как невыполненную по её идентификатору
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <response code="200">Задача помечена как невыполненная</response>
    /// <response code="404">Задача не найдена</response>
    [HttpPut("markAsNotCompleted{id:guid}")]
    public Task<ActionResult> PutNotCompleteTask(Guid id)
    {
        if (_tasks == null || _user == null) return Task.FromResult<ActionResult>(NotFound());
        if (!_user.IsAdult) return Task.FromResult<ActionResult>(Forbid());
        UserTask? task = _tasks.Find(x => x.Id == id);
        if (task == null) return Task.FromResult<ActionResult>(NotFound());
        if (task.IsCompleted)
        {
            _tasks.RemoveAll(x => x.Id == id);
            _tasks.Add(task with { IsCompleted = false });
        }
        return Task.FromResult<ActionResult>(Ok());
    }

    /// <summary>
    /// Изменяет текст задачи с данным идентификатором
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <param name="text">Новый текст задачи</param>
    /// <response code="200">Задача отредактирована</response>
    /// <response code="404">Задача не найдена</response>
    [HttpPut("changeText{id:guid}")]
    public Task<ActionResult> PutTextTask(Guid id, string text)
    {
        if (_tasks == null || _user == null) return Task.FromResult<ActionResult>(NotFound());
        if (!_user.IsAdult || text == "") return Task.FromResult<ActionResult>(Forbid());
        
        UserTask? task = _tasks.Find(x => x.Id == id);
        if (task == null) return Task.FromResult<ActionResult>(NotFound());
        
        _tasks.RemoveAll(x => x.Id == id);
        _tasks.Add(task with { Text = text });
        
        return Task.FromResult<ActionResult>(Ok());
    }

    /// <summary>
    /// Информация о добавленной задаче
    /// </summary>
    /// <param name="Text">Текст задачи</param>
    /// <param name="DateDeadline">Дата дедлайна</param>
    public record AddUserTaskDto(
        string Text, 
        DateTime DateDeadline);
}