using System.Text.Json;
using System.Text.Json.Serialization;
using AwesomeFacts.Models;

namespace AwesomeFacts.Data;

public class JsonFactRepository : IFactRepository
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    private readonly string _jsonFilePath;
    private List<Fact> _facts = new();
    private int _nextId;

    public JsonFactRepository(string jsonFilePath)
    {
        _jsonFilePath = jsonFilePath;
        Console.WriteLine($"JSON file path: {_jsonFilePath}");
        LoadFacts();
    }

    private void LoadFacts()
    {
        Console.WriteLine($"Loading facts from: {_jsonFilePath}");
        Console.WriteLine($"File exists: {File.Exists(_jsonFilePath)}");
        if (File.Exists(_jsonFilePath))
        {
            var jsonString = File.ReadAllText(_jsonFilePath);
            var jsonData = JsonSerializer.Deserialize<JsonData>(jsonString, _jsonOptions);
            _facts = jsonData?.Facts ?? new List<Fact>();
            Console.WriteLine($"Loaded {_facts.Count} facts");
        }
        else
        {
            _facts = new List<Fact>();
            Console.WriteLine("No facts file found, starting with empty list");
        }

        _nextId = _facts.Any() ? _facts.Max(f => f.Id) + 1 : 1;
    }

    private async Task SaveFactsAsync()
    {
        var jsonData = new JsonData { Facts = _facts };
        var jsonString = JsonSerializer.Serialize(jsonData, _jsonOptions);
        await File.WriteAllTextAsync(_jsonFilePath, jsonString);
    }

    public async Task<IEnumerable<Fact>> GetAllFactsAsync()
    {
        return await Task.FromResult(_facts);
    }

    public async Task<Fact?> GetFactByIdAsync(int id)
    {
        return await Task.FromResult(_facts.FirstOrDefault(f => f.Id == id));
    }

    public async Task<Fact> CreateFactAsync(Fact fact)
    {
        fact.Id = _nextId++;
        fact.CreatedAt = DateTime.UtcNow;
        _facts.Add(fact);
        await SaveFactsAsync();
        return fact;
    }

    public async Task<Fact?> UpdateFactAsync(int id, Fact fact)
    {
        var existingFact = _facts.FirstOrDefault(f => f.Id == id);
        if (existingFact == null)
            return null;

        existingFact.Text = fact.Text;
        existingFact.Category = fact.Category;
        existingFact.IsVerified = fact.IsVerified;

        await SaveFactsAsync();
        return existingFact;
    }

    public async Task<bool> DeleteFactAsync(int id)
    {
        var fact = _facts.FirstOrDefault(f => f.Id == id);
        if (fact == null)
            return false;

        _facts.Remove(fact);
        await SaveFactsAsync();
        return true;
    }

    private class JsonData
    {
        [JsonPropertyName("facts")]
        public List<Fact> Facts { get; set; } = new();
    }
} 