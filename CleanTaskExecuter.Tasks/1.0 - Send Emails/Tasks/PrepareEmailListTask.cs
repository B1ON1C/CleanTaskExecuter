using CleanTaskExecuter.Tasks._1._0___Send_Emails.Entities;
using CleanTaskExecuter.Tasks.Interface;

namespace CleanTaskExecuter.Tasks._1._0___Send_Emails.Tasks;

internal class PrepareEmailListTask : ITask<object?, EmailList>
{
	public int TaskPool => (int)TaskPools.SendEmailsPool;
	public int OrderInTaskPool => 1;
	public string TaskName => "Prepare emails task";
	public string TaskDescription => "Prepare (get and clean) emails list";

	public (bool, EmailList) Execute(object? args)
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