using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Promatech
{
    public class QQChannelHTTPClient
    {
        private const string source = "https://api.sgroup.qq.com";
        private string botid;
        private string bottoken;
        private HttpClient hc;

        public QQChannelHTTPClient(string botid, string bottoken)
        {
            this.botid = botid;
            this.bottoken = bottoken;
            hc = new HttpClient();
            hc.DefaultRequestHeaders.Add("Authorization", $"Bot {botid}.{bottoken}");
        }



        public async Task<Message> ReplyChannelMessage(Message context, MessageContent content)
        {
            var data = new StringContent(content.ToJsonString(), Encoding.UTF8, "application/json");
            var result = await hc.PostJsonAsync<Message>($"{source}/channels/{context.ChannelId}/messages", data);
            return result;
        }

        public async Task<Guild> GetGuildInfo(string guildId)
        {
            var result = await hc.DownloadJsonAsync<Guild>($"{source}/guilds/{guildId}");
            return result;
        }

        public async Task<Role> GetGuildRoles(string guildId)
        {
            var result = await hc.DownloadJsonAsync<Role>($"{source}/guilds/{guildId}/roles");
            return result;
        }

        public async Task<Member> GetGuildMember(string guildId, string userId)
        {
            var result = await hc.DownloadJsonAsync<Member>($"{source}/guilds/{guildId}/members/{userId}");
            return result;
        }
    }
}
