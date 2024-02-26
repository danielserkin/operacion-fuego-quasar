# Operación Fuego Quasar

**Ing. Daniel M. Serkin**  
**Documentación Técnica**

## Introducción

Este documento proporciona una descripción detallada de la API Operación Fuego Quasar, incluyendo su arquitectura, funcionalidades, implementación, pruebas y requisitos. La API Operación Fuego Quasar es una aplicación diseñada para resolver el desafío técnico de determinar la ubicación y el mensaje de una nave espacial en peligro a partir de la información recibida de varios satélites. A lo largo de este documento, exploraremos los componentes clave de la API, su diseño, las tecnologías utilizadas y los pasos necesarios para ejecutar, probar y desplegar la aplicación en diferentes entornos. Este documento servirá como una guía completa para comprender y trabajar con la API Operación Fuego Quasar.

## Tecnología Utilizada

- **Tecnología utilizada para la API:** La API está construida con .NET Core 7.0.
- **Dockerfile:** Se utilizó un Dockerfile para construir una imagen de contenedor Docker para la aplicación.
- **Versiones de .NET Core:** .NET Core SDK 7.0 y ASP.NET Core 7.0.
- **Utilización de contenedores Docker:** La aplicación se ejecuta dentro de un contenedor Docker.
- **Despliegue en Kubernetes:** La imagen de contenedor Docker se desplegó en un clúster de Kubernetes en Google Cloud Platform.
- **Acceso a la API:** La API se hace accesible a través de una dirección URL única proporcionada por Kubernetes.

## Arquitectura de la API

La arquitectura de la API Operación Fuego Quasar está basada principalmente en microservicios, aunque también se pueden identificar principios de Diseño Dirigido por Dominio (DDD).

### Capas de la Arquitectura

1. **Capa de Presentación**
2. **Capa de Aplicación**
3. **Capa de Dominio**
4. **Capa de Infraestructura**

## Estructura

### Api
- SatelliteController
- TopSecretController
- Program
- Application
- ShipService
- Exceptions
- Domain
  - Entities
- Infrastructure
  - SatelliteDataRepository
- ApplicationDbContext
  - Migrations

### Pruebas
- Api.Tests
- Application.Tests
- ShipServiceTests
- Infrastructure.Tests
- Integration Tests
  - TopSecretControllerIntegrationTests

### Base de Datos
- Configuración
- Modelo de Datos
- Operaciones
- Migraciones

## Documentación de la API

### Endpoints
- `/satelite/topsecret_split`
- `/topsecret`
- `/topsecret_split/{satelliteName}`

### URL DockerHub
- [Docker Hub](https://hub.docker.com/repository/docker/danielserkin/operacion-fuego-qasar)

### URL API Pública para pruebas en la nube
- [API Pública](http://34.133.116.5/index.html)

## Ejecución local

Para ejecutar localmente la API:

1. Inicia la aplicación desde Visual Studio.
2. Accede a Swagger en la URL base de la API + `/swagger`.
3. Explora la documentación de la API y envía solicitudes de prueba.

## Dockerización

Para utilizar el Dockerfile localmente:

1. Asegúrate de tener Docker instalado.
2. Coloca el Dockerfile en el directorio raíz del proyecto ASP.NET.
3. Abre una terminal en el directorio del Dockerfile.
4. Ejecuta el siguiente comando para construir la imagen Docker:

```bash
docker build -t nombre-de-tu-imagen .
```

¡Y eso es todo! Ahora deberías poder acceder a tu aplicación ASP.NET ejecutándose dentro de un contenedor Docker localmente en `http://localhost:8080`.
