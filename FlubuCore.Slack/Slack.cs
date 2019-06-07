using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FlubuCore.Slack
{
    public class Slack
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }
	
        [JsonProperty("username")]
        public string Username { get; set; }
	
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
