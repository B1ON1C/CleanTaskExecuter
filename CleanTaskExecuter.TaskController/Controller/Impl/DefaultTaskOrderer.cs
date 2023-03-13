using CleanTaskExecuter.TaskController.Controller.Iface;
using CleanTaskExecuter.TaskController.Exceptions;
using System.Collections.Immutable;

namespace CleanTaskExecuter.TaskController.Controller.Impl;

public class DefaultTaskOrderer : ITaskOrderer
{
	private const string TaskPool = "TaskPool";

	private IImmutableList<Type> Tasks { get; }
	private ITaskInstanceResolver TaskInstanceResolver { get; }

	public DefaultTaskOrderer(IImmutableList<Type> tasks, ITaskInstanceResolver taskInstanceResolver) =>
		(Tasks, TaskInstanceResolver) =
		(tasks switch
		{
			null => throw new ArgumentNullException(nameof(tasks)),
			{ Count: 0 } => throw new TasksNotFoundException("No Tasks to order"),
			var _ => tasks
		},
		taskInstanceResolver switch
		{
			null => throw new ArgumentNullException(nameof(taskInstanceResolver)),
			var _ => taskInstanceResolver
		});

	public IOrderedEnumerable<IGrouping<object, object>> OrderTasksGrupedByPool() =>
		Tasks
			.Select(TaskInstanceResolver.ResolveType)
			.GroupBy(taskInstance => taskInstance!.GetType().GetProperty(TaskPool)!.GetValue(taskInstance))
			.OrderBy(taskGrouped => taskGrouped.Key)!;
}