using NUnit.Framework;
using Moq;
using TelegramReceiver.API.Models;
using TelegramReceiver.API.Controllers;
using TelegramReceiver.API.Infrastructure.Services;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace UnitTest.TelegramReceiver.Application
{
    [TestFixture]
    public class TelegramApiTest
    {
        private readonly Mock<ITelegramService> _telegramServiceMock;

        public TelegramApiTest()
        {
            _telegramServiceMock = new Mock<ITelegramService>();
        }

        [Test]
        public async Task Handle_Message_With_No_Commands_Ok()
        {
            // Arrange
            var botTokenFake = "fakeToken";
            string mockCommandResult = "result";

            var fakeUpdateId = 1;
            var fakeUpdate = GetUpdateFake(fakeUpdateId);

            _telegramServiceMock.Setup(x => x.GenerateResponseTextAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(mockCommandResult));
            
            _telegramServiceMock.Setup(x => x.SendMessageToBotAsync(It.IsAny<string>(), It.IsAny<SendMessage>(), It.IsAny<CancellationToken>()));
                
            // Act
            var telegramController = new TelegramController(
                _telegramServiceMock.Object
            );

            var actionResult = await telegramController.HandleMessageAsync(botTokenFake, fakeUpdate) as AcceptedResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Handle_Message_With_Command_Ok()
        {
            // Arrange
            var botTokenFake = "fakeToken";
            string mockCommandResult = "result";

            var fakeEntityType = "bot_command";
            var fakeLength = 1;
            var fakeOffset = 1;


            var fakeEntity = GetMessageEntityFake(fakeEntityType, fakeLength, fakeOffset);
            MessageEntity[] fakeEntities = { fakeEntity };

            var fakeUpdateId = 1;
            var fakeUpdate = GetUpdateFake(fakeUpdateId, fakeEntities);

            _telegramServiceMock.Setup(x => x.GenerateResponseTextAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(mockCommandResult));
            
            _telegramServiceMock.Setup(x => x.SendMessageToBotAsync(It.IsAny<string>(), It.IsAny<SendMessage>(), It.IsAny<CancellationToken>()));

            // Act
            var telegramController = new TelegramController(
                _telegramServiceMock.Object
            );

            var actionResult = await telegramController.HandleMessageAsync(botTokenFake, fakeUpdate) as OkResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        private Update GetUpdateFake(int fakeId, MessageEntity[] fakeEntities=null)
        {
            var fakeMessageId = 1;
            var fakeDateId = 1;
            string fakeText = "123123";

            var fakeMessage = GetMessageFake(fakeMessageId, fakeDateId, fakeText, fakeEntities);

            return new Update()
            {
                Id = fakeId,
                Message = fakeMessage
            };
        }

        private MessageEntity GetMessageEntityFake(string fakeType, int fakeOffset, int fakeLength)
        {
            return new MessageEntity()
            {
                Type = fakeType,
                Offset = fakeOffset,
                Length = fakeLength
            };
        }

        private Message GetMessageFake(int fakeId, int fakeDate, string fakeText, MessageEntity[] fakeEntities=null)
        {
            var fakeChatId = 1;
            string fakeType = "type";
            var fakeChat = GetChatFake(fakeChatId, fakeType);

            var fakeUserId = 1;
            bool isBotFake = false;
            string fakeFirstName = "firstName";
            string fakeLastName = "lastName";
            string fakeLanguageCode = "lang";

            var fakeUser = GetUserFake(fakeUserId, isBotFake, fakeFirstName, fakeLastName, fakeLanguageCode);
            
            return new Message()
            {
                MessageId = fakeId,
                From = fakeUser,
                Chat = fakeChat,
                Date = fakeDate,
                Text = fakeText,
                Entities = fakeEntities
            };
        }

        private Chat GetChatFake(int fakeId, string fakeType)
        {
            return new Chat()
            {
                Id = fakeId,
                Type = fakeType
            };
        }

        private User GetUserFake(int fakeId, bool isBotFake, string fakeFirstName, string fakeLastName, string fakeLanguageCode)
        {
            return new User()
            {
                Id = fakeId,
                IsBot = isBotFake,
                FirstName = fakeFirstName,
                LastName = fakeLastName,
                LanguageCode = fakeLanguageCode
            };
        }
    }
}