using AuthorizationOperation.Domain.Core;
using System;

namespace AuthorizationOperation.Domain.Customer.Models.CustomerAggregate
{
    public class Customer : IAggregateRoot
    {
        public uint Id { get; set; }

        public string Firstname { get; set; }

        public string Surname { get; set; }

        public Address Address { get; set; }

        public Customer(uint id, string firstname, string surname, Address address)
        {
            this.Id = id > 0 ? id : throw new ArgumentNullException();
            this.Firstname = !string.IsNullOrEmpty(firstname) ? firstname : throw new ArgumentNullException();
            this.Surname = !string.IsNullOrEmpty(surname) ? surname : throw new ArgumentNullException();
            this.Address = address;
        }
    }
}
