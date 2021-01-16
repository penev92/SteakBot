using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Discord;
using Discord.WebSocket;
using SharpBucket.V2;
using SharpBucket.V2.EndPoints;
using SharpBucket.V2.Pocos;
using SteakBot.Core.Abstractions.Configuration.CustomMessageHandlers;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.NumberParsingMessageHandlers.BitBucketIssueNumberMessageHandlers
{
    internal class BaseBitBucketIssueNumberMessageHandler : BaseTypedNumberParsingMessageHandler
    {
        private string RepositoryOwner { get; }
        private string RepositoryName { get; }
        private string ConsumerKey { get; }
        private string ConsumerSecretKey { get; }

        protected override IReadOnlyDictionary<string, int> MinimumHandledNumberPerKeyword { get; } = new ReadOnlyDictionary<string, int>(new Dictionary<string, int>());

        private readonly string _issueIconBaseUrl;
        private readonly bool _shouldShowRepositoryIcon;
        private readonly bool _canAuthenticate;

        private readonly IssuesResource _issuesResource;
        private readonly PullRequestsResource _pullRequestsResource;
        private readonly Dictionary<string, Color> _colorPerStatus = new Dictionary<string, Color>
        {
            // Issues
            { "New", Color.Green },
            { "Open", Color.Green },
            { "Resolved", Color.Red },
            { "OnHold", Color.LightGrey },
            { "Invalid", Color.DarkRed },
            { "Duplicate", Color.DarkRed },
            { "WontFix", Color.DarkGrey },
            { "Closed", Color.Red },
            
            // Pull Requests
            { "OPEN", Color.Green },
            { "MERGED", Color.Purple },
            { "DECLINED", Color.Red }
        };

        public BaseBitBucketIssueNumberMessageHandler(SharpBucketV2 bitBucketClient, IBitBucketConfiguration configuration, CodeRepositoryConfiguration repositoryConfiguration)
        {
            _issueIconBaseUrl = repositoryConfiguration.IconsBaseUrl;
            _shouldShowRepositoryIcon = repositoryConfiguration.ShowRepositoryIcon;
            RepositoryOwner = repositoryConfiguration.Owner;
            RepositoryName = repositoryConfiguration.Name;
            ConsumerKey = configuration.ConsumerKey;
            ConsumerSecretKey = configuration.ConsumerSecretKey;
            MinimumHandledNumberPerKeyword = new ReadOnlyDictionary<string, int>(repositoryConfiguration.MinimumHandledNumberPerKeyword);

            try
            {
                var client = bitBucketClient;
                client.OAuth2ClientCredentials(ConsumerKey, ConsumerSecretKey);
                _canAuthenticate = true;

                var repositoriesEndPoint = client.RepositoriesEndPoint();
                var repositoryResource = repositoriesEndPoint.RepositoryResource(RepositoryOwner, RepositoryName);
                _pullRequestsResource = repositoryResource.PullRequestsResource();
                _issuesResource = repositoryResource.IssuesResource();
            }
            catch (Exception)
            {
                Console.WriteLine($"BitBucket authentication failed for {RepositoryOwner}/{RepositoryName}!");
            }
        }

        public override void Invoke(SocketUserMessage message)
        {
            if (!_canAuthenticate)
            {
                return;
            }

            foreach (var numberResult in GetMatchedNumbers(message.Content))
            {
                Embed embed;

                try
                {
                    if (numberResult.Type == 'i')
                    {
                        var issueInfo = _issuesResource.IssueResource(numberResult.Number).GetIssue();
                        embed = CreateEmbed(issueInfo, numberResult.Number);
                    }
                    else if (numberResult.Type == 'p')
                    {
                        var pullRequestInfo = _pullRequestsResource.PullRequestResource(numberResult.Number).GetPullRequest();
                        embed = CreateEmbed(pullRequestInfo, numberResult.Number);
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (Exception)
                {
                    continue;
                }

                message.Channel.SendMessageAsync("", embed: embed);
            }
        }

        private Embed CreateEmbed(Issue issue, int number)
        {
            var status = issue.state;
            var description = issue.content.raw.Trim();
            var embedBuilder = new EmbedBuilder
            {
                Title = issue.title,
                ThumbnailUrl = issue.reporter.links.avatar.href,
                Url = issue.links.html.href,
                Description = description.Length > 250 ? description.Substring(0, 250) + "..." : description,
                Author = new EmbedAuthorBuilder
                {
                    Name = $"Issue #{number} by {issue.reporter.display_name}  ({status})",
                    IconUrl = GetIssueIconUrl(true, status.ToString()),
                    Url = issue.links.html.href
                },
                Footer = new EmbedFooterBuilder
                {
                    Text = $"Created at {issue.createdOn.ToString("s").Replace('T', ' ') + " UTC"}",
                    IconUrl = _shouldShowRepositoryIcon ? issue.repository.links.avatar.href : null
                },
                Timestamp = issue.updatedOn,
                Color = _colorPerStatus[status.ToString()]
            };

            return embedBuilder.Build();
        }

        private Embed CreateEmbed(PullRequest pullRequest, int number)
        {
            var status = pullRequest.state;
            var description = pullRequest.description;
            var embedBuilder = new EmbedBuilder
            {
                Title = pullRequest.title,
                ThumbnailUrl = pullRequest.author.links.avatar.href,
                Url = pullRequest.links.html.href,
                Description = description.Length > 250 ? description.Substring(0, 250) + "..." : description,
                Author = new EmbedAuthorBuilder
                {
                    Name = $"Pull request #{number} by {pullRequest.author.display_name}  ({status})",
                    IconUrl = GetIssueIconUrl(false, status),
                    Url = pullRequest.links.html.href
                },
                Footer = new EmbedFooterBuilder
                {
                    Text = $"Created at {pullRequest.created_on.ToString("s").Replace('T', ' ') + " UTC"}",
                    IconUrl = _shouldShowRepositoryIcon ? pullRequest.destination.repository.links.avatar.href : null
                },
                Timestamp = pullRequest.updated_on,
                Color = _colorPerStatus[status]
            };

            return embedBuilder.Build();
        }

        private string GetIssueIconUrl(bool isIssue, string status)
        {
            var stat = status.ToLower();
            if (stat == "declined")
            {
                stat = "closed";
            }

            if (stat == "new")
            {
                stat = "open";
            }

            if (stat == "resolved")
            {
                stat = "closed";
            }

            return $"{_issueIconBaseUrl}/github-{(isIssue ? "issue" : "pr")}-{stat}.png";
        }
    }
}
