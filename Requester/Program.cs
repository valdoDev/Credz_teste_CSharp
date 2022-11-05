using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Requester
{
    internal class Program
    {
        private const string PORTAL_URL = "http://localhost:5000/Home/index";
        private static readonly CancellationTokenSource source = new CancellationTokenSource();
        private static readonly CancellationToken token = source.Token;

        private static async Task Main(string[] args)
        {
            Console.CancelKeyPress += (o, e) => source.Cancel();

            int requestQty = 0;

            do
            {
                Console.WriteLine("Número de Requests concorrentes: ");

                if (int.TryParse(Console.ReadLine(), out requestQty))
                {
                    var createTasks = new Request(requestQty, PORTAL_URL);
                    var listTask = createTasks.getListTaskAsync();

                    await Task.WhenAll(listTask);
                    
                    Console.WriteLine("Processamento finalizado!");
                    Console.WriteLine();
                    
                }
            }
            while (!token.IsCancellationRequested);
        }

        
    }
}