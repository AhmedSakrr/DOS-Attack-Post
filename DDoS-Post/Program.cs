using System;
using System.Threading;

namespace DDoS_Post
{
    class Program
    {
        static void Main()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            var slowThread = new Thread(new DdosPost("127.0.0.1", cts.Token).RunAttack);
            slowThread.Start();

            //Thread.Sleep(5000);
            //cts.Cancel();
        }
    }
}
