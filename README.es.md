[![en](https://img.shields.io/badge/lang-en-red.svg):black_large_square:](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/README.md) [![es](https://img.shields.io/badge/lang-es-yellow.svg):ballot_box_with_check:]()

# net6-ddd-advanced-example
Ejemplo NET 6 con arquitectura DDD y algunas funciones avanzadas.

## Table of Contents
- [Iniciando](#iniciando)
- [Lista de características](#lista-de-caracter%C3%ADsticas)
- [Estructura de Carpetas](#folder-structure)
  - [1- Entrypoint](#1--entrypoint)
  - [2- Core](#2--core)
  - [3- Infrastructure](#3--infrastructure)
- [Licencia](#licencia)

## Iniciando

* Descargar este repositorio
* Es requerido Visual Studio IDE :point_right: [download](https://visualstudio.microsoft.com/es/downloads)
* Es requerido SDK 6 :point_right: [download](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* Es requerido MySQL :point_right: [download](https://dev.mysql.com/downloads/installer/)
Instalar y seleccionar estas características:
  * Connector .net
  * MySQL Server
  * Worbench	
* Ingresamos a la terminal desde el Visual Studio IDE

![img_powershell_vs](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/getting_started_1.png?raw=true)

* Nos paramos en la carpeta "src" haciendo: 
    * cd src
* Ejecutamos los siguientes comandos:
  * dotnet tool install --global dotnet-ef
  * dotnet ef database update -p ./AuthorizationOperation.Infrastructure -- "server=localhost;port=3306;user=root;password=___[PASSWORD]___;database=authorization_db"
    * (reemplazar las credenciales)

* Postman:
  * Examples [click here](https://github.com/SpaikSaucus/net46-ddd-advanced-example/blob/main/postman/Net6Advance.postman_collection.json)
  
* dale play y disfruta!

## Lista de características
- [Architecture DDD (Domain Driven Design)](#architecture-ddd)
- [Api Versions](#api-versions)
- [JWT (JSON Web Tokens)](#jwt-bearer-authentication)
- [Oas3 (OpenAPI Specification - Version 3)](#oas3)
- [Swagger](#swagger)
- [MediatR](#mediatr)
- [Health Check](#health-check)
- [Logs](#logs)
- [EF (Entity Framework)](#ef-mysql)
- [Unit of Work Pattern](#unit-of-work)
- [CQRS Pattern (Command and Query Responsibility Segregation)](#cqrs-pattern)
- [Query Specification Pattern](#query-specification-pattern)
- [Multiple Environments File](#multiple-environments)
- [Unit Test](#unit-test)
- [Integrations Test](#integration-test)

## Estructura de carpetas
En este apartado explicaremos la estructura del proyecto, como están diseñadas las capas, que función cumple cada una. También haremos mención sobre algunas carpetas importantes.

### 1- ENTRYPOINT
Web Api, Listeners de queues, workers de un job, etc. No debe implementar reglas de negocio. 

  * #### API
    Recibe los request y se los delega a [MediatR](https://github.com/jbogard/MediatR). Se implementa el uso de Mediatr para el desacople entre capas y asi implementar una arquitectura CQRS entre lo transaccional y consultivo. 

    * __Controllers__: contendrá las distintas versiones de los controladores. Para mas información sobre API Version ir [aquí](#api-versions).
    * __ViewModels__: contendrá las clases Request y Response, necesarias para la interacción.

### 2- CORE
Principal objetivo, implementar lógica de negocio y casos de uso. Debemos velar para que no existan referencias a Frameworks asociados a Infraestructura (ejemplo: frameworks de accesos a datos).

  * #### APPLICATION
    Implementaremos el flujo para desarrollar una funcionalidad especifica de mi aplicación. Orquestara entre los diferentes Dominios.

    * __Behaviors__: contendrá comportamiento reutilizable por MediatR en cada interacción de la API con la capa de APPLICATION. 
    Ejemplos:
      * LoggingBehavior: Se logueara cada request y response que procese MediatR. 
      * ValidatorBehavior: Ejecuta los Validators que estén asociados a un MediatR.

    * __UserCases__: contendrá los Casos de Uso que se definan para cumplir con los requerimientos solicitados. Un UserCase puede hacer uso de otro UserCase, para reutilizar código (ejemplo: revisar el UserCase de CreateAuthorizationCommand).
      * ___Commands___: contendrá las implementaciones transaccionales.
      * ___Queries___: contendrá las implementaciones consultivas.
      * ___Validations___: contendrá la lógica de validación a los datos de entrada.
      * ___DTOs___: contendrá las clases DTO necesarias para transferir información entre capas, cuando sea necesario.

  * #### DOMAIN
    Definiremos las reglas de negocio (en términos de DDD, también llamadas Dominio y expresadas a traves de entidades, servicios de Dominio, value objects, interfaces).

    * __Core__: contiene clases Base e Interfaces que serán necesarias para implementar patrones y features críticos. 

### 3- INFRASTRUCTURE
Aquí encontraremos implementaciones concretas para acceso a datos, ORMs, MicroORMS, Request HTTP, Manejo de archivos, etc.

  * #### INFRASTRUCTURE

    * __Core__: contiene algunas de las implementaciones de las Interfaces definidas en la capa de DOMAIN, asociadas a resolver problemas de Infraestructura.
    * __EF__: contiene el DBContext y las clases necesarias para implementar Entity Framework, para el acceso a la base de datos.
      * ___Config___: contiene las clases que representan cada tabla de la base de datos, donde se permitirá configurar como EF construirá el modelo de la base de datos. Mas información [aquí](https://learn.microsoft.com/es-es/ef/core/modeling)
      * ___Migrations___: contiene las migraciones realizadas al modificar la base de datos.
    * __Services__: contiene implementaciones concretas, por ejemplo, generar un Reporte en formato Excel. Las misas deberá configurarse en el AutofacModules para luego poder utilizarse mediante la inyección de IComponentContext.

  * #### INFRASTRUCTURE.BOOSTRAP

## Architecture DDD
Cuando trabajamos con DDD hay tres partes que debemos tener en cuenta:
  * Separación de responsabilidades en capas, aislar el dominio
  * Modelar y definir el modelo.
  * Gestionar el ciclo de vida de los objetos de Dominio.

## Api Versions
---
El paquete API Versioning nos permite marcar las API como obsoletas. Entonces esto le da tiempo al cliente para preparar los cambios. De lo contrario, eliminar inmediatamente las API más antiguas podría generar problemas a los clientes.

## JWT bearer authentication
---
1. Crear el token:
```bash
curl --location 'https://localhost:5001/api/v2/login' \
--header 'Content-Type: application/json' \
--data '{
  "username": "admin",
  "password": "admin"
}'
```
2. Usar el token: (_reemplazar XXXX con el token generado_)
```bash
curl --location --request GET 'http://localhost:5000/api/v2/authorizations/findAllByCriteria?sort=customer,asc;id,desc&offset=0&limit=200' \
--header 'Authorization: Bearer XXXX' \
--header 'Content-Type: application/json' \
--data '{
    "statusIn": 1
}'
```

* Usted puede chequear la información del token en https://jwt.io/
* El token se encuentra configurado en 1 dia de expiración. En la clase JwtProvider > SecurityTokenDescriptor > Expires, usted puede cambiar la duración de la expiración asociada al token.
* Use el atributo [Authorize] en el endpoint o en todo el controller para indicar que deberá usarse el JWT Token.

## Oas3
---
* https://learn.microsoft.com/es-es/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio

## Swagger
---
  * https://localhost:5001/swagger

## MediatR
---
* https://learn.microsoft.com/es-es/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/microservice-application-layer-implementation-web-api#implement-the-command-process-pipeline-with-a-mediator-pattern-mediatr

## Health Check
---
  * https://localhost:5001/health

## Logs
---
* https://serilog.net/
* https://stackify.com/serilog-tutorial-net-logging/
		
## EF (MySQL)
---
* https://dev.mysql.com/downloads/installer/
* https://learn.microsoft.com/es-es/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-implementation-entity-framework-core
* https://www.entityframeworktutorial.net/efcore/install-entity-framework-core.aspx
* https://learn.microsoft.com/es-es/ef/core/
* https://learn.microsoft.com/es-es/ef/core/cli/dotnet
* https://github.com/fedeojeda95/N6A-AN-DA2-2019.1-Clases/blob/master/Clases/Clase%203%20-%20EntityFrameworkCore.md
* https://learn.microsoft.com/es-es/ef/core/dbcontext-configuration/

## Unit Of Work
---
* https://martinfowler.com/eaaCatalog/unitOfWork.html

## CQRS Pattern
---
* https://learn.microsoft.com/es-es/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/apply-simplified-microservice-cqrs-ddd-patterns

## Query Specification Pattern
---
* https://medium.com/@rudyzio92/net-core-using-the-specification-pattern-alongside-a-generic-repository-318cd4eea4aa

## Multiple Environments
---
Crear el json con el siguiente nombre:
*  appsettings.__environment__.json

Ejemplos:

![img_hierarchy_1](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/multiple_environments_1.png?raw=true)

![img_hierarchy_2](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/multiple_environments_2.png?raw=true)	

* References
  * https://learn.microsoft.com/es-es/aspnet/core/fundamentals/environments?view=aspnetcore-6.0

## Unit Test
---
Dichos test están escritos mediante XUnit y utilizando las siguientes bibliotecas FluentAssertions y FakeItEasy.

Reference:
* https://fluentassertions.com/
* https://fakeiteasy.readthedocs.io/en/stable/
* https://www.lambdatest.com/blog/nunit-vs-xunit-vs-mstest/
* https://learn.microsoft.com/es-es/dotnet/core/testing/unit-testing-best-practices

## Integration Test
---
Microsoft.AspNetCore.TestHost - Estos Test nos ayudan a poder realizar una prueba de integración de nuestra APP. El objetivo del mismo es poder levantar el middleware de Net Core con todas las configuraciones.

* https://learn.microsoft.com/es-es/aspnet/core/test/integration-tests?view=aspnetcore-6.0

## Lectura recomendada:
* [Learn DDD Oriented Microservice](https://learn.microsoft.com/es-es/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)

## Licencia

Tiene licencia bajo [The MIT License](LICENSE).