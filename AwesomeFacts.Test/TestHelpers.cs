using System.Text.Json;

namespace AwesomeFacts.Test;

public static class TestHelpers
{
    public static string CreateTestFactsJson()
    {
        var facts = new[]
        {
            new
            {
                id = 1,
                text = "Test fact 1",
                category = "Test",
                createdAt = DateTime.UtcNow,
                isVerified = true
            },
            new
            {
                id = 2,
                text = "Test fact 2",
                category = "Test",
                createdAt = DateTime.UtcNow,
                isVerified = false
            }
        };

        var jsonData = new { facts };
        return JsonSerializer.Serialize(jsonData, new JsonSerializerOptions { WriteIndented = true });
    }

    public static string CreateTempFactsFile()
    {
        var tempPath = Path.GetTempPath();
        var tempFile = Path.Combine(tempPath, $"facts_test_{Guid.NewGuid()}.json");
        File.WriteAllText(tempFile, CreateTestFactsJson());
        return tempFile;
    }

    public static void CleanupTempFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
} 