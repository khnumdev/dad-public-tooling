using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTest
{
    /// <summary>
    /// Based on https://github.com/MicrosoftDocs/mslearn-hotel-reservation-system/tree/master/src/HotelReservationSystemTestClient
    /// </summary>
    class Program
    {
        public ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        private static CancellationTokenSource tokenSource = new CancellationTokenSource();
        private static string ENDPOINT = "";
        private static int WORKERS = 15;

        static void Main(string[] args)
        {
            try
            {
                // Start making requests
                var driver = new TestClientsDriver(WORKERS);
                var token = tokenSource.Token;
                Task.Factory.StartNew(() => driver.Run(token));
                Task.Factory.StartNew(() => driver.WaitForEnter(tokenSource));
                driver.runCompleteEvent.WaitOne();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Application failed with error: {e.Message}");
            }
        }

        private class TestClientsDriver
        {
            public ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
            private int numClients;

            public TestClientsDriver(int numClients)
            {
                this.numClients = numClients;
            }

            public void Run(CancellationToken token)
            {
                var rnd = new Random();

                for (int clientNum = 0; clientNum < this.numClients; clientNum++)
                {
                    string clientName = $"Client{clientNum}";

                    var client = new TestClient(clientName);
                    Task.Factory.StartNew(() => client.DoWork());
                }

                while (!token.IsCancellationRequested)
                {
                    // Run until the user stops the clients by pressing Enter
                }

                this.runCompleteEvent.Set();
            }

            public void WaitForEnter(CancellationTokenSource tokenSource)
            {
                Console.WriteLine("Press Enter to stop clients");
                Console.ReadLine();
                tokenSource.Cancel();
            }
        }

        private class TestClient
        {
            private readonly string clientName;

            internal TestClient(string clientName)
            {
                this.clientName = clientName;
            }

            internal async void DoWork()
            {
                Random rnd = new Random();

                while (true)
                {
                    var client = new HttpClient();

                    try
                    {
                        // Do a query
                        var query = $"{ENDPOINT}?pageId={rnd.Next(0, 2)}";
                        Console.WriteLine($"Client {clientName}: request to {query}");
                        var data = await client.GetStringAsync(query);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error retrieving data: {e.Message}");
                    }
                }
            }
        }
    }
}
