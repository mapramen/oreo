namespace Oreo.Bots
{
    using AdaptiveCards;
    using Oreo.Common;
    using Oreo.Games;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using static Oreo.Games.Constants;

    internal static class AdaptiveCardHelper
    {
        internal static AdaptiveCard CreateFetchTaskCard()
        {
            List<AdaptiveChoice> gameChoices
                = EnumExtensions.GetValues<Constants.GameName>()
                    .Select(gameName => Constants.GameNameEnumToString(gameName))
                    .Select(gameNameString => new AdaptiveChoice
                    {
                        Title = gameNameString,
                        Value = gameNameString
                    })
                    .ToList();

            AdaptiveChoiceSetInput gameChoiceInput = new AdaptiveChoiceSetInput
            {
                Style = AdaptiveChoiceInputStyle.Compact,
                IsMultiSelect = false,
                Id = "GameChoice",
                Value = Constants.GameNameEnumToString(Constants.GameName.TicTacToe),
                Choices = gameChoices
            };

            AdaptiveTextBlock gameChoiceQuestionText = new AdaptiveTextBlock
            {
                Text = "Choose a game",
                Color = AdaptiveTextColor.Accent,
                Weight = AdaptiveTextWeight.Bolder
            };

            return new AdaptiveCard("1.0")
            {
                Body = new List<AdaptiveElement>
                {
                    gameChoiceQuestionText,
                    gameChoiceInput
                },
                Actions = new List<AdaptiveAction>
                {
                    new AdaptiveSubmitAction
                    {
                        Title = "Create Game"
                    }
                }
            };
        }

        internal static AdaptiveCard CreateSubmitActionCard(string gameId, GameName gameName)
        {
            string encodedContext = HttpUtility.UrlEncode($"{{\"subEntityId\":\"{gameId}\"}}");
            string joinGameUrl = $"https://teams.microsoft.com/l/entity/1802d040-1c49-11eb-8f51-b93fe173de76/048f6e42-4539-45b2-9ed2-f84f0b5a60c1?context={encodedContext}";

            AdaptiveImage cocoIcon = new AdaptiveImage
            {
                Size = AdaptiveImageSize.Small,
                Style = AdaptiveImageStyle.Person,
                Url = new Uri("https://img.favpng.com/8/7/17/feral-cat-kitten-giant-panda-clip-art-png-favpng-dTcWeqynZbbAziCK66YYpCbrm.jpg")
            };

            AdaptiveTextBlock cocoTitle = new AdaptiveTextBlock
            {
                Text = "Coco",
                Weight = AdaptiveTextWeight.Bolder,
                Color = AdaptiveTextColor.Dark,
                Wrap = true
            };

            AdaptiveTextBlock cocoSubTitle = new AdaptiveTextBlock
            {
                Text = "Let the fun begin!",
                Spacing = AdaptiveSpacing.None,
                IsSubtle = true,
                Wrap = false
            };

            AdaptiveColumnSet cocoTitleContainer = new AdaptiveColumnSet
            {
                Columns = new List<AdaptiveColumn>
                {
                    new AdaptiveColumn
                    {
                        Width = "auto",
                        Items = new List<AdaptiveElement>
                        {
                            cocoIcon
                        }
                    },
                    new AdaptiveColumn
                    {
                        Width = "stretch",
                        Items = new List<AdaptiveElement>
                        {
                            cocoTitle,
                            cocoSubTitle
                        }
                    }
                }
            };

            AdaptiveTextBlock bodyHeaderText = new AdaptiveTextBlock
            {
                Text = "Following game has been created. Join the game to play."
            };

            AdaptiveTextBlock bodyText = new AdaptiveTextBlock
            {
                Text = GameNameEnumToString(gameName),
                Weight = AdaptiveTextWeight.Bolder,
                Color = AdaptiveTextColor.Accent,
                Wrap = false
            };

            AdaptiveContainer bodyContainer = new AdaptiveContainer
            {
                Items = new List<AdaptiveElement>
                {
                    bodyHeaderText,
                    bodyText
                }
            };

            AdaptiveAction joinGameAction = new AdaptiveOpenUrlAction
            {
                Title = "Join game",
                Url = new Uri(joinGameUrl),
                Style = "positive"
            };

            return new AdaptiveCard("1.0")
            {
                Body = new List<AdaptiveElement>
                {
                    cocoTitleContainer,
                    bodyContainer
                },
                Actions = new List<AdaptiveAction>
                {
                    joinGameAction
                }
            };
        }
    }
}
