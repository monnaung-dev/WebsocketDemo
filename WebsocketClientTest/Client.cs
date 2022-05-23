using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebsocketClientTest
{
    public class Client
    {
        static string socketConnection = "wss://192.168.200.51:443";
        //System.Net.WebSockets.ClientWebSocket client;
        Uri url;
        public void SocketConnect()
        {
            try
            {

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : Socket connecting state ..." + ex.Message);
                connect();
            }

        }
        public async void connect()
        {
            try
            {
                Task<string> id = Login.GetSessionId();
                string bs_session_id = string.Empty;
                if (!string.IsNullOrEmpty(id.Result.ToString()))
                {
                    bs_session_id = id.Result.ToString();
                }
                using (var client = new ClientWebSocket())
                {

                    url = new Uri(socketConnection + "/wsapi");
                    await client.ConnectAsync(url, CancellationToken.None);
                    Console.WriteLine("Socket is ... " + client.State);
                    if (client !=null)
                    {
                        await ReceiveTrans(client, CancellationToken.None, bs_session_id);
                    }
                    if (client.State.ToString() == "Aborted")
                    {
                        SetTimeout();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Socket connecting state ..." + ex.Message);
                SetTimeout();
            }

        }
        private void SetTimeout()
        {
            //client.CloseAsync(WebSocketCloseStatus.InternalServerError,"Close", CancellationToken.None);
            System.Threading.Thread.Sleep(1000);
            connect();
        }
        private async Task ReceiveTrans(ClientWebSocket client, CancellationToken token, string bs_session_id)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                await Send(client, "bs-session-id=" + bs_session_id, token);
                await Login.EventStarts(bs_session_id);

                await Receive(client, token);

            }
            catch (Exception ex)
            {
                SetTimeout();
            }
        }
        private async Task Send(ClientWebSocket client, string data, CancellationToken stoppingToken)
        {
            try
            {
                var task = Encoding.UTF8.GetBytes(data);
                await client.SendAsync(new ArraySegment<byte>(task, 0, task.Length), WebSocketMessageType.Text, true, stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }


        private async Task Receive(ClientWebSocket client, CancellationToken stoppingToken)
        {
            try
            {
                var buffer = new ArraySegment<byte>(new byte[2048]);
                while (!stoppingToken.IsCancellationRequested)
                {
                    WebSocketReceiveResult result;
                    using (var ms = new MemoryStream())
                    {
                        do
                        {
                            result = await client.ReceiveAsync(buffer, stoppingToken);
                            ms.Write(buffer.Array, buffer.Offset, result.Count);
                        } while (!result.EndOfMessage);

                        if (result.MessageType == WebSocketMessageType.Close)
                            break;

                        ms.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(ms, Encoding.UTF8))
                            Console.WriteLine(await reader.ReadToEndAsync());
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                SetTimeout();
            }

        }
    }
}

