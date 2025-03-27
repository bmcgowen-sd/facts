using AwesomeFacts.Data;
using AwesomeFacts.Models;
using Xunit;

namespace AwesomeFacts.Test;

public class SqliteFactRepositoryTests : IDisposable
{
    private readonly string _dbPath;
    private readonly IFactRepository _repository;
    private bool _disposed;

    public SqliteFactRepositoryTests()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"facts_test_{Guid.NewGuid()}.db");
        _repository = new SqliteFactRepository(_dbPath);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                if (_repository is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                if (File.Exists(_dbPath))
                {
                    try
                    {
                        File.Delete(_dbPath);
                    }
                    catch (IOException)
                    {
                        // Ignore file access errors during cleanup
                    }
                }
            }
            _disposed = true;
        }
    }

    [Fact]
    [Trait("Category", "Read")]
    public async Task GetAllFactsAsync_ReturnsAllFacts()
    {
        // Arrange
        var fact1 = new Fact { Text = "Test fact 1", Category = "Test", IsVerified = true };
        var fact2 = new Fact { Text = "Test fact 2", Category = "Test", IsVerified = false };
        await _repository.CreateFactAsync(fact1);
        await _repository.CreateFactAsync(fact2);

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
        // Arrange
        var fact = new Fact { Text = "Test fact 1", Category = "Test", IsVerified = true };
        var createdFact = await _repository.CreateFactAsync(fact);

        // Act
        var retrievedFact = await _repository.GetFactByIdAsync(createdFact.Id);

        // Assert
        Assert.NotNull(retrievedFact);
        Assert.Equal(createdFact.Id, retrievedFact.Id);
        Assert.Equal(fact.Text, retrievedFact.Text);
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
        Assert.Equal(1, createdFact.Id);
        Assert.Equal(newFact.Text, createdFact.Text);
        Assert.Equal(newFact.Category, createdFact.Category);
        Assert.Equal(newFact.IsVerified, createdFact.IsVerified);
        Assert.NotEqual(default, createdFact.CreatedAt);

        // Verify it was saved
        var allFacts = await _repository.GetAllFactsAsync();
        Assert.Single(allFacts);
    }

    [Fact]
    [Trait("Category", "Update")]
    public async Task UpdateFactAsync_WithValidId_UpdatesFact()
    {
        // Arrange
        var fact = new Fact { Text = "Test fact", Category = "Test", IsVerified = true };
        var createdFact = await _repository.CreateFactAsync(fact);

        var updatedFact = new Fact
        {
            Text = "Updated test fact",
            Category = "Updated",
            IsVerified = false
        };

        // Act
        var result = await _repository.UpdateFactAsync(createdFact.Id, updatedFact);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdFact.Id, result.Id);
        Assert.Equal(updatedFact.Text, result.Text);
        Assert.Equal(updatedFact.Category, result.Category);
        Assert.Equal(updatedFact.IsVerified, result.IsVerified);

        // Verify it was saved
        var retrievedFact = await _repository.GetFactByIdAsync(createdFact.Id);
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
        // Arrange
        var fact = new Fact { Text = "Test fact", Category = "Test", IsVerified = true };
        var createdFact = await _repository.CreateFactAsync(fact);

        // Act
        var result = await _repository.DeleteFactAsync(createdFact.Id);

        // Assert
        Assert.True(result);

        // Verify it was deleted
        var deletedFact = await _repository.GetFactByIdAsync(createdFact.Id);
        Assert.Null(deletedFact);

        var allFacts = await _repository.GetAllFactsAsync();
        Assert.Empty(allFacts);
    }

    [Fact]
    [Trait("Category", "Delete")]
    public async Task DeleteFactAsync_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var fact = new Fact { Text = "Test fact", Category = "Test", IsVerified = true };
        await _repository.CreateFactAsync(fact);

        // Act
        var result = await _repository.DeleteFactAsync(999);

        // Assert
        Assert.False(result);

        // Verify nothing was deleted
        var allFacts = await _repository.GetAllFactsAsync();
        Assert.Single(allFacts);
    }

    [Fact]
    [Trait("Category", "Persistence")]
    public async Task Changes_ArePersisted_ToDatabase()
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
        var originalCreatedAt = createdFact.CreatedAt;
        
        // Create a new repository instance to read from database
        using var newRepository = new SqliteFactRepository(_dbPath);
        var loadedFact = await newRepository.GetFactByIdAsync(createdFact.Id);

        // Assert
        Assert.NotNull(loadedFact);
        Assert.Equal(createdFact.Id, loadedFact.Id);
        Assert.Equal(createdFact.Text, loadedFact.Text);
        Assert.Equal(createdFact.Category, loadedFact.Category);
        Assert.Equal(createdFact.IsVerified, loadedFact.IsVerified);
        Assert.Equal(originalCreatedAt, loadedFact.CreatedAt);
    }
}