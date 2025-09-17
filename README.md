R
# Order Flow with Microservices

Small project about Product order flow with microservices architecture

[![.NET](https://github.com/bluebetto/Order-Microservices/actions/workflows/dotnet.yml/badge.svg)](https://github.com/bluebetto/Order-Microservices/actions/workflows/dotnet.yml)

# Welcome

The objective of this project is to demonstrate technical skills about how to implement a simple order flow using microservices architecture. The project consists of two main services: Product Service and Order Service. Each service is responsible for a specific part of the order process.

# Technologies Used

## API REST

- Web API controller with Swagger documentation
- FluentValidation for request validation
- Middlewares for error handling and logging
- Health checks for monitoring service status

## EF Core

- Code First approach for database schema management
- Fluent configuration for entity mapping
- Repository pattern for data access abstraction
- Value Object mapping for complex types

## CQRS with MediatR

- Command and Query separation for better scalability
- Validation pipelines for request validation
- Clear separation of read and write operations

## RabbitMQ

- Event handling for asynchronous communication between services
- Exchange topics for message routing

## Testing

- Application tests for service layer validation
- FluentAssertions for expressive assertions
- Moq for mocking dependencies
- Test coverage of at least 80%
