# UserManagementAPI

Simple ASP.NET Core Web API solution for the TechHive Solutions user-management scenario. The solution includes CRUD endpoints, input validation, standardized error handling, request/response logging, token-based authentication, and automated integration tests.

## Solution Structure

- `simpleAPI.slnx`: solution file
- `UserManagementAPI`: Web API project
- `UserManagementAPI.Tests`: xUnit integration tests

## Features

- CRUD endpoints under `api/users`
- Validation for empty names, empty departments, and invalid email addresses
- `404 Not Found` responses for missing users
- Global exception middleware returning JSON errors
- Token-based middleware using the `Authorization: Bearer <token>` header
- Request/response logging middleware for auditing
- In-memory repository with seeded sample users for easy testing

## Run the API

```powershell
dotnet run --project .\UserManagementAPI
```

When the app starts in Development, Swagger UI is available at:

- `http://localhost:5241/swagger`

If you launch from Visual Studio or another profile-aware tool, the browser is configured to open the Swagger UI automatically on startup.

The API token is configured in `UserManagementAPI/appsettings.json`:

```json
"Authentication": {
  "Token": "techhive-internal-token"
}
```

Use that token in requests:

```http
Authorization: Bearer techhive-internal-token
```

## Test the API

Automated tests:

```powershell
dotnet test .\simpleAPI.slnx
```

Manual requests:

- Use `UserManagementAPI/UserManagementAPI.http` in Cursor/Visual Studio.
- Use Swagger UI at `http://localhost:5241/swagger` and click `Authorize` to enter `techhive-internal-token` as the bearer token.
- Or use Postman with these endpoints:
  - `GET /api/users`
  - `GET /api/users/{id}`
  - `POST /api/users`
  - `PUT /api/users/{id}`
  - `DELETE /api/users/{id}`

Suggested manual edge cases:

- Send requests without a bearer token and confirm a `401 Unauthorized` response.
- Submit invalid email or blank names and confirm a `400 Bad Request` response.
- Request a random GUID and confirm a `404 Not Found` response.

## Example Payload

```json
{
  "firstName": "Rina",
  "lastName": "Morris",
  "email": "rina.morris@techhive.local",
  "department": "Finance",
  "isActive": true
}
```

## How Copilot Helped

This project intentionally reflects the activity workflow for using Copilot:

1Copilot generated DTO classes.
2Copilot suggest validation attributes, middleware structure, and exception handling patterns during debugging.
3Copilot help generate test cases and sample requests for Postman or `.http` files.

## Middleware Order

The middleware pipeline follows the requested activity order:

1. Exception handling middleware
2. Token authentication middleware
3. Request/response logging middleware