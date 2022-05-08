using System;
using System.Threading.Tasks;
using Moq;
using Orders.Command.CreateTransaction;
using Orders.Core.Shared;
using Orders.Core.Transactions;
using Xunit;

namespace Orders.UnitTest.Command;

public class CreateTransactionCommandHandlerTests
{
    private readonly Mock<IEventBus> _eventBusMock;
    private readonly CreateTransactionCommandHandler _sut;
    private readonly Mock<ITransactionWriteOnlyRepository> _transactionRepositoryMock;

    public CreateTransactionCommandHandlerTests()
    {
        _eventBusMock = new Mock<IEventBus>();
        _transactionRepositoryMock = new Mock<ITransactionWriteOnlyRepository>();
        _transactionRepositoryMock.Setup(x => x.Add(It.IsAny<Transaction>()))
            .ReturnsAsync(true);

        _sut = new CreateTransactionCommandHandler(
            _eventBusMock.Object,
            _transactionRepositoryMock.Object
        );
    }

    [Fact]
    [Trait(
        "Transaction",
        nameof(CreateTransactionCommandHandler)
    )]
    public async Task Return_Success_Result()
    {
        // Arrange
        var command = CreateTransactionCommand();

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    [Trait(
        "Transaction",
        nameof(CreateTransactionCommandHandler)
    )]
    public async Task Return_Id_Within_Result()
    {
        // Arrange
        var command = CreateTransactionCommand();

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.NotEqual(
            Guid.Empty,
            result.Id
        );
    }

    [Fact]
    [Trait(
        "Transaction",
        nameof(CreateTransactionCommandHandler)
    )]
    public async Task Return_UniqueId_Within_Result()
    {
        // Arrange
        var command = CreateTransactionCommand();

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.Equal(
            command.UniqueId,
            result.UniqueId
        );
    }

    [Fact]
    [Trait(
        "Transaction",
        nameof(CreateTransactionCommandHandler)
    )]
    public async Task Return_ChargeDate_Within_Result()
    {
        // Arrange
        var command = CreateTransactionCommand();

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.Equal(
            command.ChargeDate,
            result.ChargeDate
        );
    }

    [Fact]
    [Trait(
        "Transaction",
        nameof(CreateTransactionCommandHandler)
    )]
    public async Task Return_Amount_Within_Result()
    {
        // Arrange
        var command = CreateTransactionCommand();

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.Equal(
            command.Amount,
            result.Amount
        );
    }

    [Fact]
    [Trait(
        "Transaction",
        nameof(CreateTransactionCommandHandler)
    )]
    public async Task Return_CurrencyCode_Within_Result()
    {
        // Arrange
        var command = CreateTransactionCommand();

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.Equal(
            command.CurrencyCode,
            result.CurrencyCode
        );
    }

    public CreateTransactionCommand CreateTransactionCommand()
    {
        return new CreateTransactionCommand(
            100M,
            "BRA",
            Guid.NewGuid(),
            Guid.NewGuid().ToString(),
            DateTimeOffset.Now
        );
    }
}