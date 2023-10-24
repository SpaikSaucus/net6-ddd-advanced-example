[![en](https://img.shields.io/badge/lang-en-red.svg):black_large_square:](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/README.md) [![es](https://img.shields.io/badge/lang-es-yellow.svg):ballot_box_with_check:](#)

# net6-ddd-advanced-example
Ejemplo NET 6 con arquitectura DDD y algunas funciones avanzadas.

## Tabla de Contenido

- [Iniciando](#iniciando)
- [Estructura de carpetas](#folder-structure)
  - [1- Entrypoint](#1--entrypoint)
  - [2- Core](#2--core)
  - [3- Infrastructure](#3--infrastructure)
- [Lista de características](#lista-de-caracter%C3%ADsticas)
- [Lectura recomendada](#lectura-recomendada)
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
  * Examples [click here](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/postman/Net6Advance.postman_collection.json)
  
* dale play y disfruta!

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
      * ___LoggingBehavior___: Se logueara cada request y response que procese MediatR. 
      * ___ValidatorBehavior___: Ejecuta los Validators que estén asociados a un MediatR.

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

    * __AutofacModules__: contiene los módulos que nosotros definamos, los cuales se utilizaran para registrar los componentes que se podrán crear con reflection. De esta forma, podremos utilizar los servicios que generemos en la capa _Infrastructure_ en la capa _Application_. Se desaconseja el uso de dichos servicios en la capa _Domain_ debido a que la misma debe estar los mas aislada posible.
    * __Extensions__: contiene las configuraciones necesarias para la iniciación de nuestra aplicación de forma segregada para mejorar su comprensión y descubrimiento, entre otras cosas.

## Lista de características

- [Arquitectura DDD (Domain Driven Design)](#arquitectura-ddd)
- [Api Versions](#api-versions)
- [JWT (JSON Web Tokens)](#jwt-bearer-authentication)
- [Swagger Oas3 (OpenAPI Specification - Version 3)](#swagger-oas3)
- [MediatR + CQRS](#mediatr--cqrs)
- [Health Check](#health-check)
- [Logs](#logs)
- [EF Code First + Migrations (MySQL)](#ef-code-first--migrations-mysql)
- [Unit of Work Pattern](#unit-of-work)
- [Query Specification Pattern](#query-specification-pattern)
- [Multiple Environments File](#multiple-environments)
- [Unit Test](#unit-test)
- [Integrations Test](#integration-test)

## Arquitectura DDD

El objetivo principal de aplicar DDD o Domain Driven Design en inglés, es poder aislar el código que pertenece al dominio de los detalles técnicos de implementación y así centrarnos en la complejidad del negocio.

### Principios centrales
Podríamos decir que la orientación al dominio se centra en tres pilares básicos:
  * Focalizar en el dominio central y la lógica de negocio.
  * Convertir diseños complejos en modelos de dominio.
  * Constante interacción y colaboración con los expertos de dominio, lo que ayudará a solventar dudas e interactuar más con el equipo de desarrollo.

A su vez, cuando trabajamos con DDD debemos tener en cuenta:
  * Separación de responsabilidades en capas, _(aislar el dominio)_.
  * Modelar y definir el modelo.
  * Gestionar el ciclo de vida de los objetos de Dominio.


### Las diferentes capas son:

* __Capa de dominio:__ 
  Responsable de representar conceptos del negocio, información sobre la situación del negocio y reglas de negocios. El estado que refleja la situación empresarial está controlado y se usa aquí, aunque los detalles técnicos de su almacenaje se delegan a la infraestructura. Este nivel es el núcleo del software empresarial, donde se expresa el negocio, en. NET, _se codifica como una biblioteca de clases_, con las entidades de dominio que capturan datos y comportamiento (métodos con lógica).
  <br/>A su vez, esta biblioteca solo tiene dependencias a las bibliotecas de .NET, pero no a otras bibliotecas personalizadas, como por ejemplo de datos o de persistencia. No debe depender de ningún otro nivel (las clases del modelo de dominio deben ser clases de objetos CLR o POCO).
  <br/>

* __Capa de aplicación:__ 
  Define los trabajos que se supone que el software debe hacer y dirige los objetos de dominio para que resuelvan problemas. Las tareas que son responsabilidad de este nivel son significativas para la empresa o necesarias para la interacción con los niveles de aplicación de otros sistemas.
  <br/>Este nivel debe mantenerse estrecho. No contiene reglas de negocios ni conocimientos, sino que solo coordina tareas y delega trabajo a colaboraciones de objetos de dominio en el siguiente nivel. No tiene ningún estado que refleje la situación empresarial, pero puede tener un estado que refleje el progreso de una tarea para el usuario o el programa.
  <br/>Normalmente, el nivel de aplicación en microservicios .NET se codifica como un proyecto de ASP.NET Core Web API. El proyecto implementa la interacción del microservicio, el acceso a redes remotas y las API web externas utilizadas desde aplicaciones cliente o de interfaz de usuario. Incluye consultas si se utiliza un enfoque de CQRS, comandos aceptados por el microservicio e incluso comunicación guiada por eventos entre microservicios (eventos de integración).      
  <br/>Básicamente, la lógica de la aplicación es el lugar en el que se implementan todos los casos de uso que dependen de un front-end determinado.
  
  <br/>En este ejemplo "_Authorization.Operation_", esta capa se divide para mejorar el enfoque del diseño, dando lugar a los siguientes dos proyectos:
  * 1- ENTRYPOINT :arrow_right: __API__
  * 2- CORE  :arrow_right: __Application__
	<br/>

* __Capa de infraestructura:__
  Es en donde reside la parte técnica de la aplicación, con sus implementaciones concretas y donde se añadirán las dependencias a software de terceros para cumplir con integraciones, base de datos, manejo de archivos, etc.

  <br/>En este ejemplo "_Authorization.Operation_", esta capa se divide para mejorar el enfoque del diseño, dando lugar a los siguientes dos proyectos:
  * 3- INFRASTRUCTURE :arrow_right: __Infrastructure__
  * 3- INFRASTRUCTURE :arrow_right: __Infrastructure.Bootstrap__
	<br/>

![ddd_1_es](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/ddd_1_es.png?raw=true)
![ddd_2_es](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/ddd_2_es.png?raw=true)

Referencias:
  * [Aprendiendo Microsoft DDD](https://learn.microsoft.com/es-es/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)
  * [Introducción DDD](https://refactorizando.com/introduccion-domain-drive-design/)

## Api Versions

El paquete API Versioning nos permite marcar las API como obsoletas. Entonces esto le da tiempo al cliente para preparar los cambios. De lo contrario, eliminar inmediatamente las API más antiguas podría generar problemas a los clientes.

Para definir una version como obsoleta, solo hace falta incluir el __Deprecated__ en su construcción, en el _Controller_ indicado, ejemplo:

```csharp
[ApiVersion("1.0", Deprecated = true)]
```

Y para seleccionar la version actual en la que se encuentra nuestra API, deberemos actualizar el archivo de environment:
```json
"AppSettings": {
    "DefaultApiVersion": "2.0",
```

Si queremos visualizar la configuración de ApiVersioning, debemos ingresar a la siguiente clase: 

  * __Infrastructure.Bootstrap__ :arrow_right: Extensions :arrow_right: ServiceCollection :arrow_right: ApiVersioningServiceCollectionExtensions    

Referencias:
  * [Blog API versioning and integrate Swagger](https://blog.christian-schou.dk/how-to-use-api-versioning-in-net-core-web-api/)


## JWT bearer authentication

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

* Usted puede chequear la información del token en [JWT.io](https://jwt.io/)
* El token se encuentra configurado en 1 dia de expiración. En la clase JwtProvider > SecurityTokenDescriptor > Expires, usted puede cambiar la duración de la expiración asociada al token.
* Use el atributo [Authorize] en el endpoint o en todo el controller para indicar que deberá usarse el JWT Token.

## Swagger Oas3

Es una herramienta basada en el estándar OpenAPI que nos permite documentar y probar nuestras Web APIs, para que sean fácilmente accesibles y entendibles por los usuarios o desarrolladores que pretendan utilizarlas.

Mediante el tag __summary__ podremos agregar información en el controller y en las clases que se utilizan como request/response, para ser visualizada en la documentación generada, ejemplo:

```csharp
public class AuthorizationPageResponse
{
    /// <summary>
    /// Total result.
    /// </summary>
    /// <example>10</example>
    public int Total { get; set; }

    /// <summary>
    /// Page result (0..N).
    /// </summary>
    /// <example>0</example>
    public uint Offset { get; set; }
```

Para acceder a la documentación generada por esta herramienta, deberemos ingresar al siguiente endpoint:
* https://localhost:5001/swagger

![swagger_oas3_1](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/swagger_oas3_1.png?raw=true)

Si queremos visualizar la configuración, debemos ingresar a la siguientes clases: 

  * __Infrastructure.Bootstrap__ :arrow_right: Extensions   
    * ApplicationBuilder :arrow_right: SwaggerApplicationBuilderExtensions    
    y en:
    * ServiceCollections :arrow_right: SwaggerServiceCollectionExtensions

Referencias:
  * [Aprendiendo Microsoft Swashbuckle](https://learn.microsoft.com/es-es/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio)
  * [Blog API versioning and integrate Swagger](https://blog.christian-schou.dk/how-to-use-api-versioning-in-net-core-web-api/)
  
## MediatR + CQRS

### MediatR
Es una biblioteca de código abierto, pequeña y simple, que implementa el patrón de mediador, para la canalización de mensajes (comandos) y enrutandolos, en memoria, a los controladores de comandos correctos.

El uso del patrón de mediador ayuda a reducir el acoplamiento y aislar el procesamiento del comando solicitado, del resto del código.

### CQRS
CQRS en ingles significa (Command Query Responsibility Segregation), el cual es un patron que busca tener dos objetos separados, uno para operaciones de lectura y otro para operaciones de escritura, a diferencia de otros enfoques que buscan tener todo en uno solo.

### Combinándolos
En este ejemplo "_Authorization.Operation_", combinamos el patron mediador con el patron CQRS, el resultado implica la creación de comandos para consultas y comandos para cambiar el estado del sistema.

  * Consultas: Estas consultas devuelven un resultado sin cambiar el estado del sistema y no tienen efectos secundarios.
    * __Application__ :arrow_right: UserCases :arrow_right: FindOne :arrow_right: Queries
    <br/>  
  * Comandos: Estos comandos cambian el estado de un sistema.
    * __Application__ :arrow_right: UserCases :arrow_right: Create :arrow_right: Commands

Referencias:
  * [CQRS web-api command process pipeline with a mediator pattern MediatR](https://learn.microsoft.com/es-es/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/microservice-application-layer-implementation-web-api#implement-the-command-process-pipeline-with-a-mediator-pattern-mediatr)
  * [Aprendiendo Microsoft CRQS Pattern in DDD](https://learn.microsoft.com/es-es/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/apply-simplified-microservice-cqrs-ddd-patterns)


## Health Check

Una aplicación se encarga de exponer las comprobaciones de estado como puntos de conexión HTTP, donde normalmente, las comprobaciones de estado se usan con un servicio de supervisión externa o un orquestador de contenedores para comprobar el estado de una aplicación. 

Antes de agregar comprobaciones de estado a una aplicación, debe decidir en qué sistema de supervisión se va a usar. El sistema de supervisión determina qué tipos de comprobaciones de estado se deben crear y cómo configurar sus puntos de conexión.

Para ello utilizamos la biblioteca:
  * Microsoft.AspNetCore.Diagnostics.HealthChecks

Dicha configuración se puede encontrar en:
  * __Infrastructure.Bootstrap__ :arrow_right: Extensions   
    * ApplicationBuilder :arrow_right: HealthChecksApplicationBuilderExtensions    
    y en:
    * ServiceCollections :arrow_right: HealthChecksServiceCollectionExtensions
    
Y podemos ingresar al endpoint __"/health"__ para comprobar su funcionamiento.
  * https://localhost:5001/health

Referencias:
  * [Aprendiendo Microsoft Health Checks](https://learn.microsoft.com/es-es/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-6.0)

## Logs
In progress....

Referencias:
  * [Serilog Web](https://serilog.net/)
  * [Serilog Tutorial](https://stackify.com/serilog-tutorial-net-logging/)
      
## EF Code First + Migrations (MySQL)

Entity Framework (EF) Core es una versión ligera, extensible, de código abierto y multiplataforma de la popular tecnología de acceso a datos Entity Framework. EF Core actúa para:
* Permite a los desarrolladores trabajar con una base de datos usando objetos.
* Permite prescindir de la mayor parte del código de acceso a datos que normalmente es necesario escribir.

EF admite los siguientes métodos de desarrollo de modelos:
* Generar un modelo a partir de una base de datos existente.
* Codificar un modelo manualmente para que coincida con la base de datos.

Migrations de EF, permite que una vez creado un modelo, podamos crear la base de datos a partir de dicho modelo. A su vez, migraciones permite que la base de datos evolucione a medida que el modelo va cambiando.

### Herramienta CLI

Usaremos la herramienta de interfaz de la línea de comandos (CLI) para Entity Framework Core que realizan tareas de desarrollo en tiempo de diseño. Por ejemplo, crean migraciones, las aplican y generan código para un modelo según una base de datos existente. Los comandos son una extensión para el comando dotnet multiplataforma, que forma parte del SDK de .NET Core. Estas herramientas funcionan con proyectos de .NET Core.

* Ingresamos a la terminal desde el Visual Studio IDE
  ![img_powershell_vs](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/getting_started_1.png?raw=true)

* Nos paramos en la carpeta "src" haciendo: 
    * cd src

Ahora podremos realizar ejecutar los siguientes comandos.

#### Instalar herramienta
  * dotnet tool install --global dotnet-ef
  
#### Agregar Migration
  * dotnet ef migrations add ___[NOMBRE_DE_LA_MIGRATION]___ -p ./AuthorizationOperation.Infrastructure -o EF\Migrations -- "server=localhost;port=3306;user=root;password=___[PASSWORD]___;database=authorization_db"

	Este comando creará la migración, con 3 archivos: 
		1) .cs: Contiene las operaciones Up() y Down() que se aplicaran a la BD para remover o añadir objetos. 
		2) .Designer.cs: Contiene la metadata que va a ser usada por EF Core. 
		3) .ModelSnapshot.cs: Contiene un snapshot del modelo actual. Que será usada para determinar qué cambio cuando se realice la siguiente migración.

#### Ejecutar Migration
  * dotnet ef database update ___[NOMBRE_DE_LA_MIGRATION]___ -p ./AuthorizationOperation.Infrastructure -- "server=localhost;port=3306;user=root;password=___[PASSWORD]___;database=authorization_db"

  Este comando ejecuta la migración hacia la Base de Datos.

#### Ejecutar Migrations no aplicados
  * dotnet ef database update -p ./AuthorizationOperation.Infrastructure -- "server=localhost;port=3306;user=root;password=___[PASSWORD]___;database=authorization_db"

  Este comando ejecuta las migraciones hacia la Base de Datos que no se encuentren aplicadas. Esto se puede corroborar en la Base de Datos, consultando la tabla __efmigrationshistory, tabla que utiliza el framework para almacenar la información asociada a que migraciones se aplicaron.

#### Obtener SQL del Migration
  * dotnet ef migrations script 0 ___[NOMBRE_DE_LA_MIGRATION]___ -p ./AuthorizationOperation.Infrastructure -- "server=localhost;port=3306;user=root;password=___[PASSWORD]___;database=authorization_db"
	
  Genera script SQL a de la migración.

Referencias:
  * [Aprendiendo Microsoft EF Core](https://learn.microsoft.com/es-es/ef/core/)
  * [Aprendiendo Microsoft EF Core CLI](https://learn.microsoft.com/es-es/ef/core/cli/dotnet)
  * [Aprendiendo Microsoft EF Core DBContext](https://learn.microsoft.com/es-es/ef/core/dbcontext-configuration/)
  * [Aprendiendo Microsoft EF Core Implementación](https://learn.microsoft.com/es-es/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-implementation-entity-framework-core)
  * [EF Core Tutorial 1](https://www.entityframeworktutorial.net/efcore/install-entity-framework-core.aspx)
  * [EF Core Tutorial 2](https://github.com/fedeojeda95/N6A-AN-DA2-2019.1-Clases/blob/master/Clases/Clase%203%20-%20EntityFrameworkCore.md)
  

## Unit Of Work

Es un patron que tiene como propósito asegurarse de que se comparta un mismo contexto de base de datos, de modo que cuando se completan las tareas a realizar en la base de datos, se pueda llamar al SaveChanges, método en esa instancia del contexto y asegurarse de que todos los cambios relacionados se coordinarán. 

Ejemplo:

```csharp
var authorization = new Authorization()
{
    UUID = cmd.UUID,
    Customer = cmd.Customer,
    StatusId = AuthorizationStatusEnum.WAITING_FOR_SIGNERS,
    Created = DateTime.UtcNow
};

this.unitOfWork.Repository<Authorization>().Add(authorization);
// ...
// this.unitOfWork.Repository<....>().Add(...);
// this.unitOfWork.Repository<....>().Remove(...);
// this.unitOfWork.Repository<....>().Update(...);
// ...
await this.unitOfWork.Complete();
```

Referencias:
  * [Aprendiendo Microsoft Unit Of Work Pattern](https://learn.microsoft.com/es-es/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application#creating-the-unit-of-work-class)
  * [Martin Fower Unit Of Work](https://martinfowler.com/eaaCatalog/unitOfWork.html)

## Query Specification Pattern

Es un patron que busca cumplir con DDD para la consulta de datos de manera que dichas especificaciones se almacenen en la capa __Domain__, separando de manera efectiva la lógica que existe en las consultas de su implementación.

Para ello se genero en la capa __Domain__ la clase base _BaseSpecification_ y la interface _ISpecification_. En la capa __Infrastructure__ existe la clase _SpecificationEvaluator_ que es utilizada por la clase _Repository_ para aplicar la especificación a utilizar.

Ejemplo:
La clase _AuthorizationGetSpecification_ que se encuentra en la carpeta
  * __Domain__ :arrow_right: Authorization :arrow_right: Queries

```csharp
public class AuthorizationGetSpecification : BaseSpecification<Models.Authorization>
{
  public AuthorizationGetSpecification(uint id = default, Guid uuid = default)
  {
    base.AddInclude(x => x.Status);
    Expression<Func<Models.Authorization, bool>> criteria = null;

    if (id != default)
      criteria = this.OrCriteria(criteria, x => x.Id == id);
    if (uuid != default)
      criteria = this.OrCriteria(criteria, x => x.UUID == uuid);
    base.SetCriteria(criteria);
  }
}
```
Dicha especificación es utilizada en la clase _AuthorizationGetQuery_ que se encuentra en la carpeta
  * __Application__ :arrow_right: UserCases :arrow_right: FindOne :arrow_right: Queries

```csharp
var spec = new AuthorizationGetSpecification(request.Id);
var result = this.unitOfWork.Repository<Authorization>().Find(spec).FirstOrDefault();
return Task.FromResult(result);
```

Referencias: 
  * [Aprendiendo Microsoft Query Specification Pattern in DDD](https://learn.microsoft.com/es-es/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-implementation-entity-framework-core#implement-the-query-specification-pattern)
  * [Medium Specification Pattern Generic Repository](https://medium.com/@rudyzio92/net-core-using-the-specification-pattern-alongside-a-generic-repository-318cd4eea4aa)

## Multiple Environments
Crear el json con el siguiente nombre:
  * appsettings.__environment__.json

Ejemplos:

![img_hierarchy_1](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/multiple_environments_1.png?raw=true)

![img_hierarchy_2](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/multiple_environments_2.png?raw=true)	

Referencias:
  * [Aprendiendo Microsoft Environments](https://learn.microsoft.com/es-es/aspnet/core/fundamentals/environments?view=aspnetcore-6.0)

## Unit Test
xUnit: Estos test están escritos mediante XUnit y utilizando las siguientes bibliotecas FluentAssertions y FakeItEasy.

Referencias:
  * [Fluent Assertions Web](https://fluentassertions.com/)
  * [Fake It Easy Web](https://fakeiteasy.readthedocs.io/en/stable/)
  * [Blog NUnit vs xUnit vs MSTest](https://www.lambdatest.com/blog/nunit-vs-xunit-vs-mstest/)
  * [Aprendiendo Microsoft Unit Testing (mejores practicas)](https://learn.microsoft.com/es-es/dotnet/core/testing/unit-testing-best-practices)

## Integration Test
Microsoft.AspNetCore.TestHost - Estos Test nos ayudan a poder realizar una prueba de integración de nuestra APP. El objetivo del mismo es poder levantar el middleware de Net Core con todas las configuraciones.

Referencias:
  * [Aprendiendo Microsoft Integration Tests](https://learn.microsoft.com/es-es/aspnet/core/test/integration-tests?view=aspnetcore-6.0)

## Lectura recomendada:
  * [Aprendiendo Microsoft DDD con CRQS](https://learn.microsoft.com/es-es/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)

## Licencia

Tiene licencia bajo [The MIT License](LICENSE).