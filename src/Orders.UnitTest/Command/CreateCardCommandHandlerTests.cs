using Moq;
using System;
using System.Threading.Tasks;
using Orders.Command.CreateCard;
using Orders.Core;
using Orders.Core.Cards;
using Orders.Core.Shared;
using Xunit;

namespace Orders.UnitTest.Command
{
    public class CreateCardCommandHandlerTests
    {
        private readonly CreateCardCommandHandler _sut;
        private readonly Mock<IEventBus> _eventBusMock;
        private readonly Mock<ICardWriteOnlyRepository> _cardRepositoryMock;

        public CreateCardCommandHandlerTests()
        {
            _eventBusMock = new Mock<IEventBus>();
            _cardRepositoryMock = new Mock<ICardWriteOnlyRepository>();
            _cardRepositoryMock.Setup(x => x.Add(It.IsAny<Card>()))
                .ReturnsAsync(true);

            _sut = new CreateCardCommandHandler(_eventBusMock.Object, _cardRepositoryMock.Object, new ValidationNotificationHandler());
        }

        [Fact]
        [Trait("Card", nameof(CreateCardCommandHandler))]
        public async Task Return_Success_Result()
        {
            // Arrange
            var command = CreateCardCommand();

            // Act
            var result = await _sut.Handle(command);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        [Trait("Card", nameof(CreateCardCommandHandler))]
        public async Task Return_CardHolder_Within_Result()
        {
            // Arrange
            var command = CreateCardCommand();

            // Act
            var result = await _sut.Handle(command);

            // Assert
            Assert.Equal(command.CardHolder, result.CardHolder);
        }

        [Fact]
        [Trait("Card", nameof(CreateCardCommandHandler))]
        public async Task Return_Number_Within_Result()
        {
            // Arrange
            var command = CreateCardCommand();

            // Act
            var result = await _sut.Handle(command);

            // Assert
            Assert.Equal(command.Number, result.Number);
        }

        [Fact]
        [Trait("Card", nameof(CreateCardCommandHandler))]
        public async Task Return_ExpirationDate_Within_Result()
        {
            // Arrange
            var command = CreateCardCommand();

            // Act
            var result = await _sut.Handle(command);

            // Assert
            Assert.Equal(command.ExpirationDate, result.ExpirationDate);
        }

        private CreateCardCommand CreateCardCommand()
        {
            return new CreateCardCommand("371449635398431", "MR FILIPE LIMA", DateTime.Now.Date);
        }
    }
}