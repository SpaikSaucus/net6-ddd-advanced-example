![en](https://img.shields.io/badge/lang-en-red.svg):ballot_box_with_check: [![es](https://img.shields.io/badge/lang-es-yellow.svg):black_large_square:](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/README.es.md)

# net6-ddd-advanced-example
NET 6 example with DDD Architecture and some advanced features.

## Table of Contents
- [Getting started](#getting-started)
- [Features list](#features-list)
- [Folder structure](#folder-structure)
  - [1- Entrypoint](#1--entrypoint)
  - [2- Core](#2--core)
  - [3- Infrastructure](#3--infrastructure)
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
  * Examples [click here](https://github.com/SpaikSaucus/net46-ddd-advanced-example/blob/main/postman/Net6Advance.postman_collection.json)
  
* play and enjoy!

## Features List
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
       * LoggingBehavior: Every request and response processed by MediatR will be logged.
       * ValidatorBehavior: Execute the Validators that are associated with a MediatR.

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

## Architecture DDD
When we work with DDD there are three parts that we must take into account:
   * Separation of responsibilities into layers, isolate the domain
   * Model and define the model.
   * Manage the life cycle of Domain objects.

## Api Versions
---
API Versioning package allows us to flag APIs as deprecated. So this gives time to the client to prepare changes. Otherwise immediately deleting older APIs could give a bad taste to clients.

## JWT bearer authentication
---
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

* Can you check the token information in https://jwt.io/
* The token is 1-day expiration. In the class JwtProvider > SecurityTokenDescriptor > Expires, you can change the expiration duration for the token.
* You use the attribute [Authorize] in the endpoint or entire controller to indicate that should use JWT Token.

## Oas3
---
* https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio

## Swagger
---
  * https://localhost:5001/swagger

## MediatR
---
* https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/microservice-application-layer-implementation-web-api#implement-the-command-process-pipeline-with-a-mediator-pattern-mediatr

## Health Check
---
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

References:
  * [Learn Microsoft Health Checks](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-6.0)

## Logs
---
* https://serilog.net/
* https://stackify.com/serilog-tutorial-net-logging/	
	
## EF (MySQL)
---
* https://dev.mysql.com/downloads/installer/
* https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-implementation-entity-framework-core
* https://www.entityframeworktutorial.net/efcore/install-entity-framework-core.aspx
* https://learn.microsoft.com/en-us/ef/core/
* https://learn.microsoft.com/en-us/ef/core/cli/dotnet
* https://github.com/fedeojeda95/N6A-AN-DA2-2019.1-Clases/blob/master/Clases/Clase%203%20-%20EntityFrameworkCore.md
* https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/

## Unit Of Work
---
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

References:
  * [Learn Microsoft Unit Of Work Pattern](https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application#creating-the-unit-of-work-class)
  * [Martin Fower Unit Of Work](https://martinfowler.com/eaaCatalog/unitOfWork.html)

## CQRS Pattern
---
It is a pattern that seeks to have two separate objects, one for reading operations and another for writing operations, unlike other approaches that seek to have everything in one.

  * Queries: These queries return a result and don't change the state of the system, and they're free of side effects.
    * __Application__ :arrow_right: UserCases :arrow_right: FindOne :arrow_right: Queries
    <br/>  
  * Commands: These commands change the state of a system.
    * __Application__ :arrow_right: UserCases :arrow_right: Create :arrow_right: Commands

References:
  * [Aprendiendo Microsoft CRQS Pattern in DDD](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/apply-simplified-microservice-cqrs-ddd-patterns)

## Query Specification Pattern
---
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
Referencias: 
  * [Aprendiendo Microsoft Query Specification Pattern in DDD](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-implementation-entity-framework-core#implement-the-query-specification-pattern)
  * [Medium Specification Pattern Generic Repository](https://medium.com/@rudyzio92/net-core-using-the-specification-pattern-alongside-a-generic-repository-318cd4eea4aa)

## Multiple Environments
---
Create the json with this naming:
  * appsettings.__environment__.json

Examples:

![img_hierarchy_1](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/multiple_environments_1.png?raw=true)

![img_hierarchy_2](https://github.com/SpaikSaucus/net6-ddd-advanced-example/blob/main/readme-img/multiple_environments_2.png?raw=true)	

References:
  * [Learn Microsoft Environments](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-6.0)

## Unit Test
---
xUnit: These tests are written using XUnit and using the following FluentAssertions and FakeItEasy libraries.

References:
  * [Fluent Assertions Web](https://fluentassertions.com/)
  * [Fake It Easy Web](https://fakeiteasy.readthedocs.io/en/stable/)
  * [Blog NUnit vs xUnit vs MSTest](https://www.lambdatest.com/blog/nunit-vs-xunit-vs-mstest/)
  * [Learn Microsoft Unit Testing (best practices)](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)

## Integration Test
---
Microsoft.AspNetCore.TestHost - These Tests help us perform an integration test of our APP. The objective of this is to be able to build the Net Core middleware with all the configurations.

References:
  * [Learn Microsoft Integration Tests](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0)

## Read Recommended:
  * [Learn Microsoft DDD with CQRS](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)

## License

Is licensed under [The MIT License](LICENSE).