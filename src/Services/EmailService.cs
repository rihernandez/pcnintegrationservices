using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using pcnintegrationservices.Configuration;

namespace pcnintegrationservices.Services
{
    public class EmailService
    {
        private readonly MailgunSettings _mailgunSettings;
        private readonly HttpClient _httpClient;

        public EmailService(IOptions<MailgunSettings> mailgunSettings)
        {
            _mailgunSettings = mailgunSettings.Value;
            _httpClient = new HttpClient();
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            var requestUri = $"https://api.mailgun.net/v3/{_mailgunSettings.Domain}/messages";
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"api:{_mailgunSettings.ApiKey}"));

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("from", $"Notificaciones pcn <{_mailgunSettings.FromEmail}>"),
                new KeyValuePair<string, string>("to", toEmail),
                new KeyValuePair<string, string>("subject", subject),
                new KeyValuePair<string, string>("text", body)
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

            var response = await _httpClient.PostAsync(requestUri, content);
            return response.IsSuccessStatusCode;
        }
    }
}
