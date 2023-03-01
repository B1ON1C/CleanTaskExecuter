using CleanTaskExecuter.TaskController.Enums;
using CleanTaskExecuter.TaskController.Exceptions;
using CleanTaskExecuter.Tasks.Interface;
using System.Collections.Immutable;
using System.Reflection;

namespace CleanTaskExecuter.TaskController;

public class TaskController : ITaskController
{
    private IImmutableList<Type> Tasks { get; set; } = default!;
    public TaskController(IList<Type> tasksList) => LoadTasksFromTypesList(tasksList);
    public TaskController(Assembly tasksAssembly) : this(tasksAssembly.GetTypes()) { }

    ExecutionStatus ITaskController.ExecuteTasks()
    {
        GroupTasksByPool()
            .ToList()
            .ForEach(ProcessTaskPool);

        return ExecutionStatus.Ok;
    }

    private void LoadTasksFromTypesList(IList<Type> tasksList)
    {
        Tasks = GetTasksInTypesList(tasksList);
        if (Tasks.Count == 0)
        {
            throw new AssemblyTasksNotFoundException($"No Tasks found");
        }
    }

    private IImmutableList<Type> GetTasksInTypesList(IList<Type> typesList) =>
        typesList
            .Where(type => type.GetInterface(typeof(ITask<,>).Name) != null)
            .ToImmutableList();

    private IList<IGrouping<object?, object?>> GroupTasksByPool()
    {
        const string TaskPool = "TaskPool";

        return Tasks
                .Select(Activator.CreateInstance)
                .Where(taskInstance => taskInstance != null)
                .GroupBy(taskInstance => taskInstance!.GetType().GetProperty(TaskPool)!.GetValue(taskInstance))
                .Where(taskGroup => taskGroup.Key != null)
                .OrderBy(taskGrouped => taskGrouped.Key)
                .ToList();
    }
    private void ProcessTaskPool(IGrouping<object?, object?> taskGroups)
    {
        const string OrderInTaskPool = "OrderInTaskPool";

        var continueExecutingPoolTasks = true;
        var lastTaskExecutionResult = (object?)null;
        taskGroups
            .OrderBy(taskGroup => taskGroup!.GetType()!.GetProperty(OrderInTaskPool)!.GetValue(taskGroup))
            .ToList()
            .ForEach(taskInstance => (continueExecutingPoolTasks, lastTaskExecutionResult) = ExecuteTask(taskInstance, continueExecutingPoolTasks, lastTaskExecutionResult));
    }
    private (bool, object?) ExecuteTask(object? taskInstance, bool continueExecutingPoolTasks, object? lastTaskExecutionResult)
    {
        const string ExecuteMethod = "Execute";

        if (!continueExecutingPoolTasks)
        {
            return (false, null);
        }

        var lastTaskExecutionEntity = lastTaskExecutionResult?.GetType()?.GetFields()?[1].GetValue(lastTaskExecutionResult);
        var currentTaskExecutionResult = taskInstance!
                                            .GetType()!
                                            .GetMethod(ExecuteMethod)!
                                            .Invoke(taskInstance, new[] { lastTaskExecutionEntity });
        var currentTaskExecutionSuccess = (bool)currentTaskExecutionResult?.GetType()?.GetFields()?[0].GetValue(currentTaskExecutionResult)!;
        return (currentTaskExecutionSuccess, currentTaskExecutionResult);
    }
}
