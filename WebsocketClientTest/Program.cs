using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebsocketClientTest
{
    class Program
    {
        static  void Main(string[] args)
        {
            Client client = new Client();
            //var t = Task.Run(() => client.SocketConnect());
            //t.Wait();
            client.SocketConnect();
            Console.ReadKey();

            //Task<string> id = Login.GetSessionId();
            //string bs_session_id = string.Empty;
            //if (!string.IsNullOrEmpty(id.Result.ToString()))
            //{
            //    bs_session_id = id.Result.ToString();
            //}
            //ClientNet client = new ClientNet();
            //client.ScoketManager(bs_session_id);
        }
    }
}
