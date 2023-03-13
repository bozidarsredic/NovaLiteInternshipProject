using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoCore;

namespace ToDo.Infrastructure.Configurations
{
    public class ToDoListConfiguration : IEntityTypeConfiguration<ToDoList>
    {
        public void Configure(EntityTypeBuilder<ToDoList> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).IsRequired().HasMaxLength(20);

            builder.HasMany(x => x.Notes)
                .WithOne(x => x.ToDoList)
                .HasForeignKey(x => x.ToDoListId);
        }


    }
}
