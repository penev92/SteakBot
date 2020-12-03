using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using Octokit;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.NumberParsingMessageHandlers.GitHubIssueNumberMessageHandlers
{
    internal abstract class BaseGitHubIssueNumberMessageHandler : BaseSimpleNumberParsingMessageHandler
    {
        protected abstract string RepositoryOwner { get; }
        protected abstract string RepositoryName { get; }
        //private static readonly string IssueIconBaseUrl = ConfigurationManager.AppSettings["GitHubIconsBaseUrl"];
        //private static readonly bool ShouldShowRepositoryIcon = bool.Parse(ConfigurationManager.AppSettings["ShowRepositoryIcon"]);
        protected readonly IGitHubIssueNumberOptions Options;
        protected readonly IGitHubClient GitHubClient;

        private readonly Dictionary<string, Color> _colorPerStatus = new Dictionary<string, Color>
        {
            { "open", Color.Green },
            { "closed", Color.Red },
            { "merged", Color.Purple }
        };

        internal BaseGitHubIssueNumberMessageHandler(IGitHubClient gitHubClient,
            IGitHubIssueNumberOptions options)
        {
            GitHubClient = gitHubClient;
            Options = options;
        }

        public override void Invoke(SocketUserMessage message)
        {
            var ownerUser = GitHubClient.User.Get(RepositoryOwner).Result;
            foreach (var numberResult in GetMatchedNumbers(message.Content))
            {
                var issue = GitHubClient.Issue.Get(RepositoryOwner, RepositoryName, numberResult.Number).Result;
                var isIssue = issue.PullRequest == null;
                var type = isIssue ? "Issue" : "Pull request";
                var labels = string.Join(", ", issue.Labels?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                var embedFields = new List<EmbedFieldBuilder>();
                if (!string.IsNullOrEmpty(labels))
                {
                    embedFields.Add(new EmbedFieldBuilder
                    {
                        Name = "Labels:",
                        Value = labels,
                        IsInline = true
                    });
                }

                var status = issue.State;

                if (!isIssue && status == "Closed")
                {
                    var pullRequest = GitHubClient.PullRequest.Get(RepositoryOwner, RepositoryName, numberResult.Number).Result;

                    embedFields.Add(new EmbedFieldBuilder
                    {
                        Name = "Status:",
                        Value = pullRequest.MergeableState,
                        IsInline = true
                    });

                    status = pullRequest.Merged ? "merged" : issue.State;
                    if (pullRequest.Merged)
                    {
                        embedFields.Add(new EmbedFieldBuilder
                        {
                            Name = "Merged by:",
                            Value = pullRequest.MergedBy?.Login,
                            IsInline = true
                        });
                    }
                }

                var embed = new EmbedBuilder
                {
                    Title = issue.Title,
                    ThumbnailUrl = issue.User?.AvatarUrl,
                    Url = issue.HtmlUrl,
                    Description = issue.Body.Length > 250 ? issue.Body.Substring(0, 250) + "..." : issue.Body,
                    Author = new EmbedAuthorBuilder
                    {
                        Name = $"{type} #{numberResult.Number} by {issue.User?.Login}  ({status})",
                        IconUrl = GetIssueIconUrl(isIssue, status.StringValue),
                        Url = issue.HtmlUrl
                    },
                    Fields = embedFields,
                    Footer = new EmbedFooterBuilder
                    {
                        Text = $"Created at {issue.CreatedAt.ToString("s").Replace('T', ' ') + " UTC"}",
                        IconUrl = Options.ShouldShowRepositoryIcon ? ownerUser.AvatarUrl : null
                    },
                    Timestamp = issue.UpdatedAt,
                    Color = _colorPerStatus[status.StringValue]
                };

                message.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }

        private string GetIssueIconUrl(bool isIssue, string status)
        {
            return $"{Options.IssueIconBaseUrl}/github-{(isIssue ? "issue" : "pr")}-{status}.png";
        }
    }
}
