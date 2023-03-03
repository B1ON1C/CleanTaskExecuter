using CleanTaskExecuter.Tasks._1._0___Send_Emails.Entities;
using CleanTaskExecuter.Tasks.Interface;

namespace CleanTaskExecuter.Tasks._1._0___Send_Emails.Tasks;

internal class PrepareEmailDataTask : ITask<EmailList, AdvertisingEmail>
{
    public int TaskPool => (int)TaskPools.SendEmailsPool;
    public int OrderInTaskPool => 2;
    public string TaskName => "Prepare email details (title and body)";
    public string TaskDescription => "Prepare email data in HTML";

    public (bool, AdvertisingEmail) Execute(EmailList emailList)
    {
        return (true, new AdvertisingEmail(emailList, GenerateDummyEmailData()));
    }

    EmailData GenerateDummyEmailData()
    {
        return new EmailData("DUMMY EMAIL TITLE", "DUMMY EMAIL BODY");
    }
}
