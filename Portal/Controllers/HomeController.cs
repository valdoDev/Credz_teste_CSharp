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
                var resultKey = await Functions.General.Key.GenerateKey(Program.API_ADDRESS);

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

        

        
    }
}