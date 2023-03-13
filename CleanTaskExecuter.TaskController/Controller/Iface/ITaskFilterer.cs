using System.Collections.Immutable;

namespace CleanTaskExecuter.TaskController.Controller.Iface;

public interface ITaskFilterer
{
	IImmutableList<Type> FilterTasks();
}