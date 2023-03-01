using CleanTaskExecuter.Tasks._1._0___Send_Emails.Entities;
using CleanTaskExecuter.Tasks.Interface;

namespace CleanTaskExecuter.Tasks._1._0___Send_Emails.Tasks;

internal class PrepareEmailDataTask : ITask<EmailList, AdvertisingEmail>
{
    int ITask<EmailList, AdvertisingEmail>.TaskPool => (int)TaskPools.SendEmailsPool;
    int ITask<EmailList, AdvertisingEmail>.OrderInTaskPool => 2;
    string ITask<EmailList, AdvertisingEmail>.TaskName => "Prepare email details (title and body)";
    string ITask<EmailList, AdvertisingEmail>.TaskDescription => "Prepare email data in HTML";

    (bool, AdvertisingEmail) ITask<EmailList, AdvertisingEmail>.Execute(EmailList emailList)
    {
        return (true, new AdvertisingEmail(emailList, GenerateDummyEmailData()));
    }

    private EmailData GenerateDummyEmailData()
    {
        return new EmailData("DUMMY EMAIL TITLE", "DUMMY EMAIL BODY");
    }
}
