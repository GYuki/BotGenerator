using NUnit.Framework;
using Moq;
using TelegramReceiver.API.Models;
using TelegramReceiver.API.Controllers;
using TelegramReceiver.API.Infrastructure.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace UnitTest.TelegramReceiver.Application
{
    [TestFixture]
    public class CommandsApiTest
    {
        private readonly Mock<ICommandRepository> _commandRepositoryMock;

        public CommandsApiTest()
        {
            _commandRepositoryMock = new Mock<ICommandRepository>();
        }

        [Test]
        public async Task Get_Command_Success()
        {
            // Arrange
            var fakeId = 1;
            var fakeToken = "token";
            var fakeRequest = "request";
            var fakeDescription = "description";
            var fakeResponse = "response";

            var fakeCommand = GetCommandFake(fakeId, fakeToken, fakeRequest, fakeDescription, fakeResponse);

            _commandRepositoryMock.Setup(x => x.GetCommandAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(fakeCommand));
            
            // Act
            var commandsController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = await commandsController.GetCommandByIdAsync(fakeId);

            // Assert
            Assert.AreEqual((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.AreEqual((((ObjectResult)actionResult.Result).Value as Command).Id, fakeCommand.Id);
        }

        [Test]
        public async Task Get_Command_With_Zero_Id_Should_Return_Bad_Request()
        {
            //Arrange
            var fakeCommandId = 0;

            // Act
            var commandsController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = (await commandsController.GetCommandByIdAsync(fakeCommandId)).Result as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Get_Command_Which_Does_Not_Exist()
        {
            // Arrange
            var fakeCommandId = 1;

            _commandRepositoryMock.Setup(x => x.GetCommandAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((Command)null));
            
            // Act
            var commandsController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = (await commandsController.GetCommandByIdAsync(fakeCommandId)).Result as NotFoundResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Post_Command_Success()
        {
            // Arrange
            var fakeId = 1;
            var fakeToken = "token";
            var fakeRequest = "request";
            var fakeDescription = "description";
            var fakeResponse = "response";

            var fakeCommand = GetCommandFake(fakeId, fakeToken, fakeRequest, fakeDescription, fakeResponse);

            _commandRepositoryMock.Setup(x => x.GetCommandByTokenAndRequestAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult((Command)null));
            
            // Act
            var commandsController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = await commandsController.CreateCommandAsync(fakeCommand) as OkResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Post_Command_Conflict()
        {
            // Arrange
            var fakeId = 1;
            var fakeToken = "token";
            var fakeRequest = "request";
            var fakeDescription = "description";
            var fakeResponse = "response";

            var fakeCommand = GetCommandFake(fakeId, fakeToken, fakeRequest, fakeDescription, fakeResponse);

            _commandRepositoryMock.Setup(x => x.GetCommandByTokenAndRequestAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(fakeCommand));
            
            // Act
            var commandsController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = await commandsController.CreateCommandAsync(fakeCommand) as ConflictResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        public async Task Post_Command_With_Null_Command_Should_Return_Bad_Request()
        {
            // Arrange
            Command fakeCommand = null;

            // Act
            var commandsController = new CommandsController(
                _commandRepositoryMock.Object
            );

            var actionResult = await commandsController.CreateCommandAsync(fakeCommand) as BadRequestResult;

            //Assert
            Assert.NotNull(actionResult);
        }

        private Command GetCommandFake(int fakeId, string fakeToken, string fakeRequest, string fakeDescription, string fakeResponse)
        {
            return new Command()
            {
                Id = fakeId,
                Token = fakeToken,
                Request = fakeRequest,
                Description = fakeDescription,
                Response = fakeResponse
            };
        }
    }
}