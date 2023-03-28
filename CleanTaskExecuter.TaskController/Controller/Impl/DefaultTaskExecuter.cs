using CleanTaskExecuter.TaskController.Controller.Iface;
using CleanTaskExecuter.TaskController.Exceptions;

namespace CleanTaskExecuter.TaskController.Controller.Impl;

public class DefaultTaskExecuter : ITaskExecuter
{
	private const string OrderInTaskPool = "OrderInTaskPool";
	private const string ExecuteMethod = "Execute";

	private IOrderedEnumerable<IGrouping<object, object>> TasksGroupedByPool { get; }

	public DefaultTaskExecuter(IOrderedEnumerable<IGrouping<object, object>> tasksGroupedByPool) =>
		TasksGroupedByPool = tasksGroupedByPool switch
		{
			null => throw new ArgumentNullException(nameof(tasksGroupedByPool)),
			var _ => tasksGroupedByPool.Any() ? tasksGroupedByPool : throw new TasksNotFoundException("No Tasks to execute"),
		};

	public void ExecuteTasks() =>
		Task.WaitAll(TasksGroupedByPool
			.Select(taskPool => Task.Factory.StartNew(() => ProcessTaskPool(taskPool)))
			.ToArray());

	private void ProcessTaskPool(IGrouping<object, object> tasksPool)
	{
		var lastTaskExecutionResult = (object?)null;
		tasksPool
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