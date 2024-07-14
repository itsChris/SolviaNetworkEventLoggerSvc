using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NetworkEventLoggerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string pingAddress1 = "1.1.1.1";
        private readonly string pingAddress2 = "192.168.147.1";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(NetworkAddressChanged);

            _logger.LogInformation("Monitoring network events and pinging addresses.");

            _ = StartPinging(pingAddress1, stoppingToken);
            _ = StartPinging(pingAddress2, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void NetworkAddressChanged(object sender, EventArgs e)
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var ni in networkInterfaces)
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    LogNetworkEvent(ni, "Connected");
                }
                else
                {
                    LogNetworkEvent(ni, "Disconnected");
                }
            }
        }

        private void LogNetworkEvent(NetworkInterface ni, string status)
        {
            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_NetworkEvents.csv";
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss};{ni.Name};{ni.NetworkInterfaceType};{status}";

            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine(logMessage);
            }

            _logger.LogInformation(logMessage);
        }

        private async Task StartPinging(string address, CancellationToken stoppingToken)
        {
            Ping pingSender = new Ping();
            string tag = address.Replace('.', '-');
            string fileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_ping-{tag}.txt";

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    PingReply reply = await pingSender.SendPingAsync(address);

                    string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss};{address};{reply.Status};{reply.RoundtripTime}ms";

                    using (StreamWriter writer = new StreamWriter(fileName, true))
                    {
                        writer.WriteLine(logMessage);
                    }

                    _logger.LogInformation(logMessage);
                }
                catch (Exception ex)
                {
                    string errorLogMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss};{address};Error;{ex.Message}";

                    using (StreamWriter writer = new StreamWriter(fileName, true))
                    {
                        writer.WriteLine(errorLogMessage);
                    }

                    _logger.LogError(errorLogMessage);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
