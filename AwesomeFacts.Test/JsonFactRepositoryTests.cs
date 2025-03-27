using AwesomeFacts.Data;
using AwesomeFacts.Models;
using Xunit;

namespace AwesomeFacts.Test;

public class JsonFactRepositoryTests : IDisposable
{
    private readonly string _tempFilePath;
    private readonly JsonFactRepository _repository;

    public JsonFactRepositoryTests()
    {
        _tempFilePath = TestHelpers.CreateTempFactsFile();
        _repository = new JsonFactRepository(_tempFilePath);
    }

    public void Dispose()
    {
        TestHelpers.CleanupTempFile(_tempFilePath);
    }

    [Fact]
    [Trait("Category", "Read")]
    public async Task GetAllFactsAsync_ReturnsAllFacts()
    {
        // Act
        var facts = await _repository.GetAllFactsAsync();

        // Assert
        Assert.NotNull(facts);
        Assert.Equal(2, facts.Count());
    }

    [Fact]
    [Trait("Category", "Read")]
    public async Task GetFactByIdAsync_WithValidId_ReturnsFact()
    {
        // Act
        var fact = await _repository.GetFactByIdAsync(1);

        // Assert
        Assert.NotNull(fact);
        Assert.Equal(1, fact.Id);
        Assert.Equal("Test fact 1", fact.Text);
    }

    [Fact]
    [Trait("Category", "Read")]
    public async Task GetFactByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Act
        var fact = await _repository.GetFactByIdAsync(999);

        // Assert
        Assert.Null(fact);
    }

    [Fact]
    [Trait("Category", "Create")]
    public async Task CreateFactAsync_AddsNewFact()
    {
        // Arrange
        var newFact = new Fact
        {
            Text = "New test fact",
            Category = "Test",
            IsVerified = true
        };

        // Act
        var createdFact = await _repository.CreateFactAsync(newFact);

        // Assert
        Assert.NotNull(createdFact);
        Assert.Equal(3, createdFact.Id); // Next ID should be 3
        Assert.Equal(newFact.Text, createdFact.Text);
        Assert.Equal(newFact.Category, createdFact.Category);
        Assert.Equal(newFact.IsVerified, createdFact.IsVerified);
        Assert.NotEqual(default, createdFact.CreatedAt);

        // Verify it was saved
        var allFacts = await _repository.GetAllFactsAsync();
        Assert.Equal(3, allFacts.Count());
    }

    [Fact]
    [Trait("Category", "Update")]
    public async Task UpdateFactAsync_WithValidId_UpdatesFact()
    {
        // Arrange
        var updatedFact = new Fact
        {
            Text = "Updated test fact",
            Category = "Updated",
            IsVerified = false
        };

        // Act
        var result = await _repository.UpdateFactAsync(1, updatedFact);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(updatedFact.Text, result.Text);
        Assert.Equal(updatedFact.Category, result.Category);
        Assert.Equal(updatedFact.IsVerified, result.IsVerified);

        // Verify it was saved
        var retrievedFact = await _repository.GetFactByIdAsync(1);
        Assert.NotNull(retrievedFact);
        Assert.Equal(updatedFact.Text, retrievedFact.Text);
    }

    [Fact]
    [Trait("Category", "Update")]
    public async Task UpdateFactAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var updatedFact = new Fact
        {
            Text = "Updated test fact",
            Category = "Updated",
            IsVerified = false
        };

        // Act
        var result = await _repository.UpdateFactAsync(999, updatedFact);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    [Trait("Category", "Update")]
    public async Task UpdateFactAsync_PreservesCreatedAtTimestamp()
    {
        // Arrange
        var newFact = new Fact
        {
            Text = "Test fact",
            Category = "Test",
            IsVerified = true
        };
        var createdFact = await _repository.CreateFactAsync(newFact);
        var originalCreatedAt = createdFact.CreatedAt;

        // Act
        var updatedFact = new Fact
        {
            Text = "Updated test fact",
            Category = "Updated",
            IsVerified = false
        };
        var result = await _repository.UpdateFactAsync(createdFact.Id, updatedFact);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(originalCreatedAt, result.CreatedAt);

        // Verify it was saved
        var retrievedFact = await _repository.GetFactByIdAsync(createdFact.Id);
        Assert.NotNull(retrievedFact);
        Assert.Equal(originalCreatedAt, retrievedFact.CreatedAt);
    }

    [Fact]
    [Trait("Category", "Delete")]
    public async Task DeleteFactAsync_WithValidId_DeletesFact()
    {
        // Act
        var result = await _repository.DeleteFactAsync(1);

        // Assert
        Assert.True(result);

        // Verify it was deleted
        var deletedFact = await _repository.GetFactByIdAsync(1);
        Assert.Null(deletedFact);

        var allFacts = await _repository.GetAllFactsAsync();
        Assert.Single(allFacts);
    }

    [Fact]
    [Trait("Category", "Delete")]
    public async Task DeleteFactAsync_WithInvalidId_ReturnsFalse()
    {
        // Act
        var result = await _repository.DeleteFactAsync(999);

        // Assert
        Assert.False(result);

        // Verify nothing was deleted
        var allFacts = await _repository.GetAllFactsAsync();
        Assert.Equal(2, allFacts.Count());
    }

    [Fact]
    [Trait("Category", "Persistence")]
    public async Task Changes_ArePersisted_ToDisk()
    {
        // Arrange
        var newFact = new Fact
        {
            Text = "Test persistence",
            Category = "Test",
            IsVerified = true
        };

        // Act
        var createdFact = await _repository.CreateFactAsync(newFact);
        
        // Create a new repository instance to read from disk
        var newRepository = new JsonFactRepository(_tempFilePath);
        var loadedFact = await newRepository.GetFactByIdAsync(createdFact.Id);

        // Assert
        Assert.NotNull(loadedFact);
        Assert.Equal(createdFact.Id, loadedFact.Id);
        Assert.Equal(createdFact.Text, loadedFact.Text);
        Assert.Equal(createdFact.Category, loadedFact.Category);
        Assert.Equal(createdFact.IsVerified, loadedFact.IsVerified);
        Assert.Equal(createdFact.CreatedAt, loadedFact.CreatedAt);
    }
} 