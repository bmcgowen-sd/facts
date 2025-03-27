using AwesomeFacts.Models;

namespace AwesomeFacts.Data;

public interface IFactRepository
{
    Task<IEnumerable<Fact>> GetAllFactsAsync();
    Task<Fact?> GetFactByIdAsync(int id);
    Task<Fact> CreateFactAsync(Fact fact);
    Task<Fact?> UpdateFactAsync(int id, Fact fact);
    Task<bool> DeleteFactAsync(int id);
} 