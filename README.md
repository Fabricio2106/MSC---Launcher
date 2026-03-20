# Microservicios-Launcher

## Introducción

**Microservicios-Launcher** es una solución desarrollada en **.NET 10.0** que implementa una arquitectura de microservicios escalable. Este proyecto incluye múltiples microservicios independientes que se comunican entre sí, cada uno con responsabilidades específicas.

La solución utiliza:
- **Entity Framework Core** para el acceso a datos
- **Base de datos en memoria** para desarrollo sin dependencias de SQL Server
- **API REST** para la comunicación entre servicios
- **ASP.NET Core** para la construcción de los servicios web

## Microservicios Implementados

### 1. **Cliente** (`cliente/`)
Gestiona la información de clientes del sistema.

**Endpoints disponibles:**
- `GET /api/clientes` - Obtener todos los clientes
- `GET /api/clientes/{id}` - Obtener un cliente específico
- `POST /api/clientes` - Crear un nuevo cliente
- `PUT /api/clientes/{id}` - Actualizar un cliente
- `DELETE /api/clientes/{id}` - Eliminar un cliente

**Ejecución:**
```powershell
cd cliente
dotnet run
```
El servicio correrá en: `http://localhost:5000`

### 2. **Pedido** (`pedido/`)
Gestiona los pedidos realizados por los clientes.

**Datos del Pedido:**
- `id` - Identificador único
- `numero` - Número del pedido
- `fecha` - Fecha del pedido
- `idCliente` - Identificador del cliente asociado

**Endpoints disponibles:**
- `GET /api/pedidos` - Listar todos los pedidos
- `GET /api/pedidos/{id}` - Obtener un pedido específico
- `POST /api/pedidos` - Registrar un nuevo pedido
- `PUT /api/pedidos/{id}` - Actualizar un pedido
- `DELETE /api/pedidos/{id}` - Eliminar un pedido

**Características:**
- ✅ Validación de datos
- ✅ Validación de existencia antes de actualizar o eliminar
- ✅ Manejo de errores con códigos HTTP correctos (400, 404, 500)
- ✅ Logging de todas las operaciones

**Ejecución:**
```powershell
cd pedido
dotnet run
```
El servicio correrá en: `http://localhost:5001`

## Estructura del Proyecto

```
Microservicios-Launcher/
├── cliente/
│   ├── Controllers/
│   │   └── ClientesController.cs
│   ├── Data/
│   │   └── ClienteDbContext.cs
│   ├── Models/
│   │   └── Cliente.cs
│   ├── Properties/
│   ├── cliente.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── cliente.http
│
├── pedido/
│   ├── Controllers/
│   │   └── PedidosController.cs
│   ├── Data/
│   │   └── PedidoDbContext.cs
│   ├── Models/
│   │   └── Pedido.cs
│   ├── Properties/
│   ├── pedido.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── pedido.http
│
├── Microservicios-Launcher.sln
└── README.md
```

## Pasos para Crear un Microservicio

Si deseas crear un nuevo microservicio siguiendo la estructura de este proyecto, sigue estos pasos:

### 1. Crear la estructura de carpetas
```powershell
mkdir nombre_microservicio\Controllers
mkdir nombre_microservicio\Data
mkdir nombre_microservicio\Models
mkdir nombre_microservicio\Properties
```

### 2. Crear el modelo de datos
En `Models/MiModelo.cs`:
```csharp
namespace nombre_microservicio.Models
{
    public class MiModelo
    {
        public int Id { get; set; }
        // Agregar propiedades según sea necesario
    }
}
```

### 3. Crear el contexto de base de datos
En `Data/MiModeloDbContext.cs`:
```csharp
using Microsoft.EntityFrameworkCore;
using nombre_microservicio.Models;

namespace nombre_microservicio.Data
{
    public class MiModeloDbContext : DbContext
    {
        public MiModeloDbContext(DbContextOptions<MiModeloDbContext> options) : base(options)
        {
        }

        public DbSet<MiModelo> MiModelos { get; set; }
    }
}
```

### 4. Crear el controlador
En `Controllers/MiModeloController.cs` con los endpoints CRUD.

### 5. Configurar Program.cs
```csharp
using nombre_microservicio.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MiModeloDbContext>(options =>
    options.UseInMemoryDatabase("MiModeloDb"));

builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();
```

### 6. Crear archivos de configuración
- `appsettings.json`
- `appsettings.Development.json`
- `Properties/launchSettings.json`

### 7. Crear archivo .csproj
Con las dependencias necesarias de Entity Framework Core.

### 8. Agregar a la solución
Actualizar `Microservicios-Launcher.sln` con la nueva referencia del proyecto.

## Probar los Endpoints

Cada microservicio incluye un archivo `.http` que contiene ejemplos de las peticiones disponibles. Puedes usar la extensión **REST Client** de VS Code para probar los endpoints directamente.

**Ejemplo para el microservicio de Pedidos:**
```http
### GET todos los pedidos
GET http://localhost:5001/api/pedidos

### POST crear un nuevo pedido
POST http://localhost:5001/api/pedidos
Content-Type: application/json

{
  "numero": "PED-001",
  "fecha": "2026-03-19",
  "idCliente": 1
}
```

## Requisitos Previos

- .NET 10.0 SDK instalado
- Visual Studio Code (recomendado) o Visual Studio
- Extensión **REST Client** para VS Code (opcional, para pruebas)

## Instalación y Ejecución

### Ejecutar la solución completa

Desde la raíz del proyecto:
```powershell
# Abrir la solución
dotnet sln list

# Para ejecutar un microservicio específico
cd cliente
dotnet run
```

O en otra terminal:
```powershell
cd pedido
dotnet run
```

Cada microservicio se ejecutará en su propio puerto:
- Cliente: `http://localhost:5000`
- Pedido: `http://localhost:5001`

---

## ⚠️ Importante - Submodulos Git

> **Si se trabaja en el repositorio que tiene los sub-módulos, primero actualizar y hacer push en el sub-módulo y después en el repositorio principal.**
>
> **Si se hace al revés, se perderán las referencias de los sub-módulos en el repositorio principal y tendremos que resolver conflictos.**

### Workflow correcto con submodulos:

1. Realiza cambios en el sub-módulo
2. Haz commit y push en el sub-módulo
3. Actualiza la referencia en el repositorio principal
4. Haz commit y push en el repositorio principal

---

## Notas de Desarrollo

- Todos los microservicios utilizan base de datos **en memoria** para facilitar el desarrollo sin dependencias externas.
- Cada microservicio es **independiente** y puede escalarse de forma individual.
- Los errores están correctamente manejados con códigos HTTP estándar.
- Se incluye **logging** para todas las operaciones críticas.

## Autor

Desarrollado como parte del Ciclo V de Microservicios.