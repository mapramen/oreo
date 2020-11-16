namespace Oreo.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Oreo.Games;
    using System;

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private IGameRunnerRepository gameRunnerRepository;

        public GamesController(IGameRunnerRepository gameRunnerRepository)
        {
            this.gameRunnerRepository = gameRunnerRepository;
        }

        [HttpGet]
        public ChannelDetails ChannelId(string gameId)
        {
            IGameRunner gameRunner = this.gameRunnerRepository.Get(gameId);
            return new ChannelDetails { ChannelId = gameRunner.GetPlayerChannelId() };
        }

        public class ChannelDetails
        {
            public string ChannelId { get; set; }
        }
    }
}
