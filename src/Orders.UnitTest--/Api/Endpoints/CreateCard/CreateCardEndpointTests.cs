using System;
using System.Threading.Tasks;
using CqrsEssentials;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Orders.Api.Endpoints.CreateCard;
using Orders.Command.CreateCard;
using Xunit;

namespace Orders.UnitTest.Api.Endpoints.CreateCard;

public class CreateCardEndpointTests
{
    private readonly Mock<ICommandDispatcher> _commandDispatcherMock;
    private readonly CreateCardEndpoint _sut;

    public CreateCardEndpointTests()
    {
        _commandDispatcherMock = new Mock<ICommandDispatcher>();
        _sut = new CreateCardEndpoint(_commandDispatcherMock.Object);
    }

    [Fact]
    public async Task Should_Return_CreatedResult_With_Correct_Data()
    {
        // Arrange
        var request = new CreateCardRequest
        {
            Number = "xxxx-xxxx-xxxx-xxx",
            CardHolder = "Filipe A. L. Souza",
            ExpirationDate = new DateTime(
                2022,
                1,
                12
            )
        };

        var commandResult = new CreateCardCommandResult(
            Guid.NewGuid(),
            "xxxx-xxxx-xxxx-xxx",
            "Filipe A. L. Souza",
            new DateTime(
                2022,
                1,
                12
            ),
            true
        );

        _commandDispatcherMock
            .Setup(x => x.DispatchAsync(It.IsAny<CreateCardCommand>()))
            .ReturnsAsync(commandResult);

        // Act
        var actionResult = await _sut.Post(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult);
        var response = Assert.IsAssignableFrom<CreateCardResponse>(createdResult.Value);
        AssertResponse(
            commandResult,
            response
        );
    }

    private void AssertResponse(
        CreateCardCommandResult commandResult,
        CreateCardResponse response
    )
    {
        Assert.Equal(
            commandResult.Id,
            response.Id
        );
        Assert.Equal(
            commandResult.Number,
            response.Number
        );
        Assert.Equal(
            commandResult.CardHolder,
            response.CardHolder
        );
        Assert.Equal(
            commandResult.ExpirationDate,
            response.ExpirationDate
        );
    }
}