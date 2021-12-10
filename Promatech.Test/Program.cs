using System;
using System.IO;
using System.Threading.Tasks;


namespace Promatech.Test
{
    class Program
    {
        public static QQChannelSocketClient SocketClient;
        public static QQChannelHTTPClient HttpClient;
        static void Main(string[] args)
        {
            var botid = File.ReadAllText("C:\\Users\\TheRealKamisama\\Desktop\\botid.txt").Trim();
            var bottoken = File.ReadAllText("C:\\Users\\TheRealKamisama\\Desktop\\bottoken.txt").Trim();
            SocketClient = new QQChannelSocketClient(botid, bottoken);
            SocketClient.Connect();
            HttpClient = new QQChannelHTTPClient(botid, bottoken);
            SocketClient.PayloadReceived += (sender, opcode) =>
            {
                Console.WriteLine(opcode.ToJsonString());
            };
            SocketClient.ATMessageReceived += (sender, message) =>
            {
                Console.WriteLine(message.Content);
                Console.WriteLine(message.Id);
                Console.WriteLine(message.ChannelId);
                if (message.Content.Contains("我需要机器人"))
                {
                    Task.WaitAll(HttpClient.ReplyChannelMessage(message,
                        new MessageContent { MsgId = message.Id, Content = "buzai,cnm." }));
                }
            };
            Console.ReadLine();
        }
    }
}
