using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace WebsocketClientTest
{
    public class ClientNet
    {
        public void ScoketManager(string bs_session_id)
        {
            try
            {
                string socketConnection = "wss://192.168.200.51:443//wsapi";
                var exitEvent = new ManualResetEvent(false);
                var url = new Uri(socketConnection);
               
                using (var client = new WebsocketClient(url))
                {
                    
                    client.ReconnectTimeout = TimeSpan.FromSeconds(30);
                    client.ReconnectionHappened.Subscribe(info =>
                    {
                        Console.WriteLine("Reconnection happened, type: " + info.Type);
                    });
                    client.MessageReceived.Subscribe(msg =>
                    {
                        Console.WriteLine("Message received: " + msg);
                        if (msg.ToString().ToLower() == "connected")
                        {
                            Send(client, bs_session_id);
                        }
                        else
                        {
                            Console.WriteLine(msg);
                        }
                    });
                    client.Start();
                    Console.WriteLine("Started"+client.IsStarted);
                    Console.WriteLine("Running"+client.IsRunning);
                    //Task.Run(() => client.Send("{ message }"));
                    exitEvent.WaitOne(1000);
                    Send(client, bs_session_id);
                    //Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
            }
            Console.ReadKey();
        }
        public void Send(WebsocketClient client,string bs_session_id)
        {
            //System.Threading.Thread.Sleep(1000);
            client.Send("bs-session-id=" + bs_session_id);
            Task.Run(()=>Login.EventStarts(bs_session_id));
        }
       
    }
}
    

