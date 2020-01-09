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