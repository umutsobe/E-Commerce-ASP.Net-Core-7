# ASP.NET Mini-E-Commerce App

**Before You Begin:** To try out the application, you can visit the following link: [E-Commerce App](https://e-commerce-ssr-bc738295e48f.herokuapp.com/). Please note that the initial API request may take 10-15 seconds to respond because I use Azure student subscription, but next requests will be much faster.

[Swagger All (67) HTTP Methods Image](https://github.com/umutsobe/mini-e-trade-backend---asp.net-core-6/assets/120561448/cb24ae39-9b4e-4164-aeba-0d00e24c3112)

[Database Entity–Relationship Diagram](https://github.com/umutsobe/mini-e-trade-backend---asp.net-core-6/assets/120561448/b3bde43c-d2b0-46fe-9f38-f6f92bc393d6)

[All Images](https://imgur.com/a/Xh4jWX5)

## About the Project

This project is an ASP.NET API that provides E-Commerce functionality. The API is developed using two fundamental architectural principles: **Onion Architecture** and **CQRS Pattern**.

### Onion Architecture

Onion Architecture is a design pattern that promotes a separation of concerns in software development. It divides the application into concentric layers, each with a specific responsibility. These layers include:

- **Domain Layer**: This innermost layer exclusively contains entity classes, representing the core business logic and domain models. These entities are used for data access via Entity Framework. It is independent of any external concerns.

- **Application Layer**: This layer contains application-specific logic, including the implementation of the CQRS pattern, Service interfaces and Data Transfer Objects (DTOs) are used to transfer data between different layers of the application.

- **Infrastructure Layer**: The infrastructure layer deals with external concerns such as email services, external APIs, and non-database-related commands.

- **Persistence Layer**: The Persistence Layer is responsible for managing database-related commands and operations. This includes tasks such as:

  - Entity Framework for data access.
  - Database schema and migration management.
  - Repository pattern implementation for database interactions.

- **SignalR Integration**: SignalR is integrated into the API for real-time communication and notifications. It enables features such as:

  - Real-time chat functionality.
  - Live updates on order status changes.
  - Interactive user notifications.

- **Presentation Layer**: This is the outermost layer, responsible for handling HTTP requests. It contains HTTP controllers to respond to incoming client requests.

Onion Architecture promotes maintainability, testability, and flexibility by ensuring that each layer only depends on the layer closer to the core.

## Features

### Multiple Role Management

This API supports multiple role management for different types of users, including the following roles:

- Admin Role
- Moderator Role
- Customer Service Role
- Normal User Role
  .
  .
  .

### Repository Pattern

The Repository pattern abstracts database operations and is used for communication with the database. This helps keep the code cleaner and easier to maintain.

### JWT Authentication

The API secures users with JSON Web Token (JWT) authentication. This makes it easier to manage user sessions and security.

### Email Service

The API includes email services for tasks such as password reset, email updates, and sending notifications to customers when orders are completed.

### Two-Factor Authentication

Two-factor authentication is used to enhance security by requiring users to verify their identity through a two-step process.

### Rate Limiter for DDoS Protection

To protect against Distributed Denial of Service (DDoS) attacks, the API employs a rate limiter. This restricts unwanted traffic and ensures the server's ability to continue serving.

### Azure Blob Storage for Photo Storage

For photo storage, the API utilizes Azure Blob Storage. This allows for secure storage of large amounts of media files.

### Google Login

Register and login without a password using Google Login.

## Getting Started

To run and develop the API locally, you can follow these steps:

1. Copy the project files to a local directory.
2. Run the `dotnet restore` command to install dependencies.
3. Configure the database connection and other settings in the `appsettings.json` file.
4. Start the API using the `dotnet run` command.

For more details, please refer to the [Documentation](/docs) directory.
