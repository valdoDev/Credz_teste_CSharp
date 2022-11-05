using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Portal.Functions
{
    public static class General
    {
        public static class HttRequest
        {

            public static async Task<String> GetValueBasekey(string url)
            {
                // Busca as informações de data da API TimeAPI
                var response = await new HttpClient().GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new Exception(message: "Erro ao requistar url!");

                return await response.Content.ReadAsStringAsync();
            }
        }

        public static class Key
        {
            public static async Task<Dictionary<string, string>> GenerateKey(string url)
            {
                var response = new Dictionary<string, string>();

                string resultRequestApi = await General.HttRequest.GetValueBasekey(url);

                int day = 0;
                string key = "";
                var random = new Random();
                int sumOddNumbers = 0;

                if (int.TryParse(resultRequestApi.Split(",")[1].Split(" ")[1], out day))
                {
                    // Calcula e gera a chave
                    for (int sequencial = 0; sequencial < 4096; sequencial++)
                        key = string.Concat(key, day * sequencial * random.Next(100, 9999));

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
}
