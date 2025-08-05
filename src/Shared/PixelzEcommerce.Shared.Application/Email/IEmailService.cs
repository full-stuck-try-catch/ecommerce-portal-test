namespace PixelzEcommerce.Shared.Application.Email;

public interface IEmailService
{
    Task SendAsync(string recipient, string subject, string body);
}
