namespace CleanTaskExecuter.Tasks.Interface;

public interface ITask<T, TResult>
{
    int TaskPool { get; }
    int OrderInTaskPool { get; }
    string TaskName { get; }
    string TaskDescription { get; }

    (bool, TResult) Execute(T arg);
}