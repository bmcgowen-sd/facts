using AwesomeFacts.Data;
using AwesomeFacts.Models;
using AwesomeFacts.Services;
using NSubstitute;
using Xunit;

namespace AwesomeFacts.Test;

public class FactServiceTests
{
    private readonly IFactRepository _repository;
    private readonly FactService _service;

    public FactServiceTests()
    {
        _repository = Substitute.For<IFactRepository>();
        _service = new FactService(_repository);
    }

    [Fact]
    [Trait("Category", "Read")]
    public async Task GetAllFactsAsync_ReturnsAllFacts()
    {
        // Arrange
        var expectedFacts = new List<Fact>
        {
            new() { Id = 1, Text = "Test fact 1", Category = "Test", IsVerified = true },
            new() { Id = 2, Text = "Test fact 2", Category = "Test", IsVerified = false }
        };
        _repository.GetAllFactsAsync().Returns(expectedFacts);

        // Act
        var facts = await _service.GetAllFactsAsync();

        // Assert
        Assert.NotNull(facts);
        Assert.Equal(expectedFacts.Count, facts.Count());
        await _repository.Received(1).GetAllFactsAsync();
    }

    [Fact]
    [Trait("Category", "Read")]
    public async Task GetFactByIdAsync_WithValidId_ReturnsFact()
    {
        // Arrange
        var expectedFact = new Fact { Id = 1, Text = "Test fact", Category = "Test", IsVerified = true };
        _repository.GetFactByIdAsync(1).Returns(expectedFact);

        // Act
        var fact = await _service.GetFactByIdAsync(1);

        // Assert
        Assert.NotNull(fact);
        Assert.Equal(expectedFact.Id, fact.Id);
        Assert.Equal(expectedFact.Text, fact.Text);
        await _repository.Received(1).GetFactByIdAsync(1);
    }

    [Fact]
    [Trait("Category", "Read")]
    public async Task GetFactByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        _repository.GetFactByIdAsync(999).Returns((Fact?)null);

        // Act
        var fact = await _service.GetFactByIdAsync(999);

        // Assert
        Assert.Null(fact);
        await _repository.Received(1).GetFactByIdAsync(999);
    }

    [Fact]
    [Trait("Category", "Create")]
    public async Task CreateFactAsync_CreatesNewFact()
    {
        // Arrange
        var newFact = new Fact { Text = "New test fact", Category = "Test", IsVerified = true };
        var createdFact = new Fact
        {
            Id = 1,
            Text = newFact.Text,
            Category = newFact.Category,
            IsVerified = newFact.IsVerified,
            CreatedAt = DateTime.UtcNow
        };
        _repository.CreateFactAsync(Arg.Any<Fact>()).Returns(createdFact);

        // Act
        var result = await _service.CreateFactAsync(newFact);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdFact.Id, result.Id);
        Assert.Equal(createdFact.Text, result.Text);
        Assert.Equal(createdFact.Category, result.Category);
        Assert.Equal(createdFact.IsVerified, result.IsVerified);
        await _repository.Received(1).CreateFactAsync(Arg.Any<Fact>());
    }

    [Fact]
    [Trait("Category", "Update")]
    public async Task UpdateFactAsync_WithValidId_UpdatesFact()
    {
        // Arrange
        var updatedFact = new Fact { Text = "Updated test fact", Category = "Test", IsVerified = true };
        var existingFact = new Fact
        {
            Id = 1,
            Text = updatedFact.Text,
            Category = updatedFact.Category,
            IsVerified = updatedFact.IsVerified,
            CreatedAt = DateTime.UtcNow
        };
        _repository.UpdateFactAsync(1, Arg.Any<Fact>()).Returns(existingFact);

        // Act
        var result = await _service.UpdateFactAsync(1, updatedFact);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingFact.Id, result.Id);
        Assert.Equal(existingFact.Text, result.Text);
        Assert.Equal(existingFact.Category, result.Category);
        Assert.Equal(existingFact.IsVerified, result.IsVerified);
        await _repository.Received(1).UpdateFactAsync(1, Arg.Any<Fact>());
    }

    [Fact]
    [Trait("Category", "Update")]
    public async Task UpdateFactAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var updatedFact = new Fact { Text = "Updated test fact", Category = "Test", IsVerified = true };
        _repository.UpdateFactAsync(999, Arg.Any<Fact>()).Returns((Fact?)null);

        // Act
        var result = await _service.UpdateFactAsync(999, updatedFact);

        // Assert
        Assert.Null(result);
        await _repository.Received(1).UpdateFactAsync(999, Arg.Any<Fact>());
    }

    [Fact]
    [Trait("Category", "Delete")]
    public async Task DeleteFactAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        _repository.DeleteFactAsync(1).Returns(true);

        // Act
        var result = await _service.DeleteFactAsync(1);

        // Assert
        Assert.True(result);
        await _repository.Received(1).DeleteFactAsync(1);
    }

    [Fact]
    [Trait("Category", "Delete")]
    public async Task DeleteFactAsync_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        _repository.DeleteFactAsync(999).Returns(false);

        // Act
        var result = await _service.DeleteFactAsync(999);

        // Assert
        Assert.False(result);
        await _repository.Received(1).DeleteFactAsync(999);
    }
} 