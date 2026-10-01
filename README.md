# API de Inventario y Stock

API REST para gestionar el inventario de un negocio: productos, categorías, proveedores y movimientos de stock, con alertas automáticas cuando un producto llega a su stock mínimo.

**Autor:** Linares y Gagliardo
**Materia:** Desarrollo de sistemas

## Tecnologias

- C# y .NET 8
- ASP.NET Core Web API
- Entity Framework Core (Code First con migraciones)
- SQL Server (LocalDB)
- Swagger para probar la API

## Funcionalidades

- CRUD de categorías, proveedores y productos.
- Registro de movimientos de stock (entradas y salidas) con historial.
- Alertas automáticas al llegar al stock mínimo.
- Validaciones: SKU y CUIT únicos, stock no negativo, no se puede sacar más stock del disponible.
- Baja lógica de productos (se desactivan, no se borran).

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server LocalDB (o cualquier SQL Server; ajustar la cadena de conexión)
- Herramienta de migraciones: `dotnet tool install --global dotnet-ef`

## Como ejecutarlo

1. Clonar el repositorio y entrar a la carpeta del proyecto:

   ```
   git clone <URL-del-repositorio>
   cd <carpeta-del-proyecto>
   ```

2. (Opcional) Ajustar la cadena de conexión en `appsettings.json` → `ConnectionStrings:DefaultConnection`.
   Si no se define, usa por defecto `(localdb)\mssqllocaldb`.

3. Crear la base de datos aplicando las migraciones:

   ```
   dotnet ef database update
   ```

4. Ejecutar la API:

   ```
   dotnet run
   ```

5. Abrir Swagger en el navegador: `http://localhost:<puerto>/swagger`
   (el puerto aparece en la consola: `Now listening on: ...`).

## Endpoints principales

| Método | Ruta | Descripción |
|---|---|---|
| GET / POST | `/api/Categorias` | Listar / crear categorías |
| GET / PUT / DELETE | `/api/Categorias/{id}` | Ver / editar / eliminar |
| GET / POST | `/api/Proveedores` | Listar / crear proveedores |
| GET / PUT / DELETE | `/api/Proveedores/{id}` | Ver / editar / eliminar |
| GET / POST | `/api/Productos` | Listar / crear productos |
| GET / PUT / DELETE | `/api/Productos/{id}` | Ver / editar / desactivar |
| GET / POST | `/api/Movimientos` | Historial / registrar entrada o salida |
| GET | `/api/Alertas` | Listar alertas (`?soloNoLeidas=true`) |
| PUT | `/api/Alertas/{id}/leer` | Marcar alerta como leída |

## Como probarlo 

1. Crear una categoría (`POST /api/Categorias`).
2. Crear un proveedor (`POST /api/Proveedores`).
3. Crear un producto con `categoriaId` y `proveedorId` existentes (`POST /api/Productos`).
4. Registrar una salida (`POST /api/Movimientos`) con `"tipo": "Salida"`.
5. Consultar las alertas (`GET /api/Alertas`).

## Estructura

```
Controllers/   Endpoints de la API
Data/          AppDbContext (acceso a la base de datos)
Models/        Entidades (tablas)
Services/      Lógica de negocio del stock y las alertas
Migrations/    Historial de la base de datos
Program.cs     Configuración y arranque
```

