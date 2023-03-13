namespace CleanTaskExecuter.TaskController.Controller.Iface;

public interface ITaskInstanceResolver
{
	object ResolveType(Type type);
}