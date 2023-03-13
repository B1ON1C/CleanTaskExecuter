using CleanTaskExecuter.TaskController.Controller.Iface;
using CleanTaskExecuter.TaskController.Enums;
using System.Collections.Immutable;
using System.Reflection;

namespace CleanTaskExecuter.TaskController.Controller.Impl;

public class DefaultTaskController : ITaskController
{
	private ITaskFilterer TaskFilterer { get; }
	private ITaskInstanceResolver TaskInstanceResolver { get; }
	private ITaskOrderer TaskOrderer { get; }
	private ITaskExecuter TaskExecuter { get; }

	public DefaultTaskController(ITaskFilterer taskFilterer, ITaskInstanceResolver taskInstanceResolver, ITaskOrderer taskOrderer, ITaskExecuter taskExecuter) =>
		(TaskFilterer, TaskInstanceResolver, TaskOrderer, TaskExecuter) =
		(taskFilterer switch
		{
			null => throw new ArgumentNullException(nameof(taskFilterer)),
			var _ => taskFilterer
		},
		taskInstanceResolver switch
		{
			null => throw new ArgumentNullException(nameof(taskInstanceResolver)),
			var _ => taskInstanceResolver
		},
		taskOrderer switch
		{
			null => throw new ArgumentNullException(nameof(taskOrderer)),
			var _ => taskOrderer
		},
		taskExecuter switch
		{
			null => throw new ArgumentNullException(nameof(taskExecuter)),
			var _ => taskExecuter
		});

	public DefaultTaskController(IImmutableList<Type> Types, IServiceProvider? serviceProvider = null)
	{
		TaskFilterer = new DefaultTaskFilterer(Types);
		TaskInstanceResolver = new DefaultTaskInstanceResolver(serviceProvider);
		TaskOrderer = new DefaultTaskOrderer(TaskFilterer.FilterTasks(), TaskInstanceResolver);
		TaskExecuter = new DefaultTaskExecuter(TaskOrderer.OrderTasksGrupedByPool());
	}

	public DefaultTaskController(Assembly assembly, IServiceProvider? serviceProvider = null)
	{
		TaskFilterer = new DefaultTaskFilterer(assembly);
		TaskInstanceResolver = new DefaultTaskInstanceResolver(serviceProvider);
		TaskOrderer = new DefaultTaskOrderer(TaskFilterer.FilterTasks(), TaskInstanceResolver);
		TaskExecuter = new DefaultTaskExecuter(TaskOrderer.OrderTasksGrupedByPool());
	}

	public ExecutionStatus ExecuteTasks()
	{
		try
		{
			TaskExecuter.ExecuteTasks();
			return ExecutionStatus.Ok;
		}
		catch
		{
			return ExecutionStatus.Fail;
		}
	}
}