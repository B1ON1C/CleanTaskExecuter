namespace CleanTaskExecuter.Tasks.Interface;

public interface ITask<T, TResult>
{
    int TasksPool { get; }
    int OrderInTasksPool { get; }
    string TaskName { get; }
    string TaskDescription { get; }
    (bool, TResult) Execute(T arg);
}