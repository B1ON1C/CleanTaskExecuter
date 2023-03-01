using CleanTaskExecuter.TaskController;
using System.Reflection;

const string TasksAssemblyName = "CleanTaskExecuter.Tasks";
var tasksAssembly = Assembly.Load(TasksAssemblyName);

var taskController = (ITaskController)new TaskController(tasksAssembly);
taskController.ExecuteTasks();