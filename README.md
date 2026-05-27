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
{
  "Authentication": {
    "Token": "techhive-internal-token"
  }
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

## Middleware Order

The middleware pipeline follows the requested activity order:

1. Exception handling middleware
2. Token authentication middleware
3. Request/response logging middleware

## How Copilot Supported the Work

Copilot was used throughout development as a support tool rather than as an automatic project builder. Its main value was in speeding up routine work, explaining concepts, and helping refine implementation details while the project decisions and final coding choices remained manual.

### Areas Where It Helped

- **Explaining API design concepts**  
  It helped connect RESTful ideas to practical endpoint behavior and real-world request/response patterns.

- **Improving maintainability**  
  It suggested cleaner ways to organize controllers, contracts, models, repositories, and middleware.

- **Assisting during debugging**  
  It was useful when reviewing validation problems, exception handling behavior, and logging flow.

- **Drafting repetitive code**  
  It helped produce boilerplate-style code for standard components, especially middleware and similar supporting pieces.

- **Refining written explanations**  
  It supported the wording of README content, technical descriptions, and architecture-related notes.

### What Remained Developer-Driven

- Copilot did **not** define the project scope or requirements.
- Copilot did **not** make the final design or architecture decisions.
- Copilot did **not** independently build the full application.
- Copilot did **not** replace manual testing, debugging, or understanding of the code.

In this project, Copilot functioned more like a pair-programming assistant: helpful for guidance and productivity, but not a substitute for developer judgment or implementation ownership.

