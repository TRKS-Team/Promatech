using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WebSocket4Net;

namespace Promatech
{
    public class QQChannelSocketClient
    {
        private WebSocket ws;
        private string botid;
        private string bottoken;
        public long? HeartbeatSN = null;
        private Timer heartbeatTimer;
        private Timer heartbeatACKTimer;
        private HttpClient hc;
        public event EventHandler<ReadyEvent> ReadyEvent;
        public event EventHandler<Message> ATMessageReceived;
        public event EventHandler<QQ_opcode> PayloadReceived; 

        public QQChannelSocketClient(string botid, string bottoken)
        {
            this.botid = botid;
            this.bottoken = bottoken;
            hc = new HttpClient();
            hc.DefaultRequestHeaders.Add("Authorization", $"Bot {botid}.{bottoken}");
            ws = new WebSocket(GetWSUrl());
            ws.MessageReceived += ProcessWebSocketMessage;
            ws.Closed += (sender, args) =>
            {
                Console.WriteLine("WS closed, Reopening.");
                ws.Open();
            };
        }



        private void ProcessWebSocketMessage(object sender, MessageReceivedEventArgs e)
        {
            var rawmessage = e.Message.JsonDeserialize<QQ_opcode>();
            OnPayloadReceived(rawmessage);
            HeartbeatSN = rawmessage.S;
            switch (rawmessage.Op)
            {
                case 0:
                    // Dispatch 服务端进行消息推送
                    switch (rawmessage.T)
                    {
                        case "READY":
                            var ready = rawmessage.D.JsonDeserialize<ReadyEvent>();
                            OnReadyEvent(ready);
                            break;
                        case "RESUMED":
                            // D为一个空string.
                            break;

                        // 频道事件
                        case "GUILD_CREATE": // 当机器人加入新guild时
                            break;
                        case "GUILD_UPDATE ": // 当guild资料发生变更时
                            break;
                        case "GUILD_DELETE": // 当机器人退出guild时
                            break;

                        // 子频道事件
                        case "CHANNEL_CREATE": // 当channel被创建时
                            break;
                        case "CHANNEL_UPDATE": // 当channel被更新时
                            break;
                        case "CHANNEL_DELETE": // 当channel被删除时
                            break;

                        // 频道成员事件
                        case "GUILD_MEMBER_ADD": // 当成员加入时
                            break;
                        case "GUILD_MEMBER_UPDATE": // 当成员资料变更时
                            break;
                        case "GUILD_MEMBER_REMOVE": // 当成员被移除时
                            break;

                        // 消息事件
                        case "DIRECT_MESSAGE_CREATE": // 当收到用户发给机器人的私信消息时
                            break;

                        // 音频事件
                        case "AUDIO_START": // 音频开始播放时
                            break;
                        case "AUDIO_FINISH": // 音频播放结束时
                            break;
                        case "AUDIO_ON_MIC": // 上麦时
                            break;
                        case "AUDIO_OFF_MIC": // 下麦时
                            break;

                        // 群@消息事件
                        case "AT_MESSAGE_CREATE": // 当收到@机器人的消息时
                            var message = rawmessage.D.JsonDeserialize<Message>();
                            OnATMessageReceived(message);
                            break;
                    }
                    break;

                case 1:
                    // Heartbeat 服务端发送心跳
                    // 目前服务端不会发送心跳
                    var heartbeat = rawmessage.D.JsonDeserialize<long>();
                    break;

                case 7:
                    // Reconnect 服务端通知客户端重新连接
                    break;

                case 9:
                    // Invalid Session 当identify或resume的时候，如果参数有错，服务端会返回该消息
                    break;

                case 10:
                    // Hello 当客户端与网关建立ws连接之后, 网关下发的第一条消息
                    ws.Send(GenerateIdentification(botid, bottoken).ToJsonString());
                    var hello = rawmessage.D.JsonDeserialize<Hello>();
                    heartbeatTimer = new Timer(hello.Interval);
                    heartbeatTimer.Elapsed += HeartbeatTimer_Elapsed;
                    heartbeatTimer.Start();
                    break;

                case 11:
                    //Heartbeat ACK 发送心跳成功消息
                    heartbeatACKTimer.Dispose();
                    break;

            }
        }

        public void Connect()
        {
            ws.Open();
        }
        private void HeartbeatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ws.Send(GenerateHeartBeat().ToJsonString());
            heartbeatACKTimer = new Timer(TimeSpan.FromSeconds(10).TotalMilliseconds);
            heartbeatACKTimer.Elapsed += (s, e) =>
            {
                Console.WriteLine("服务器未收到心跳, 可能是腾讯服务器离线.");
                heartbeatACKTimer.Dispose();
            };
            heartbeatACKTimer.Start();
        }

        public HeartBeat GenerateHeartBeat()
        {
            return new HeartBeat { D = HeartbeatSN };
            // 第一次连接是null
        }
        public Identification GenerateIdentification(string appid, string token)
        {
            var result = new Identification();
            result.D = new Identification_D();
            result.D.Token = $"Bot {appid}.{token}";
            result.D.Intents = (1<<30) + (1<<1) + (1<<0);
            return result;
        }
        public string GetWSUrl()
        {
            return hc.GetStringAsync("https://api.sgroup.qq.com/gateway").Result.JsonDeserialize<Gateway>().Url;
        }

        protected virtual void OnReadyEvent(ReadyEvent e)
        {
            ReadyEvent?.Invoke(this, e);
        }

        protected virtual void OnATMessageReceived(Message e)
        {
            ATMessageReceived?.Invoke(this, e);
        }

        protected virtual void OnPayloadReceived(QQ_opcode e)
        {
            PayloadReceived?.Invoke(this, e);
        }
    }
}
