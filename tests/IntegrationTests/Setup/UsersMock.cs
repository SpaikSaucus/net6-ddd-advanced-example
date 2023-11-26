using AuthorizationOperation.Domain.User.Models;
using System;
using System.Collections.Generic;

namespace IntegrationTests.Setup
{
	public static class UsersMock
	{
        public static readonly List<User> Get = new() {
			new User(Guid.Parse("830ff91c-6bde-4cb5-a93c-846f15cc7a1b"), "UserName1", "MyPassword", "username1@mail.com"),
			new User(Guid.Parse("ef5fec4e-a364-4723-b905-c68faacaef6d"), "UserName2", "MyPassword", "username2@mail.com")
		};
	}
}
