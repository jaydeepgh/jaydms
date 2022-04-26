using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }
        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var  httpContent = new StringContent(
                JsonSerializer.Serialize(platform), 
                Encoding.UTF8, 
                "application/json");
                
            ServicePointManager.ServerCertificateValidationCallback =  delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 
            { 
                return true; 
            };    
            
            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}",httpContent);  
            if(response.IsSuccessStatusCode)
            {
                System.Console.WriteLine("--> Sync POST to Command Service is OK");
            }  
            else
            {
                System.Console.WriteLine("--> Sync POST to Command Service is NOT OK");

            }
        }
    }
}