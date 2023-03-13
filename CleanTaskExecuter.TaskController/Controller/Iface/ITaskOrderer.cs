namespace CleanTaskExecuter.TaskController.Controller.Iface;

public interface ITaskOrderer
{
	IOrderedEnumerable<IGrouping<object, object>> OrderTasksGrupedByPool();
}