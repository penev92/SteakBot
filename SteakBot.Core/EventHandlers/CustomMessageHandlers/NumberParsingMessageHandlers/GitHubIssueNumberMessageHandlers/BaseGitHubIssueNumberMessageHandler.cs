using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Discord;
using Discord.WebSocket;
using Octokit;
using SteakBot.Core.Abstractions.Configuration.CustomMessageHandlers;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.NumberParsingMessageHandlers.GitHubIssueNumberMessageHandlers
{
    internal class BaseGitHubIssueNumberMessageHandler : BaseSimpleNumberParsingMessageHandler
    {
        protected string RepositoryOwner { get; }

        protected string RepositoryName { get; }

        protected override IReadOnlyDictionary<string, int> MinimumHandledNumberPerKeyword { get; } = new ReadOnlyDictionary<string, int>(new Dictionary<string, int>());

        private readonly IGitHubClient _gitHubClient;
        private readonly string _issueIconBaseUrl;
        private readonly bool _shouldShowRepositoryIcon;

        private readonly Dictionary<string, Color> _colorPerStatus = new Dictionary<string, Color>
        {
            { "open", Color.Green },
            { "closed", Color.Red },
            { "merged", Color.Purple }
        };

        public BaseGitHubIssueNumberMessageHandler(IGitHubClient gitHubClient, CodeRepositoryConfiguration repositoryConfiguration)
        {
            _gitHubClient = gitHubClient;

            _issueIconBaseUrl = repositoryConfiguration.IconsBaseUrl;
            _shouldShowRepositoryIcon = repositoryConfiguration.ShowRepositoryIcon;
            RepositoryOwner = repositoryConfiguration.Owner;
            RepositoryName = repositoryConfiguration.Name;
            MinimumHandledNumberPerKeyword = new ReadOnlyDictionary<string, int>(repositoryConfiguration.MinimumHandledNumberPerKeyword);
        }

        public override void Invoke(SocketUserMessage message)
        {
            var ownerUser = _gitHubClient.User.Get(RepositoryOwner).Result;
            foreach (var numberResult in GetMatchedNumbers(message.Content))
            {
                var issue = _gitHubClient.Issue.Get(RepositoryOwner, RepositoryName, numberResult.Number).Result;
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
                    var pullRequest = _gitHubClient.PullRequest.Get(RepositoryOwner, RepositoryName, numberResult.Number).Result;

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
                        IconUrl = _shouldShowRepositoryIcon ? ownerUser.AvatarUrl : null
                    },
                    Timestamp = issue.UpdatedAt,
                    Color = _colorPerStatus[status.StringValue]
                };

                message.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }

        private string GetIssueIconUrl(bool isIssue, string status)
        {
            return $"{_issueIconBaseUrl}/github-{(isIssue ? "issue" : "pr")}-{status}.png";
        }
    }
}
