using CleanTaskExecuter.TaskController.Enums;

namespace CleanTaskExecuter.TaskController;

public interface ITaskController
{
    ExecutionStatus ExecuteTasks();
}
