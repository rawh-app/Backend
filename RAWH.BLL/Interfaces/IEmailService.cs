namespace RAWH.BLL.Interfaces
{
    public interface IEmailService
    {
        Task<string> sendEmail(string email, string message);
    }
}
