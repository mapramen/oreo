﻿namespace Oreo.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;

    [Route("api/messages")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter adapter;
        private readonly IBot bot;

        public BotController(IBotFrameworkHttpAdapter adapter, IBot bot)
        {
            this.adapter = adapter;
            this.bot = bot;
        }

        [HttpPost]
        public async Task PostAsync()
        {
            // Delegate the processing of the HTTP POST to the adapter.
            // The adapter will invoke the bot.
            await this.adapter.ProcessAsync(Request, Response, this.bot);
        }
    }
}
