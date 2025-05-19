# AuthenService

A secure authentication service built with ASP.NET Core that provides user authentication and authorization using JWT tokens.

## Features

- User registration and authentication
- JWT-based authentication
- Role-based authorization
- Secure password hashing
- Database seeding with initial roles and users
- Swagger/OpenAPI documentation
- Docker support
- Serilog logging

## Project Structure

The project follows the Clean Architecture pattern:

- `AuthenService.API`: ASP.NET Core Web API project with controllers and configuration
- `AuthenService.Application`: Business logic and interfaces
- `AuthenService.Domain`: Core domain entities and models
- `AuthenService.Infrastructure`: Data access and external service implementations

## Prerequisites

- .NET 8.0 SDK
- SQL Server
- Docker (optional, for containerized deployment)

## Configuration

The service uses the following configuration settings:

1. Connection Strings:
   ```json
   "ConnectionStrings": {
     "Default": "Server=.;Database=AuthDB;Trusted_Connection=True;Encrypt=false;TrustServerCertificate=true;"
   }
   ```

2. JWT Settings:
   ```json
   "JwtSettings": {
     "Issuer": "localhost:8000",
     "key": "ZAeqod53wQ8GmPbakbbkjbq0x3qWn0EsafakfbafkbkfbqgQhBJH7kZlnnba",
     "Audience": "localhost:8000",
     "DurationInMinutes": "10"
   }
   ```

## API Endpoints
## Default Login Credentials (After Database Seeding)

After the database is seeded with initial data, you can use the following default credentials to log in:

- Email: TestUser@Test.com
- Username: Testuser
- Password: micr0s0ft_

This user is assigned the "User" role and can be used to test the authentication system.

### Authentication

- `POST /api/auth/login`: User login
- `POST /api/user/register`: User registration

### User Management (Requires Authentication)

- `GET /api/user/all`: Get all users
- `GET /api/user/{userId}`: Get user by ID

## Running the Application

1. Clone the repository
2. Update connection strings in `appsettings.json`
3. Run the application:
   ```bash
   dotnet run
   ```

   Or using Docker:
   ```bash
   docker build -t authenservice .
   docker run -p 8080:8080 -p 8081:8081 authenservice
   ```

## Security Features

- Password requirements can be configured in `Program.cs`
- JWT token validation with configurable expiration
- Role-based authorization using ASP.NET Core Identity
- HTTPS redirection in production
- Secure logging with Serilog

## Logging

The application uses Serilog for logging with:
- Console output
- Rolling file logs (daily)
- Request logging

## Contributing

1. Fork the repository
2. Create your feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request
