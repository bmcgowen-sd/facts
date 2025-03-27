using AwesomeFacts.Data;
using AwesomeFacts.Models;

namespace AwesomeFacts.Services;

public class FactService : IFactService
{
    private readonly IFactRepository _repository;

    public FactService(IFactRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Fact>> GetAllFactsAsync()
    {
        return await _repository.GetAllFactsAsync();
    }

    public async Task<Fact?> GetFactByIdAsync(int id)
    {
        return await _repository.GetFactByIdAsync(id);
    }

    public async Task<Fact> CreateFactAsync(Fact fact)
    {
        return await _repository.CreateFactAsync(fact);
    }

    public async Task<Fact?> UpdateFactAsync(int id, Fact fact)
    {
        return await _repository.UpdateFactAsync(id, fact);
    }

    public async Task<bool> DeleteFactAsync(int id)
    {
        return await _repository.DeleteFactAsync(id);
    }
} 