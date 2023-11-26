using AuthorizationOperation.Application.UserCases.FindOne.Queries;
using AuthorizationOperation.Domain.Authorization.Models;
using AuthorizationOperation.Domain.Core;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.UserCases
{
	public class FindOneTests
	{
		private static readonly Guid uuid = Guid.NewGuid();

		[Fact]
		public async Task GetOneQuery_GuidExists_ReturnResult()
		{
			//Arrange
			var query = CreateAuthorizationGetQuery();
			var authorization = new List<Authorization> { new Authorization() };

			var stubUnitOfWork = A.Fake<IUnitOfWork>();
			var stubRepository = A.Fake<IRepository<Authorization>>();
			var stubLogger = A.Fake<ILogger<AuthorizationGetQueryHandler>>();

			A.CallTo(() => stubUnitOfWork.Repository<Authorization>()).Returns(stubRepository);
			A.CallTo(() => stubRepository.Find(A<ISpecification<Authorization>>._)).Returns(authorization);

			var handler = new AuthorizationGetQueryHandler(stubUnitOfWork, stubLogger);

			//Act
			var result = await handler.Handle(query, CancellationToken.None);

			//Assert
			Assert.NotNull(result);
		}

		[Fact]
		public async Task GetOneQuery_GuidNotExists_ReturnEmpty()
		{
			//Arrange
			var query = CreateAuthorizationGetQuery();
			var authorization = new List<Authorization>();

			var stubUnitOfWork = A.Fake<IUnitOfWork>();
			var stubRepository = A.Fake<IRepository<Authorization>>();
			var stubLogger = A.Fake<ILogger<AuthorizationGetQueryHandler>>();

			A.CallTo(() => stubUnitOfWork.Repository<Authorization>()).Returns(stubRepository);
			A.CallTo(() => stubRepository.Find(A<ISpecification<Authorization>>._)).Returns(authorization);

			var handler = new AuthorizationGetQueryHandler(stubUnitOfWork, stubLogger);

			//Act
			var result = await handler.Handle(query, CancellationToken.None);

			//Assert
			Assert.Null(result);
		}

		private static AuthorizationGetQuery CreateAuthorizationGetQuery()
		{
			return new AuthorizationGetQuery() { UUID = uuid };
		}
	}
}
