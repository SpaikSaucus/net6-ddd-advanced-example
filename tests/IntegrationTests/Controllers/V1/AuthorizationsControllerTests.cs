using IntegrationTests.Setup;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Controllers.V1
{
	public class AuthorizationsControllerTests : ScenarioBase
    {
        private const string apiVersion = "api/v1/";

		[Fact]
		public async Task GetOne_Exists_ReturnOk()
		{
			//Arrange
			using var server = CreateServer();

			//Act
			var response = await server.CreateClient().GetAsync(apiVersion + Get.Authorizations + 1);

			//Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Theory]
		[InlineData(999)]
		[InlineData(9999)]
		public async Task GetOne_NotExists_ReturnNotFound(int authorizationId)
		{
			//Arrange
			using var server = CreateServer();

			//Act
			var response = await server.CreateClient().GetAsync(apiVersion + Get.Authorizations + authorizationId);

			//Assert
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetOne_InvalidValues_ReturnBadRequest(int authorizationId)
        {
            //Arrange
            using var server = CreateServer();

            //Act
            var response = await server.CreateClient().GetAsync(apiVersion + Get.Authorizations + authorizationId);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
	}
}
