# Demokrata - UserManagementApi

Este proyecto es una API REST desarrollada en **.NET 8** que permite realizar operaciones **CRUD** sobre usuarios. Los usuarios tienen datos como nombres, apellidos, fecha de nacimiento, salario, y fechas de creación y modificación. La API incluye paginación, búsquedas dinámicas y documentación con **Swagger**.

## Requisitos

- **.NET 8 SDK** instalado.
- **Visual Studio 2022** o superior.
- **SQL Server** instalado localmente o accesible remotamente.
- **Postman** o navegador para probar los endpoints (opcional).

## Configuración Inicial del Proyecto

### 1. Clonar el repositorio

```bash
git clone https://github.com/felipeosouri/Demokrata.git
cd UserManagementApi
```

### 2. Cambiar la cadena de conexión
Modifica la cadena de conexión en el archivo appsettings.json para que apunte a tu servidor SQL Server:
```bash
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=UserDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```
Si usas un servidor remoto, cambia la cadena a:
```bash
"DefaultConnection": "Server=<tu-servidor>;Database=UserDB;User Id=<usuario>;Password=<contraseña>;"
```

### 3. Ejecutar migraciones para crear la base de datos
Abre la Consola del Administrador de Paquetes en Visual Studio y ejecuta:
```bash
Add-Migration InitialCreate
Update-Database
```

## Pruebas Unitarias
Las pruebas unitarias se encuentran en el proyecto UserManagementApi.Tests. A continuación, se describe cada prueba:

### Descripción de las Pruebas

| Prueba                                | Descripción                                                           |
|---------------------------------------|-----------------------------------------------------------------------|
| **GetAll_ReturnsUsers**               | Verifica que se devuelven todos los usuarios disponibles.             |
| **GetById_ReturnsUser_WhenUserExists** | Verifica que se obtiene el usuario correcto al buscar por su ID.      |
| **GetById_ReturnsNotFound_WhenUserDoesNotExist** | Verifica que se devuelve `NotFound` si el usuario no existe.          |
| **Create_ReturnsCreatedAtActionResult** | Verifica que se puede crear un usuario correctamente.                 |
| **Delete_ReturnsNoContent_WhenUserExists** | Verifica que se elimina un usuario existente correctamente.           |
| **Delete_ReturnsNotFound_WhenUserDoesNotExist** | Verifica que se devuelve `NotFound` si el usuario a eliminar no existe. |
