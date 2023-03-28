using CleanTaskExecuter.Tasks._1._0___Send_Emails.Entities;
using CleanTaskExecuter.Tasks.Interface;

namespace CleanTaskExecuter.Tasks._1._0___Send_Emails.Tasks;

internal class SendAdvertisingEmailsTask : ITask<AdvertisingEmail, object?>
{
	public int TaskPool => (int)TaskPools.SendEmailsPool;

	public int OrderInTaskPool => 3;

	public string TaskName => "Send advertising emails task";

	public string TaskDescription => "Send advertising emails to all subscribed email list";

	public (bool, object?) Execute(AdvertisingEmail advertisingEmail)
	{
		DummySendEmails(advertisingEmail);
		return (true, null);
	}

	private void DummySendEmails(AdvertisingEmail advertisingEmail) =>
		advertisingEmail.emailList.Emails.ToList().ForEach(email => Task.Delay(100));
}