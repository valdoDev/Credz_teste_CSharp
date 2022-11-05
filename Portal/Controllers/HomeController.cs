using Humanizer.Bytes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    public class HomeController : Controller
    {
        public static int _requestsCounter = 0;

        public async Task<IActionResult> Index()
        {
            // Incrementa o contador de requisições
            _requestsCounter++;

            // Verifica se a API tem recursos para atender a requisição
            if (!Program.CheckIfThereIsThreadAvailable())
                return BadRequest(Messages.THREAD_IS_NO_THREAD_AVAILABLE);

            string dateTimeNow = "", key = "", sum = "";

            try
            {
                var resultKey = await GenerateKey(Program.API_ADDRESS);

                dateTimeNow = resultKey["DateTimeNow"] ?? "";
                key = resultKey["Key"] ?? "";
                sum = resultKey["Sum"] ?? "";

            }
            catch (Exception ex)
            {
                return BadRequest(error:ex.Message);
            }
            
            // Aplica valores para View
            ViewData["DateTimeNow"] = dateTimeNow;
            ViewData["Key"] = key;
            ViewData["Sum"] = sum;
            ViewData["VirtualMachines"] = Program.NUMBER_OF_VIRTUAL_MACHINES;
            ViewData["RequestsCounter"] = _requestsCounter;

            // Aplica informações de memória para View
            ViewData["gc0"] = GC.CollectionCount(0);
            ViewData["gc1"] = GC.CollectionCount(1);
            ViewData["gc2"] = GC.CollectionCount(2);
            ViewData["currentMemory"] = ByteSize.FromBytes(GC.GetTotalMemory(false)).ToString();
            ViewData["privateBytes"] = ByteSize.FromBytes(Process.GetCurrentProcess().WorkingSet64);

            // Retorna View
            return View();
        }

        public IActionResult Information()
        {
            return View();
        }

        private async Task<String> GetValueBasekey(string url)
        {
            // Busca as informações de data da API TimeAPI
            var response = await new HttpClient().GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception(message: "Erro ao requistar valor base para chave!");

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<Dictionary<string, string>> GenerateKey(string url)
        {
            var response = new Dictionary<string, string>();

            string resultRequestApi = await GetValueBasekey(url);

            int day = 0;
            string key = "";
            var random = new Random();
            int sumOddNumbers = 0;

            if (int.TryParse(resultRequestApi.Split(",")[1].Split(" ")[1], out day))
            {
                // Calcula e gera a chave
                for (int sequencial = 0; sequencial< 4096; sequencial++)
                    key = string.Concat(key, day* sequencial * random.Next(100, 9999));

                // Obtem os números ímpares gerados na chave
                var oddNumbers = new List<int>();
                foreach (var c in key.ToArray())
                    if (int.Parse(c.ToString()) % 2 != 0)
                        oddNumbers.Add(int.Parse(c.ToString()));

                // Soma números ímpares
                sumOddNumbers = oddNumbers.Sum(x => x);

            }
            else
            {
                throw new Exception(message: "Erro ao obter valor! Dia não é um número válido!");
            }

            // Aplica valores para View
            response.Add("DateTimeNow", resultRequestApi);
            response.Add("Key", key);
            response.Add("Sum", sumOddNumbers.ToString());

            return response;
        }
    }
}