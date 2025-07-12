
using DiscountCodesGenerator.Repositories.DiscountCodeRespository;
using DiscountCodesGenerator.Services.DiscountCodes.Generate;
using DiscountCodesGenerator.Tools.NanoIdGenerator;
using FluentAssertions;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NanoidDotNet;
using System.Collections.Concurrent;

namespace Test.DiscountCodesGenerator.UnitTests.DiscountCodes.Generate;

public class GenerateDiscountCodesCommandHandlerTests
{
    private readonly Mock<IDiscountCodeRepository> _mockRepo = new();
    private readonly Mock<IMapper> _mockMapper = new();
    private readonly Mock<ILogger<Handler>> _mockLogger = new();
    private readonly Mock<IIdGenerator> _mockIdGenerator = new();
    private readonly Handler _handler;

    public GenerateDiscountCodesCommandHandlerTests()
    {
        _handler = new Handler(_mockRepo.Object, _mockLogger.Object, _mockIdGenerator.Object);
    }

    [Fact]
    public async Task ShouldGenerateCorrectNumberOfDiscountCodes()
    {
        // Arrange
        const int amount = 100;
        var command = new Command(amount, 8);

        _mockIdGenerator.Setup(r => r.GenerateAsync(It.IsAny<int>()))
                .Returns(async (int size) => await Nanoid.GenerateAsync(size: size));
        _mockRepo.Setup(r => r.CodeExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Codes
            .Should()
            .HaveCount(amount);
    }

    [Fact]
    public async Task ShouldRetryCodeGenerationWhenItIsDuplicate()
    {
        // Arrange
        var command = new Command(1, 8);
        var duplicateCode = "DUPLICATE";
        var validCode = "VALIDCODE";
        var callCount = 0;

        _mockIdGenerator.SetupSequence(x => x.GenerateAsync(It.IsAny<int>()))
                      .Returns(Task.FromResult("DUPLICATE"))
                      .Returns(Task.FromResult("VALIDCODE"));

        _mockRepo.Setup(r => r.CodeExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string code, CancellationToken _) =>
                {
                    callCount++;
                    return code == duplicateCode;
                });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Codes
            .Should()
            .ContainSingle()
            .And
            .Contain(validCode);
        callCount.Should().Be(2);
    }

    [Fact]
    public async Task ShouldGenerateCorrectNumberOfDiscountCodesWithParallelism()
    {
        // Arrange
        const int amount = 1000;
        var command = new Command(amount, 8);
        var generatedCodes = new ConcurrentBag<string>();
        var semaphore = new SemaphoreSlim(1, 1);

        _mockIdGenerator.Setup(r => r.GenerateAsync(It.IsAny<int>()))
            .Returns(async (int size) => await Nanoid.GenerateAsync(size: size));

        _mockRepo.Setup(r => r.CodeExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(async (string code, CancellationToken ct) =>
                {
                    await semaphore.WaitAsync(ct);
                    try
                    {
                        return generatedCodes.Contains(code);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result
            .Codes
            .Should()
            .HaveCount(amount)
            .And
            .OnlyHaveUniqueItems();
    }
}