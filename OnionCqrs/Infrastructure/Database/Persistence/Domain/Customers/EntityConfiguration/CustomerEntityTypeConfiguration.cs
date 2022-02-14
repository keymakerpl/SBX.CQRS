using Domain.Utils;
using Domain.Utils.Customers;
using Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Domain.Customers.EntityConfiguration
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(TableNames.CUSTOMERS, SchemaNames.PERSONS);

            builder.HasKey(customer => customer.Id);
            builder.Property(customer => customer.Id).HasColumnName("CustomerId");

            builder.Property(customer => customer.FirstName).HasColumnName("FirstName")
                .HasConversion(firstName => (string)firstName, firstName => PersonName.Create(firstName).Value);
            builder.Property(customer => customer.LastName).HasColumnName("LastName")
                .HasConversion(lastName => (string)lastName, lastName => PersonName.Create(lastName).Value);
            builder.Property(customer => customer.Email).HasColumnName("Email")
                .HasConversion(email => (string)email, email => Email.Create(email).Value);
        }
    }
}
