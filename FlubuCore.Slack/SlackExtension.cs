using System;
using System.Collections.Generic;
using System.Text;
using FlubuCore.Context.FluentInterface.Interfaces;

namespace FlubuCore.Slack
{
    public static class SlackExtension
    {
        public static SlackTask Slack(this ITaskFluentInterface flubu, string account, string accessToken, Slack slackMessage)
        {
            return new SlackTask(account, accessToken, slackMessage);
        }
    }
}
