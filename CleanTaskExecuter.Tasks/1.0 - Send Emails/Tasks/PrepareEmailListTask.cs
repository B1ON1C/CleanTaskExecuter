using CleanTaskExecuter.Tasks._1._0___Send_Emails.Entities;
using CleanTaskExecuter.Tasks.Interface;

namespace CleanTaskExecuter.Tasks._1._0___Send_Emails.Tasks;

internal class PrepareEmailListTask : ITask<object?, EmailList>
{
    int ITask<object?, EmailList>.TaskPool => (int)TaskPools.SendEmailsPool;
    int ITask<object?, EmailList>.OrderInTaskPool => 1;
    string ITask<object?, EmailList>.TaskName => "Prepare emails task";
    string ITask<object?, EmailList>.TaskDescription => "Prepare (get and clean) emails list";
    (bool, EmailList) ITask<object?, EmailList>.Execute(object? args)
    {
        return (true, new EmailList(GenerateDummyEmails().ToList()));
    }

    private IEnumerable<string> GenerateDummyEmails()
    {
        yield return "dummyemailA@gmail.com";
        yield return "dummyemailB@gmail.com";
        yield return "dummyemailC@gmail.com";
        yield return "dummyemailD@gmail.com";
        yield return "dummyemailE@gmail.com";
    }
}