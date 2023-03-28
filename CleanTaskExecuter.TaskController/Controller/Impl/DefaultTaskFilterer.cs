using CleanTaskExecuter.TaskController.Controller.Iface;
using CleanTaskExecuter.TaskController.Exceptions;
using CleanTaskExecuter.Tasks.Interface;
using System.Collections.Immutable;
using System.Reflection;

namespace CleanTaskExecuter.TaskController.Controller.Impl;

public class DefaultTaskFilterer : ITaskFilterer
{
	private IImmutableList<Type> Types { get; }

	public DefaultTaskFilterer(IEnumerable<Type> types) =>
		Types = types switch
		{
			null => throw new ArgumentNullException(nameof(types)),
			var _ => types.ToImmutableList()
		};

	public DefaultTaskFilterer(Assembly assembly)
		: this(assembly switch
		{
			null => throw new ArgumentNullException(nameof(assembly)),
			var _ => assembly.GetTypes()
		})
	{ }

	public IImmutableList<Type> FilterTasks() =>
		FilterTasksFromTypesList() switch
		{
			{ Count: 0 } => throw new TasksNotFoundException("No Tasks were found"),
			var filteredTasksFromTypesList => filteredTasksFromTypesList
		};

	private IImmutableList<Type> FilterTasksFromTypesList() =>
		Types
			.Where(type => type.GetInterface(typeof(ITask<,>).Name) is not null)
			.ToImmutableList();
}