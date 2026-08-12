# Prueba Técnica — Gestión de Clientes · Transfors S.A.S.

Aplicación **CRUD de clientes** con arquitectura **Frontend → Backend/API → Base de datos**, construida con el stack de la empresa: **Angular + .NET 8 (C#) + SQL Server**.


---

## 1. Stack y justificación

| Capa | Tecnología | Por qué |
|------|-----------|---------|
| **Frontend** | Angular 20 (standalone components, signals, Reactive Forms) | Es el framework de la empresa. Uso componentes *standalone* y *signals* (enfoque actual de Angular) y **Reactive Forms** para validación robusta y tipada. |
| **Backend** | .NET 8 Web API (C#) | LTS de la empresa. API REST con controllers, DTOs, capa de servicios e inyección de dependencias. |
| **ORM / Datos** | EF Core 8 **Code-First** + **Stored Procedure** | Code-First permite versionar el esquema con **migraciones**. Se incluye además un **procedimiento almacenado** para el listado/búsqueda, demostrando el manejo de SQL Server que pide la oferta. |
| **Base de datos** | SQL Server 2022 (Docker) | Reproducible en cualquier máquina sin instalar SQL Server localmente. |
| **Documentación API** | Swagger / OpenAPI | Permite probar y explorar la API sin herramientas externas. |

### Decisiones de arquitectura

- **Separación en capas dentro de la API**: `Domain` (entidad), `Data` (DbContext/EF), `Dtos` (contratos de entrada/salida), `Services` (lógica de negocio), `Controllers` (HTTP). Es una separación **pragmática y proporcional** al tamaño del problema: suficiente para demostrar orden y testabilidad, sin sobre-ingeniería (no se añade Clean Architecture completa para un CRUD).
- **DTOs en vez de exponer la entidad**: el request se valida con *Data Annotations* y el response añade descripciones legibles de los enums. Evita acoplar el contrato HTTP al modelo de base de datos.
- **Enums de dominio** (`TipoDocumento`, `Genero`) en lugar de strings sueltos: consistencia y validación. Se serializan como texto en el JSON para legibilidad.
- **Regla de negocio de unicidad**: no se permiten dos clientes con el mismo `TipoDocumento + NumeroDocumento` (validado en servicio y reforzado con **índice único** en BD → doble barrera).
- **Manejo centralizado de errores** (middleware) que traduce excepciones a respuestas `ProblemDetails` consistentes: `409` para conflictos de negocio, `400` para validación, `500` controlado.
- **Campos de auditoría** añadidos (`FechaCreacion`, `FechaModificacion`) — la prueba permite agregar campos; aportan trazabilidad.

---

## 2. Modelo de datos

Tabla `dbo.Clientes` (los campos mínimos de la prueba + auditoría):

| Campo | Tipo SQL | Notas |
|-------|----------|-------|
| Id | int IDENTITY | PK |
| TipoDocumento | int | enum |
| NumeroDocumento | nvarchar(20) | único junto con TipoDocumento |
| Nombres | nvarchar(100) | |
| Apellidos | nvarchar(100) | |
| FechaNacimiento | date | |
| Genero | int | enum |
| Telefono | nvarchar(20) | |
| CorreoElectronico | nvarchar(150) | indexado |
| Direccion | nvarchar(200) | |
| Ciudad | nvarchar(100) | |
| Estado | bit | Activo/Inactivo |
| FechaCreacion | datetime2 | default `SYSUTCDATETIME()` |
| FechaModificacion | datetime2 null | |

**Índices**: único en `(TipoDocumento, NumeroDocumento)`, e índice en `CorreoElectronico`.

**Stored Procedure** `dbo.usp_Clientes_Listar(@Search, @Estado)`: filtra por texto (nombres, apellidos, documento, correo, ciudad) y por estado. Creado y versionado dentro de la migración inicial de EF Core.

---

## 3. API REST

Base: `http://localhost:5080/api/clientes`

| Método | Ruta | Descripción | Respuestas |
|--------|------|-------------|-----------|
| GET | `/api/clientes?search=&estado=` | Lista/busca (vía SP) | `200` |
| GET | `/api/clientes/{id}` | Obtener por id | `200`, `404` |
| POST | `/api/clientes` | Crear | `201`, `400`, `409` |
| PUT | `/api/clientes/{id}` | Actualizar | `200`, `400`, `404`, `409` |
| DELETE | `/api/clientes/{id}` | Eliminar | `204`, `404` |

Swagger disponible en `http://localhost:5080/swagger` (entorno Development).

---

## 4. Cómo ejecutar

### Requisitos
- Docker · .NET 8 SDK · Node.js 20+ (probado con 24)

### 4.1 Base de datos (SQL Server en Docker)
```bash
docker compose up -d
```

### 4.2 Backend (API)
```bash
cd backend/src/Transfors.Clientes.Api
dotnet run
```
La API **aplica las migraciones automáticamente** al arrancar (crea la tabla y el stored procedure). Queda escuchando en `http://localhost:5080`.

> La cadena de conexión está en `appsettings.json`. En producción se movería a *user-secrets* o variables de entorno; se deja en el archivo para facilitar la evaluación local.

### 4.3 Frontend (Angular)
```bash
cd frontend
npm install
npm start        # ng serve → http://localhost:4200
```

---

## 5. Estructura del repositorio

```
.
├── docker-compose.yml              # SQL Server 2022
├── global.json                     # fija SDK .NET 8
├── backend/
│   └── src/Transfors.Clientes.Api/
│       ├── Domain/                 # Cliente, enums
│       ├── Data/                   # AppDbContext (EF Core)
│       ├── Dtos/                   # ClienteRequest / ClienteResponse + validaciones
│       ├── Services/               # IClienteService, ClienteService, mapeo
│       ├── Controllers/            # ClientesController (REST)
│       ├── Middleware/             # Manejo global de errores
│       └── Migrations/             # Migración inicial + stored procedure
└── frontend/
    └── src/app/
        ├── models/                 # interfaces y enums alineados con la API
        ├── services/               # ClienteService (HttpClient)
        └── features/clientes/
            ├── clientes-list/      # listado, búsqueda, filtro, eliminar
            └── cliente-form/       # crear / editar (Reactive Forms)
```

---

## 6. Qué se validó funcionando

- CRUD completo end-to-end (Angular → API → SQL Server).
- Listado, **búsqueda** y **filtro por estado** ejecutando el **stored procedure**.
- Validaciones de formulario en frontend **y** backend (defensa en profundidad).
- Documento duplicado → `409 Conflict` con mensaje claro.
- Manejo de errores y CORS configurado para el frontend.

---

## 7. Posibles mejoras (siguiente iteración)

- Autenticación/autorización (JWT) y roles.
- Paginación y ordenamiento en el listado.
- Pruebas automatizadas (xUnit en backend, Jasmine/Karma o Vitest en frontend).
- CI/CD y despliegue con contenedores para API y frontend.
- Borrado lógico (soft-delete) usando el campo `Estado` en vez de borrado físico, según la regla de negocio real.
