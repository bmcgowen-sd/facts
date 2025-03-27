using Microsoft.EntityFrameworkCore;
using AwesomeFacts.Models;

namespace AwesomeFacts.Data;

public class SqliteFactRepository : IFactRepository, IDisposable
{
    private readonly FactsDbContext _context;
    private bool _disposed;

    public SqliteFactRepository(string dbPath)
    {
        var options = new DbContextOptionsBuilder<FactsDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;
        _context = new FactsDbContext(options);
        _context.Database.EnsureCreated();
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
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    public async Task<IEnumerable<Fact>> GetAllFactsAsync()
    {
        return await _context.Facts.ToListAsync();
    }

    public async Task<Fact?> GetFactByIdAsync(int id)
    {
        return await _context.Facts.FindAsync(id);
    }

    public async Task<Fact> CreateFactAsync(Fact fact)
    {
        fact.CreatedAt = DateTime.UtcNow;
        _context.Facts.Add(fact);
        await _context.SaveChangesAsync();
        return fact;
    }

    public async Task<Fact?> UpdateFactAsync(int id, Fact fact)
    {
        var existingFact = await _context.Facts.FindAsync(id);
        if (existingFact == null)
            return null;

        existingFact.Text = fact.Text;
        existingFact.Category = fact.Category;
        existingFact.IsVerified = fact.IsVerified;

        await _context.SaveChangesAsync();
        return existingFact;
    }

    public async Task<bool> DeleteFactAsync(int id)
    {
        var fact = await _context.Facts.FindAsync(id);
        if (fact == null)
            return false;

        _context.Facts.Remove(fact);
        await _context.SaveChangesAsync();
        return true;
    }
} 