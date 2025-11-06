# Backend - Gestión de Videojuegos (Clean DDD Architecture)

## 📋 Descripción del Proyecto

Sistema backend completo para gestión de videojuegos implementado con **Clean Architecture** y **Domain-Driven Design (DDD)**. El proyecto utiliza .NET 8.0, NHibernate como ORM y LocalDB para persistencia.

## 🏗️ Arquitectura

```
PlantUML/
├── ApplicationCore/          # Capa de dominio (sin dependencias externas)
│   └── Domain/
│       ├── EN/              # Entidades del dominio (11 entidades)
│       ├── Enums/           # Enumeraciones (4 enums)
│       ├── CEN/             # Componentes de negocio (11 CENs)
│       ├── CP/              # Casos de uso transaccionales (10 CPs)
│       └── Repositories/    # Interfaces de repositorios
├── Infrastructure/          # Capa de infraestructura
│   ├── Repositories/        # Implementación de repositorios con NHibernate
│   ├── NHibernate/          # Configuración y mappings XML
│   └── UnitOfWork/          # Patrón Unit of Work
└── InitializeDb/            # Proyecto de inicialización y seed
```

## ✅ Requisitos Implementados

### 1. **Operaciones CRUD Completas en CEN** ✅
Todas las entidades cuentan con las 5 operaciones CRUD básicas:
- **New**: Crear nueva entidad
- **Modify**: Modificar entidad existente  
- **Destroy**: Eliminar entidad
- **ReadOID**: Obtener entidad por ID
- **ReadAll**: Obtener todas las entidades

**Entidades con CRUD completo:**
- ✅ Usuario (UsuarioCEN)
- ✅ Vendedor (VendedorCEN)
- ✅ Moderador (ModeradorCEN)
- ✅ Videojuego (VideojuegoCEN)
- ✅ Categoria (CategoriaCEN)
- ✅ Pedido (PedidoCEN)
- ✅ Transaccion (TransaccionCEN)
- ✅ Reporte (ReporteCEN)
- ✅ Soporte (SoporteCEN)
- ✅ Insignia (InsigniaCEN)
- ✅ Desafio (DesafioCEN)

### 2. **Método Login Implementado** ✅
Sistema de autenticación implementado para:
- ✅ **Usuario** (`UsuarioCEN.Login`)
- ✅ **Vendedor** (`VendedorCEN.Login`)
- ✅ **Moderador** (`ModeradorCEN.Login`)

Validación por correo y contraseña con manejo de excepciones.

### 3. **Filtros ReadFilter (Mínimo 4)** ✅
Implementados **7 filtros**:
1. ✅ `UsuarioCEN.ReadFilterByNombre` - Filtra usuarios por nombre
2. ✅ `UsuarioCEN.ReadFilterByCorreo` - Filtra usuarios por correo
3. ✅ `UsuarioCEN.ReadFilterByApodo` - Filtra usuarios por apodo
4. ✅ `VendedorCEN.ReadFilterByCorreo` - Filtra vendedores por correo
5. ✅ `ModeradorCEN.ReadFilterByCorreo` - Filtra moderadores por correo
6. ✅ `VideojuegoCEN.ReadFilterByPrecio` - Filtra videojuegos por rango de precio
7. ✅ `VideojuegoCEN.ReadFilterByCategoria` - Filtra videojuegos por categoría

### 4. **Operaciones Custom en CEN (Mínimo 3)** ✅
Implementadas **8 operaciones custom**:
1. ✅ `UsuarioCEN.ObtenerUsuariosPorInsignia` - Obtiene usuarios con una insignia específica
2. ✅ `UsuarioCEN.CambiarContraseña` - Cambio seguro de contraseña
3. ✅ `UsuarioCEN.ExisteCorreo` - Verifica existencia de correo
4. ✅ `VendedorCEN.ObtenerNumeroVideojuegosPublicados` - Cuenta videojuegos de un vendedor
5. ✅ `ModeradorCEN.ObtenerNumeroReportesGestionados` - Cuenta reportes gestionados
6. ✅ `ModeradorCEN.ObtenerNumeroSoportesGestionados` - Cuenta soportes gestionados  
7. ✅ `CategoriaCEN.ObtenerVideojuegoPorNombre` - Busca videojuego por nombre de categoría
8. ✅ `DesafioCEN.ObtenerPorVideojuego` - Obtiene desafíos de un videojuego

### 5. **CustomTransactions en CP (Mínimo 2)** ✅
Implementados **10 casos de uso transaccionales**:
1. ✅ `ConfirmarCompraCP` - Confirma compra y crea transacción
2. ✅ `RegistrarUsuarioCP` - Registro con validación de duplicados
3. ✅ `PublicarVideojuegoCP` - Publica videojuego con vendedor y categoría
4. ✅ `ComprarVideojuegosCP` - Compra múltiple de videojuegos
5. ✅ `ReportarUsuarioCP` - Crear reporte de usuario
6. ✅ `EnviarSolicitudSoporteCP` - Crear solicitud de soporte
7. ✅ `AsignarInsigniaCP` - Asignar insignia a usuario
8. ✅ `GestionarReporteCP` - Moderador gestiona reportes
9. ✅ `CrearDesafioVideojuegoCP` - Crear desafío para videojuegos
10. ✅ `FiltrarVideojuegosPorPrecioCP` - Filtrado avanzado con categoría

### 6. **InitializeDB con Validaciones** ✅
Implementado seed completo que valida:
- ✅ Creación de todas las entidades
- ✅ Operaciones CRUD (New, Modify, Destroy, ReadOID, ReadAll)
- ✅ Login de Usuario, Vendedor y Moderador
- ✅ **Todos los 7 filtros ReadFilter**
- ✅ **Todas las 8 operaciones Custom**
- ✅ **Todos los 10 CustomTransactions (CP)**

## 🗄️ Modelo de Datos

### Entidades Principales (11)
1. **Usuario** - Usuarios base del sistema
2. **Vendedor** - Hereda de Usuario, publica videojuegos
3. **Moderador** - Gestiona reportes y soportes
4. **Videojuego** - Productos con precio y categoría
5. **Categoria** - Clasificación de videojuegos
6. **Pedido** - Órdenes de compra
7. **Transaccion** - Pagos y cobros
8. **Reporte** - Denuncias de usuarios
9. **Soporte** - Solicitudes de ayuda técnica
10. **Insignia** - Logros y reconocimientos
11. **Desafio** - Retos en videojuegos

### Enumeraciones (4)
- `EstadoReporte` (SinSolucionar, Solucionado)
- `EstadoSoporte` (SinSolucionar, Solucionado)
- `TipoInsignia` (Perfil, Marco, Fondo, Icono)
- `TipoOperacion` (Cobro, Pago)

## 🚀 Ejecución del Proyecto

### Prerrequisitos
- .NET 8.0 SDK
- SQL Server LocalDB
- Visual Studio 2022 o VS Code

### Compilar
```bash
cd PlantUML
dotnet build Solution.sln
```

### Ejecutar InitializeDB
```bash
cd InitializeDb
dotnet run
```

Esto:
1. Elimina la base de datos existente
2. Crea nuevo esquema (17 tablas, 21 foreign keys)
3. Ejecuta seed con datos de prueba
4. Valida todas las operaciones implementadas

## 📊 Resultados de Ejecución

El InitializeDB ejecuta y valida:
- **26 pasos** de seed y validación
- **3 Logins** (Usuario, Vendedor, Moderador)
- **7 Filtros** ReadFilter
- **8 Operaciones** Custom
- **10 Transacciones** CustomTransaction (CPs)
- **Validación completa** de CRUD en todas las entidades

## 🔧 Tecnologías Utilizadas

- **.NET 8.0** - Framework base
- **C# 12** - Lenguaje de programación
- **NHibernate 5.6.0** - ORM con mappings XML
- **SQL Server LocalDB** - Base de datos
- **Microsoft.Extensions.DependencyInjection** - Inyección de dependencias
- **Clean Architecture** - Arquitectura del proyecto
- **DDD (Domain-Driven Design)** - Diseño del dominio

## 📝 Patrones Implementados

- **Repository Pattern** - Abstracción de persistencia
- **Unit of Work** - Gestión de transacciones
- **Dependency Injection** - Inversión de dependencias
- **CEN (Component Entity Business)** - Lógica de negocio
- **CP (Use Case / Custom Procedure)** - Casos de uso transaccionales
- **Value Objects** - Enumeraciones tipadas

## ✨ Características Destacadas

- ✅ **Validaciones robustas** en todas las operaciones
- ✅ **Transacciones ACID** con rollback automático
- ✅ **Separación de concerns** (EN, CEN, CP)
- ✅ **Herencia** (Vendedor hereda de Usuario)
- ✅ **Relaciones complejas** (many-to-many, one-to-many)
- ✅ **HiLo ID Generation** para performance
- ✅ **Lazy Loading** en relaciones
- ✅ **Documentación XML** en todos los métodos

## 👥 Autores

[Tu Nombre]  
Universidad de Alicante - Diseño y Desarrollo de Software Multiplataforma

## 📅 Fecha de Entrega

Noviembre 2025

---

**Nota**: Este proyecto cumple con **todos los requisitos** especificados en el enunciado de la práctica, incluyendo operaciones CRUD completas, Login, filtros ReadFilter (7), operaciones Custom (8), y CustomTransactions (10).
