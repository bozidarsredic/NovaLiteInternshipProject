using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using ToDoApi.Repositories;
using ToDoApi.Security;
using ToDoCore;

namespace ToDoApi.Services
{
    public class RemainderService : IHostedService, IDisposable
    {
        private Timer? _timer = null;
        public readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ApiKeyOptions _apiKeyOptions;

        public RemainderService(IServiceScopeFactory serviceScopeFactory, IOptions<ApiKeyOptions> apiKeyOptions)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _apiKeyOptions = apiKeyOptions.Value;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
             TimeSpan.FromSeconds(15));
            return Task.CompletedTask;
        }


        private void DoWork(object? state)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IRepository<ToDoList>>();

            var lists = service.Find(list => list.Remainder < DateTime.UtcNow && !list.IsRemind);
            if (lists != null)
            {
                foreach (var list in lists)
                {
                    var apiKey = _apiKeyOptions.Key;
                    var client = new SendGridClient(apiKey);
                    var from = new EmailAddress("sredicb9@gmail.com", "bozo");
                    var subject = "Alert";
                    var to = new EmailAddress("vasilijeu99@gmail.com", "Vasilije");
                    var plainTextContent = "Your reminder" + list.Title + "has expired!";
                    var htmlContent = "<strong>Your reminder " + list.Title + " has expired!</strong>";
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                    var response = client.SendEmailAsync(msg);

                    list.IsRemind = true;
                    service.SaveChanges();
                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
