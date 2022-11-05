using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Requester
{
    public class Request
    {
        public int _Qty { get; private set; }
        public string _Url { get; private set; }

        public Request(int Qty, string Url)
        {
            _Qty = Qty;
            _Url = Url;
        }


        public List<Task> getListTaskAsync()
        {
            var tasks = new List<Task>();
            tasks.Clear();

            for (int numReqs = 0; numReqs < _Qty; numReqs++)
            {
                var nRqs = numReqs;
                tasks.Add(DoTaskRequest(nRqs, _Url));
            }

            return tasks;

        }

        private async Task DoTaskRequest(int requestId, string requestURL) {

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Busca as informações de data da API TimeAPI
            var response = await new HttpClient().GetAsync(requestURL);

            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Request {requestId} ({stopwatch.Elapsed}) => Erro: StatusCode {response.StatusCode}");
                return;
            }

            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Request {requestId} ({stopwatch.Elapsed}) => Ok: {!string.IsNullOrEmpty(result)}");

        }


    }
}
