using NUnit.Framework;
using Moq;
using BotService.API.Model;
using BotService.API.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;


namespace UnitTest.BotService.Application
{
    [TestFixture]
    public class BotWebApiTest
    {
        private readonly Mock<IBotRepository> _botRepositoryMock;

        public BotWebApiTest()
        {
            _botRepositoryMock = new Mock<IBotRepository>();
        }

        [Test]
        public async Task Get_Bot_Success()
        {
            // Arrange
            var fakeBotId = 1;
            var fakeToken = "token";
            var fakeName = "name";
            var fakeBot = GetBotFake(fakeBotId, fakeToken, fakeName);
            
            _botRepositoryMock.Setup(x => x.GetBotAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(fakeBot));
            
            // Act
            var botController = new BotsController(
                _botRepositoryMock.Object
            );

            var actionResult = await botController.BotByIdAsync(fakeBotId);

            // Assert
            Assert.AreEqual((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.AreEqual((((ObjectResult)actionResult.Result).Value as Bot).Id, fakeBot.Id);
        }

        private Bot GetBotFake(int fakeBotId, string fakeToken, string fakeName)
        {
            User fakeOwner = new User()
            {
                Id = 1,
                Bots = new List<Bot>()
            };

            return new Bot()
            {
                Id = fakeBotId,
                Name = fakeName,
                Token = fakeToken,
                Owner = fakeOwner,
                OwnerId = fakeOwner.Id
            };
        }
    }
}