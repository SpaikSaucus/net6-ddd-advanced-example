using AuthorizationOperation.Application.UserCases.Create.Commands;
using AuthorizationOperation.Domain.Authorization.Models;
using AuthorizationOperation.Domain.Core;
using AuthorizationOperation.Domain.Exceptions;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.UserCases
{
	public class CreateTests
	{
		private static readonly Guid uuid = Guid.NewGuid();
		private const string customer = "Customer1";

		[Fact]
		public async Task CreateCommand_AuthorizationOperationExists_ReturnError()
		{
			//Arrange
			var cmd = CreateCommand();
			var exceptionMsg = $"Authorization { cmd.UUID } already exists in status";
			var stubUnitOfWork = A.Fake<IUnitOfWork>();
			var stubLogger = A.Fake<ILogger<CreateAuthorizationCommandHandler>>();
			var stubMediator = A.Fake<IMediator>();

			A.CallTo(() => stubMediator.Send(A<IRequest<Authorization>>._, default)).Returns(CreateAuthorization());

			var handler = new CreateAuthorizationCommandHandler(stubUnitOfWork, stubLogger, stubMediator);

			//Act
			var exception = await Assert.ThrowsAsync<DomainException>(() => handler.Handle(cmd, CancellationToken.None));

			//Assert
			Assert.Equal(exceptionMsg, exception.Message.Substring(0, exceptionMsg.Length));
		}

		[Fact]
		public async Task CreateCommand_AuthorizationOperationNotExists_ReturnPermissionOk()
		{
			//Arrange
			var cmd = CreateCommand();
			var stubUnitOfWork = A.Fake<IUnitOfWork>();
			var stubRepository = A.Fake<IRepository<Authorization>>();
			var stubLogger = A.Fake<ILogger<CreateAuthorizationCommandHandler>>();
			var stubMediator = A.Fake<IMediator>();

			A.CallTo(() => stubMediator.Send(A<IRequest<Authorization>>._, default)).Returns(Task.FromResult<Authorization>(null));
			A.CallTo(() => stubUnitOfWork.Repository<Authorization>()).Returns(stubRepository);

			var handler = new CreateAuthorizationCommandHandler(stubUnitOfWork, stubLogger, stubMediator);

			//Act
			var result = await handler.Handle(cmd, CancellationToken.None);

			//Assert
			Assert.NotNull(result);
		}

		private static CreateAuthorizationCommand CreateCommand()
		{
			return new CreateAuthorizationCommand()
			{
				UUID = uuid,
				Customer = customer
			};
		}

		private static Task<Authorization> CreateAuthorization()
		{
			return Task.FromResult(new Authorization()
			{
				UUID = uuid,
				Customer = customer,
				StatusId = AuthorizationStatusEnum.WAITING_FOR_SIGNERS,
				Created = DateTime.UtcNow
			});
		}
	}
}
