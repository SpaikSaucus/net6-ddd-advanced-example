#in progress....


### Multiple Environments
---
* https://learn.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-6.0
	
### Logs
---
* https://serilog.net/
* https://stackify.com/serilog-tutorial-net-logging/
	
### UnitTest & Integration Test
---
* https://www.lambdatest.com/blog/nunit-vs-xunit-vs-mstest/
* https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
* https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0

### Oas3
---
* https://learn.microsoft.com/es-es/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-7.0&tabs=visual-studio

### Api Versions
---
API Versioning package allows us to flag APIs as deprecated. So this gives time to the client to prepare changes. Otherwise immediately deleting older APIs could give a bad taste to clients.
	
### EF Core (MySQL)
---
* https://dev.mysql.com/downloads/installer/
* https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-implementation-entity-framework-core
* https://www.entityframeworktutorial.net/efcore/install-entity-framework-core.aspx
* https://learn.microsoft.com/en-us/ef/core/
* https://learn.microsoft.com/en-us/ef/core/cli/dotnet
* https://github.com/fedeojeda95/N6A-AN-DA2-2019.1-Clases/blob/master/Clases/Clase%203%20-%20EntityFrameworkCore.md
* https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/

### UnitOfWork
---
* https://martinfowler.com/eaaCatalog/unitOfWork.html

### Query specification pattern
---
* https://medium.com/@rudyzio92/net-core-using-the-specification-pattern-alongside-a-generic-repository-318cd4eea4aa

### MediatR
---
* https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/microservice-application-layer-implementation-web-api#implement-the-command-process-pipeline-with-a-mediator-pattern-mediatr

### CQRS
---
* https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/apply-simplified-microservice-cqrs-ddd-patterns

### JWT bearer authentication
---
1. Insert a new User in the Database.
2. Create Token:
```bash
curl --location 'https://localhost:5001/api/v2/login' \
--header 'Content-Type: application/json' \
--data '{
  "username": "admin",
  "password": "xxxxx"
}'
```
3. Use Token: (_replace XXXX with the token generated_)
```bash
curl --location --request GET 'http://localhost:5000/api/v2/Authorizations?sort=customer,asc;id,desc&offset=0&limit=200' \
--header 'Authorization: Bearer XXXX' \
--header 'Content-Type: application/json' \
--data '{
    "statusIn": 0
}'
```

* Can you check the token information in https://jwt.io/
* The token is 1-day expiration. In the class JwtProvider > SecurityTokenDescriptor > Expires, you can change the expiration duration for the token.
* You use the attribute [Authorize] in the endpoint or entire controller to indicate that should use JWT Token.