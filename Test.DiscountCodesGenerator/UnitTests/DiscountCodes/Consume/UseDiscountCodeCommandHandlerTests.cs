using DiscountCodesGenerator.Repositories.DiscountCodeRespository;
using DiscountCodesGenerator.Services.DiscountCodes.Consume;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.DiscountCodesGenerator.UnitTests.DiscountCodes.Consume;
public class UseDiscountCodeCommandHandlerTests
{
    private readonly Mock<IDiscountCodeRepository> _mockRepo = new();
    private readonly Mock<ILogger<Handler>> _mockLogger = new();
    private readonly Handler _handler;

    public UseDiscountCodeCommandHandlerTests()
    {
        _handler = new Handler(_mockRepo.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ShouldIncrementUsageOfDiscountCodeCount()
    {
        // Arrange
        const string code = "TESTCODE";
        _mockRepo.Setup(r => r.IncrementCodeUsageAsync(code, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

        var command = new Command(code);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Success
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task ShouldReturnFalseForNonExistantDiscountCode()
    {
        // Arrange
        const string code = "INVALID";
        _mockRepo.Setup(r => r.IncrementCodeUsageAsync(code, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

        var command = new Command(code);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Success
            .Should()
            .BeFalse();
    }

    [Fact]
    public async Task ShouldHandleConcurrentUpdates()
    {
        // Arrange
        const string code = "CONCURRENT";
        const int parallelRequests = 100;
        var counter = 0;

        _mockRepo.Setup(r => r.IncrementCodeUsageAsync(code, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Interlocked.Increment(ref counter) == 1);

        var command = new Command(code);

        // Act
        var tasks = Enumerable.Range(0, parallelRequests)
            .Select(_ => _handler.Handle(command, CancellationToken.None))
            .ToArray();

        var results = await Task.WhenAll(tasks);

        // Assert
        results.Count(r => r.Success).Should().Be(1); // Check Success property
        counter.Should().Be(parallelRequests);
    }
}
