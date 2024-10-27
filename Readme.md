# Smokeball SEO Rank API

This project is a .NET 8.0 web API for performing SEO rank searches.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Getting Started

### Clone the Repository
Run this command in the terminal:
```sh
git clone https://github.com/mahesvarier/Smokeball-SEORank-Api.git
```

### Running the Application
To run the application, use the following command:
```sh
cd Smokeball-SEOrank-Api
dotnet run
```

The application will run on ```https://localhost:5001```

To run the tests, use the following command:
```sh
cd Smokeball-SEOrank-Api
dotnet test
```
### Run [smokeball-seorank-web](https://github.com/mahesvarier/smokeball-seorank-web) along side this project.

# Project Structure
## Program.cs
The main entry point of the application.

## Models
Contains the data models used in the application.
- **ErrorResponse**
- **SearchRequest**
- **SearchEngineSettings**

## Services
Contains the service interfaces and implementations.

## Tests
Contains the unit tests and test fixtures.

## Fixtures/ResponseContent.html
Contains HTML content used for testing.

## Configuration
Base URL for Search Engines.

## API Endpoints
Can we seen using Swagger: ```https://localhost:5001/swagger/index.html```
- **GET /**: Returns "Hello World!" for testing if the application is running.
- **POST /seo-rankings**: Performs an SEO rank search.

## Error Handling
- Returns `429 Too Many Requests` if the rate limit is exceeded.
- Returns `500 Internal Server Error` for any other exceptions.

## Future Enhancements

### Protecting the Endpoint
We can add authentication and authorization to protect the `/seo-rankings` endpoint. 
This will ensure that only authorized aplication can access the SEO ranking functionality.