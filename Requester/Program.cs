using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Requester
{
    internal class Program
    {
        private const string PORTAL_URL = "http://localhost:5000/Home/information";
        private static readonly CancellationTokenSource source = new CancellationTokenSource();
        private static readonly CancellationToken token = source.Token;

        private static void Main(string[] args)
        {
            Console.CancelKeyPress += (o, e) => source.Cancel();

            int requestQty = 0;

            do
            {
                Console.WriteLine("Número de Requests concorrentes: ");

                if (int.TryParse(Console.ReadLine(), out requestQty))
                {
                    var createTasks = new Request(requestQty, PORTAL_URL, DoRequest);

                    createTasks.getListTask().All((taskRun) => { 
                        taskRun.Start(); 
                        return true; 
                    } );

                    Console.WriteLine();

                }
            }
            while (!token.IsCancellationRequested);
        }

        private static Action<int, string> DoRequest = async (int requestId, string requestURL) =>
        {
            var timeStart = DateTime.Now.ToString("HH:mm:ss.fffffff");

            // Busca as informações de data da API TimeAPI
            var response = await new HttpClient().GetAsync(requestURL);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Request {requestId} ({timeStart}) => Erro: StatusCode {response.StatusCode}");
                return;
            }

            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Request {requestId} ({timeStart}) => Ok: {!string.IsNullOrEmpty(result)}");

        };
    }
}