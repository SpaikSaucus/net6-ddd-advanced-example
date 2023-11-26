using AuthorizationOperation.API.ViewModels;
using IntegrationTests.Setup;
using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Controllers.V2
{
    public class AuthorizationsControllerTests : ScenarioBase
    {
		private const string apiVersion = "api/v2/";

		[Fact]
		public async Task GetOne_Exists_ReturnOk()
		{
			//Arrange
			var uuid = AuthorizationsMock.Get.First().UUID;
			using var server = CreateServer();

			//Act
			var response = await server.CreateClient().GetAsync(apiVersion + Get.Authorizations + uuid);

			//Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Theory]
		[InlineData("9932ccea-7188-4f65-b7e0-c0029af4520e")]
		[InlineData("98846bcb-f8de-40db-8266-5fa477d6614a")]
		public async Task GetOne_NotExists_ReturnNotFound(Guid uuid)
		{
			//Arrange
			using var server = CreateServer();

			//Act
			var response = await server.CreateClient().GetAsync(apiVersion + Get.Authorizations + uuid);

			//Assert
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public async Task GetOne_InvalidValue_ReturnBadRequest()
		{
			//Arrange
			var uuid = "858585";
			using var server = CreateServer();

			//Act
			var response = await server.CreateClient().GetAsync(apiVersion + Get.Authorizations + uuid);

			//Assert
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task Create_IncompleteData_ReturnBadRequest()
		{
			//Arrange
			var req = CreateAuthorizationRequest(Guid.Parse("04e591fb-5a07-4ade-a31f-3c6c6e4a91ec"), "");
			using var server = CreateServer();

			//Act
			var response = await server.CreateClient().PostAsJsonAsync(apiVersion + Post.Authorizations, req);

			//Assert
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task Create_CompleteData_ReturnCreated()
		{
			//Arrange
			var req = CreateAuthorizationRequest(Guid.Parse("04e591fb-5a07-4ade-a31f-3c6c6e4a91ec"), "Customer1");
			using var server = CreateServer();

			//Act
			var response = await server.CreateClient().PostAsJsonAsync(apiVersion + Post.Authorizations, req);

			//Assert
			Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public async Task Create_ExistsAuthorizationOperation_ReturnBadRequest()
		{
			//Arrange
			var req = CreateExistAuthorizationRequest();
			using var server = CreateServer();

			//Act
			var response = await server.CreateClient().PostAsJsonAsync(apiVersion + Post.Authorizations, req);

			//Assert
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		//[Fact]
		//public async Task Update_ExistsPermission_ReturnBadRequest()
		//{
		//	//Arrange
		//	var req = CreateExistPermissionRequest();
		//	using var server = CreateServer();

		//	//Act
		//	var response = await server.CreateClient().PatchAsJsonAsync(apiVersion + Patch.Permissions + "/" + 2, req);

		//	//Assert
		//	Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		//}

		private static CreateAuthorizationRequest CreateExistAuthorizationRequest()
		{
			var mock = AuthorizationsMock.Get.First();
			return CreateAuthorizationRequest(mock.UUID, mock.Customer);
		}

		private static CreateAuthorizationRequest CreateAuthorizationRequest(Guid uuid, string customer)
		{
			return new CreateAuthorizationRequest()
			{
				UUID = uuid,
				Customer = customer
			};
		}
	}
}
