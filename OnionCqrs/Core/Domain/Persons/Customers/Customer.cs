using CSharpFunctionalExtensions;
using Domain.Common;
using System;

namespace Domain.Persons.Customers
{
    public class Customer : Entity<int>
    {
        public PersonName FirstName { get; private set; }
        public PersonName LastName { get; private set; }
        public Email Email { get; private set; }

        public Customer(int id) : base(id) { }

        public Customer(PersonName firstName,
                        PersonName lastName,
                        Email email)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
        }

        public void SetFirstName(PersonName firstName) =>
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));

        public void SetLastName(PersonName lastName) =>
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));

        public void SetEmail(Email email) =>
            Email = email ?? throw new ArgumentNullException(nameof(email));
    }
}
