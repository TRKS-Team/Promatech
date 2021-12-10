using System;
using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;

namespace Promatech
{
    public partial class QQ_opcode
    {

        [JsonProperty("op")] 
        public long Op { get; set; }

        [JsonProperty("d")] 
        public object D { get; set; }

        [JsonProperty("s")] 
        public long S { get; set; }

        [JsonProperty("t")] 
        public string T { get; set; }
    }

    public partial class Hello
    {
        [JsonProperty("heartbeat_interval")] 
        public long Interval { get; set; }
    }

    public partial class ReadyEvent
    {
        [JsonProperty("version")] 
        public long Version { get; set; }

        [JsonProperty("session_id")] 
        public Guid SessionId { get; set; }

        [JsonProperty("user")] 
        public Bot_User BotUser { get; set; }

        [JsonProperty("shard")] 
        public List<long> Shard { get; set; }
    }

    public partial class Bot_User
    {
        [JsonProperty("id")] 
        public string Id { get; set; }

        [JsonProperty("username")] 
        public string Username { get; set; }

        [JsonProperty("bot")] 
        public bool Bot { get; set; }
    }

    public partial class Identification
    {
        [JsonProperty("op")] 
        public long Op = 2;

        [JsonProperty("d")] 
        public Identification_D D { get; set; }
    }

    public partial class Identification_D
    {
        [JsonProperty("token")] 
        public string Token { get; set; }

        [JsonProperty("intents")] 
        public int Intents = (1 << 30) + (1 << 0) + (1 << 1);
        // TODO 自定义intends
        [JsonProperty("shard")]
        public List<long> Shard = new List<long> {0,1};

        [JsonProperty("properties")] 
        public Bot_Properties Properties = new Bot_Properties();
    }

    public partial class Bot_Properties
    {
        [JsonProperty("$os")] 
        public string Os = "linux";

        [JsonProperty("$browser")] 
        public string Browser = "my_library";

        [JsonProperty("$device")] 
        public string Device = "my_library";
    }

    public partial class HeartBeat
    {
        [JsonProperty("op")] 
        public long Op = 1;

        [JsonProperty("d")] 
        public long? D { get; set; }
    }

    public partial class Message
    {
        [JsonProperty("author")] 
        public Author Author { get; set; }

        [JsonProperty("channel_id")] 
        public long ChannelId { get; set; }

        [JsonProperty("content")] 
        public string Content { get; set; }

        [JsonProperty("guild_id")] 
        public string GuildId { get; set; }

        [JsonProperty("id")] 
        public string Id { get; set; }

        [JsonProperty("member")] 
        public Message_Member Member { get; set; }

        [JsonProperty("timestamp")] 
        public DateTimeOffset Timestamp { get; set; }
    }

    public partial class Author
    {
        [JsonProperty("avatar")] 
        public Uri Avatar { get; set; }

        [JsonProperty("bot")] 
        public bool Bot { get; set; }

        [JsonProperty("id")] 
        public BigInteger Id { get; set; }

        [JsonProperty("username")] 
        public string Username { get; set; }
    }

    public partial class Message_Member
    {
        [JsonProperty("joined_at")] 
        public DateTimeOffset JoinedAt { get; set; }

        [JsonProperty("roles")] 
        public List<long> Roles { get; set; }
    }
    public partial class Gateway
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
    public partial class MessageContent
    {
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("msg_id")]
        public string MsgId { get; set; }
        [JsonProperty("image")]
        public string ImageUrl { get; set; }
        [JsonProperty("embed")]
        public MessageEmbed Embed { get; set; }
        [JsonProperty("ark")]
        public MessageArk Ark { get; set; }

    }
    public partial class MessageArk
    {
        [JsonProperty("template_id")]
        public int TemplateId { get; set; }
        [JsonProperty("kv")] 
        private MessageArkKv[] ArkKvs { get; set; }
    }
    public partial class MessageArkKv
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("obj")]
        public MessageArkObj[] ArkObjs { get; set; }
    }
    public partial class MessageArkObj
    {
        [JsonProperty("obj_kv")]
        public MessageArkObjKv[] ObjKvs { get; set; }
    }
    public partial class MessageArkObjKv
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
    public partial class MessageEmbed
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("prompt")]
        public string Prompt { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp /*ISO8601 timestamp*/{ get; set; }
        [JsonProperty("fields")]
        public MessageEmbedField[] Fields { get; set; }

    }
    public partial class MessageEmbedField
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
    public partial class Role
    {
        [JsonProperty("guild_id")]
        public string GuildId { get; set; }

        [JsonProperty("roles")]
        public List<RoleElement> Roles { get; set; }

        [JsonProperty("role_num_limit")]
        public long RoleNumLimit { get; set; }
    }

    public partial class RoleElement
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public long Color { get; set; }

        [JsonProperty("hoist")]
        public long Hoist { get; set; }

        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("member_limit")]
        public long MemberLimit { get; set; }
    }
    public partial class Guild
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("owner_id")]
        public string OwnerId { get; set; }

        [JsonProperty("owner")]
        public bool Owner { get; set; }

        [JsonProperty("member_count")]
        public long MemberCount { get; set; }

        [JsonProperty("max_members")]
        public long MaxMembers { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
    public partial class Member
    {
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("nick")]
        public string Nick { get; set; }

        [JsonProperty("roles")]
        public List<long> Roles { get; set; }

        [JsonProperty("joined_at")]
        public DateTimeOffset JoinedAt { get; set; }

        [JsonProperty("deaf")]
        public bool Deaf { get; set; }

        [JsonProperty("mute")]
        public bool Mute { get; set; }

        [JsonProperty("pending")]
        public bool Pending { get; set; }
    }

    public partial class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("avatar")]
        public Uri Avatar { get; set; }

        [JsonProperty("bot")]
        public bool Bot { get; set; }

        [JsonProperty("public_flags")]
        public long PublicFlags { get; set; }

        [JsonProperty("system")]
        public bool System { get; set; }
    }
}

