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

        [Test]
        public async Task Create_Bot_Success()
        {
            // Arrange
            var fakeBotId = 1;
            var fakeToken = "token";
            var fakeName = "name";
            var fakeBot = GetBotFake(fakeBotId, fakeToken, fakeName);

            _botRepositoryMock.Setup(x => x.CreateBotAsync(It.IsAny<Bot>()));
            // Act
            var botController = new BotsController(
                _botRepositoryMock.Object
            );

            var actionResult = await botController.CreateBotAsync(fakeBot) as OkResult;

            Assert.AreEqual(actionResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            
        }

        [Test]
        public async Task Get_Bots_Of_Owner_Success()
        {
            // Arrange
            var fakeBotId = 1;
            var fakeToken = "token";
            var fakeName = "name";
            var fakeBot = GetBotFake(fakeBotId, fakeToken, fakeName);

            _botRepositoryMock.Setup(x => x.GetBotsOfOwnerAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((List<Bot>)fakeBot.Owner.Bots));
            
            // Act
            var botController = new BotsController(
                _botRepositoryMock.Object
            );

            var actionResult = await botController.BotsOfOwnerAsync(fakeBot.OwnerId);

            // Assert
            Assert.AreEqual((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.AreEqual((((ObjectResult)actionResult.Result).Value as List<Bot>), fakeBot.Owner.Bots);
        }
        private Bot GetBotFake(int fakeBotId, string fakeToken, string fakeName)
        {
            int fakeUserId = 1;
            User fakeOwner = GetUserFake(fakeUserId);

            return new Bot()
            {
                Id = fakeBotId,
                Name = fakeName,
                Token = fakeToken,
                Owner = fakeOwner,
                OwnerId = fakeOwner.Id
            };
        }

        private User GetUserFake(int fakeId)
        {
            return new User()
            {
                Id = fakeId,
                Bots = new List<Bot>()
            };
        }
    }
}