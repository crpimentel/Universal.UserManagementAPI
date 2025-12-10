# CQRS Structure for Application Layer

- Commands: Write operations (e.g., RegisterUserCommand)
- Queries: Read operations (e.g., AuthenticateUserQuery)
- Handlers: Logic for each Command/Query

This structure supports MediatR and CQRS, improving separation of concerns and scalability.