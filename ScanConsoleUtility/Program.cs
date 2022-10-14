using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ScanConsoleUtility
{
    class Program
    {
        // Загрузка конфигураций из appsettings.json
        private static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            return builder.Build();
        }
        static async Task Main(string[] args)
        {
            var configuration = GetConfiguration();
            var url = configuration["ServiceUrl"];
            
            
            var command = Console.ReadLine();

            if (command != ScanCommands.ScanService)
            {
                Console.WriteLine("Unknown command");
                return;
            }
            
            await LaunchUtility(url);
        }

        static async Task LaunchUtility(string serviceUrl)
        {
            Console.WriteLine("Scan service was started. \n Press <Enter> to exit...");

            var command = Console.ReadLine();

            while (command != "")
            {
                var commands = command.Split(" ");

                if (commands.Length != 3)
                {
                    Console.WriteLine("Unknown command");
                    command = Console.ReadLine();
                    continue;
                }
                
                // создаем httpClient
                using var serviceHttpClient = new HttpClient();
                serviceHttpClient.BaseAddress = new Uri(serviceUrl);
                
                switch (commands[0] + " " + commands[1])
                {
                    case ScanCommands.ScanUtilScan:
                        await ScanUtilScanCommand(commands[2], serviceHttpClient);
                        break;
                    case ScanCommands.ScanUtilStatus:
                        await ScanUtilStatusCommand(commands[2], serviceHttpClient);
                        break;
                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
                
                command = Console.ReadLine();
            }
        }

        static async Task PrintError(HttpResponseMessage message)
        {
            var stringResult = await message.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Response>(stringResult);

            if (result.Message != null)
            {
                Console.WriteLine(result.Message);
            }
            else if (result.Error != null)
            {
                Console.WriteLine(result.Message);
            }
        }
        static async Task ScanUtilScanCommand(string scanDirectory, HttpClient scanClient)
        {
            var result = await scanClient.PostAsync("/api/Scan?directory=" + scanDirectory, null);
            
            if (result.StatusCode != HttpStatusCode.OK)
            {
                await PrintError(result);
                return;
            }

            var id = await result.Content.ReadAsStringAsync();
            
            Console.WriteLine($"Scan task was created with ID: {id}");
        }

        static async Task ScanUtilStatusCommand(string id, HttpClient scanClient)
        {
            var result = await scanClient.GetAsync("/api/Scan?id=" + id);
            
            if (result.StatusCode != HttpStatusCode.OK)
            {
                await PrintError(result);
                return;
            }

            var status = await result.Content.ReadAsStringAsync();
            
            Console.WriteLine(status);
        }
        
    }
}