# 📚 BookShopAPI - .NET 8 Web API

A modern, production-ready **Book Shop API** built with .NET 8, demonstrating enterprise-level architecture patterns and best practices.

## 🌟 Key Features

- **🏗️ N-Tier Architecture** - Clean separation of concerns (API → Services → Repositories → Data Access)
- **🗃️ Entity Framework Core** - Code-first approach with SQL Server
- **🔄 Soft Delete** - Safe data removal without permanent loss
- **⚡ Fluent API** - Configuration without data annotations
- **🗺️ AutoMapper** - Seamless entity-to-DTO mapping
- **📦 Repository Pattern + Unit of Work** - Testable and maintainable data layer
- **🔢 API Versioning** - Future-proof with V1 & V2 support
- **⚡ Caching** - Response caching and in-memory caching for performance
- **🔧 JSON Patch** - Efficient partial updates
- **🧪 Comprehensive Unit Testing** - XUnit and Moq with high code coverage

## 🛠️ Technology Stack

| Category | Technology | Version |
|----------|------------|---------|
| **Framework** | .NET | 8.0 |
| **Database** | SQL Server | 2019+ |
| **ORM** | Entity Framework Core | Latest |
| **Mapping** | AutoMapper | Latest |
| **Documentation** | Swagger/OpenAPI | 3.0 |
| **Versioning** | Microsoft.AspNetCore.Mvc.Versioning | Latest |
| **JSON Patch** | Microsoft.AspNetCore.JsonPatch | Latest |
| **Testing** | XUnit + Moq + FluentAssertions | Latest |
| **Test Data** | AutoFixture | Latest |

## 🏛️ Architecture Overview

```
BookShopAPI/
├── 🎮 Controllers/           # API Controllers (V1 & V2)
├── 🏢 Services/             # Business Logic Layer
├── 📦 Repositories/         # Data Access Layer
├── 🗄️ DataAccess/           # EF Core DbContext & Configurations
├── 📋 Models/
│   ├── Entities/            # Domain Models
│   └── DTOs/               # Data Transfer Objects
├── 🗺️ Mapping/             # AutoMapper Profiles
└── 🧪 Tests/               # Unit Tests (XUnit + Moq)
```

## 🧪 Testing Framework

### **Test Project Structure:**
```
APITask.Tests/
├── Controllers/
│   ├── CategoryControllerTests.cs          # Main test class
│   └── CategoryController_AdvancedTests.cs # Advanced scenarios
├── Helpers/
│   └── TestDataHelper.cs                   # Test data creation
├── Base/
│   └── ControllerTestBase.cs               # Base test class
└── APITask.Tests.csproj                    # Test project file
```

### 🎯 **Test Coverage:**

#### **CategoryController Tests Cover:**
1. **✅ GetCategories** - All scenarios (valid/invalid parameters, pagination, headers, exceptions)
2. **✅ GetCategory** - Valid ID, invalid ID, not found, exceptions
3. **✅ CreateCategory** - Valid creation, validation errors, conflicts, exceptions
4. **✅ UpdateCategory** - Valid updates, invalid ID, not found, conflicts
5. **✅ DeleteCategory** - Successful deletion, invalid ID, not found, business rules
6. **✅ Constructor Tests** - Null checks and proper initialization
7. **✅ Integration Scenarios** - Full CRUD workflow simulation

### 🛠️ **Testing Technologies:**
- **XUnit** - Test framework
- **Moq** - Mocking framework
- **FluentAssertions** - Readable assertions
- **AutoFixture** - Test data generation
- **Microsoft.AspNetCore.Mvc.Testing** - ASP.NET Core testing utilities

### 🔍 **Key Test Features:**

#### **Comprehensive Scenario Coverage:**
- ✅ Happy path scenarios
- ✅ Edge cases and boundary conditions
- ✅ Error handling and exceptions
- ✅ Model validation testing
- ✅ Service interaction verification
- ✅ HTTP response validation

#### **Advanced Testing Patterns:**
- ✅ **Theory tests** with multiple test cases
- ✅ **Mock verification** for service calls
- ✅ **HTTP context setup** for header testing
- ✅ **Custom test data helpers** for maintainability
- ✅ **Base test classes** for code reuse

### 📊 **Test Results Overview:**

The test suite includes **25+ individual test scenarios** covering:
- ✅ **6 GET scenarios** (valid, invalid, not found, exceptions)
- ✅ **4 CREATE scenarios** (valid, invalid model, conflicts, exceptions)  
- ✅ **5 UPDATE scenarios** (valid, invalid ID, not found, conflicts, exceptions)
- ✅ **4 DELETE scenarios** (valid, invalid ID, not found, business rules)
- ✅ **2 CONSTRUCTOR scenarios** (null check, valid initialization)
- ✅ **4+ ADVANCED scenarios** (workflows, parameter verification)

## 🗃️ Database Schema

The API manages two main entities with a one-to-many relationship:

### 📂 Categories Table (`MasterSchema.Categories`)
| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `Id` | int | Primary Key, Identity | Unique identifier |
| `CatName` | nvarchar(50) | Required, Indexed, Unique | Category name |
| `CatOrder` | int | Required | Display order |
| `IsDeleted` | bit | Default: false | Soft delete flag |
| `CreatedDate` | datetime2 | Computed | Auto-generated timestamp |

### 📚 Products Table (`MasterSchema.Products`)
| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `Id` | int | Primary Key, Identity | Unique identifier |
| `Title` | nvarchar(50) | Required | Book title |
| `Description` | nvarchar(250) | Optional | Book description |
| `Author` | nvarchar(50) | Required | Book author |
| `BookPrice` | decimal(18,2) | Range: 1-1000 | Book price |
| `CategoryId` | int | Foreign Key | Reference to Category |
| `IsDeleted` | bit | Default: false | Soft delete flag |

## ✨ Features

### 🔧 CRUD Operations
- ✅ Full CRUD for Categories and Products
- ✅ Model state validation with detailed error messages
- ✅ Comprehensive null checks and error handling
- ✅ Business rule validation

### 🔢 API Versioning
- **V1** (`/api/v1/product`) - Standard JSON responses
- **V2** (`/api/v2/product`) - Enhanced responses with additional metadata

### ⚡ Performance & Caching
- **Response Caching**: GET endpoints cached for 30 seconds
- **In-Memory Caching**: Category service with 30-minute expiration
- **Optimized Queries**: Efficient database queries with includes

### 🔧 Advanced Features
- **JSON Patch Support**: Partial updates using `JsonPatchDocument<T>`
- **Soft Delete**: Safe data removal with recovery capability
- **Pagination**: Efficient data loading with customizable page sizes
- **Global Query Filters**: Automatic filtering of deleted entities

## 🚀 API Endpoints

### 📂 Categories Management
| Method | Endpoint | Description | Cache |
|--------|----------|-------------|-------|
| `GET` | `/api/category` | Get paginated categories | ✅ 30s |
| `GET` | `/api/category/{id}` | Get category by ID | ✅ 30s |
| `POST` | `/api/category` | Create new category | ❌ |
| `PUT` | `/api/category/{id}` | Update category | ❌ |
| `DELETE` | `/api/category/{id}` | Soft delete category | ❌ |

### 📚 Products V1 - Standard API
| Method | Endpoint | Description | Cache |
|--------|----------|-------------|-------|
| `GET` | `/api/v1/product` | Get paginated products | ✅ 30s |
| `GET` | `/api/v1/product/{id}` | Get product by ID | ✅ 30s |
| `POST` | `/api/v1/product` | Create new product | ❌ |
| `PUT` | `/api/v1/product/{id}` | Update product | ❌ |
| `PATCH` | `/api/v1/product/{id}` | Partial update (JSON Patch) | ❌ |
| `DELETE` | `/api/v1/product/{id}` | Soft delete product | ❌ |
| `GET` | `/api/v1/product/category/{id}` | Get products by category | ✅ 30s |

### 📚 Products V2 - Enhanced API
Same endpoints as V1 but with enhanced response format including:
- ✨ Additional metadata
- 📊 Enhanced error details
- 🔍 Extended product information

## 🚀 Getting Started

### 📋 Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or Full)
- [Git](https://git-scm.com/) (for cloning)

### 📥 Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/BookShopAPI.git
   cd BookShopAPI
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database connection**
   
   Edit `appsettings.json` with your SQL Server connection:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=BookShopDB;Trusted_Connection=true;TrustServerCertificate=true;"
     }
   }
   ```

4. **Create and update database**
   ```bash
   dotnet ef database update
   ```

5. **Build and run**
   ```bash
   dotnet build
   dotnet run
   ```

6. **Access the API**
   - 🌐 **API Base URL**: `https://localhost:7015` or `http://localhost:5015`
   - 📖 **Swagger UI**: `https://localhost:7015/swagger`

### 🧪 Running Tests

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run tests with coverage (requires coverage tools)
dotnet test --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "CategoryControllerTests"

# Run specific test method
dotnet test --filter "GetCategories_WithValidParameters_ReturnsOkResultWithCategories"
```

### 🔧 Development Setup

For development with hot reload:
```bash
dotnet watch run
```

### 🗄️ Database Configuration

**Default Connection String:**
```
Server=localhost\SQLEXPRESS;Database=BookShopDB;Trusted_Connection=true;TrustServerCertificate=true;
```

**Alternative configurations:**
```json
// LocalDB
"Server=(localdb)\\mssqllocaldb;Database=BookShopDB;Trusted_Connection=true;"

// SQL Server with authentication
"Server=localhost;Database=BookShopDB;User Id=sa;Password=YourPassword;TrustServerCertificate=true;"
```

## 📊 Sample Data

The application automatically seeds the database with sample data:

### 📂 Categories (5 items)
- Fiction, Science, Technology, Biography, History

### 📚 Products (5 items)
- Books distributed across different categories
- Realistic pricing ($12.99 - $29.99)
- Complete author and description information

## 🛡️ API Testing

### 🌐 Browser Testing (GET requests)
```
# Get all categories
http://localhost:5015/api/category

# Get all products (V1)
http://localhost:5015/api/v1/product?page=1&pageSize=10

# Get all products (V2 - Enhanced)
http://localhost:5015/api/v2/product?page=1&pageSize=10
```

### 🔧 Swagger UI Testing
Access the interactive API documentation:
```
http://localhost:5015/swagger
```

### 📝 Sample Requests

**Create Category:**
```json
POST /api/category
{
  "catName": "Science Fiction",
  "catOrder": 10
}
```

**Create Product:**
```json
POST /api/v1/product
{
  "title": "Dune",
  "description": "Epic science fiction novel",
  "author": "Frank Herbert",
  "bookPrice": 24.99,
  "categoryId": 1
}
```

**JSON Patch Update:**
```json
PATCH /api/v1/product/1
[
  {
    "op": "replace",
    "path": "/bookPrice",
    "value": 19.99
  }
]
```

## ✅ Data Validation

### 📂 Category Validation
| Field | Rules | Error Message |
|-------|-------|---------------|
| `CatName` | Required, Max 50 chars, Unique | "Category name is required and must be unique" |
| `CatOrder` | Required, Integer | "Category order must be a valid number" |

### 📚 Product Validation  
| Field | Rules | Error Message |
|-------|-------|---------------|
| `Title` | Required, Max 50 chars | "Product title is required" |
| `Description` | Optional, Max 250 chars | "Description too long (max 250 characters)" |
| `Author` | Required, Max 50 chars | "Author name is required" |
| `BookPrice` | Required, Range 1-1000 | "Price must be between $1 and $1000" |
| `CategoryId` | Must exist | "Invalid category selected" |

## 🗑️ Soft Delete Implementation

Both Categories and Products implement soft delete functionality:

- ✅ **Safe Deletion**: Entities marked with `IsDeleted = true`
- ✅ **Data Preservation**: No permanent data loss
- ✅ **Automatic Filtering**: Global query filters hide deleted entities
- ✅ **Cascade Handling**: Deleting category soft-deletes related products
- ✅ **Recovery Possible**: Deleted entities can be restored

## 🚨 Error Handling

The API provides comprehensive error handling with appropriate HTTP status codes:

| Status Code | Scenario | Response Format |
|-------------|----------|-----------------|
| `200 OK` | Successful operation | Data with success message |
| `201 Created` | Resource created | Created resource with location |
| `400 Bad Request` | Validation errors | Detailed validation messages |
| `404 Not Found` | Resource not found | Error message with resource info |
| `409 Conflict` | Business rule violation | Conflict details |
| `500 Internal Server Error` | Server errors | Generic error message |

## 📈 Performance Features

### ⚡ Caching Strategy
- **Response Caching**: 30-second cache for GET endpoints
- **In-Memory Cache**: Categories cached for 30 minutes
- **Cache Headers**: Proper cache-control headers
- **Cache Invalidation**: Automatic cache clearing on updates

### 🗄️ Database Optimization
- **Indexed Columns**: CatName indexed for fast lookups
- **Efficient Queries**: Optimized EF Core queries
- **Lazy Loading**: Disabled for better performance
- **Connection Resilience**: Retry logic for transient failures

## 🏗️ Project Structure

```
BookShopAPI/
├── 📁 Controllers/
│   ├── CategoryController.cs
│   └── V1/
│   │   └── ProductV1Controller.cs
│   └── V2/
│       └── ProductV2Controller.cs
├── 📁 Services/
│   ├── ICategoryService.cs
│   ├── CategoryService.cs
│   ├── IProductService.cs
│   └── ProductService.cs
├── 📁 Repositories/
│   ├── IRepository.cs
│   ├── Repository.cs
│   ├── ICategoryRepository.cs
│   ├── CategoryRepository.cs
│   ├── IProductRepository.cs
│   ├── ProductRepository.cs
│   ├── IUnitOfWork.cs
│   └── UnitOfWork.cs
├── 📁 DataAccess/
│   ├── ApplicationDbContext.cs
│   └── Configurations/
│       ├── CategoryConfiguration.cs
│       └── ProductConfiguration.cs
├── 📁 Models/
│   ├── Entities/
│   │   ├── Category.cs
│   │   └── Product.cs
│   └── DTOs/
│       ├── CategoryDTO.cs
│       ├── CategoryCreateDTO.cs
│       ├── CategoryUpdateDTO.cs
│       ├── ProductDTO.cs
│       ├── ProductCreateDTO.cs
│       └── ProductUpdateDTO.cs
├── 📁 Mapping/
│   └── AutoMapperProfile.cs
├── 📁 Tests/                   # Unit Testing
│   ├── Controllers/
│   │   ├── CategoryControllerTests.cs
│   │   └── CategoryController_AdvancedTests.cs
│   ├── Helpers/
│   │   └── TestDataHelper.cs
│   ├── Base/
│   │   └── ControllerTestBase.cs
│   └── APITask.Tests.csproj
├── 📁 Migrations/
└── 📄 Program.cs
```

### 📏 Code Standards
- Follow **C# coding conventions**
- Use **meaningful variable names**
- Add **XML documentation** for public methods
- Include **unit tests** for new features
- Maintain **clean architecture** principles
- **High test coverage** with comprehensive scenarios

## 🎯 Quality Assurance

This project demonstrates **industry best practices** with:

- ✅ **Clean Architecture** - Proper separation of concerns
- ✅ **SOLID Principles** - Maintainable and extensible code
- ✅ **Comprehensive Testing** - High code coverage with quality tests
- ✅ **Error Handling** - Robust exception management
- ✅ **Performance Optimization** - Caching and efficient queries
- ✅ **Security** - Input validation and safe deletion
- ✅ **Documentation** - Clear API documentation with Swagger

This comprehensive test suite ensures **high code coverage** and **robust validation** of the CategoryController functionality, following **industry best practices** for unit testing in .NET applications.

---

⭐ **Star this repository** if you find it helpful!

📝 **Feedback** and contributions are always welcome!
