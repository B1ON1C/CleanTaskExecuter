using CleanTaskExecuter.TaskController;
using CleanTaskExecuter.Tasks;

var tasksAssembly = typeof(TasksPools).Assembly;
var taskController = new TaskController(tasksAssembly);
taskController.LoadTasksFromAssembly();
taskController.ExecuteTasks();