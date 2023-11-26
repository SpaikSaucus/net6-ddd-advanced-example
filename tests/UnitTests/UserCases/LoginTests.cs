using AuthorizationOperation.Application.UserCases.Login.Commands;
using AuthorizationOperation.Domain.Authorization.Models;
using AuthorizationOperation.Domain.Core;
using AuthorizationOperation.Domain.User.Models;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.UserCases
{
	public class LoginTests
	{
		private static readonly Guid uuid = Guid.NewGuid();

		[Fact]
		public async Task LoginCommand_UserExists_ReturnToken()
		{
			//Arrange
			var cmd = CreateCommand();
			var stubUnitOfWork = A.Fake<IUnitOfWork>();
			var stubLogger = A.Fake<ILogger<UserLoginCommandHandler>>();
			var stubMediator = A.Fake<IMediator>();
			var stubTokenProvider = A.Fake<ITokenProvider>();

			A.CallTo(() => stubMediator.Send(A<IRequest<User>>._, default)).Returns(CreateUser());
			A.CallTo(() => stubTokenProvider.GenerateToken(A<IUserInformation>._)).Returns("tokenGenerated");

			var handler = new UserLoginCommandHandler(stubLogger, stubMediator, stubTokenProvider);

			//Act
			var result = await handler.Handle(cmd, CancellationToken.None);

			//Assert
			Assert.NotEmpty(result);
		}

		[Fact]
		public async Task LoginCommand_UserNotExists_ReturnEmpty()
		{
			//Arrange
			var cmd = CreateCommand();
			var stubUnitOfWork = A.Fake<IUnitOfWork>();
			var stubRepository = A.Fake<IRepository<Authorization>>();
			var stubLogger = A.Fake<ILogger<UserLoginCommandHandler>>();
			var stubMediator = A.Fake<IMediator>();
			var stubTokenProvider = A.Fake<ITokenProvider>();

			A.CallTo(() => stubMediator.Send(A<IRequest<User>>._, default)).Returns(Task.FromResult<User>(null));

			var handler = new UserLoginCommandHandler(stubLogger, stubMediator, stubTokenProvider);

			//Act
			var result = await handler.Handle(cmd, CancellationToken.None);

			//Assert
			Assert.Empty(result);
		}

		private static UserLoginCommand CreateCommand()
		{
			return new UserLoginCommand()
			{
				UserName = "Test",
				Password = "Test"
			};
		}

		private static Task<User> CreateUser()
		{
			return Task.FromResult(new User()
			{
				UUID = uuid,
				Email = "test@mail.com",
				Created = DateTime.Now
			});
		}
	}
}
