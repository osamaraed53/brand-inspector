# Brand Inspector

## Overview
**Brand Inspector** is a desktop application built using **Windows Forms (.NET Framework 4.8.1)** that validates PowerPoint (`.pptx`) files against predefined brand guidelines.

The system consists of two main parts:
- **Frontend (WinForms)**: Handles UI, file selection, and local PPTX scanning using Open XML SDK.
- **Backend (.NET 9 Web API)**: Provides authentication and brand configuration (fonts, colors, sizes).



## Project Structure
```
src/
|__ Backend/ -> ASP.NET Core Web API (.NET 9)
|__ Frontend/ -> WinForms App (.NET Framework 4.8.1)
```

---

## Features

### Frontend (WinForms)
- Login screen with authentication
- Browse and select `.pptx` files
- Scan presentation for:
  - Fonts
  - Colors
  - Sizes
- Display results in `DataGridView`
- Sidebar for grouped errors
- Status bar with:
  - Total scanned items
  - error count
  - Progress indicator
- Non-blocking UI using async (`Task.Run`)

### Backend (.NET 9 API)
- JWT Authentication
- Brand configuration APIs
- PostgreSQL database integration
- NoSQL JSONB-based storage for brand config   
- Swagger UI for testing APIs
- Custom exception handling with `ProblemDetails`

---

## Technologies Used

### Frontend
- .NET Framework 4.8.1
- WinForms
- Open XML SDK
- Newtonsoft.Json
- Microsoft.Extensions.DependencyInjection
- MVP Pattern

### Backend
- .NET 9 Web API
- Entity Framework Core
- PostgreSQL (Npgsql)
- JWT Authentication
- BCrypt (password hashing)
- Swagger (Swashbuckle)

---

## Backend Setup

### 1. Configure Database

Update connection string in:
appsettings.json 

```json
"ConnectionStrings": {
  "Database":"Server=localhost;Port=5432;Database=BrandInspector;Username=your_user;Password=your_password"

}
```
### API Endpoints
#### Authentication :
```
POST /auth/login   
POST /auth/sign-up  
```
#### Brand Configuration :
```
GET /brand/fonts
GET /brand/colors
GET /brand/sizes
````
### Authentication
- Uses JWT Bearer Token
- Token is returned after successful login
- All /brand/* endpoints require:
``` Authorization: Bearer <token>```

#### Security
- Passwords are hashed using BCrypt
- JWT is used for secure API communication

### Error Handling
The backend uses a custom exception handler that returns standardized responses using **ProblemDetails**:
```
{
  "title": "Error Title",
  "detail": "Error description",
  "status": 400,
  "instance": "/api/endpoint"
}
```
#### Supported status codes:
- 400 – Bad Request
- 404 – Not Found
- 500 – Internal Server Error
---
## Frontend Setup
### 1. Configure API URL

#### Update App.config:
```
<appSettings>
  <add key="ApiBaseUrl" value="https://localhost:7238" />
</appSettings>
``` 

### Main Window
<img width="1917" height="1044" alt="Image" src="https://github.com/user-attachments/assets/8057e913-9ba4-416d-9b8b-0ca86f3d95b8" />

### Login window
<img width="1166" height="525" alt="Image" src="https://github.com/user-attachments/assets/93b2d98a-6b95-4d70-9054-5b3df3092bde" />

### Swagger page
<img width="1917" height="1114" alt="Image" src="https://github.com/user-attachments/assets/abb1a09b-7100-4f68-8252-09b7c661e280" />


# Notes
- **Scanning is fully local (no API calls during scan)**
- **Backend is only used for authentication and configuration**
- **Uses MVP architecture for clean separation of concerns**