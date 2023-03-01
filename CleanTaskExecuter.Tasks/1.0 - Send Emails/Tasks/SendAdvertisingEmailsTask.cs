using CleanTaskExecuter.Tasks._1._0___Send_Emails.Entities;
using CleanTaskExecuter.Tasks.Interface;

namespace CleanTaskExecuter.Tasks._1._0___Send_Emails.Tasks;

internal class SendAdvertisingEmailsTask : ITask<AdvertisingEmail, object?>
{
    int ITask<AdvertisingEmail, object?>.TaskPool => (int)TaskPools.SendEmailsPool;

    int ITask<AdvertisingEmail, object?>.OrderInTaskPool => 3;

    string ITask<AdvertisingEmail, object?>.TaskName => "Send advertising emails task";

    string ITask<AdvertisingEmail, object?>.TaskDescription => "Send advertising emails to all subscribed email list";

    (bool, object?) ITask<AdvertisingEmail, object?>.Execute(AdvertisingEmail advertisingEmail)
    {
        DummySendEmails(advertisingEmail);
        return (true, null);
    }

    private void DummySendEmails(AdvertisingEmail advertisingEmail)
    {
        advertisingEmail.emailList.Emails.ToList().ForEach(email => Thread.Sleep(100));
    }
}