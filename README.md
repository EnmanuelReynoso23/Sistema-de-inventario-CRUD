# Sistema de Inventario CRUD - PostgreSQL & C#

<div align="center">

![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Windows Forms](https://img.shields.io/badge/Windows%20Forms-0078D6?style=for-the-badge&logo=windows&logoColor=white)

[![Made with C#](https://img.shields.io/badge/Made%20with-C%23-239120?style=flat&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Database](https://img.shields.io/badge/Database-PostgreSQL-336791?style=flat&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Framework](https://img.shields.io/badge/Framework-.NET%208-512BD4?style=flat&logo=.net&logoColor=white)](https://dotnet.microsoft.com/)

</div>

Sistema de gestión de inventario desarrollado con **Windows Forms (.NET 8)** y **PostgreSQL** que permite realizar operaciones CRUD completas sobre productos comerciales.

## Descripción del Proyecto

Aplicación de escritorio que simula un sistema de punto de venta para gestión de inventario, implementando las mejores prácticas en diseño de bases de datos relacionales y desarrollo de interfaces gráficas en C#.

### Características Principales

- **Operaciones CRUD completas**: Crear, leer, actualizar y eliminar productos
- **Interfaz intuitiva**: Windows Forms con validación de datos en tiempo real
- **Base de datos robusta**: PostgreSQL con restricciones y esquemas bien definidos
- **Manejo de errores**: Gestión completa de excepciones y validaciones
- **Arquitectura escalable**: Separación de capas y buenas prácticas de programación

## Tecnologías Utilizadas

<div align="center">

![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=for-the-badge&logo=visual-studio&logoColor=white)
![Windows](https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white)

</div>

| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| ![PostgreSQL](https://img.shields.io/badge/-PostgreSQL-336791?style=flat&logo=postgresql&logoColor=white) PostgreSQL | 16+ | Base de datos relacional |
| ![C#](https://img.shields.io/badge/-C%23-239120?style=flat&logo=c-sharp&logoColor=white) C# | .NET 8 | Lenguaje de programación principal |
| ![Windows Forms](https://img.shields.io/badge/-Windows%20Forms-0078D6?style=flat&logo=windows&logoColor=white) Windows Forms | .NET 8 | Framework de interfaz gráfica |
| ![Npgsql](https://img.shields.io/badge/-Npgsql-336791?style=flat&logo=nuget&logoColor=white) Npgsql | 8.0+ | Driver de conexión PostgreSQL |
| ![Visual Studio](https://img.shields.io/badge/-Visual%20Studio-5C2D91?style=flat&logo=visual-studio&logoColor=white) Visual Studio | 2022 | IDE de desarrollo |

## Requisitos del Sistema

<div align="center">

![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16+-336791?style=flat&logo=postgresql&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-2022-5C2D91?style=flat&logo=visual-studio&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=.net&logoColor=white)
![Windows](https://img.shields.io/badge/Windows-10/11-0078D6?style=flat&logo=windows&logoColor=white)

</div>

### Software Necesario

| Software | Versión | Link de Descarga |
|----------|---------|-----------------|
| ![PostgreSQL](https://img.shields.io/badge/-PostgreSQL-336791?style=flat&logo=postgresql&logoColor=white) | 16+ | [Descargar PostgreSQL](https://www.postgresql.org/download/) |
| ![Visual Studio](https://img.shields.io/badge/-Visual%20Studio-5C2D91?style=flat&logo=visual-studio&logoColor=white) | 2022 | [Descargar VS 2022](https://visualstudio.microsoft.com/) |
| ![.NET](https://img.shields.io/badge/-.NET%20SDK-512BD4?style=flat&logo=.net&logoColor=white) | 8.0 | [Descargar .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0) |
| ![pgAdmin](https://img.shields.io/badge/-pgAdmin-336791?style=flat&logo=postgresql&logoColor=white) | 4+ | [Descargar pgAdmin](https://www.pgadmin.org/download/) |

### Dependencias NuGet

<div align="center">

![Npgsql](https://img.shields.io/badge/Npgsql-8.0+-336791?style=for-the-badge&logo=nuget&logoColor=white)

</div>

```xml
<PackageReference Include="Npgsql" Version="8.0.0" />
```

## Estructura de la Base de Datos

### Esquema: `inventario`

```sql
-- Tabla productos
CREATE TABLE inventario.productos (
    id_producto SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    precio NUMERIC(10,2) CHECK (precio > 0),
    stock INTEGER DEFAULT 0,
    fecha_registro DATE DEFAULT CURRENT_DATE
);
```

### Campos de la Tabla

| Campo | Tipo | Restricciones | Descripción |
|-------|------|---------------|-------------|
| `id_producto` | SERIAL | PRIMARY KEY | Identificador único autoincrementable |
| `nombre` | VARCHAR(100) | NOT NULL | Nombre del producto |
| `precio` | NUMERIC(10,2) | CHECK (> 0) | Precio unitario |
| `stock` | INTEGER | DEFAULT 0 | Cantidad disponible |
| `fecha_registro` | DATE | DEFAULT TODAY | Fecha de registro automática |

## Instalación y Configuración

### 1. Configurar Base de Datos

<div align="center">

![PostgreSQL](https://img.shields.io/badge/Configurar-PostgreSQL-336791?style=for-the-badge&logo=postgresql&logoColor=white)

</div>

```bash
# Crear base de datos
createdb inventario_db

# Ejecutar script de inicialización
psql -d inventario_db -f scripts/proyecto_crud_productos.sql
```

### 2. Configurar Aplicación C#

<div align="center">

![C#](https://img.shields.io/badge/Configurar-C%23%20App-239120?style=for-the-badge&logo=c-sharp&logoColor=white)

</div>

```bash
# Clonar repositorio
git clone https://github.com/tu-usuario/sistema-inventario-crud.git

# Restaurar paquetes NuGet
dotnet restore

# Configurar cadena de conexión en App.config
```

### 3. Cadena de Conexión

<div align="center">

![Config](https://img.shields.io/badge/Configurar-Conexión-FF6B6B?style=for-the-badge&logo=gear&logoColor=white)

</div>

```xml
<connectionStrings>
    <add name="PostgreSQLConnection" 
         connectionString="Host=localhost;Database=inventario_db;Username=tu_usuario;Password=tu_password" />
</connectionStrings>
```

## Uso de la Aplicación

### Interfaz Principal

La aplicación cuenta con:

- **Formulario de datos**: Campos para ID, nombre, precio y stock
- **Botones de acción**: Crear, Actualizar, Eliminar, Limpiar
- **DataGridView**: Visualización completa de productos
- **Validaciones**: Control de entrada de datos en tiempo real

### Operaciones Disponibles

1. **Crear Producto**: Agregar nuevos productos al inventario
2. **Actualizar Producto**: Modificar información existente
3. **Eliminar Producto**: Remover productos del sistema
4. **Consultar Productos**: Visualizar inventario completo

## Estructura del Proyecto

```
SistemaInventarioCRUD/
├── src/
│   ├── Forms/
│   │   ├── MainForm.cs
│   │   └── MainForm.Designer.cs
│   ├── Data/
│   │   ├── DatabaseConnection.cs
│   │   └── ProductRepository.cs
│   ├── Models/
│   │   └── Product.cs
│   └── Utils/
│       └── Validator.cs
├── scripts/
│   └── proyecto_crud_productos.sql
├── docs/
│   └── informe_tecnico.md
└── README.md
```

## Características Técnicas

### Validaciones Implementadas

<div align="center">

![Validation](https://img.shields.io/badge/Validación-Completa-28A745?style=for-the-badge&logo=check-circle&logoColor=white)
![Error Handling](https://img.shields.io/badge/Manejo%20de-Errores-DC3545?style=for-the-badge&logo=exclamation-triangle&logoColor=white)

</div>

- **Precio**: Debe ser mayor a 0 y formato numérico válido
- **Stock**: Valores enteros no negativos
- **Nombre**: Campo obligatorio, longitud máxima 100 caracteres
- **Conexión BD**: Manejo de errores de conectividad

### Manejo de Excepciones

<div align="center">

![Exception](https://img.shields.io/badge/Exception-Handling-FD7E14?style=flat&logo=bug&logoColor=white)
![Transaction](https://img.shields.io/badge/Safe-Transactions-20C997?style=flat&logo=shield&logoColor=white)
![Rollback](https://img.shields.io/badge/Auto-Rollback-E83E8C?style=flat&logo=undo&logoColor=white)

</div>

- Errores de conexión a base de datos
- Violaciones de restricciones SQL
- Validación de tipos de datos
- Transacciones seguras con rollback

## Casos de Uso

### Escenario Típico: Tienda de Retail

1. **Alta de productos**: Registro de nuevos artículos
2. **Actualización de precios**: Modificación masiva de precios
3. **Control de stock**: Seguimiento de inventario disponible
4. **Consultas rápidas**: Búsqueda y filtrado de productos

## Testing y Validación

### Datos de Prueba

```sql
-- Productos de ejemplo
INSERT INTO inventario.productos (nombre, precio, stock) VALUES
('Laptop Dell XPS 13', 1299.99, 15),
('Mouse Inalámbrico', 29.99, 50),
('Teclado Mecánico', 89.99, 25);
```

## Objetivos de Aprendizaje Alcanzados

### Competencias Técnicas

<div align="center">

![Database](https://img.shields.io/badge/%20Diseño%20BD-Relacionales-336791?style=flat&logo=postgresql&logoColor=white)
![CRUD](https://img.shields.io/badge/%20Operaciones-CRUD-239120?style=flat&logo=c-sharp&logoColor=white)
![UI](https://img.shields.io/badge/%20Interfaces-Windows%20Forms-0078D6?style=flat&logo=windows&logoColor=white)
![Integration](https://img.shields.io/badge/%20Integración-Npgsql-FF6B6B?style=flat&logo=nuget&logoColor=white)
![Validation](https://img.shields.io/badge/%20Validación-Datos-28A745?style=flat&logo=check-circle&logoColor=white)

</div>

- Diseño e implementación de BD relacionales
- Operaciones CRUD con PostgreSQL y C#
- Desarrollo de interfaces Windows Forms
- Integración con Npgsql y manejo de conexiones
- Validación de datos y manejo de excepciones

### Competencias Transversales

<div align="center">

![Problem Solving](https://img.shields.io/badge/%20Resolución-Problemas-6F42C1?style=flat&logo=puzzle-piece&logoColor=white)
![Documentation](https://img.shields.io/badge/%20Documentación-Técnica-17A2B8?style=flat&logo=file-text&logoColor=white)
![Autonomous](https://img.shields.io/badge/%20Trabajo-Autónomo-FD7E14?style=flat&logo=user&logoColor=white)
![Best Practices](https://img.shields.io/badge/%20Buenas-Prácticas-20C997?style=flat&logo=star&logoColor=white)

</div>

- Resolución de problemas técnicos
- Documentación técnica clara
- Trabajo autónomo y planificación
- Aplicación de buenas prácticas de desarrollo




