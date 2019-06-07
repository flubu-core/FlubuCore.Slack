using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using FlubuCore.Context;
using FlubuCore.Tasks;
using Newtonsoft.Json;

namespace FlubuCore.Slack
{
    public class SlackTask : TaskBase<int, SlackTask>
    {
        private readonly string _account;

        private readonly string _token;

        private readonly Slack _slackMessage;

        private readonly Encoding _encoding = new UTF8Encoding();

        public SlackTask(string account, string token, Slack slackMessage)
        {
            _account = account;
            _token = token;
            _slackMessage = slackMessage;
        }

        protected override int DoExecute(ITaskContextInternal context)
        {
            var urlWithAccessToken = $"https://{_account}.slack.com/services/hooks/incoming-webhook?token={_token}";
            var uri = new Uri(urlWithAccessToken);
            string payloadJson = JsonConvert.SerializeObject(_slackMessage);

            using (WebClient client = new WebClient())
            {
                NameValueCollection data = new NameValueCollection();
                data["payload"] = payloadJson;

                var response = client.UploadValues(uri, "POST", data);

                string responseText = _encoding.GetString(response);
            }

            return 0;
        }

        protected override string Description { get; set; }
    }
}
