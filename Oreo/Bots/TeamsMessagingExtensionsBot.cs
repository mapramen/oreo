namespace Oreo.Bots
{
    using AdaptiveCards;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Teams;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema.Teams;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Oreo.Games;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using static Oreo.Games.Constants;

    public class TeamsMessagingExtensionsBot : TeamsActivityHandler
    {
        private readonly IGameRunnerRepository gameRunnerRepository;

        public TeamsMessagingExtensionsBot(IGameRunnerRepository gameRunnerRepository)
        {
            this.gameRunnerRepository = gameRunnerRepository;
        }

        protected override async Task OnMessageActivityAsync(
            ITurnContext<IMessageActivity> turnContext,
            CancellationToken cancellationToken)
        {
            if (turnContext.Activity.Value != null)
            {
                // This was a message from the card.
                var obj = (JObject)turnContext.Activity.Value;
                var answer = obj["Answer"]?.ToString();
                var choices = obj["Choices"]?.ToString();
                await turnContext.SendActivityAsync(MessageFactory.Text($"{turnContext.Activity.From.Name} answered '{answer}' and chose '{choices}'."), cancellationToken);
            }
            else
            {
                // This is a regular text message.
                await turnContext.SendActivityAsync(MessageFactory.Text($"Hello from the TeamsMessagingExtensionsActionPreviewBot."), cancellationToken);
            }
        }

        protected override Task<MessagingExtensionActionResponse> OnTeamsMessagingExtensionFetchTaskAsync(
            ITurnContext<IInvokeActivity> turnContext,
            MessagingExtensionAction action,
            CancellationToken cancellationToken)
        {
            var fetchTaskResponse = new MessagingExtensionActionResponse
            {
                Task = new TaskModuleContinueResponse
                {
                    Value = new TaskModuleTaskInfo
                    {
                        Title = "Let the fun begin!",
                        Width = 225,
                        Height = 150,
                        Card = new Attachment
                        {
                            Content = AdaptiveCardHelper.CreateFetchTaskCard(),
                            ContentType = AdaptiveCard.ContentType
                        }
                    }
                }
            };

            return Task.FromResult(fetchTaskResponse);
        }

        protected override async Task<MessagingExtensionActionResponse> OnTeamsMessagingExtensionSubmitActionAsync(
            ITurnContext<IInvokeActivity> turnContext,
            MessagingExtensionAction action,
            CancellationToken cancellationToken)
        {
            var gameChoiceSubmitData = JsonConvert.DeserializeObject<GameChoiceSubmitData>(action.Data.ToString());
            GameName gameName = GameNameStringToEnum(gameChoiceSubmitData.GameChoice);
            string gameId = Guid.NewGuid().ToString();

            var message = MessageFactory.Attachment(new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCardHelper.CreateSubmitActionCard(gameId, gameName)
            });

            await turnContext.SendActivityAsync(message);
            this.gameRunnerRepository.Add(gameId, gameName);
            return null;
        }

        public override Task OnTurnAsync(
            ITurnContext turnContext,
            CancellationToken cancellationToken = default)
        {
            turnContext.OnSendActivities(this.SendActivitiesHandler);
            return base.OnTurnAsync(turnContext, cancellationToken);
        }

        private Task<ResourceResponse[]> SendActivitiesHandler(
            ITurnContext turnContext,
            List<Activity> activities,
            Func<Task<ResourceResponse[]>> next)
        {
            return next();
        }

        private class GameChoiceSubmitData
        {
            public string GameChoice { get; set; }
        }
    }
}