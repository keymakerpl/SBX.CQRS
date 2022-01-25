using Domain.Persons;
using Domain.Persons.Customers;
using Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Domain.EntityConfiguration.Customers
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(TableNames.CUSTOMERS, SchemaNames.PERSONS);

            builder.HasKey(customer => customer.Id);
            builder.Property(customer => customer.Id).HasColumnName("CustomerId");

            builder.Property(customer => customer.FirstName).HasColumnName("FirstName")
                .HasConversion(firstName => (string)firstName, firstName => CustomerName.Create(firstName).Value);
            builder.Property(customer => customer.LastName).HasColumnName("LastName")
                .HasConversion(lastName => (string)lastName, lastName => CustomerName.Create(lastName).Value);
            builder.Property(customer => customer.Email).HasColumnName("Email")
                .HasConversion(email => (string)email, email => Email.Create(email).Value);
        }
    }
}
