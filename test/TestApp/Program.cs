using System;
using System.Threading.Tasks;
using ProtoBuf.Grpc.Client;
using Service.FrontendKeyValue.Client;
using Service.FrontendKeyValue.Grpc.Models;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GrpcClientFactory.AllowUnencryptedHttp2 = true;

            Console.Write("Press enter to start");
            Console.ReadLine();

            await Task.Delay(100);

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
