using CleanTaskExecuter.TaskController;
using System.Reflection;

const string TasksAssemblyName = "CleanTaskExecuter.Tasks";
var tasksAssembly = Assembly.Load(TasksAssemblyName);

var taskController = new TaskController(tasksAssembly);
taskController.ExecuteTasks();