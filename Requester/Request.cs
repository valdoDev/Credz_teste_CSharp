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

            bool isSucessRequest = false;
            String responseString = "";
            System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;

            try
            {
                // Busca as informações de data da API TimeAPI
                var response = await new HttpClient().GetAsync(requestURL);
                isSucessRequest = !response.IsSuccessStatusCode;
                responseString = await response.Content.ReadAsStringAsync();
                statusCode = response.StatusCode;
            }
            catch (Exception ex)
            {}
            
            stopwatch.Stop();

            if (isSucessRequest)
                Console.WriteLine($"Request {requestId} | Running time {stopwatch.Elapsed} => Erro: StatusCode {statusCode}");
            else
                Console.WriteLine($"Request {requestId} | Running time {stopwatch.Elapsed}  => Ok: {!string.IsNullOrEmpty(responseString)}");
            
        }


    }
}
