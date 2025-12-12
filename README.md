# Universal.UserManagementAPI

La arquitectura del proyecto es monolítica por las siguientes razones:
1.Es una prueba técnica:  no sera puesto en funcionamiento para un entorno real de producción.
2.Simplicidad:	Es un proyecto nuevo  con un solo desarrollador no habra varias personas haciendo cambios.
3.Costo y Velocidad:	Necesidad de desplegar rápidamente, bajo costo de infraestructura (menos servidores/contenedores).
4.Transacciones Simples


Servicio de gestión de usuarios para Grupo Universal — Prueba técnica .NET

## Arquitectura y Seguridad

El proyecto implementa Clean Architecture y CQRS con MediatR, siguiendo buenas prácticas empresariales:
- Endpoints protegidos con JWT ([Authorize])
- Validaciones automáticas con FluentValidation y manejo global de errores con middleware
- Integración externa segura y resiliente usando IHttpClientFactory, Polly y el patrón Gateway
- Documentación clara en Swagger, incluyendo formatos de error estándar (ValidationProblemDetails)

## Endpoints de integración externa

- `GET /api/posts` — Obtiene todos los posts desde la API externa (requiere JWT)
- `POST /api/posts` — Crea un nuevo post en la API externa (requiere JWT, validaciones automáticas)

## Manejo de errores y validaciones

- Los errores de validación se retornan en formato estándar `ValidationProblemDetails` (HTTP 400)
- Los errores inesperados se manejan globalmente y retornan mensajes genéricos (HTTP 500)
- Los endpoints solo son accesibles para usuarios autenticados 

## Ejemplo de uso

1. Autentícate y obtén un JWT
2. Usa el JWT en la cabecera `Authorization: Bearer <token>` para acceder a los endpoints protegidos
3. Consulta y crea posts externos de forma segura y profesional

## Recomendaciones y extensibilidad

- La arquitectura permite escalar y agregar nuevos endpoints protegidos fácilmente
- El manejo de errores y validaciones es centralizado y profesional
- El código sigue SOLID, es limpio y fácil de mantener
## GITHUB FLOW
1. Seguimos la metodologia propuesta para la evaluación técnica.
2. Creamos un branch master/main limpio y funcional.
3. Desde el branch master, creamos un branch feature/nombre-descriptivo para cada nueva funcionalidad o fix. 
4. Realizamos commits atómicos y descriptivos en el branch feature.
5. Al completar una funcionalidad, abrimos un Pull Request (PR) desde el branch feature al branch master.
6. En el PR, describimos los cambios realizados.No pusismos aprobadores porque  solo era yo.
7. No creamos el cd porque para azure tendriamos que autenticarnos.
8. Protegimos la rama master y cada PR debia pasar el CI
- 
## Objetivo
Implementar una API RESTful en .NET 8 que soporte:
- Registro de usuarios con validación y hashing (BCrypt).
- Autenticación con JWT.
- Endpoints protegidos que consumen una API externa (`jsonplaceholder.typicode.com`).
- Base de datos en memoria (EF Core InMemory) durante la ejecución.
- Buenas prácticas: Clean Architecture , DI, FluentValidation, Swagger, tests.
- Creacion de politicas de seguridad para los endpoints.
- Creacion de politicas retry y circuit braker para las llamadas a la api externa.
- Documentacion clara y profesional.
- Patrones cqrs , mediatr, api gateway y polly.
- CQRS, Factory, y Polly

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
