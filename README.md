# Universal.UserManagementAPI

Arquitectura el proyecto , la arquitectura del proyecto sera monolotica por las siguientes razones:
1.Es una prueba técnica:  no sera puesto en funcionamiento para un entorno real de producción.
2.Simplicidad:	Es un proyecto nuevo  con un solo desarrollador.
3.Costo y Velocidad:	Necesidad de desplegar rápidamente, bajo costo de infraestructura (menos servidores/contenedores).
4.Transacciones Simples

Servicio de gestión de usuarios para Grupo Universal — Prueba técnica .NET

## Objetivo
Implementar una API RESTful en .NET 8 que soporte:
- Registro de usuarios con validación y hashing (BCrypt).
- Autenticación con JWT.
- Endpoints protegidos que consumen una API externa (`jsonplaceholder.typicode.com`).
- Base de datos en memoria (EF Core InMemory) durante la ejecución.
- Buenas prácticas: Clean Architecture , DI, FluentValidation, Swagger, tests.

## Estructura del repositorio
- `Universal.UserService.Api` - Capa Presentation (Web API)
- `Universal.UserService.Application` - Lógica de aplicación / casos de uso
- `Universal.UserService.Domain` - Entidades y reglas del dominio
- `Universal.UserService.Infrastructure` - Implementaciones concretas (EF Core, repos)
- `Universal.UserService.Tests` - Pruebas unitarias

> Justificación: la estructura sigue *Arquitectura por Capas (Layered Architecture)* (dominio en el centro, infraestructura fuera), lo que facilita pruebas, mantenimiento y cambios de tecnología.

## Requisitos
- .NET 8 SDK
- Git
- (Opcional) GitHub CLI (`gh`) para crear PRs y manejar repos

## Cómo ejecutar localmente
1. Clona el repo (o usa el que te entregaron).
2. Desde la raíz:
```bash
dotnet restore
dotnet build
dotnet run --project Universal.UserService.Api/Universal.UserService.Api.csproj
