using CSharpFunctionalExtensions;
using Domain.Common;
using System;

namespace Domain.Persons.Customers
{
    public class Customer : Entity<int>
    {
        public CustomerName FirstName { get; private set; }
        public CustomerName LastName { get; private set; }
        public Email Email { get; private set; }

        public Customer(int id) : base(id) { }

        public Customer(CustomerName firstName,
                        CustomerName lastName,
                        Email email)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
        }

        public void SetFirstName(CustomerName firstName) =>
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));

        public void SetLastName(CustomerName lastName) =>
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));

        public void SetEmail(Email email) =>
            Email = email ?? throw new ArgumentNullException(nameof(email));
    }
}
