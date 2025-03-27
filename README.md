# Awesome Facts API

A .NET 8.0 Web API that provides interesting facts through a RESTful interface. This project is built with a clean architecture approach, separating concerns into distinct layers for better maintainability and scalability.

## Project Structure

The solution consists of several projects:

- **AwesomeFacts.Api**: The main Web API project that handles HTTP requests and responses
- **AwesomeFacts.Data**: Data access layer for managing fact storage and retrieval
- **AwesomeFacts.Models**: Shared domain models and DTOs
- **AwesomeFacts.Services**: Business logic layer for fact processing
- **AwesomeFacts.Test**: Unit tests for the application

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- An IDE (Visual Studio 2022, Visual Studio Code, or Rider)

## Getting Started

1. Clone the repository:
   ```bash
   git clone [repository-url]
   cd facts-api-dev
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Build the solution:
   ```bash
   dotnet build
   ```

4. Run the API:
   ```bash
   cd AwesomeFacts.Api
   dotnet run
   ```

The API will be available at `https://localhost:7001` and `http://localhost:5001` by default.

## API Documentation

The API includes Swagger documentation for easy exploration of endpoints. Once the application is running, you can access the Swagger UI at:
- `https://localhost:7001/swagger`
- `http://localhost:5001/swagger`

## Development

### Running Tests

To run the unit tests:
```bash
dotnet test
```

### Project Structure

The solution follows a clean architecture pattern:

- **Models**: Contains domain models and DTOs
- **Data**: Handles data access and persistence
- **Services**: Contains business logic
- **Api**: Manages HTTP requests and responses

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.

## Author

Bret McGowen

## Acknowledgments

- Built with [.NET 8.0](https://dotnet.microsoft.com/)
- API documentation powered by [Swagger](https://swagger.io/) 