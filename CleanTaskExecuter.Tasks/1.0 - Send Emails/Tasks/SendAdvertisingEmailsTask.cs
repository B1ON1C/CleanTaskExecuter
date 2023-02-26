using CleanTaskExecuter.Tasks._1._0___Send_Emails.Entities;
using CleanTaskExecuter.Tasks.Interface;

namespace CleanTaskExecuter.Tasks._1._0___Send_Emails.Tasks;

internal class SendAdvertisingEmailsTask : ITask<EmailList, object?>
{
    public int TasksPool => (int)TasksPools.SendEmailsPool;

    public int OrderInTasksPool => 2;

    public string TaskName => "Send advertising emails task";

    public string TaskDescription => "Send advertising emails to all subscribed (and cleaned) emails list";

    public (bool, object?) Execute(EmailList args)
    {
        return (true, null);
    }
}
