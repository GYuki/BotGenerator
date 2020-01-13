using Moq;
using NUnit.Framework;
using BotService.API.Model;
using BotService.API.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;


namespace UnitTest.BotService.Application
{
    [TestFixture]
    public class CommandWebApiTest
    {
        private readonly Mock<ICommandRepository> _commandRepositoryMock;

        public CommandWebApiTest()
        {
            _commandRepositoryMock = new Mock<ICommandRepository>();
        }

        [Test]
        public async Task Get_Command_Success()
        {
            // Arrange
            var fakeCommandId = 1;
            var fakeRequest = "request";
            var fakeResponse = "response";
            var fakeCommand = GetCommandFake(fakeCommandId, fakeRequest, fakeResponse);

            _commandRepositoryMock.Setup(x => x.GetCommandAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(fakeCommand));
            
            // Act
            var commandController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = await commandController.CommandByIdAsync(fakeCommandId);

            // Assert
            Assert.AreEqual((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.AreEqual((((ObjectResult)actionResult.Result).Value as Command).Id, fakeCommand.Id);
        }

        [Test]
        public async Task Post_Command_Success()
        {
            // Arrange
            var fakeCommandId = 1;
            var fakeRequest = "request";
            var fakeResponse = "response";
            var fakeCommand = GetCommandFake(fakeCommandId, fakeRequest, fakeResponse);

            _commandRepositoryMock.Setup(x => x.CreateCommandAsync(It.IsAny<Command>()));

            // Act
            var commandController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = await commandController.CreateCommandAsync(fakeCommand) as OkResult;

            // Arrange
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Get_Bot_Commands_Success()
        {
            // Arrange
            var fakeCommandId = 1;
            var fakeRequest = "request";
            var fakeResponse = "response";
            var fakeCommand = GetCommandFake(fakeCommandId, fakeRequest, fakeResponse);

            _commandRepositoryMock.Setup(x => x.GetBotCommandsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((List<Command>)fakeCommand.Bot.Commands));
            
            // Act
            var commandController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = await commandController.GetCommandsByBotIdAsync(fakeCommand.BotId);

            // Assert
            Assert.AreEqual((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.AreEqual((((ObjectResult)actionResult.Result).Value as List<Command>), fakeCommand.Bot.Commands);
        }

        [Test]
        public async Task Put_Command_Success()
        {
            // Assert
            var fakeCommandId = 1;
            var fakeRequest = "request";
            var fakeResponse = "response";
            var fakeCommand = GetCommandFake(fakeCommandId, fakeRequest, fakeResponse);

            Command fakeNewCommand = new Command()
            {
                Id = fakeCommand.Id,
                Response = "response_new"
            };

            _commandRepositoryMock.Setup(x => x.UpdateCommandResponseAsync(It.IsAny<Command>()))
                .Returns(Task.FromResult(new Command()
                {
                    Id = fakeCommand.Id,
                    Request = fakeCommand.Request,
                    Response = fakeNewCommand.Response,
                    Bot = fakeCommand.Bot,
                    BotId = fakeCommand.Id
                }));
            
            // Act
            var commandController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = await commandController.UpdateCommandResponseAsync(fakeNewCommand);

            // Assert
            Assert.AreEqual((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.AreEqual((((ObjectResult)actionResult.Result).Value as Command).Id, fakeCommand.Id);
            Assert.AreEqual((((ObjectResult)actionResult.Result).Value as Command).Response, fakeNewCommand.Response);
        }

        [Test]
        public async Task Delete_Command_Success()
        {
            // Arrange
            var fakeCommandId = 1;

            _commandRepositoryMock.Setup(x => x.DeleteCommandAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            
            // Act
            var commandController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = await commandController.DeleteCommandOfBotAsync(fakeCommandId) as NoContentResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Get_Bot_Commands_With_Empty_List_Should_Return_Success()
        {
            // Arrange
            var fakeBotId = 1;

            _commandRepositoryMock.Setup(x => x.GetBotCommandsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new List<Command>()));
            
            // Act
            var commandController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = await commandController.GetCommandsByBotIdAsync(fakeBotId);

            // Assert
            Assert.AreEqual((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsEmpty(((ObjectResult)actionResult.Result).Value as List<Command>);
        }

        [Test]
        public async Task Get_Bot_Command_With_LessOrZero_Id_Should_Return_Bad_Request()
        {
            // Arrange
            var fakeBotIdZero = 0;
            var fakeBotIdLessThanZero = -1;

            _commandRepositoryMock.Setup(x => x.GetBotCommandsAsync(It.IsAny<int>()));
            
            // Act
            var commandController = new CommandsController(
                _commandRepositoryMock.Object
            );
            
            var actionResultZero = (await commandController.GetCommandsByBotIdAsync(fakeBotIdZero)).Result as BadRequestResult;
            var actionResultLessThanZero = (await commandController.GetCommandsByBotIdAsync(fakeBotIdLessThanZero)).Result as BadRequestResult;

            // Assert
            Assert.NotNull(actionResultZero);
            Assert.NotNull(actionResultLessThanZero);
        }

        [Test]
        public async Task Put_Command_With_Null_Command_Should_Return_Bad_Request()
        {
            // Arrange
            Command fakeBadParam = null;

            _commandRepositoryMock.Setup(x => x.UpdateCommandResponseAsync(It.IsAny<Command>()));

            // Act
            var commandController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = (await commandController.UpdateCommandResponseAsync(fakeBadParam)).Result as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Put_Nonexistent_Command_Should_Return_Not_Found()
        {
            // Assert
            var fakeCommandId = 1;
            var fakeRequest = "request";
            var fakeResponse = "response";
            var fakeCommand = GetCommandFake(fakeCommandId, fakeRequest, fakeResponse);

            _commandRepositoryMock.Setup(x => x.UpdateCommandResponseAsync(It.IsAny<Command>()))
                .Returns(Task.FromResult((Command)null));
            
            // Act
            var commandController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = (await commandController.UpdateCommandResponseAsync(fakeCommand)).Result as NotFoundResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Delete_Non_Existent_Command_Should_Return_Not_Found()
        {
            // Arrange
            var fakeCommandId = 1;

            _commandRepositoryMock.Setup(x => x.DeleteCommandAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(false));
            
            // Act
            var commandController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = await commandController.DeleteCommandOfBotAsync(fakeCommandId) as NotFoundResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Get_Non_Existent_Command_Should_Return_Not_Found()
        {
            // Arrange
            var fakeCommandId = 1;

            _commandRepositoryMock.Setup(x => x.GetCommandAsync(It.IsAny<int>()))
                 .Returns(Task.FromResult((Command)null));
            
            // Act
            var commandController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = (await commandController.CommandByIdAsync(fakeCommandId)).Result as NotFoundResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Get_Command_With_LessOrZero_Id_Should_Return_Bad_Request()
        {
            // Arrange
            var fakeCommandId = 0;
            var fakeCommandIdLess = -1;

            _commandRepositoryMock.Setup(x => x.GetCommandAsync(It.IsAny<int>()))
                 .Returns(Task.FromResult((Command)null));
            
            // Act
            var commandController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = (await commandController.CommandByIdAsync(fakeCommandId)).Result as BadRequestResult;
            var actionResultLess = (await commandController.CommandByIdAsync(fakeCommandIdLess)).Result as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResultLess);
        }

        [Test]
        public async Task Post_Command_With_Null_Param_Should_Return_Bad_Request()
        {
            // Arrange
            Command fakeCommand = null;

            _commandRepositoryMock.Setup(x => x.CreateCommandAsync(It.IsAny<Command>()));

            // Act
            var commandController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = await commandController.CreateCommandAsync(fakeCommand) as BadRequestResult;

            // Arrange
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Delete_Command_With_LessOrZero_Id_Should_Return_Bad_Request()
        {
            // Arrange
            var fakeCommandId = 0;
            var fakeCommandIdLess = 0;

            _commandRepositoryMock.Setup(x => x.DeleteCommandAsync(It.IsAny<int>()));

            // Act
            var commandController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = await commandController.DeleteCommandOfBotAsync(fakeCommandId) as BadRequestResult;
            var actionResultLess = await commandController.DeleteCommandOfBotAsync(fakeCommandId) as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResultLess);
        }

        private Command GetCommandFake(int fakeCommandId, string fakeRequest, string fakeResponse)
        {
            var fakeBotId = 1;
            var fakeToken = "token";
            var fakeName = "name";
            Bot fakeBot = GetBotFake(fakeBotId, fakeToken, fakeName);

            return new Command()
            {
                Id = fakeCommandId,
                Request = fakeRequest,
                Response = fakeResponse,
                Bot = fakeBot,
                BotId = fakeBot.Id
            };
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