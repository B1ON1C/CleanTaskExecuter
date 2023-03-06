using CleanTaskExecuter.TaskController.Enums;

namespace CleanTaskExecuter.TaskController.Controller.Iface;

public interface ITaskController
{
    ExecutionStatus ExecuteTasks();
}