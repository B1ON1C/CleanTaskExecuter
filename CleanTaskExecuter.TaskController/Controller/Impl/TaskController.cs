﻿using CleanTaskExecuter.TaskController.Controller.Iface;
using CleanTaskExecuter.TaskController.Enums;
using CleanTaskExecuter.TaskController.Exceptions;
using CleanTaskExecuter.Tasks.Interface;
using System.Collections.Immutable;
using System.Reflection;

namespace CleanTaskExecuter.TaskController.Controller.Impl;

public class TaskController : ITaskController
{
    private const string TaskPool = "TaskPool";
    private const string OrderInTaskPool = "OrderInTaskPool";
    private const string ExecuteMethod = "Execute";

    private IImmutableList<Type> Tasks { get; set; } = default!;
    private IServiceProvider? ServiceProvider { get; set; } = default;

    public TaskController(IEnumerable<Type> tasksList, IServiceProvider? serviceProvider = null) =>
        (Tasks, ServiceProvider) =
            (TasksInTypesList(tasksList switch
            {
                null => throw new ArgumentNullException(nameof(tasksList)),
                var tasks => tasks
            }),
            serviceProvider);

    public TaskController(Assembly tasksAssembly, IServiceProvider? serviceProvider = null)
        : this(tasksAssembly switch
        {
            null => throw new ArgumentNullException(nameof(tasksAssembly)),
            var assembly => assembly.GetTypes()
        }, serviceProvider)
    { }

    ExecutionStatus ITaskController.ExecuteTasks()
    {
        GroupTasksByPool()
            .ToList()
            .ForEach(ProcessTaskPool);

        return ExecutionStatus.Ok;
    }

    private IImmutableList<Type> TasksInTypesList(IEnumerable<Type> tasksList) =>
        FilterTasksFromTypesList(tasksList) switch
        {
            { Count: 0 } => throw new TasksNotFoundException("No Tasks were found"),
            var loadedTassFromTypesList => loadedTassFromTypesList
        };

    private IImmutableList<Type> FilterTasksFromTypesList(IEnumerable<Type> typesList) =>
        typesList
            .Where(type => type.GetInterface(typeof(ITask<,>).Name) is not null)
            .ToImmutableList();

    private IOrderedEnumerable<IGrouping<object, object>> GroupTasksByPool() =>
        Tasks
            .Select(taskType => ServiceProvider?.GetService(taskType) ?? Activator.CreateInstance(taskType))
            .GroupBy(taskInstance => taskInstance!.GetType().GetProperty(TaskPool)!.GetValue(taskInstance))
            .OrderBy(taskGrouped => taskGrouped.Key)!;

    private void ProcessTaskPool(IGrouping<object, object> taskGroups)
    {
        var lastTaskExecutionResult = (object?)null;
        taskGroups
            .OrderBy(taskGroup => taskGroup.GetType().GetProperty(OrderInTaskPool)!.GetValue(taskGroup))
            .ToList()
            .ForEach(taskInstance => lastTaskExecutionResult = ExecuteTask(taskInstance, lastTaskExecutionResult));
    }

    private object ExecuteTask(object taskInstance, object? lastTaskExecutionResult)
    {
        var lastTaskExecutionSuccess = (bool?)lastTaskExecutionResult?.GetType().GetFields()[0].GetValue(lastTaskExecutionResult) ?? true;
        if (!lastTaskExecutionSuccess)
        {
            return (false, (object?)null);
        }

        var lastTaskExecutionEntity = lastTaskExecutionResult?.GetType().GetFields()[1].GetValue(lastTaskExecutionResult);
        var currentTaskExecutionResult = taskInstance
                                            .GetType()
                                            .GetMethod(ExecuteMethod)!
                                            .Invoke(taskInstance, new[] { lastTaskExecutionEntity });

        return currentTaskExecutionResult!;
    }
}