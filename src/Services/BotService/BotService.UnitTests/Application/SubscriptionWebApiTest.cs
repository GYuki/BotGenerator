using NUnit.Framework;
using Moq;
using BotService.API.Model;
using BotService.API.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace UnitTest.BotService.Application
{
    [TestFixture]
    public class SubscriptionWebApiTest
    {
        private readonly Mock<ISubscribeRepository> _subscribeRepositoryMock;

        public SubscriptionWebApiTest()
        {
            _subscribeRepositoryMock = new Mock<ISubscribeRepository>();
        }

        [Test]
        public async Task Get_Subscriptions_Success()
        {
            // Arrange
            var fakeSubId = 1;
            var fakeChatId = 1;
            var fakeSub = GetSubscribeFake(fakeSubId, fakeChatId);
            List<Subscribe> fakeSubscribes = new List<Subscribe>()
            {
                fakeSub
            };

            _subscribeRepositoryMock.Setup(x => x.GetSubscribersAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(fakeSubscribes.Select(x => x.ChatId).ToList()));
            
            // Act
            var subController = new SubscribesController(
                _subscribeRepositoryMock.Object
            );

            var actionResult = await subController.GetSubscribersAsync(fakeSub.BotId);

            // Assert
            Assert.AreEqual((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.AreEqual((((ObjectResult)actionResult.Result).Value as List<int>), fakeSubscribes.Select(x => x.ChatId).ToList());
        }

        [Test]
        public async Task Get_Subscriptions_With_Empty_List_Success()
        {
            // Arrange
            var fakeBotId = 1;

            _subscribeRepositoryMock.Setup(x => x.GetSubscribersAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new List<int>()));
            // Act
            var subController = new SubscribesController(
                _subscribeRepositoryMock.Object
            );

            var actionResult = await subController.GetSubscribersAsync(fakeBotId);
            
            // Assert
            Assert.AreEqual((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.IsEmpty(((ObjectResult)actionResult.Result).Value as List<int>);
        }

        [Test]
        public async Task Get_Subscriptions_With_LessOrZero_Id_Should_Return_Bad_Request()
        {
            // Arrange
            var fakeBotId = 0;
            var fakeBotIdLess = -1;

            _subscribeRepositoryMock.Setup(x => x.GetSubscribersAsync(It.IsAny<int>()));
            
            // Act
            var subController = new SubscribesController(
                _subscribeRepositoryMock.Object
            );

            var actionResult = (await subController.GetSubscribersAsync(fakeBotId)).Result as BadRequestResult;
            var actionResultLess = (await subController.GetSubscribersAsync(fakeBotIdLess)).Result as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResultLess);
        }

        [Test]
        public async Task Delete_Subscription_Success()
        {
            // Arrange
            var fakeBotName = "name";
            var fakeChatId = 1;

            _subscribeRepositoryMock.Setup(x => x.DeleteSubscriptionAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            // Act
            var subController = new SubscribesController(
                _subscribeRepositoryMock.Object
            );

            var actionResult = await subController.DeleteSubscribeAsync(fakeBotName, fakeChatId) as NoContentResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Delete_Nonexistent_Subscription_Should_Return_Not_Found()
        {
            // Arrange
            var fakeBotName = "name";
            var fakeChatId = 1;

            _subscribeRepositoryMock.Setup(x => x.DeleteSubscriptionAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(false));
            
            // Act
            var subController = new SubscribesController(
                _subscribeRepositoryMock.Object
            );

            var actionResult = await subController.DeleteSubscribeAsync(fakeBotName, fakeChatId) as NotFoundResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Subscribe_With_Success()
        {
            // Arrange
            var fakeSubId = 1;
            var fakeChatId = 1;
            var fakeSub = GetSubscribeFake(fakeSubId, fakeChatId);

            _subscribeRepositoryMock.Setup(x => x.SubscribeAsync(It.IsAny<Subscribe>()));

            // Act
            var subController = new SubscribesController(
                _subscribeRepositoryMock.Object
            );

            var actionResult = await subController.SubscribeAsync(fakeSub) as OkResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Subscribe_With_Null_Subscription_Should_Return_Bad_Request()
        {
            // Arrange
            Subscribe subscription = null;

            _subscribeRepositoryMock.Setup(x => x.SubscribeAsync(It.IsAny<Subscribe>()));

            // Act
            var subController = new SubscribesController(
                _subscribeRepositoryMock.Object
            );

            var actionResult = await subController.SubscribeAsync(subscription) as BadRequestResult;
            
            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Delete_With_Zero_Chat_Id_Should_Return_Bad_Request()
        {
            // Arrange
            var fakeBotName = "name";
            var fakeChatId = 0;
            
            _subscribeRepositoryMock.Setup(x => x.DeleteSubscriptionAsync(It.IsAny<string>(), It.IsAny<int>()));

            // Act
            var subController = new SubscribesController(
                _subscribeRepositoryMock.Object
            );

            var actionResultEmpty = await subController.DeleteSubscribeAsync(fakeBotName, fakeChatId) as BadRequestResult;

            // Assert
            Assert.NotNull(actionResultEmpty);
        }

        [Test]
        public async Task Delete_With_Null_Or_Empty_Bot_Name_Should_Return_Bad_Request()
        {
            // Arrange
            var fakeBotNameEmpty = string.Empty;
            string fakeBotNameNull = null;
            var fakeChatId = 123;
            
            _subscribeRepositoryMock.Setup(x => x.DeleteSubscriptionAsync(It.IsAny<string>(), It.IsAny<int>()));

            // Act
            var subController = new SubscribesController(
                _subscribeRepositoryMock.Object
            );

            var actionResultEmpty = await subController.DeleteSubscribeAsync(fakeBotNameEmpty, fakeChatId) as BadRequestResult;
            var actionResultNull = await subController.DeleteSubscribeAsync(fakeBotNameNull, fakeChatId) as BadRequestResult;

            // Assert
            Assert.NotNull(actionResultEmpty);
            Assert.NotNull(actionResultNull);
        }

        private Subscribe GetSubscribeFake(int fakeId, int fakeChatId)
        {
            var fakeBotId = 1;
            Bot botFake = GetBotFake(fakeBotId);
            return new Subscribe()
            {
                Id = fakeId,
                Bot = botFake,
                BotId = botFake.Id,
                ChatId = fakeChatId
            };
        }

        private Bot GetBotFake(int fakeBotId)
        {
            int fakeUserId = 1;
            User fakeOwner = GetUserFake(fakeUserId);

            return new Bot()
            {
                Id = fakeBotId,
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