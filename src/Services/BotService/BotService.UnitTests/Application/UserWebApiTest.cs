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
    public class UserWebApiTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public UserWebApiTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
        }

        [Test]
        public async Task Get_User_Success()
        {
            // Arrange
            var fakeUserId = 1;
            var fakeSenderId = "1";
            var fakeUser = GetUserFake(fakeUserId, fakeSenderId);
            
            _userRepositoryMock.Setup(x => x.GetUserAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(fakeUser));
            
            // Act
            var userController = new UsersController(
                _userRepositoryMock.Object
            );

            var actionResult = await userController.UserByIdAsync(fakeUserId);

            // Assert
            Assert.AreEqual((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.AreEqual((((ObjectResult)actionResult.Result).Value as User).Id, fakeUserId);
            Assert.AreEqual((((ObjectResult)actionResult.Result).Value as User).SenderId, fakeSenderId);
        }

        [Test]
        public async Task Post_User_Success()
        {
            // Arrange
            var fakeUserId = 1;
            var fakeSenderId = "1";
            var fakeUser = GetUserFake(fakeUserId, fakeSenderId);

            _userRepositoryMock.Setup(x => x.CreateUserAsync(It.IsAny<User>()));
            _userRepositoryMock.Setup(x => x.GetUserBySenderIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((User)null));
            
            // Act
            var userController = new UsersController(
                _userRepositoryMock.Object
            );

            var actionResult = await userController.CreateUserAsync(fakeUser) as OkResult;
            
            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Get_Nonexistent_User()
        {
            // Arrange
            var fakeUserId = 2;

            _userRepositoryMock.Setup(x => x.GetUserAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((User)null));
            
            // Act
            var userController = new UsersController(
                _userRepositoryMock.Object
            );

            var actionResult = (await userController.UserByIdAsync(fakeUserId));
            
            // Asssert
            Assert.AreEqual((actionResult.Result as NotFoundResult).StatusCode, (int)System.Net.HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Get_User_Who_Already_Exists()
        {
            // Arrange
            var fakeId = 1;
            var fakeSenderId = "123";
            var fakeUser = GetUserFake(fakeId, fakeSenderId);

            _userRepositoryMock.Setup(x => x.GetUserBySenderIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(fakeUser));
            
            // Act
            var userController = new UsersController(
                _userRepositoryMock.Object
            );

            var actionResult = await userController.CreateUserAsync(fakeUser) as ConflictResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Get_User_with_Zero_Id_Should_Return_Bad_Request()
        {
            // Arrange
            var fakeUserId = 0;

            _userRepositoryMock.Setup(x => x.GetUserAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((User)null));
            
            // Act
            var userController = new UsersController(
                _userRepositoryMock.Object
            );
        
            var actionResult = (await userController.UserByIdAsync(fakeUserId)).Result as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Null_User_Creataion_Should_Return_Bad_Request()
        {
            _userRepositoryMock.Setup(x => x.CreateUserAsync(It.IsAny<User>()));
            
            // Act
            var userController = new UsersController(
                _userRepositoryMock.Object
            );

            var actionResult = await userController.CreateUserAsync(null) as BadRequestResult;
            
            // Assert
            Assert.NotNull(actionResult);
        }

        private User GetUserFake(int id, string senderId)
        {
            return new User
            {
                Id = id,
                Bots = new List<Bot>(),
                SenderId = senderId
            };
        }
    }
}