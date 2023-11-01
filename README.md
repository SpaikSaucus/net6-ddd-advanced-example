[![en](https://img.shields.io/badge/lang-en-red.svg):ballot_box_with_check:](#) [![es](https://img.shields.io/badge/lang-es-yellow.svg):black_large_square:](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/README.es.md)

# net6-ddd-advanced-example
NET 6 example with DDD Architecture and some advanced features.

## Table of Contents

- [Getting started](#getting-started)
- [Folder structure](#folder-structure)
  - [1- Entrypoint](#1--entrypoint)
  - [2- Core](#2--core)
  - [3- Infrastructure](#3--infrastructure)
- [Features list](#features-list)
- [Read recommended](#read-recommended)
- [License](#license)

## Getting Started

* Download this repository
* Is required Visual Studio IDE :point_right: [download](https://visualstudio.microsoft.com/es/downloads)
* Is required SDK 6 :point_right: [download](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* Is required MySQL :point_right: [download](https://dev.mysql.com/downloads/installer/)
Install and select these features:
  * Connector .net
  * MySQL Server
  * Worbench	
* We enter the terminal from the Visual Studio IDE.
  ![img_powershell_vs](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/getting_started_1.png?raw=true)

* We go to the "src" folder by doing:
  * cd src
* Run these commands:
  * dotnet tool install --global dotnet-ef
  * dotnet ef database update -p ./AuthorizationOperation.Infrastructure -- "server=localhost;port=3306;user=root;password=___[PASSWORD]___;database=authorization_db"
    * (replace credentials)

* Postman:
  * Examples [click here](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/postman/Net6Advance.postman_collection.json)
  
* play and enjoy!

## Folder structure

In this section, we will explain the structure of the project, how the layers are designed, and what function each one fulfills. We will also mention some important folders.

### 1- ENTRYPOINT
Web Api, queue listeners, job workers, etc. You should not implement business rules.

   * #### API
     Receives the requests and delegates them to [MediatR](https://github.com/jbogard/MediatR). The use of Mediatr is implemented for decoupling between layers and thus implements a CQRS architecture between transactional and consultative.

     * __Controllers__: will contain the different versions of the drivers. For more information about API Version go [here](#api-versions).
     * __ViewModels__: will contain the Request and Response classes, necessary for the interaction.

### 2- CORE
Main objective, implement business logic and use cases. We must ensure that there are no references to Frameworks associated with Infrastructure (for example: data access frameworks).

   * #### APPLICATION
     We will implement the flow to develop a specific functionality of my application. Orchestrate between the different Domains.

     * __Behaviors__: will contain behavior reusable by MediatR in each API interaction with the APPLICATION layer.
     Examples:
       * ___LoggingBehavior___: Every request and response processed by MediatR will be logged.
       * ___ValidatorBehavior___: Execute the Validators that are associated with a MediatR.

     * __UserCases__: will contain the Use Cases that are defined to meet the requested requirements. A UserCase can make use of another UserCase, to reuse code (for example: reviewing the UserCase of CreateAuthorizationCommand).
       * ___Commands___: will contain the transactional implementations.
       * ___Queries___: will contain the consultative implementations.
       * ___Validations___: will contain the validation logic for the input data.
       * ___DTOs___: will contain the DTO classes necessary to transfer information between layers, when necessary.

   * #### DOMAIN
     We will define the business rules (in DDD terms, also called Domain and expressed through entities, Domain services, value objects and interfaces).

     * __Core__: contains Base and Interfaces classes that will be necessary to implement critical patterns and features.

### 3- INFRASTRUCTURE
Here we will find specific implementations for data access, ORMs, MicroORMS, HTTP Request, File Management, etc.

   * #### INFRASTRUCTURE

     * __Core__: contains some of the implementations of the Interfaces defined in the DOMAIN layer, associated with solving Infrastructure problems.
     * __EF__: contains the DBContext and the classes necessary to implement Entity Framework, for access to the database.
       * ___Config___: contains the classes that represent each table in the database, where it will be possible to configure how EF will build the database model. More information [here](https://learn.microsoft.com/en-us/ef/core/modeling)
       * ___Migrations___: contains the migrations made when modifying the database.
     * __Services__: contains specific implementations, for example, generating a Report in Excel format. The masses must be configured in the AutofacModules to then be used by injecting IComponentContext.

   * #### INFRASTRUCTURE.BOOSTRAP
   
      * __AutofacModules__: contains the modules that we define, which will be used to register the components that can be created with reflection. In this way, we can use the services that we generate in the _Infrastructure_ layer in the _Application_ layer. The use of these services in the _Domain_ layer is discouraged because it must be as isolated as possible.
      * __Extensions__: contains the configurations necessary for launching our application in a segregated way to improve its understanding and discovery, among other things.

## Features List

- [Architecture DDD (Domain Driven Design)](#architecture-ddd)
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

## :large_blue_diamond: Architecture DDD
The main objective of applying Domain Driven Design is to be able to isolate the code that belongs to the domain from the technical implementation details and thus focus on the complexity of the business.

### Core Principles
We could say that domain orientation focuses on three basic pillars:
  * Focus on the core domain and business logic.
  * Convert complex designs into domain models.
  * Constant interaction and collaboration with domain experts, which will help resolve doubts and interact more with the development team.

In turn, when we work with DDD we must take into account:
  * Separation of responsibilities into layers, _(isolate the domain)_.
  * Model and define the model.
  * Manage the life cycle of Domain objects.

### The different layers:

* __Domain layer:__
  Responsible for representing business concepts, information about the business situation and business rules. The state that reflects the business situation is controlled and used here, although the technical details of its storage are delegated to the infrastructure. This level is the core of enterprise software, where the business is expressed, in. NET, _is coded as a class library_, with domain entities that capture data and behavior (methods with logic).
  <br/>In turn, this library only has dependencies on .NET libraries, but not on other custom libraries, such as data or persistence. It should not depend on any other level (domain model classes must be CLR or POCO object classes).
  <br/>

* __Application layer:__
  It defines the jobs that the software is supposed to do and directs domain objects to solve problems. The tasks that are the responsibility of this level are significant to the business or necessary for interaction with the application levels of other systems.
  <br/>This level should be kept narrow. It does not contain business rules or knowledge, but only coordinates tasks and delegates work to domain object collaborations at the next level. It does not have any status that reflects the business situation, but it can have a status that reflects the progress of a task for the user or the program.
  <br/>Typically, the application layer in .NET microservices is coded as an ASP.NET Core Web API project. The project implements microservice interaction, remote network access, and external web APIs used by client or front-end applications. It includes queries if using a CQRS approach, commands accepted by the microservice, and even event-driven communication between microservices (integration events).
  <br/>Basically, application logic is where all use cases that depend on a given front end are implemented.

  <br/>In this "_Authorization.Operation_" example, this layer is split to improve the design focus, resulting in the following two projects:
  * 1- ENTRYPOINT :arrow_right: __API__
  * 2- CORE :arrow_right: __Application__
  <br/>

* __Infrastructure layer:__
  It is where the technical part of the application resides, with its specific implementations and where dependencies on third-party software will be added to comply with integrations, database, file management, etc.

  <br/>In this "_Authorization.Operation_" example, this layer is split to improve the design focus, resulting in the following two projects:
  * 3- INFRASTRUCTURE :arrow_right: __Infrastructure__
  * 3- INFRASTRUCTURE :arrow_right: __Infrastructure.Bootstrap__
  <br/>

![ddd_1_en](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/ddd_1_en.png?raw=true)
![ddd_2_en](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/ddd_2_en.png?raw=true)

### References: :triangular_flag_on_post:
  * [Learn Microsoft DDD](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)
  * [Introduction DDD (spanish)](https://refactorizando.com/introduccion-domain-drive-design/)

## :large_blue_diamond: Api Versions

API Versioning package allows us to flag APIs as deprecated. So this gives time to the client to prepare changes. Otherwise immediately deleting older APIs could give a bad taste to clients.

To define a version as obsolete, you only need to include the __Deprecated__ in its construction, in the indicated _Controller_, example:

```csharp
[ApiVersion("1.0", Deprecated = true)]
```

To select the current version in which our API is located, we will have to update the environment file:
```json
"AppSettings": {
     "DefaultApiVersion": "2.0",
```

If we want to view the ApiVersioning configuration, we must enter the following class:

  * __Infrastructure.Bootstrap__ :arrow_right: Extensions :arrow_right: ServiceCollection :arrow_right: ApiVersioningServiceCollectionExtensions

### References: :triangular_flag_on_post:
  * [Blog API versioning and integrate Swagger](https://blog.christian-schou.dk/how-to-use-api-versioning-in-net-core-web-api/)

## :large_blue_diamond: JWT bearer authentication

1. Create Token:
```bash
curl --location 'https://localhost:5001/api/v2/login' \
--header 'Content-Type: application/json' \
--data '{
  "username": "admin",
  "password": "admin"
}'
```
2. Use Token: (_replace XXXX with the token generated_)
```bash
curl --location --request GET 'http://localhost:5000/api/v2/authorizations/findAllByCriteria?sort=customer,asc;id,desc&offset=0&limit=200' \
--header 'Authorization: Bearer XXXX' \
--header 'Content-Type: application/json' \
--data '{
    "statusIn": 1
}'
```

* Can you check the token information in [JWT.io](https://jwt.io/)
* The token is 1-day expiration. In the class JwtProvider > SecurityTokenDescriptor > Expires, you can change the expiration duration for the token.
* You use the attribute [Authorize] in the endpoint or entire controller to indicate that should use JWT Token.

## :large_blue_diamond: Swagger Oas3

It is a tool based on the OpenAPI standard that allows us to document and test our Web APIs, so that they are easily accessible and understandable by users or developers who intend to use them.

Using the __summary__ tag we can add information in the controller and in the classes that are used as request/response, to be displayed in the generated documentation, for example:

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

To access the documentation generated by this tool, we must enter the following endpoint:
* https://localhost:5001/swagger

![swagger_oas3_1](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/swagger_oas3_1.png?raw=true)

If we want to view the configuration, we must enter the following classes:

   * __Infrastructure.Bootstrap__ :arrow_right: Extensions
     * ApplicationBuilder :arrow_right: SwaggerApplicationBuilderExtensions
     and in:
     * ServiceCollections :arrow_right: SwaggerServiceCollectionExtensions

### References: :triangular_flag_on_post:
  * [Learn Microsoft Swashbuckle](https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio)
  * [Blog API versioning and integrate Swagger](https://blog.christian-schou.dk/how-to-use-api-versioning-in-net-core-web-api/)

## :large_blue_diamond: MediatR + CQRS

### MediatR
It is a small and simple open source library that implements the mediator pattern, for piping messages (commands) and routing them, in memory, to the correct command handlers.

Using the mediator pattern helps reduce coupling and isolate the processing of the requested command from the rest of the code.

### CQRS
Command Query Responsibility Segregation, It is a pattern that seeks to have two separate objects, one for reading operations and another for writing operations, unlike other approaches that seek to have everything in one.

### Combining them
In this example "_Authorization.Operation_", we combine the mediator pattern with the CQRS pattern, the result involves the creation of commands for queries and commands to change the state of the system.

  * Queries: These queries return a result and don't change the state of the system, and they're free of side effects.
    * __Application__ :arrow_right: UserCases :arrow_right: FindOne :arrow_right: Queries
    <br/>  
  * Commands: These commands change the state of a system.
    * __Application__ :arrow_right: UserCases :arrow_right: Create :arrow_right: Commands

### References: :triangular_flag_on_post:
  * [CQRS web-api command process pipeline with a mediator pattern MediatR](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/microservice-application-layer-implementation-web-api#implement-the-command-process-pipeline-with-a-mediator-pattern-mediatr)
  * [Learn Microsoft CRQS Pattern in DDD](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/apply-simplified-microservice-cqrs-ddd-patterns)

## :large_blue_diamond: Health Check

Health checks are exposed by an app as HTTP endpoints, are typically used with an external monitoring service or container orchestrator to check the status of an app. 

Before adding health checks to an app, decide on which monitoring system to use. The monitoring system dictates what types of health checks to create and how to configure their endpoints.

Use the library:
   * Microsoft.AspNetCore.Diagnostics.HealthChecks

The configuration can be found in:
   * __Infrastructure.Bootstrap__ :arrow_right: Extensions
     * ApplicationBuilder :arrow_right: HealthChecksApplicationBuilderExtensions
     and in:
     * ServiceCollections :arrow_right: HealthChecksServiceCollectionExtensions
    
And we can call the __"/health"__ endpoint to check its operation.
   * https://localhost:5001/health

### References: :triangular_flag_on_post:
  * [Learn Microsoft Health Checks](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-6.0)

## :large_blue_diamond: Logs

To obtain information and record errors that occur in our application, we will use the Serilog library, which makes it easier for us to implement this very useful feature for diagnosis.

### How to use it
Define a variable in our class to store the logger:

```csharp
private readonly ILogger<MyClass> logger;
```
Inject the logger into the constructor:
```csharp
public MyClass(ILogger<MyClass> logger)
{
  this.logger = logger;
}
```
Make use of the logger, examples:
```csharp
this.logger.Log(LogLevel.Information, "Authorization {0} already exists", auth.UUID);
...
this.logger.LogInformation("Authorization {0} already exists", auth.UUID);
...
this.logger.LogError("API Error: {api}: \n{result}",
  apiException.RequestMessage.RequestUri, 
  apiException.Content);
```

### Configuration
The Serilog configuration is in the following class:
  * __API__ :arrow_right: Program.cs    

And in the file appsettings.[Environment].json:
```bash
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Microsoft": "Information",
      "Microsoft.Hosting.Lifetime": "Information",
      "Override": {
        "System": "Information",
        "Microsoft": "Information"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "theme": "Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console",
          "outputTemplate": "{Timestamp:u} [{Level:u3}] [{RequestId}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}"
        }
      }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName" ]
  },
```

#### Priority Level 

* _Verbose_ :arrow_right: _Debug_ :arrow_right: _Information_ :arrow_right: _Warning_ :arrow_right: _Error_ :arrow_right: _Fatal_

For example, if we indicate the "Information" level, the "Verbose" and "Debug" level logs will not be displayed.


### References: :triangular_flag_on_post:
  * [Serilog Web](https://serilog.net/)
  * [Serilog Tutorial](https://stackify.com/serilog-tutorial-net-logging/)
    
## :large_blue_diamond: EF Code First + Migrations (MySQL)

Entity Framework (EF) Core is a lightweight, extensible, open source, cross-platform version of the popular Entity Framework data access technology. EF Core works to:
* Allows developers to work with a database using objects.
* Allows you to eliminate most of the data access code that you normally need to write.

EF supports the following model development methods:
* Generate a model from an existing database.
* Code a model manually to match the database.

EF Migrations allows once a model is created, we can create the database from the model. At the same time, migrations allow the database to evolve as the model changes.

### CLI Tool

We will use the command line interface (CLI) tool for Entity Framework Core that performs development tasks at design time. For example, they create migrations, apply them, and generate code for a model based on an existing database. The commands are an extension to the cross-platform dotnet command, which is part of the .NET Core SDK. These tools work with .NET Core projects.

* We enter the terminal from the Visual Studio IDE
   ![img_powershell_vs](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/getting_started_1.png?raw=true)

* We stop in the "src" folder doing:
     * cd src

Now we can execute the following commands.

#### Install tool
   * dotnet tool install --global dotnet-ef

#### Add Migration
   * dotnet ef migrations add ___[MIGRATION_NAME]___ -p ./AuthorizationOperation.Infrastructure -o EF\Migrations -- "server=localhost;port=3306;user=root;password=___[PASSWORD]___;database=authorization_db"

  This command will create the migration, with 3 files:
  1) .cs: Contains the Up() and Down() operations that will be applied to the DB to remove or add objects.
  2) .Designer.cs: Contains the metadata that will be used by EF Core.
  3) .ModelSnapshot.cs: Contains a snapshot of the current model. Which will be used to determine what changes when the next migration is performed.

#### Run Migration
   * dotnet ef database update ___[MIGRATION_NAME]___ -p ./AuthorizationOperation.Infrastructure -- "server=localhost;port=3306;user=root;password=___[PASSWORD]___;database=authorization_db"

   This command executes the migration to the Database.

#### Run Unapplied Migrations
   * dotnet ef database update -p ./AuthorizationOperation.Infrastructure -- "server=localhost;port=3306;user=root;password=___[PASSWORD]___;database=authorization_db"

   This command executes the migrations to the Database that are not applied. This can be corroborated in the Database, by consulting the __efmigrationshistory table, a table used by the framework to store the information associated with which migrations were applied.

#### Get Migration SQL
   * dotnet ef migrations script 0 ___[MIGRATION_NAME]___ -p ./AuthorizationOperation.Infrastructure -- "server=localhost;port=3306;user=root;password=___[PASSWORD]___;database=authorization_db"

   Generate SQL script for migration.

### References: :triangular_flag_on_post:
  * [Learn Microsoft EF Core](https://learn.microsoft.com/en-us/ef/core/)
  * [Learn Microsoft EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)
  * [Learn Microsoft EF Core DBContext](https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/)
  * [Learn Microsoft EF Core Implementation](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-implementation-entity-framework-core)
  * [EF Core Tutorial 1](https://www.entityframeworktutorial.net/efcore/install-entity-framework-core.aspx)
  * [EF Core Tutorial 2](https://github.com/fedeojeda95/N6A-AN-DA2-2019.1-Clases/blob/master/Clases/Clase%203%20-%20EntityFrameworkCore.md)

## :large_blue_diamond: Unit Of Work

It is a pattern that has the purpose of ensuring that the same database context is shared so that when the tasks to be performed in the database are completed, the SaveChanges method can be called on that instance of the context and ensure that all related changes will be coordinated.

Example:

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

### References: :triangular_flag_on_post:
  * [Learn Microsoft Unit Of Work Pattern](https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application#creating-the-unit-of-work-class)
  * [Martin Fower Unit Of Work](https://martinfowler.com/eaaCatalog/unitOfWork.html)

## :large_blue_diamond: Query Specification Pattern

It is a pattern that seeks to comply with DDD for data queries so that these specifications are stored in the __Domain__ layer, effectively separating the logic that exists in the queries from their implementation.

To do this, the base class _BaseSpecification_ and the interface _ISpecification_ were generated in the __Domain__ layer. In the __Infrastructure__ layer there is the _SpecificationEvaluator_ class that is used by the _Repository_ class to apply the specification to be used.

Example:
The _AuthorizationGetSpecification_ class found in the folder
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
This specification is used in the _AuthorizationGetQuery_ class found in the folder
  * __Application__ :arrow_right: UserCases :arrow_right: FindOne :arrow_right: Queries

```csharp
var spec = new AuthorizationGetSpecification(request.Id);
var result = this.unitOfWork.Repository<Authorization>().Find(spec).FirstOrDefault();
return Task.FromResult(result);
```
### References: :triangular_flag_on_post: 
  * [Learn Microsoft Query Specification Pattern in DDD](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-implementation-entity-framework-core#implement-the-query-specification-pattern)
  * [Medium Specification Pattern Generic Repository](https://medium.com/@rudyzio92/net-core-using-the-specification-pattern-alongside-a-generic-repository-318cd4eea4aa)

## :large_blue_diamond: Multiple Environments
Create the json with this naming:
  * appsettings.__environment__.json

Examples:

![img_hierarchy_1](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/multiple_environments_1.png?raw=true)

![img_hierarchy_2](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/multiple_environments_2.png?raw=true)	

### References: :triangular_flag_on_post:
  * [Learn Microsoft Environments](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-6.0)

## :large_blue_diamond: Unit Test
xUnit: These tests are written using XUnit and using the following FluentAssertions and FakeItEasy libraries.

### References: :triangular_flag_on_post:
  * [Fluent Assertions Web](https://fluentassertions.com/)
  * [Fake It Easy Web](https://fakeiteasy.readthedocs.io/en/stable/)
  * [Blog NUnit vs xUnit vs MSTest](https://www.lambdatest.com/blog/nunit-vs-xunit-vs-mstest/)
  * [Learn Microsoft Unit Testing (best practices)](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)

## :large_blue_diamond: Integration Test
Microsoft.AspNetCore.TestHost - These Tests help us perform an integration test of our APP. The objective of this is to be able to build the Net Core middleware with all the configurations.

### References: :triangular_flag_on_post:
  * [Learn Microsoft Integration Tests](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0)

## Read Recommended:
  * [Learn Microsoft DDD with CQRS](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)

## License

Is licensed under [The MIT License](LICENSE).