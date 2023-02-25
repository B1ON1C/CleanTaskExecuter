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
        const string TaskName = "TaskName";
        const string TaskDescription = "TaskDescription";
        const string ExecuteMethod = "Execute";

        Tasks
            .Select(Activator.CreateInstance)
            .Where(taskInstance => taskInstance != null)
            .GroupBy(taskInstance => taskInstance!.GetType().GetField(TasksPool))
            .OrderBy(taskGrouped => taskGrouped.Key)
            .ToList()
            .ForEach(taskGroups =>
            {
                object lastTaskExecutionResult = null;
                foreach (var taskInstance in taskGroups.OrderBy(taskGroup => taskGroup!.GetType().GetField(OrderInTasksPool)).ToList())
                {
                    lastTaskExecutionResult = taskInstance!
                                            .GetType()
                                            .GetMethod(ExecuteMethod)!
                                            .Invoke(taskInstance, new[] { lastTaskExecutionResult?.GetType()?.GetFields()?[1].GetValue(lastTaskExecutionResult) })!;

                    var executeOk = lastTaskExecutionResult?.GetType()?.GetFields()?[0].GetValue(lastTaskExecutionResult);
                    var result = lastTaskExecutionResult?.GetType()?.GetFields()?[1].GetValue(lastTaskExecutionResult);

                    if (!(bool)executeOk)
                    {
                        break;
                    }
                }
            });

        return ExecutionStatus.Ok;
    }

    private IImmutableList<Type> GetTasksInAssembly(Assembly TasksAssembly) =>
        TasksAssembly
            .GetTypes()
            .Where(type => type.GetInterface(typeof(ITask<,>).Name) != null)
            .ToImmutableList();
}
