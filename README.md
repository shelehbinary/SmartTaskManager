# Smart Task Manager API 🚀

Smart Task Manager is a backend API for a task management system that demonstrates best practices for .NET 9 development. The project includes authentication, authorization, background tasks, and is ready for production deployment.

## 🛠 Tech Stack

### Backend
- **ASP.NET Core**
- **Entity Framework Core**
- **SQL Server**
- **JWT Bearer Authentication**
- **API Communication - REST**

### Architecture & Patterns
- **Clean Architecture**
- **Repository Pattern**
- **Unit of Work**
- **Dependency Injection**
- **DTO Pattern**

### Infrastructure
- **Docker & Docker Compose**
- **Background Services**
- **Health Checks**
- **Structured Logging**
- **Swagger**

### 🎯 Key Features

- 🔐 **JWT Authentication** - secure registration and login
- 📝 **CRUD Operations** - Full task management
- ⏰ **Background Services** - Automatic marking of overdue tasks
- 🏗 **Clean Architecture** - Separation of responsibilities between layers
- 🐳 **Docker containerization** - Deployment-ready
- 📊 **Health Checks** - System health monitoring
- 📚 **Swagger Documentation** - Interactive API documentation

## 🚀 Quick Start

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/)

### Starting with Docker Compose

1. Clone the repository
```
git clone https://github.com/yourusername/smart-task-manager.git
cd smart-task-manager
```

2. Start all services with one command
```
docker-compose up -d
```

3. The application will be available at the following addresses

- API: http://localhost:8080
- Swagger UI: http://localhost:8080
- Health Check: http://localhost:8080/api/health
