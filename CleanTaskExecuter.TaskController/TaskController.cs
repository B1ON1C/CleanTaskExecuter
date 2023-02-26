using CleanTaskExecuter.TaskController.Enums;
using CleanTaskExecuter.Tasks.Interface;
using System.Collections.Immutable;
using System.Reflection;

namespace CleanTaskExecuter.TaskController;

public class TaskController
{
    private Assembly TasksAssembly { get; }
    private IImmutableList<Type> Tasks { get; set; } = default!;
    public TaskController(Assembly tasksAssembly)
    {
        TasksAssembly = tasksAssembly ?? throw new ArgumentNullException(nameof(tasksAssembly));
    }

    public void LoadTasksFromAssembly()
    {
        Tasks = GetTasksInAssembly(TasksAssembly);
    }

    public ExecutionStatus ExecuteTasks()
    {
        const string TasksPool = "TasksPool";
        const string OrderInTasksPool = "OrderInTasksPool";
        const string ExecuteMethod = "Execute";

        Tasks
            .Select(Activator.CreateInstance)
            .Where(taskInstance => taskInstance != null)
            .GroupBy(taskInstance => taskInstance!.GetType().GetProperty(TasksPool)!.GetValue(taskInstance))
            .Where(taskGroup => taskGroup.Key != null)
            .OrderBy(taskGrouped => taskGrouped.Key)
            .ToList()
            .ForEach(taskGroups =>
            {
                var lastTaskExecutionResult = (object?)null;
                var continueExecutingPoolTasks = true;

                taskGroups
                    .OrderBy(taskGroup => taskGroup!.GetType()!.GetProperty(OrderInTasksPool)!.GetValue(taskGroup))
                    .ToList()
                    .ForEach(taskInstance =>
                    {
                        if (!continueExecutingPoolTasks)
                        {
                            return;
                        }

                        var lastTaskExecutionEntity = lastTaskExecutionResult?.GetType()?.GetFields()?[1].GetValue(lastTaskExecutionResult);
                        lastTaskExecutionResult = taskInstance!
                                                    .GetType()!
                                                    .GetMethod(ExecuteMethod)!
                                                    .Invoke(taskInstance, new[] { lastTaskExecutionEntity });

                        var lastTaskExecutionSuccess = lastTaskExecutionResult?.GetType()?.GetFields()?[0].GetValue(lastTaskExecutionResult);
                        continueExecutingPoolTasks = (bool)lastTaskExecutionSuccess!;
                    });
            });

        return ExecutionStatus.Ok;
    }

    private IImmutableList<Type> GetTasksInAssembly(Assembly TasksAssembly) =>
        TasksAssembly
            .GetTypes()
            .Where(type => type.GetInterface(typeof(ITask<,>).Name) != null)
            .ToImmutableList();
}
