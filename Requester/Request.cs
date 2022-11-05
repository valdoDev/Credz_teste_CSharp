using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Requester
{
    public class Request
    {
        public int _Qty { get; private set; }
        public string _Url { get; private set; }

        public Action<int, string> _DoRequest { get; private set; }

        public Request(int Qty, string Url, Action<int, string> DoRequest)
        {
            _Qty = Qty;
            _Url = Url;
            _DoRequest = DoRequest;
        }



        public List<Task> getListTask()
        {
            var tasks = new List<Task>();
            tasks.Clear();

            for (int numReqs = 0; numReqs < _Qty; numReqs++)
            {
                var nRqs = numReqs;
                var t = new Task(() => _DoRequest(nRqs, _Url));
                tasks.Add(t);
            }

            return tasks;

        }


    }
}
