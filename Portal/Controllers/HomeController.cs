using Humanizer.Bytes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;

namespace Portal.Controllers
{
    public class HomeController : Controller
    {
        public static int _requestsCounter = 0;

        public IActionResult Index()
        {
            // Incrementa o contador de requisições
            _requestsCounter++;

            // Verifica se a API tem recursos para atender a requisição
            if (!Program.CheckIfThereIsThreadAvailable())
                return BadRequest(Messages.THREAD_IS_NO_THREAD_AVAILABLE);

            // Busca as informações de data da API TimeAPI
            var httpClient = new HttpClient();
            var result = httpClient.GetStringAsync(Program.API_ADDRESS).Result;

            // Extrai o dia da data
            var d = int.Parse(result.Split(",")[1].Split(" ")[1]);

            // Calcula e gera a chave
            var key = "";
            for (int i = 0; i < 4096; i++)
            {
                var random = new Random();
                key = string.Concat(key, d * i * random.Next(100, 9999));
            }

            // Obtem os números ímpares gerados na chave
            var oddNumbers = new List<int>();
            foreach (var c in key.ToArray())
                if (int.Parse(c.ToString()) % 2 != 0)
                    oddNumbers.Add(int.Parse(c.ToString()));

            // Soma números ímpares
            var sumOddNumbers = oddNumbers.Sum(x => x);

            // Aplica valores para View
            ViewData["DateTimeNow"] = result;
            ViewData["Key"] = key;
            ViewData["Sum"] = sumOddNumbers;
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
    }
}