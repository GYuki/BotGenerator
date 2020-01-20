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
            _botRepositoryMock.Setup(x => x.GetBotByTokenAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((Bot)null));
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

        [Test]
        public async Task Delete_Bot_Success()
        {
            // Arrange
            var fakeBotId = 1;
            var fakeToken = "token";
            var fakeName = "name";
            var fakeBot = GetBotFake(fakeBotId, fakeToken, fakeName);

            _botRepositoryMock.Setup(x => x.DeleteBotAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            // Act
            var botController = new BotsController(
                _botRepositoryMock.Object
            );

            await botController.CreateBotAsync(fakeBot);
            var actionResult = await botController.DeleteBotAsync(fakeBotId) as NoContentResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Get_Bots_For_User_With_No_Bots()
        {
            // Arrange
            var fakeUserId = 1;
            
            _botRepositoryMock.Setup(x => x.GetBotsOfOwnerAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new List<Bot>()));
            
            // Act
            var botController = new BotsController(
                _botRepositoryMock.Object
            );

            var actionResult = await botController.BotsOfOwnerAsync(fakeUserId);

            // Assert
            Assert.AreEqual((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsEmpty(((ObjectResult)actionResult.Result).Value as List<Bot>);
        }

        [Test]
        public async Task Get_User_With_Zero_Id_Should_Return_Bad_Request()
        {
            // Arrange
            var fakeBotId = 0;

            _botRepositoryMock.Setup(x => x.GetBotAsync(It.IsAny<int>()));
            
            // Act
            var botController = new BotsController(
                _botRepositoryMock.Object
            );
            
            var actionResult = (await botController.BotByIdAsync(fakeBotId)).Result as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Get_Nonexistent_Bot()
        {
            // Arrange
            var fakeBotId = 2;

            _botRepositoryMock.Setup(x => x.GetBotAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((Bot)null));
            
            // Act
            var botController = new BotsController(
                _botRepositoryMock.Object
            );

            var actionResult = (await botController.BotByIdAsync(fakeBotId)).Result as NotFoundResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Get_Bots_Of_Owner_With_Zero_Owner_Should_Return_Bad_Request()
        {
            // Arrange
            var fakeOwnerId = 0;

            _botRepositoryMock.Setup(x => x.GetBotsOfOwnerAsync(It.IsAny<int>()));
            
            // Act
            var botController = new BotsController(
                _botRepositoryMock.Object
            );

            var actionResult = (await botController.BotsOfOwnerAsync(fakeOwnerId)).Result as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Delete_Bot_With_Zero_Bot_Id_Should_Return_Bad_Request()
        {
            // Arrange
            var fakeBotId = 0;

            _botRepositoryMock.Setup(x => x.DeleteBotAsync(It.IsAny<int>()));
            
            // Act
            var botController = new BotsController(
                _botRepositoryMock.Object
            );

            var actionResult = await botController.DeleteBotAsync(fakeBotId) as BadRequestResult;
            
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Delete_Nonexistent_Bot()
        {
            // Arrange
            var fakeBotId = 2;
            
            _botRepositoryMock.Setup(x => x.DeleteBotAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(false));

            // Act
            var botController = new BotsController(
                _botRepositoryMock.Object
            );

            var actionResult = await botController.DeleteBotAsync(fakeBotId) as NotFoundResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Create_Bot_With_Null_Bot_Should_Return_Bad_Request()
        {
            // Arrange
            Bot fakeBot = null;

            _botRepositoryMock.Setup(x => x.CreateBotAsync(It.IsAny<Bot>()));

            // Act
            var botController = new BotsController(
                _botRepositoryMock.Object
            );

            var actionResult = await botController.CreateBotAsync(fakeBot) as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        public async Task Create_Existing_Bot_Should_Return_Conflict()
        {
            // Arrange
            var fakeBotId = 1;
            var fakeBotName = "name";
            var fakeBotToken = "token";
            var fakeBot = GetBotFake(fakeBotId, fakeBotToken, fakeBotName);

            _botRepositoryMock.Setup(x => x.GetBotByTokenAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(fakeBot));

            // Act
            var botController = new BotsController(
                _botRepositoryMock.Object
            );

            var actionResult = await botController.CreateBotAsync(fakeBot) as ConflictResult;

            // Arrange
            Assert.NotNull(actionResult);
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