using AuthorizationOperation.API.ViewModels;
using AuthorizationOperation.Application.UserCases.FindAll.Queries;
using AuthorizationOperation.Domain.Authorization.Models;
using AuthorizationOperation.Domain.Core;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.UserCases
{
    public class FindAllTests
    {
        const ushort LIMIT = 200;
        const ushort OFFSET = 0;

        [Fact]
        public async Task GetAllQuery_FilterEmpty_ReturnEmpty()
        {
            //Arrange
            var query = CreateAuthorizationGetAllQuery();
            var stubUnitOfWork = A.Fake<IUnitOfWork>();
            var stubRepository = A.Fake<IRepository<Authorization>>();
            var stubLogger = A.Fake<ILogger<AuthorizationGetAllQueryHandler>>();

            A.CallTo(() => stubUnitOfWork.Repository<Authorization>()).Returns(stubRepository);
            A.CallTo(() => stubRepository.Count(A<Expression<Func<Authorization, bool>>>._)).Returns(0);
            A.CallTo(() => stubRepository.Find(A<ISpecification<Authorization>>._)).Returns(null);

            var handler = new AuthorizationGetAllQueryHandler(stubUnitOfWork, stubLogger);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.Equal(0, result.Total);
        }

        [Fact]
        public async Task GetAllQuery_FilterEmpty_ReturnOneResult()
        {
            //Arrange
            var query = CreateAuthorizationGetAllQuery();
            var authorizations = new List<Authorization> { new Authorization() };

            var stubUnitOfWork = A.Fake<IUnitOfWork>();
            var stubRepository = A.Fake<IRepository<Authorization>>();
            var stubLogger = A.Fake<ILogger<AuthorizationGetAllQueryHandler>>();

            A.CallTo(() => stubUnitOfWork.Repository<Authorization>()).Returns(stubRepository);
            A.CallTo(() => stubRepository.Count(A<Expression<Func<Authorization, bool>>>._)).Returns(authorizations.Count);
            A.CallTo(() => stubRepository.Find(A<ISpecification<Authorization>>._)).Returns(authorizations);

            var handler = new AuthorizationGetAllQueryHandler(stubUnitOfWork, stubLogger);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.Equal(1, result.Total);
        }

        private static AuthorizationGetAllQuery CreateAuthorizationGetAllQuery(AuthorizationCriteriaRequest criteria = null)
        {
            criteria ??= new AuthorizationCriteriaRequest();

            var query = new AuthorizationGetAllQuery()
            {
                Limit = LIMIT,
                Offset = OFFSET,
                Sort = string.Empty,
                StatusIn = criteria.ConvertToEnum(),
                StatusInDefaultSelected = criteria.StatusInDefaultSelected
            };
            return query;
        }
    }
}
