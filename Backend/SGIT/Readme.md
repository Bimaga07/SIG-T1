# SIG-T: Sistema Integral de Gestión de Tareas

Este proyecto es una aplicación **Full Stack** desarrollada para el cierre del semillero IT de Excellentiam. 
El sistema permite gestionar la creación, actualización y auditoría de tareas, siguiendo estándares de arquitectura limpia y procesamiento asíncrono.

## 🚀 Tecnologías Utilizadas

* **Frontend:** Blazor WebAssembly (WASM) - .NET 8
* **Backend:** Minimal API - .NET 8
* **Procesamiento Asíncrono:** .NET Worker Service
* **Base de Datos:** SQL Server (Transact-SQL)
* **Acceso a Datos:** ADO.NET (Uso de Procedimientos Almacenados y Vistas)

## 🏗️ Arquitectura del Proyecto

El sistema está dividido en las siguientes capas:
- **SIGT.Domain:** Entidades de negocio, Interfaces y DTOs.
- **SIGT.Infrastructure:** Contexto de base de datos y repositorios (ADO.NET).
- **SIGT.API:** Endpoints RESTful y lógica de servidor.
- **SIGT.Worker:** Servicio en segundo plano para generación de reportes (Polling cada 30s).
- **SIGT.Frontend:** Interfaz de usuario interactiva con Blazor.

## 🛠️ Requisitos Funcionales Implementados

### 1. Base de Datos (SQL Server)
- **Modelado:** Tablas de `Usuarios`, `Tareas` y `RegistroDeActividad` con integridad referencial.
- **Seguridad:** Todas las operaciones de escritura (Create/Update) se realizan mediante **Procedimientos Almacenados**.
- **Auditoría:** Implementación de un **Trigger** `AFTER INSERT` en la tabla Tareas.
- **Optimización:** Uso de una **Vista** con `INNER JOIN` para el listado de tareas con usuarios.

### 2. Backend (API)
- **CRUD Completo:** Implementación de verbos HTTP (GET, POST, PUT, DELETE).
- **Asincronía:** Uso extensivo de `async/await` en todas las capas.
- **Tareas Lentas:** Endpoint `POST /api/reporte/tareas-finalizadas` que responde con **HTTP 202 Accepted**.

### 3. Frontend (Blazor)
- **Data Binding:** Formulario de tareas con `@bind` bidireccional.
- **Consumo de API:** Inyección de `HttpClient` para comunicación asíncrona.
- **UI/UX:** Componentes organizados para el listado y gestión de estados.

## ⚙️ Configuración y Ejecución

1. **Base de Datos:** Ejecutar el script ubicado en `/Database/SIGT_Setup.sql`.
2. **Visual Studio:** - Abrir el archivo `SIGT.slnx`.
   - Configurar **Inicio Múltiple** para los proyectos: API, Frontend y Worker.
3. **Ejecución:** Presionar `F5`.

## 📜 Documentación Adicional
Para detalles sobre el despliegue en ambientes de producción e IIS, consulte el archivo `DeploymentInstructions.txt` en la raíz del proyecto.

---
**Desarrollado como proyecto final para el Semillero IT.**