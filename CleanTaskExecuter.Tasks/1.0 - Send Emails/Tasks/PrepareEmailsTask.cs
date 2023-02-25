using CleanTaskExecuter.Tasks._1._0___Send_Emails.Entities;
using CleanTaskExecuter.Tasks.Interface;

namespace CleanTaskExecuter.Tasks._1._0___Send_Emails.Tasks;

public class PrepareEmailsTask : ITask<object?, EmailList>
{
    public int TasksPool => (int)TasksPools.SendEmailsPool;

    public int OrderInTasksPool => 1;

    public string TaskName => "Prepare emails task";

    public string TaskDescription => "Prepare (get and clean) emails list";

    public (bool, EmailList) Execute(object? args)
    {
        return (true, new EmailList());
    }
}
