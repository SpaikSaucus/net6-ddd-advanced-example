using AuthorizationOperation.API.ViewModels;
using IntegrationTests.Setup;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Controllers.V2
{
	public class LoginControllerTests : ScenarioBase
    {
		private const string apiVersion = "api/v2/";

		[Fact]
		public async Task Login_UserExists_ReturnOk()
		{
			//Arrange
			using var server = CreateServer();
			var req = new LoginRequest()
			{
				UserName = UsersMock.Get.First().UserName,
				Password = UsersMock.Get.First().Password
			};

			//Act
			var response = await server.CreateClient().PostAsJsonAsync(apiVersion + Post.Login, req);

			//Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public async Task Login_UserNotExists_ReturnUnauthorized()
		{
			//Arrange
			var req = new LoginRequest()
			{
				UserName = "UserNotExists",
				Password = "MyPassword"
			};
			using var server = CreateServer();

			//Act
			var response = await server.CreateClient().PostAsJsonAsync(apiVersion + Post.Login, req);

			//Assert
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task Login_InvalidData_ReturnBadRequest()
		{
			//Arrange
			var req = new LoginRequest()
			{
				UserName = "",
				Password = ""
			};
			using var server = CreateServer();

			//Act
			var response = await server.CreateClient().PostAsJsonAsync(apiVersion + Post.Login, req);

			//Assert
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}
	}
}
