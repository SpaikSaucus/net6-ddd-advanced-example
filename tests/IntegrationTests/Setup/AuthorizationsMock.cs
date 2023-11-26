using AuthorizationOperation.Domain.Authorization.Models;
using System;
using System.Collections.Generic;

namespace IntegrationTests.Setup
{
	public static class AuthorizationsMock
	{
        public static readonly List<Authorization> Get = new() {
			new Authorization(1, Guid.Parse("7eb7c8db-5b0c-4755-ba3e-283b7ad7466c"), "Customer1", AuthorizationStatusEnum.WAITING_FOR_SIGNERS),
			new Authorization(2, Guid.Parse("0c39bb11-587a-4af2-80d7-4e392f92add2"), "Customer1", AuthorizationStatusEnum.AUTHORIZED),
			new Authorization(3, Guid.Parse("285b2373-429e-49bd-87eb-1d2fab501b48"), "Customer2", AuthorizationStatusEnum.CANCELLED),
			new Authorization(4, Guid.Parse("80b4cfb1-20c4-41ed-8e77-86a1c69a675a"), "Customer3", AuthorizationStatusEnum.EXPIRED)
		};
	}
}
